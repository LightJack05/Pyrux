// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Pyrux.LevelIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
            string appdataFolderPath = ApplicationData.Current.LocalFolder.Path;
            System.Diagnostics.Process.Start("explorer.exe", appdataFolderPath);
        }

        private void btnCopyAppdataPath_Click(object sender, RoutedEventArgs e)
        {
            string appdataFolderPath = ApplicationData.Current.LocalFolder.Path;
            DataPackage datapackage = new();
            datapackage.SetText(appdataFolderPath);
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
