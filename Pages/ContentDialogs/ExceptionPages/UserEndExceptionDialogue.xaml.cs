// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Pyrux.Pages.ContentDialogs.ExceptionPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserExceptionDialogue : Page
    {
        public Exception DialogueException { get; set; }
        public string DialogueStackTrace { get; set; }
        public UserExceptionDialogue()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            txtExceptionInfo.Text = $"Pyrux.{DialogueException.GetType().ToString().Split(".").Last()}: {DialogueException.Message}";
            txtStackTrace.Text = DialogueStackTrace;
        }
    }
}
