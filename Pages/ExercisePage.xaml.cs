// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Microsoft.UI;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Media.Imaging;
using Pyrux.DataManagement;
using Pyrux.Pages.ContentDialogs;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Pyrux.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExercisePage : Page
    {
        /// <summary>
        /// Current instance of the Page.
        /// </summary>
        public static ExercisePage Instance { get; private set; }
        /// <summary>
        /// The index of the selected tool.
        /// 0 - Walls
        /// 1 - Screws
        /// 2 - Player Movement
        /// </summary>
        private static int SelectedToolIndex { get; set; }
        /// <summary>
        /// Determines whether the python script is currently being executed.
        /// </summary>
        public bool PythonScriptRunning { get; private set; }

        /// <summary>
        /// Whether the current execution of the script should be cancelled.
        /// </summary>
        public bool ExecutionCancelled { get => _executionCanceled; set => _executionCanceled = value; }
        private bool _executionCanceled = false;
        /// <summary>
        /// The image of the Char.
        /// </summary>
        private Image _charImage;
        private PyruxLevelMapLayout _displayedMapLayout;
        /// <summary>
        /// The currently active level in the page.
        /// </summary>
        internal PyruxLevel ActiveLevel
        {
            get => _activeLevel;
            set
            {
                _activeLevel = value;
            }
        }
        private PyruxLevel _activeLevel;
        /// <summary>
        /// Initialize the page and set the instance to itself.
        /// </summary>
        public ExercisePage()
        {
            this.InitializeComponent();
            Instance = this;

        }
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ActiveLevel = StaticDataStore.ActiveLevel;
            if (StaticDataStore.ActiveLevel == null)
            {
                ActiveLevel = new("", "", false, new(new bool[10, 10], new int[10, 10], new(), 0), "");
            }
            LoadLevelIntoPage();
            StaticDataStore.OriginalActiveLevelMapLayout = ActiveLevel.MapLayout.Copy();
            FullDisplayRedraw();
            if (ActiveLevel.IsBuiltIn)
            {
                btnScrewTool.IsEnabled = false;
                btnRotate.IsEnabled = false;
                btnWallTool.IsEnabled = false;
                btnPlayerTool.IsEnabled = false;
                SelectedToolIndex = -1;
            }
            else
            {
                btnWallTool.IsEnabled = false;
                SelectedToolIndex = 0;
            }
        }
        /// <summary>
        /// Start the ArbitraryCodeExecution method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (ActiveLevel.IsBuiltIn)
            {
                ResetLayoutToStart();
            }
            ArbitraryCodeExecution();
        }
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            ResetLayoutToStart();
        }

        private void btnWallTool_Click(object sender, RoutedEventArgs e)
        {
            SelectedToolIndex = 0;
            btnWallTool.IsEnabled = false;

            btnScrewTool.IsEnabled = true;
            btnPlayerTool.IsEnabled = true;
        }

        private void btnScrewTool_Click(object sender, RoutedEventArgs e)
        {
            SelectedToolIndex = 1;
            btnScrewTool.IsEnabled = false;

            btnWallTool.IsEnabled = true;
            btnPlayerTool.IsEnabled = true;
        }

        private void btnPlayerTool_Click(object sender, RoutedEventArgs e)
        {
            SelectedToolIndex = 2;
            btnPlayerTool.IsEnabled = false;

            btnWallTool.IsEnabled = true;
            btnScrewTool.IsEnabled = true;
        }

        private void btnRotate_Click(object sender, RoutedEventArgs e)
        {
            TurnPlayer();
        }

        private void ResetLayoutToStart()
        {
            ActiveLevel.MapLayout = StaticDataStore.OriginalActiveLevelMapLayout.Copy();
            UpdateDisplay();
        }

        /// <summary>
        /// Load a level into the exercise page
        /// </summary>
        void LoadLevelIntoPage()
        {
            expTaskExpander.Header = ActiveLevel.LevelName;
            txtCodeEditor.Text = ActiveLevel.Script;
            BuildPlayGrid();
            UpdateDisplay();
        }

        /// <summary>
        /// Build the Grid for the level that is loaded.
        /// </summary>
        void BuildPlayGrid()
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
                    Border border = (Border)grdPlayField.Children[i * grdPlayField.ColumnDefinitions.Count() + j];
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
                    Border border = (Border)grdPlayField.Children[i * grdPlayField.ColumnDefinitions.Count() + j];
                    Image image = (Image)border.Child;
                    if (mapLayout.WallLayout[j, i])
                    {
                        if ((mapLayout.WallLayout[j, i] != _displayedMapLayout.WallLayout[j, i]))
                        {
                            image.Source = new BitmapImage(new Uri("ms-appx:///Assets/Textures/Wall.png"));
                        }
                    }
                    else if ((!mapLayout.WallLayout[j, i] && (mapLayout.CollectablesLayout[j, i] == 0)))
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
        /// Execute the code that has been written by the user.
        /// Script is taken from ActiveLevel.Script instance of PyruxLevel class.
        /// Will add variables of the basic movement methods to the python environment.
        /// NOTE: Code will execute in another thread to keep UI responsive.
        /// </summary>
        void ArbitraryCodeExecution()
        {
            //TODO: Add a mockup library for the code editor's Autocomplete/Intellisense.
            //TODO: Add non-execution block that is used to import the mockup library that is removed once the file is imported into the program.

            if (!PythonScriptRunning)
            {
                btnStart.IsEnabled = false;
                PythonScriptRunning = true;
                string pythonCode = ActiveLevel.Script;
                ScriptEngine scriptEngine = Python.CreateEngine();
                ScriptScope scriptScope = scriptEngine.CreateScope();
                scriptScope.SetVariable("TurnLeft", () => this.ActiveLevel.TurnLeft());
                scriptScope.SetVariable("TurnRight", () => this.ActiveLevel.TurnRight());
                scriptScope.SetVariable("GoForward", () => this.ActiveLevel.GoForward());
                scriptScope.SetVariable("TakeScrew", () => this.ActiveLevel.TakeScrew());
                scriptScope.SetVariable("PlaceScrew", () => this.ActiveLevel.PlaceScrew());
                scriptScope.SetVariable("WallAhead", () => this.ActiveLevel.WallAhead());
                scriptScope.SetVariable("ScrewThere", () => this.ActiveLevel.ScrewThere());


                Task.Factory.StartNew(() =>
                {
                    scriptEngine.Execute(pythonCode, scriptScope);

                    ExercisePage.Instance.PythonScriptRunning = false;

                    DispatcherQueue dispatcherQueue = ExercisePage.Instance.DispatcherQueue;
                    dispatcherQueue.TryEnqueue(() =>
                    {
                        ExercisePage.Instance.btnStart.IsEnabled = true;
                    });

                });
            }
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            Pyrux.LevelIO.LevelSaving.Save(ActiveLevel);
        }
        /// <summary>
        /// Update the script property of the level when the text box's text changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCodeEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            ActiveLevel.Script = txtCodeEditor.Text;
        }

        /// <summary>
        /// On clicking a tile, apply the selected tool to the tile that was clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tile_Clicked(object sender, RoutedEventArgs e)
        {

            Border clickedBorder = VisualTreeHelper.GetParent((Image)sender) as Border;
            switch (SelectedToolIndex)
            {
                case 0:
                    SwitchWall(new PositionVector2(Grid.GetColumn(clickedBorder), Grid.GetRow(clickedBorder)));
                    break;
                case 1:
                    ChangeScrewsTool(new PositionVector2(Grid.GetColumn(clickedBorder), Grid.GetRow(clickedBorder)));
                    break;
                case 2:
                    PositionVector2 newPlayerPosition = new(Grid.GetColumn(clickedBorder), Grid.GetRow(clickedBorder));
                    MovePlayer(newPlayerPosition);
                    break;
                default:
                    break;
            }

        }
        /// <summary>
        /// On the given position, switch between placed wall and empty square.
        /// Refuse if there are screws on the tile or the player is on it.
        /// </summary>
        /// <param name="position">Position of the tile that should be changed.</param>
        void SwitchWall(PositionVector2 position)
        {
            if (position != ActiveLevel.MapLayout.CurrentPlayerPosition && ActiveLevel.MapLayout.GetScrewNumberAtPosition(position) == 0)
            {
                ActiveLevel.MapLayout.WallLayout[position.Y, position.X] = !ActiveLevel.MapLayout.WallLayout[position.Y, position.X];
            }
            UpdateDisplay();
            SaveNewLayout();
        }
        void MovePlayer(PositionVector2 position)
        {
            if (!ActiveLevel.MapLayout.WallLayout[position.Y, position.X])
            {
                ActiveLevel.MapLayout.CurrentPlayerPosition = position.Copy();
            }
            UpdateDisplay();
            SaveNewLayout();
        }

        void TurnPlayer()
        {
            ActiveLevel.MapLayout.CurrentPlayerDirection++;
            UpdateDisplay();
            SaveNewLayout();
        }

        void SaveNewLayout()
        {
            StaticDataStore.OriginalActiveLevelMapLayout = ActiveLevel.MapLayout.Copy();
        }

        private void btnEditTask_Click(object sender, RoutedEventArgs e)
        {
            ShowTaskEditDialogAsync();
        }

        private async void ShowTaskEditDialogAsync()
        {
            ContentDialog taskEditDialog = new();
            taskEditDialog.XamlRoot = this.Content.XamlRoot;
            taskEditDialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            taskEditDialog.Title = "Change the level description";
            taskEditDialog.PrimaryButtonText = "Save";
            taskEditDialog.SecondaryButtonText = "Cancel";
            taskEditDialog.DefaultButton = ContentDialogButton.Primary;
            taskEditDialog.Content = new ContentDialogs.EditTaskContentDialog();

            ContentDialogResult result = await taskEditDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                ChangeTaskDialogCompleted();
            }
        }

        private void ChangeTaskDialogCompleted()
        {
            UpdateTaskText(ContentDialogs.EditTaskContentDialog.LevelDescription);
        }
        private void UpdateTaskText(string text)
        {
            ActiveLevel.Task = text;
            txtLevelTask.Text = ActiveLevel.Task;
        }

        private void ChangeScrewsTool(PositionVector2 position)
        {
            if (!ActiveLevel.MapLayout.WallLayout[position.Y, position.X])
            {
                PlaceScrewsDialog.Position = position;
                ShowScrewNumberChangeDialog();
            }
        }

        private async void ShowScrewNumberChangeDialog()
        {
            ContentDialog screwNumberChangeDialog = new();
            screwNumberChangeDialog.XamlRoot = this.Content.XamlRoot;
            screwNumberChangeDialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            screwNumberChangeDialog.Title = "Screws";
            screwNumberChangeDialog.PrimaryButtonText = "Save";
            screwNumberChangeDialog.SecondaryButtonText = "Cancel";
            screwNumberChangeDialog.DefaultButton = ContentDialogButton.Primary;
            screwNumberChangeDialog.Content = new PlaceScrewsDialog();

            ContentDialogResult result = await screwNumberChangeDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                ScrewNumberChangeDialogFinished();
            }

        }
        private void ScrewNumberChangeDialogFinished()
        {
            UpdateScrewCount(PlaceScrewsDialog.Position, PlaceScrewsDialog.ScrewNumber);
        }

        private void UpdateScrewCount(PositionVector2 position, int count)
        {
            ActiveLevel.MapLayout.SetScrewNumberAtPosition(position, count);
            SaveNewLayout();
            UpdateDisplay();
        }

    }


}
