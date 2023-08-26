// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.
using Pyrux.Pages.DocsPages;
namespace Pyrux.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DocsPage : Page
    {
        

        public DocsPage()
        {
            this.InitializeComponent();
        }

        

        private void btnPopoutDocs_Click(object sender, RoutedEventArgs e)
        {
            Window docsWindow = new ApplicationWindows.DocumentationWindow();
            docsWindow.Activate();
        }
    }
}
