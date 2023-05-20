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
    public sealed partial class MovementDocsPage : Page
    {
        public string DocMarkDown { get => """
                # __Movement documentation__
                ## Basics
                Pyrux uses methods to coordinate the robot's movement.
                To move it, you have to call the methods, like so:

                ```
                GoForward()
                ```

                You may also call these methods in loops, methods or other structures:

                ```
                for i in range(5):
                    GoForward()
                ```

                ## Available Movement Methods
                ### Forward movement
                Move the robot forward one square:

                ```
                GoForward()
                ```

                May throw a `Pyrux.WallAheadException` if the robot runs into a wall or border of the play field.
                ### Turning
                Turn the robot left:

                ```
                TurnLeft()
                ```

                Turn the robot right:

                ```
                TurnRight()
                ```

                ## Checks
                ### Check whether a wall is infront of the player

                ```
                WallAhead()
                ```

                This method will return True or False, use it like this in if statements:

                ```
                if WallAhead():
                    TurnLeft()
                ```

                ```
                while not WallAhead():
                    GoForward()
                ```
                
                """; }
        public MovementDocsPage()
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
