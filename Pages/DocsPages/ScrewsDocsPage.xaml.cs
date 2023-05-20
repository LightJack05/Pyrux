// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using CommunityToolkit.WinUI.UI.Controls;
using Windows.System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Pyrux.Pages.DocsPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ScrewsDocsPage : Page
    {
        public string DocMarkDown { get => """
                # __Screw management documentation__
                ## __Taking Screws__
                To take a screw from the tile the robot is standing on, you can use the `TakeScrew` method.

                ```
                TakeScrew()
                ```

                ## __Placing Screws__
                To Place a screw on the tile the robot is standing on, you can use the `PlaceScrew` method:

                ```
                PlaceScrew()
                ```

                ## __Checks__
                You may check whether a screw is on the current tile with the `ScrewThere` method:

                ```
                ScrewThere()
                ```

                This method returns True or False, and should be used in other statements like so:

                ```
                if ScrewThere():
                    GoForward()
                ```
                
                """; }
        public ScrewsDocsPage()
        {
            this.InitializeComponent();
        }
        private async void MarkdownText_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            if (Uri.TryCreate(e.Link, UriKind.Absolute, out Uri link))
            {
                await Launcher.LaunchUriAsync(link);
            }
        }
    }
}
