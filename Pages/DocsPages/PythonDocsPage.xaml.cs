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
    public sealed partial class PythonDocsPage : Page
    {
        /// <summary>
        /// String that is used as page content.
        /// </summary>
        public string DocMarkDown { get => """
                # __Python logic documentation__
                This documentation is a VERY simplified breakdown and includes only the features that are essential for using Python within Pyrux.
                If you are looking for a more complete documentation, please use the python documentation website: https://docs.python.org/3/

                Pyrux is running on IronPython 3.4, based on Python 3.4. Please make sure you use Python 3.4 syntax and features only!
                The documentation for Python 3.4 can be found here: https://docs.python.org/release/3.4.0/

                ## __How is Python code executed?__
                Python executes instructions from top to bottom. Whatever you write first will be executed first.

                See this example:

                ```
                GoForward()
                TurnLeft()
                ```

                The robot will go forward first, and then turn left.

                ## __Conditions__
                If you want to check for something, you can use an `if` block.

                ```
                if WallAhead():
                    TurnLeft()
                ```

                In this case the robot will turn left if there is a wall ahead of it.

                You may also structure your if blocks using `elif` and `else`:

                ```
                if WallThere():
                    TurnLeft()
                elif ScrewThere():
                    TakeScrew()
                else:
                    GoForward()
                ```

                Look at the python documentation for if here: https://docs.python.org/release/3.4.0/reference/compound_stmts.html#the-if-statement

                ## __Loops__
                There are 2 types of loops in Python, `for` and `while` loops.

                ### While loops
                While loops are similar to if statements, but they execute their contents as long as the condition is True:

                ```
                while ScrewThere():
                    TakeScrew()
                ```

                The robot will take all screws from the tile it is on.
                
                Take a look at the python documentation for while loops: https://docs.python.org/release/3.4.0/reference/compound_stmts.html#while

                ### For loops
                A for loop can be used in multiple ways, but we will focus on running it's contents a certain number of times:

                ```
                for i in range(4):
                    GoForward()
                ```

                This for loop will run `GoForward` 4 times, and therefore move the robot 4 tiles forward.

                See the documentation for for loops here: https://docs.python.org/release/3.4.0/reference/compound_stmts.html#for
                ## __Methods__
                Code you want to use multiple times, you can put in a method. Put a method definition like this at the top of the file:

                ```
                def MoveFour():
                    for i in range(4):
                        GoForward()
                ```

                Then call your method like you do with Pyrux built in functions:

                ```
                MoveFour()
                ```

                See the documentation for function/method definitions here: https://docs.python.org/release/3.4.0/reference/compound_stmts.html#function-definitions
                
                (Also includes instructions for arguments and other advaced techniques.)
                
                """; }
        public PythonDocsPage()
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
