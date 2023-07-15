// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

using Microsoft.Graphics.Canvas;

namespace Pyrux.Pages.ContentDialogs
{

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditTaskContentDialog : Page
    {
        public static string LevelDescription = "";

        public EditTaskContentDialog()
        {
            this.InitializeComponent();
        }

        private void txtLevelDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            LevelDescription = txtLevelDescription.Text;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (StaticDataStore.ActiveLevel != null)
            {
                LevelDescription = StaticDataStore.ActiveLevel.Task;
                txtLevelDescription.Text = LevelDescription;
            }
        }
    }
}
