// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Pyrux.Pages.ContentDialogs
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditHintDialogue : Page
    {
        public static string Hint = "";
        public EditHintDialogue()
        {
            this.InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Hint = txtHintEditBox.Text;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Hint = StaticDataStore.ActiveLevel.Hint;
            txtHintEditBox.Text = Hint;
        }
    }
}
