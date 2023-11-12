// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Pyrux.LevelIO;
using Pyrux.Pages.ContentDialogs;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Pyrux.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LevelSelectPage : Page
    {
        private PyruxSettings _pyruxSettings { get => PyruxSettings.Instance; }
        Dictionary<int, TeachingTip> PageTeachingTips = new()
        {
            
        };
        public LevelSelectPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!AppdataManagement.AppdataCorrupted)
            {
                try
                {
                    LoadBuiltinLevelsIntoMenu();
                    LoadCustomLevelsIntoMenu();
                }
                catch
                {
                    AppdataManagement.AppdataCorrupted = true;
                    DisplayAppdataError();
                }
            }
            InitTutorial();

        }

        private void InitTutorial()
        {
            PageTeachingTips.Clear();
            PageTeachingTips.Add(0, tctTutorialIntro);
            PageTeachingTips.Add(1, tctBuiltInLevels);
            PageTeachingTips.Add(2, tctUserCreatedLevels);
            PageTeachingTips.Add(3, tctImport);
            PageTeachingTips.Add(4, tctNewLevel);

            if(!PyruxSettings.SkipTutorialEnabled)
            {
                try
                {
                    PageTeachingTips[PyruxSettings.TutorialStateId].IsOpen = true;
                }
                catch 
                { 
                
                }
            }
        }

        private void TeachingTipNext_Click(object sender, RoutedEventArgs e)
        {
            PageTeachingTips[PyruxSettings.TutorialStateId].IsOpen = false;
            PyruxSettings.TutorialStateId++;
            if(PageTeachingTips.Count <= PyruxSettings.TutorialStateId)
            {
                btnNewLevel_Click(null,null);
            }
            else
            {
                PageTeachingTips[PyruxSettings.TutorialStateId].IsOpen = true;
            }
        }

        private void TeachingTipCloseButtonClick(TeachingTip sender, object args)
        {
            PyruxSettings.SkipTutorialEnabled = true;
            PyruxSettings.TutorialStateId = 0;
        }
        /// <summary>
        /// Display an appdata error should the appdata be corrupted. Otherwise do nothing.
        /// </summary>
        public async void DisplayAppdataError()
        {
            if (AppdataManagement.AppdataCorrupted)
            {
                ContentDialog appdataErrorDialogue = new();
                appdataErrorDialogue.XamlRoot = this.Content.XamlRoot;
                appdataErrorDialogue.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
                appdataErrorDialogue.Title = "Appdata corrupted.";
                appdataErrorDialogue.PrimaryButtonText = "RESET";
                appdataErrorDialogue.SecondaryButtonText = "Exit";
                appdataErrorDialogue.DefaultButton = ContentDialogButton.Primary;
                appdataErrorDialogue.Content = new Pages.ContentDialogs.ConfirmRepairAppdataFolderDialogue();

                ContentDialogResult result = await appdataErrorDialogue.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    await AppdataManagement.ResetAppdata();
                    MainWindow.Instance.NavViewSetEnabled(false);
                }
                else
                {
                    Environment.Exit(0);
                }
            }
        }

        /// <summary>
        /// Load the builtin levels from AppData into the Menu.
        /// </summary>
        private async void LoadBuiltinLevelsIntoMenu()
        {
            vsgBuiltinLevels.Children.Clear();
            List<PyruxLevel> levels = await Pyrux.LevelIO.LevelLoading.FindBuiltInLevels();
            StaticDataStore.BuiltInLevels = new(levels);
            foreach (PyruxLevel level in levels)
            {
                Button levelButton = new();
                TextBlock levelText = new();
                levelText.Text = level.LevelName;
                levelText.TextWrapping = TextWrapping.Wrap;

                levelButton.Content = levelText;
                levelButton.Width = 150;
                levelButton.Height = 150;
                levelButton.Margin = new Thickness(25);
                levelButton.Click += LevelButton_Clicked;

                vsgBuiltinLevels.Children.Add(levelButton);
            }
        }
        /// <summary>
        /// Load the custom levels from appdata into the menu.
        /// </summary>
        private async void LoadCustomLevelsIntoMenu()
        {
            vsgCustomLevels.Children.Clear();
            List<PyruxLevel> levels = await Pyrux.LevelIO.LevelLoading.FindUserCreatedLevels();
            StaticDataStore.UserCreatedLevels = new(levels);
            foreach (PyruxLevel level in levels)
            {
                Button levelButton = new();
                TextBlock levelText = new();
                levelText.Text = level.LevelName;
                levelText.TextWrapping = TextWrapping.Wrap;

                levelButton.Content = levelText;
                levelButton.Width = 150;
                levelButton.Height = 150;
                levelButton.Margin = new Thickness(25);
                levelButton.Click += CustomLevelButton_Clicked;

                vsgCustomLevels.Children.Add(levelButton);
            }
        }
        private void LevelButton_Clicked(object sender, RoutedEventArgs e)
        {

            PyruxLevel level = StaticDataStore.BuiltInLevels.Find(x =>
            {
                ContentPresenter contentPresenter = VisualTreeHelper.GetChild((Button)sender, 0) as ContentPresenter;
                TextBlock textBlock = VisualTreeHelper.GetChild(contentPresenter, 0) as TextBlock;
                return textBlock.Text == x.LevelName;
            });
            LoadLevelIntoStaticStorage(level);
            MainWindow.Instance.NavViewNavigate("exerciseView", new Microsoft.UI.Xaml.Media.Animation.CommonNavigationTransitionInfo());
            MainWindow.Instance.NavViewSetSelection(1);
        }

        private void CustomLevelButton_Clicked(object sender, RoutedEventArgs e)
        {
            PyruxLevel level = StaticDataStore.UserCreatedLevels.Find(x =>
            {
                ContentPresenter contentPresenter = VisualTreeHelper.GetChild((Button)sender, 0) as ContentPresenter;
                TextBlock textBlock = VisualTreeHelper.GetChild(contentPresenter, 0) as TextBlock;
                return textBlock.Text == x.LevelName;
            });
            LoadLevelIntoStaticStorage(level);
            MainWindow.Instance.NavViewNavigate("exerciseView", new Microsoft.UI.Xaml.Media.Animation.CommonNavigationTransitionInfo());
            MainWindow.Instance.NavViewSetSelection(1);
        }
        /// <summary>
        /// Load the given level into the static data store, and create a copy of the layout to be used as original layout.
        /// </summary>
        /// <param name="level">The level to be loaded.</param>
        private void LoadLevelIntoStaticStorage(PyruxLevel level)
        {
            DataManagement.StaticDataStore.ActiveLevel = level.Copy();
            DataManagement.StaticDataStore.OriginalActiveLevelMapLayout = level.MapLayout.Copy();
        }

        private void btnNewLevel_Click(object sender, RoutedEventArgs e)
        {
            if (StaticDataStore.UnsavedChangesPresent)
            {
                ShowConfirmationDialogue();
            }
            else
            {
                CreateNewLevel();
            }
        }
        /// <summary>
        /// Show a confirmation dialogue to confirm overwriting of the current level state.
        /// </summary>
        private async void ShowConfirmationDialogue()
        {
            ContentDialog confirmationDialogue = new();
            confirmationDialogue.XamlRoot = this.Content.XamlRoot;
            confirmationDialogue.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            confirmationDialogue.Title = "Are you sure?";
            confirmationDialogue.MinHeight = 300;
            confirmationDialogue.PrimaryButtonText = "Continue";
            confirmationDialogue.SecondaryButtonText = "Cancel";
            confirmationDialogue.DefaultButton = ContentDialogButton.Primary;
            confirmationDialogue.Content = new ConfirmLevelCreationWithOpenLevel();

            ContentDialogResult result = await confirmationDialogue.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                CreateNewLevel();
            }
        }
        /// <summary>
        /// Handler for creating a new level.
        /// </summary>
        private static void CreateNewLevel()
        {
            StaticDataStore.ActiveLevel = null;
            MainWindow.Instance.NavViewNavigate("exerciseView", new Microsoft.UI.Xaml.Media.Animation.CommonNavigationTransitionInfo());
            MainWindow.Instance.NavViewSetSelection(1);
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            ImportLevel();
        }
        /// <summary>
        /// Import a level from local storage.
        /// </summary>
        private async void ImportLevel()
        {
            await LevelIO.LevelImporting.ImportLevel();
            Page_Loaded(null, null);

        }

        
    }
}
