// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Pyrux.DataManagement;

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
            LoadBuiltinLevelsIntoMenu();
            LoadCustomLevelsIntoMenu();
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
    }
}
