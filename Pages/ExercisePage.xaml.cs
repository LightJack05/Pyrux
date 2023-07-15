// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

using Pyrux.DataManagement;
using Pyrux.Pages.ContentDialogs;

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

        public int ExecutionSpeed { get => PyruxSettings.ExecutionSpeed; set => PyruxSettings.ExecutionSpeed = value; }
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
            get => StaticDataStore.ActiveLevel;
            set
            {
                StaticDataStore.ActiveLevel = value;
            }
        }
        /// <summary>
        /// Initialize the page and set the instance to itself.
        /// </summary>
        public ExercisePage()
        {
            int executionSpeed = PyruxSettings.ExecutionSpeed;
            this.InitializeComponent();
            Instance = this;
            PyruxSettings.ExecutionSpeed = executionSpeed;
            sldExecutionSpeed.Value = PyruxSettings.ExecutionSpeed;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            sldExecutionSpeed.Value = PyruxSettings.ExecutionSpeed;
            if (StaticDataStore.ActiveLevel == null)
            {
                CreateNewLevel();
            }
            else
            {
                LoadLevelIntoPage();
                StaticDataStore.OriginalActiveLevelMapLayout = ActiveLevel.MapLayout.Copy();
                FullDisplayRedraw();
                PrepareToolSelection();
            }
        }



        private async void btnExport_Click(object sender, RoutedEventArgs e)
        {
            if (StaticDataStore.ActiveLevel != null)
            {
                ContentDialog levelExportDialogue = new();
                levelExportDialogue.XamlRoot = this.Content.XamlRoot;
                levelExportDialogue.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
                levelExportDialogue.Title = "Export the active level";
                levelExportDialogue.PrimaryButtonText = "No";
                levelExportDialogue.SecondaryButtonText = "Yes";
                levelExportDialogue.CloseButtonText = "Cancel";
                levelExportDialogue.DefaultButton = ContentDialogButton.Primary;
                levelExportDialogue.Content = new LevelExportDialogue();

                ContentDialogResult result = await levelExportDialogue.ShowAsync();

                if (result == ContentDialogResult.Primary)
                {
                    PyruxLevel exportedLevel = StaticDataStore.ActiveLevel.Copy();
                    exportedLevel.Script = "";
                    Pyrux.LevelIO.LevelExporting.ExportProcess(exportedLevel);
                }
                else if (result == ContentDialogResult.Secondary)
                {
                    PyruxLevel exportedLevel = StaticDataStore.ActiveLevel.Copy();
                    Pyrux.LevelIO.LevelExporting.ExportProcess(exportedLevel);
                }
                
            }
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

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Pyrux.LevelIO.LevelSaving.Save(ActiveLevel);
        }

        private void sldExecutionSpeed_ValueChanged(object sender, Microsoft.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            PyruxSettings.ExecutionSpeed = (int)e.NewValue;
        }

        private void sldExecutionSpeed_PointerReleased(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            PyruxSettings.SaveSettings();
        }
    }


}
