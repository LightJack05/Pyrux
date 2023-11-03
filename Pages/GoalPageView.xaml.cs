// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Pyrux.Pages.ContentDialogs;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Pyrux.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GoalPageView : Page
    {
        Dictionary<int, TeachingTip> PageTeachingTips = new()
        {

        };
        /// <summary>
        /// The image of the character.
        /// </summary>
        private Image _charImage;
        /// <summary>
        /// The selected tool index.
        /// </summary>
        private static int SelectedToolIndex { get; set; }
        /// <summary>
        /// The currently displayed map layout.
        /// </summary>
        private PyruxLevelMapLayout _displayedMapLayout;

        private PyruxLevel ActiveLevel
        {
            get => StaticDataStore.ActiveLevel;
            set => StaticDataStore.ActiveLevel = value;
        }

        public GoalPageView()
        {
            InitializeComponent();
        }
        private void grdPlayFieldBoundary_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double size = Math.Min(grdPlayFieldBoundary.ActualWidth, grdPlayFieldBoundary.ActualHeight);
            grdPlayField.Height = size;
            grdPlayField.Width = size;
        }
        /// <summary>
        /// Load a level into the page from the goal layout, build the display grid and fully redraw the screen.
        /// </summary>
        private void LoadLevelIntoPage()
        {
            //ActiveLevel.GoalMapLayout = StaticDataStore.OriginalActiveLevelMapLayout.Copy();

            BuildPlayGrid();
            FullDisplayRedraw();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (ActiveLevel != null)
            {
                if (ActiveLevel.GoalMapLayout == null)
                {
                    ActiveLevel.GoalMapLayout = StaticDataStore.OriginalActiveLevelMapLayout.Copy();
                }
                LoadLevelIntoPage();
                FullDisplayRedraw();
                PrepareToolSelection();
            }
            InitTutorial();
        }

        private void PrepareToolSelection()
        {
            if (ActiveLevel.IsBuiltIn)
            {
                btnChipTool.IsEnabled = false;
                btnRotate.IsEnabled = false;
                btnPlayerTool.IsEnabled = false;
                SelectedToolIndex = -1;
            }
            else
            {
                btnChipTool.IsEnabled = false;
                btnRotate.IsEnabled = true;
                btnPlayerTool.IsEnabled = true;
                SelectedToolIndex = 1;
            }
        }

        /// <summary>
        /// Build the Grid for the level that is loaded.
        /// </summary>
        private void BuildPlayGrid()
        {
            grdPlayField.Children.Clear();
            grdPlayField.RowDefinitions.Clear();
            grdPlayField.ColumnDefinitions.Clear();

            for (int i = 0; i < ActiveLevel.GoalMapLayout.SizeY; i++)
            {
                grdPlayField.RowDefinitions.Add(new RowDefinition());
            }
            for (int j = 0; j < ActiveLevel.GoalMapLayout.SizeX; j++)
            {
                grdPlayField.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < grdPlayField.RowDefinitions.Count(); i++)
            {
                for (int j = 0; j < grdPlayField.ColumnDefinitions.Count(); j++)
                {
                    Border border = new();
                    Image image = new();
                    image.PointerPressed += Tile_Clicked;
                    border.Child = image;
                    border.Background = new SolidColorBrush(Colors.Black);
                    image.Margin = new(3);

                    grdPlayField.Children.Add(border);
                    Grid.SetColumn(border, i);
                    Grid.SetRow(border, j);
                }
            }

            Image charImage = new();
            charImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/Textures/Player.png"));
            charImage.RenderTransformOrigin = new(.5, .5);
            charImage.RenderTransform = new RotateTransform { Angle = 90d * ActiveLevel.GoalMapLayout.CurrentPlayerDirection };
            grdPlayField.Children.Add(charImage);
            Grid.SetColumn(charImage, ActiveLevel.GoalMapLayout.StartPosition.X);
            Grid.SetRow(charImage, ActiveLevel.GoalMapLayout.StartPosition.Y);
            _charImage = charImage;
        }
        /// <summary>
        /// Completely redraw the entire grid.
        /// </summary>
        public void FullDisplayRedraw()
        {
            PyruxLevelMapLayout mapLayout = ActiveLevel.GoalMapLayout;
            Image charImage = (Image)grdPlayField.Children.Last();
            charImage.RenderTransform = new RotateTransform { Angle = 90 * ActiveLevel.GoalMapLayout.CurrentPlayerDirection };
            Grid.SetColumn(charImage, ActiveLevel.GoalMapLayout.CurrentPlayerPosition.X);
            Grid.SetRow(charImage, ActiveLevel.GoalMapLayout.CurrentPlayerPosition.Y);


            for (int i = 0; i < mapLayout.WallLayout.GetLength(0); i++)
            {
                for (int j = 0; j < mapLayout.WallLayout.GetLength(1); j++)
                {
                    Border border = (Border)grdPlayField.Children[(i * grdPlayField.ColumnDefinitions.Count()) + j];
                    Image image = (Image)border.Child;
                    if (mapLayout.WallLayout[j, i])
                    {
                        image.Source = new BitmapImage(new Uri("ms-appx:///Assets/Textures/Wall.png"));
                    }
                    else if (!mapLayout.WallLayout[j, i] && (mapLayout.CollectablesLayout[j, i] == 0))
                    {
                        if (Application.Current.RequestedTheme == ApplicationTheme.Dark)
                        {
                            image.Source = new BitmapImage(new Uri("ms-appx:///Assets/Textures/Background/dark.png"));
                        }
                        else
                        {
                            image.Source = new BitmapImage(new Uri("ms-appx:///Assets/Textures/Background/light.png"));
                        }
                    }
                    else
                    {
                        if (mapLayout.CollectablesLayout[j, i] <= 9 && mapLayout.CollectablesLayout[j, i] > 0)
                        {
                            image.Source = new BitmapImage(new Uri($"ms-appx:///Assets/Textures/Collectables/Collectables{mapLayout.CollectablesLayout[j, i]}.png"));
                        }
                        else
                        {
                            image.Source = new BitmapImage(new Uri($"ms-appx:///Assets/Textures/Collectables/Collectables9.png"));
                        }
                    }

                }
            }
            _displayedMapLayout = mapLayout.Copy();
        }
        /// <summary>
        /// Update the grid that is displaying the playing field so it matches the background data.
        /// </summary>
        public void UpdateDisplay()
        {
            if (_displayedMapLayout == null)
            {
                FullDisplayRedraw();
                return;
            }

            PyruxLevelMapLayout mapLayout = ActiveLevel.GoalMapLayout;
            Image charImage = (Image)grdPlayField.Children.Last();
            charImage.RenderTransform = new RotateTransform { Angle = 90 * ActiveLevel.GoalMapLayout.CurrentPlayerDirection };
            Grid.SetColumn(charImage, ActiveLevel.GoalMapLayout.CurrentPlayerPosition.X);
            Grid.SetRow(charImage, ActiveLevel.GoalMapLayout.CurrentPlayerPosition.Y);


            for (int i = 0; i < mapLayout.WallLayout.GetLength(0); i++)
            {
                for (int j = 0; j < mapLayout.WallLayout.GetLength(1); j++)
                {
                    Border border = (Border)grdPlayField.Children[(i * grdPlayField.ColumnDefinitions.Count()) + j];
                    Image image = (Image)border.Child;
                    if (mapLayout.WallLayout[j, i])
                    {
                        if (mapLayout.WallLayout[j, i] != _displayedMapLayout.WallLayout[j, i])
                        {
                            image.Source = new BitmapImage(new Uri("ms-appx:///Assets/Textures/Wall.png"));
                        }
                    }
                    else if (!mapLayout.WallLayout[j, i] && (mapLayout.CollectablesLayout[j, i] == 0))
                    {
                        if (mapLayout.WallLayout[j, i] != _displayedMapLayout.WallLayout[j, i] || mapLayout.CollectablesLayout[j, i] != _displayedMapLayout.CollectablesLayout[j, i])
                        {
                            if (Application.Current.RequestedTheme == ApplicationTheme.Dark)
                            {
                                image.Source = new BitmapImage(new Uri("ms-appx:///Assets/Textures/Background/dark.png"));
                            }
                            else
                            {
                                image.Source = new BitmapImage(new Uri("ms-appx:///Assets/Textures/Background/light.png"));
                            }
                        }
                    }
                    else
                    {
                        if (mapLayout.CollectablesLayout[j, i] != _displayedMapLayout.CollectablesLayout[j, i])
                        {
                            //TODO: Add light mode collectables.
                            if (mapLayout.CollectablesLayout[j, i] <= 9 && mapLayout.CollectablesLayout[j, i] > 0)
                            {
                                image.Source = new BitmapImage(new Uri($"ms-appx:///Assets/Textures/Collectables/Collectables{mapLayout.CollectablesLayout[j, i]}.png"));
                            }
                            else
                            {
                                image.Source = new BitmapImage(new Uri($"ms-appx:///Assets/Textures/Collectables/Collectables9.png"));
                            }
                        }
                    }
                }
            }

            _displayedMapLayout = mapLayout.Copy();

        }
        private void Tile_Clicked(object sender, RoutedEventArgs e)
        {

            Border clickedBorder = VisualTreeHelper.GetParent((Image)sender) as Border;
            switch (SelectedToolIndex)
            {
                case 1:
                    ChangeChipsTool(new PositionVector2(Grid.GetColumn(clickedBorder), Grid.GetRow(clickedBorder)));
                    break;
                case 2:
                    PositionVector2 newPlayerPosition = new(Grid.GetColumn(clickedBorder), Grid.GetRow(clickedBorder));
                    MovePlayer(newPlayerPosition);
                    break;
                default:
                    break;
            }

        }

        private void btnChipTool_Click(object sender, RoutedEventArgs e)
        {
            SelectedToolIndex = 1;
            btnChipTool.IsEnabled = false;

            btnPlayerTool.IsEnabled = true;
        }

        private void btnPlayerTool_Click(object sender, RoutedEventArgs e)
        {
            SelectedToolIndex = 2;
            btnPlayerTool.IsEnabled = false;

            btnChipTool.IsEnabled = true;
        }

        private void btnRotate_Click(object sender, RoutedEventArgs e)
        {
            TurnPlayer();
        }



        private void MovePlayer(PositionVector2 position)
        {
            if (!ActiveLevel.GoalMapLayout.WallLayout[position.Y, position.X])
            {
                ActiveLevel.GoalMapLayout.CurrentPlayerPosition = position.Copy();
            }
            UpdateDisplay();
            //SaveNewLayout();
            StaticDataStore.UnsavedChangesPresent = true;
        }

        private void TurnPlayer()
        {
            ActiveLevel.GoalMapLayout.CurrentPlayerDirection++;
            UpdateDisplay();
            //SaveNewLayout();
            StaticDataStore.UnsavedChangesPresent = true;
        }

        //private void SaveNewLayout()
        //{
        //    StaticDataStore.OriginalActiveLevelMapLayout = ActiveLevel.GoalMapLayout.Copy();
        //}


        private void ChangeChipsTool(PositionVector2 position)
        {
            if (!ActiveLevel.GoalMapLayout.WallLayout[position.Y, position.X])
            {
                PlaceChipsDialog.Position = position;
                ShowChipNumberChangeDialog();
            }
        }

        private async void ShowChipNumberChangeDialog()
        {
            ContentDialog chipNumberChangeDialog = new();
            chipNumberChangeDialog.XamlRoot = this.Content.XamlRoot;
            chipNumberChangeDialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            chipNumberChangeDialog.Title = "Chips";
            chipNumberChangeDialog.PrimaryButtonText = "Save";
            chipNumberChangeDialog.SecondaryButtonText = "Cancel";
            chipNumberChangeDialog.DefaultButton = ContentDialogButton.Primary;
            chipNumberChangeDialog.Content = new PlaceChipsDialog();

            ContentDialogResult result = await chipNumberChangeDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                ChipNumberChangeDialogFinished();
            }

        }
        private void ChipNumberChangeDialogFinished()
        {
            UpdateChipCount(PlaceChipsDialog.Position, PlaceChipsDialog.ChipNumber);
        }

        private void UpdateChipCount(PositionVector2 position, int count)
        {
            ActiveLevel.GoalMapLayout.SetChipNumberAtPosition(position, count);
            //SaveNewLayout();
            UpdateDisplay();
            StaticDataStore.UnsavedChangesPresent = true;
        }

        private void InitTutorial()
        {
            PageTeachingTips.Clear();
            PageTeachingTips.Add(19, tctGoalPageIntro);
            PageTeachingTips.Add(20, tctToolsGoalLayout);

            if (!PyruxSettings.SkipTutorialEnabled)
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
            if (PageTeachingTips.Count + 19 <= PyruxSettings.TutorialStateId)
            {
                MainWindow.Instance.NavViewSetSelection(4);
                MainWindow.Instance.NavViewNavigate("docs", new Microsoft.UI.Xaml.Media.Animation.CommonNavigationTransitionInfo());

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
    }
}
