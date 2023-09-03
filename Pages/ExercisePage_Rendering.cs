namespace Pyrux.Pages;
public sealed partial class ExercisePage
{
    /// <summary>
    /// On resize, change the size of the grid to keep every tile square.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void grdPlayFieldBoundary_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        double size = Math.Min(grdPlayFieldBoundary.ActualWidth, grdPlayFieldBoundary.ActualHeight);
        grdPlayField.Height = size;
        grdPlayField.Width = size;
    }

    /// <summary>
    /// Build the Grid for the level that is loaded.
    /// </summary>
    private void BuildPlayGrid()
    {
        grdPlayField.Children.Clear();
        grdPlayField.RowDefinitions.Clear();
        grdPlayField.ColumnDefinitions.Clear();

        for (int i = 0; i < ActiveLevel.MapLayout.SizeY; i++)
        {
            grdPlayField.RowDefinitions.Add(new RowDefinition());
        }
        for (int j = 0; j < ActiveLevel.MapLayout.SizeX; j++)
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
                image.PointerEntered += Tile_Entered;
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
        charImage.RenderTransform = new RotateTransform { Angle = 90d * ActiveLevel.MapLayout.CurrentPlayerDirection };
        grdPlayField.Children.Add(charImage);
        Grid.SetColumn(charImage, ActiveLevel.MapLayout.StartPosition.X);
        Grid.SetRow(charImage, ActiveLevel.MapLayout.StartPosition.Y);
        _charImage = charImage;
    }
    /// <summary>
    /// Completely redraw the entire grid.
    /// </summary>
    public void FullDisplayRedraw()
    {
        PyruxLevelMapLayout mapLayout = ActiveLevel.MapLayout;
        Image charImage = (Image)grdPlayField.Children.Last();
        charImage.RenderTransform = new RotateTransform { Angle = 90 * ActiveLevel.MapLayout.CurrentPlayerDirection };
        Grid.SetColumn(charImage, ActiveLevel.MapLayout.CurrentPlayerPosition.X);
        Grid.SetRow(charImage, ActiveLevel.MapLayout.CurrentPlayerPosition.Y);


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

        PyruxLevelMapLayout mapLayout = ActiveLevel.MapLayout;
        Image charImage = (Image)grdPlayField.Children.Last();
        charImage.RenderTransform = new RotateTransform { Angle = 90 * ActiveLevel.MapLayout.CurrentPlayerDirection };
        Grid.SetColumn(charImage, ActiveLevel.MapLayout.CurrentPlayerPosition.X);
        Grid.SetRow(charImage, ActiveLevel.MapLayout.CurrentPlayerPosition.Y);


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

    /// <summary>
    /// Prepare the tool selection depending on the level type.
    /// </summary>
    private void PrepareToolSelection()
    {
        if (ActiveLevel.IsBuiltIn)
        {
            btnChipTool.IsEnabled = false;
            btnRotate.IsEnabled = false;
            btnWallTool.IsEnabled = false;
            btnPlayerTool.IsEnabled = false;
            SelectedToolIndex = -1;
        }
        else
        {
            btnWallTool.IsEnabled = false;
            btnChipTool.IsEnabled = true;
            btnRotate.IsEnabled = true;
            btnPlayerTool.IsEnabled = true;
            SelectedToolIndex = 0;
        }
    }

    private void btnReset_Click(object sender, RoutedEventArgs e)
    {
        if (PythonScriptRunning)
        {
            CancelScriptExecution();

        }
        else
        {
            ExecutionRanState = false;
            PrepareToolSelection();
            ResetLayoutToStart();
        }

    }
    /// <summary>
    /// Reset the current level layout to the original layout preceding the execution of the script.
    /// </summary>
    private void ResetLayoutToStart()
    {
        ActiveLevel.MapLayout = StaticDataStore.OriginalActiveLevelMapLayout.Copy();
        UpdateDisplay();
        btnStart.IsEnabled = true;
        btnStep.IsEnabled = true;
        IsStepModeEnabled = false;
    }
    /// <summary>
    /// Reset the layout when navigating away from the page.
    /// </summary>
    public void NavigationLayoutReset()
    {
        if (ActiveLevel != null && StaticDataStore.OriginalActiveLevelMapLayout != null)
        {
            ActiveLevel.MapLayout = StaticDataStore.OriginalActiveLevelMapLayout.Copy();
        }
    }

    /// <summary>
    /// Load a level into the exercise page
    /// </summary>
    private void LoadLevelIntoPage()
    {
        StaticDataStore.OriginalActiveLevelMapLayout = ActiveLevel.MapLayout.Copy();

        expTaskExpander.Header = new TextBlock { Text = ActiveLevel.LevelName, FontSize = 20, Margin = new(0,-3,0,0) };
        txtLevelTask.Text = ActiveLevel.Task;
        txtCodeEditor.Text = ActiveLevel.Script;

        btnStart.IsEnabled = true;
        btnStep.IsEnabled= true;
        btnReset.IsEnabled = true;
        btnSave.IsEnabled = true;
        btnExport.IsEnabled = true;

        if (StaticDataStore.ActiveLevel.GoalMapLayout == null)
        {
            StaticDataStore.ActiveLevel.GoalMapLayout = StaticDataStore.ActiveLevel.MapLayout.Copy();
        }

        BuildPlayGrid();
        FullDisplayRedraw();
    }

}