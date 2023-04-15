// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Pyrux.DataManagement;
using Pyrux.LevelIO;
using Pyrux.Pages.ContentDialogs;
using System.Drawing;
using Windows.UI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Pyrux.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LevelSelectPage : Page
    {
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
        }

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

        private async void LoadCustomLevelsIntoMenu()
        {
            vsgBuiltinLevels.Children.Clear();
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

        private void LoadLevelIntoStaticStorage(PyruxLevel level)
        {
            DataManagement.StaticDataStore.ActiveLevel = level.Copy();
        }

        private void btnNewLevel_Click(object sender, RoutedEventArgs e)
        {
            if (StaticDataStore.ActiveLevel != null)
            {
                ShowConfirmationDialogue();
            }
            else
            {
                CreateNewLevel();
            }
        }

        async void ShowConfirmationDialogue()
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

        private static void CreateNewLevel()
        {
            StaticDataStore.ActiveLevel = null;
            MainWindow.Instance.NavViewNavigate("exerciseView", new Microsoft.UI.Xaml.Media.Animation.CommonNavigationTransitionInfo());
            MainWindow.Instance.NavViewSetSelection(1);
        }

    }
}
