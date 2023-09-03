// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using CommunityToolkit.WinUI.UI.Controls;
using Windows.System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Pyrux.Pages.DocsPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChipsDocsPage : Page
    {
        /// <summary>
        /// String that is used as page content.
        /// </summary>
        public string DocMarkDown { get => """
                # __Chip management documentation__
                ## __Taking Chips__
                To take a chip from the tile the robot is standing on, you can use the `TakeChip` method.

                ```
                TakeChip()
                ```

                ## __Placing Chips__
                To Place a chip on the tile the robot is standing on, you can use the `PlaceChip` method:

                ```
                PlaceChip()
                ```

                ## __Checks__
                You may check whether a chip is on the current tile with the `ChipThere` method:

                ```
                ChipThere()
                ```

                This method returns True or False, and should be used in other statements like so:

                ```
                if ChipThere():
                    GoForward()
                ```
                
                """; }
        public ChipsDocsPage()
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
