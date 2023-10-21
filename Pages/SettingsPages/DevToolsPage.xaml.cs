// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Pyrux.LevelIO;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Pyrux.Pages
{
    /// <summary>
    /// Provides several developer tools, not intended for end user use.
    /// </summary>
    public sealed partial class DevToolsPage : Page
    {
        public DevToolsPage()
        {
            this.InitializeComponent();
        }

        private void btnOpenAppdata_Click(object sender, RoutedEventArgs e)
        {
            
            if (Windows.ApplicationModel.Package.Current.IsBundle)
            {
                string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string packageFamilyName = Windows.ApplicationModel.Package.Current.Id.FamilyName;
                string packagePath = Path.Combine(appDataPath, "Packages", packageFamilyName, "LocalCache");
                System.Diagnostics.Process.Start("explorer.exe", packagePath);
            }
            else
            {
                string appdataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Pyrux");

                System.Diagnostics.Process.Start("explorer.exe", appdataFolder);
            }

            
        }

        private void btnCopyAppdataPath_Click(object sender, RoutedEventArgs e)
        {
            string appdataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Pyrux");

            DataPackage datapackage = new();
            datapackage.SetText(appdataFolder);
            Clipboard.SetContent(datapackage);

        }

        private async void btnClearAppdata_Click(object sender, RoutedEventArgs e)
        {
            await AppdataManagement.ClearAppdataAsync();
            await AppdataManagement.ConstructAppdataAsync();
            Microsoft.Windows.AppLifecycle.AppInstance.Restart("");
        }
    }
}
