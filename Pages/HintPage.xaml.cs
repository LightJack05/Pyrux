// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Pyrux.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HintPage : Page
    {
        public HintPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (StaticDataStore.ActiveLevel != null && StaticDataStore.ActiveLevel.IsBuiltIn)
            {
                mtbHintRenderer.Text = StaticDataStore.ActiveLevel.Hint;
                if (!StaticDataStore.ActiveLevel.IsBuiltIn)
                {
                    btnEditHint.IsEnabled = true;
                }
            }
        }
        /// <summary>
        /// Open the hint edit dialogue.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog taskEditDialog = new();
            taskEditDialog.XamlRoot = this.Content.XamlRoot;
            taskEditDialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            taskEditDialog.Title = "Edit the level's hint";
            taskEditDialog.PrimaryButtonText = "Save";
            taskEditDialog.SecondaryButtonText = "Cancel";
            taskEditDialog.DefaultButton = ContentDialogButton.Primary;
            taskEditDialog.Content = new ContentDialogs.EditHintDialogue();

            ContentDialogResult result = await taskEditDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                ChangeHintDialogueCompleted();
            }
        }
        /// <summary>
        /// Update the hint from the last execution state of an EditHintDialogue.
        /// </summary>
        private void ChangeHintDialogueCompleted()
        {
            UpdateHintText(ContentDialogs.EditHintDialogue.Hint);
        }
        /// <summary>
        /// Update the hint on the page with the given string as markdown.
        /// </summary>
        /// <param name="hint">The new hint, formatted as markdown.</param>
        private void UpdateHintText(string hint)
        {
            mtbHintRenderer.Text = hint;
            StaticDataStore.ActiveLevel.Hint = hint;
        }
    }
}
