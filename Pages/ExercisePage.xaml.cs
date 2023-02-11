// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Media.Imaging;
using Pyrux.DataManagement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Pyrux.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExercisePage : Page
    {
        public ExercisePage()
        {
            this.InitializeComponent();
        }

        private void grdPlayFieldBoundary_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double size = Math.Min(grdPlayFieldBoundary.ActualWidth, grdPlayFieldBoundary.ActualHeight);
            grdPlayField.Height = size;
            grdPlayField.Width = size;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Test level creation
            PyruxLevelMapLayout levelLayout = new PyruxLevelMapLayout(
                        new bool[3, 3] {
                        {true, true, true},
                        {false, true, false },
                        {false, false, false }
                        },

                        new int[3, 3] {
                            {0,0,0},
                            {1,0,0 },
                            {1,1,1 }
                        },

                        new PositionVector2(1, 0),
                        new PositionVector2(1, 0),
                        5,
                        5
                    );

            LoadLevelIntoPage(new PyruxLevel("Testlevel", "Test your shit!", true, levelLayout));
        }



        /// <summary>
        /// Load a level into the exercise page
        /// </summary>
        /// <param name="level">The level instance to load.</param>
        void LoadLevelIntoPage(PyruxLevel level)
        {
            expTaskExpander.Header = level.LevelName;
            txtTaskBox.Text = level.Task;
            BuildPlayGrid(level);
        }

        /// <summary>
        /// Build the Grid for the level that is loaded.
        /// </summary>
        /// <param name="level">The level</param>
        void BuildPlayGrid(PyruxLevel level)
        {
            grdPlayField.Children.Clear();
            grdPlayField.RowDefinitions.Clear();
            grdPlayField.ColumnDefinitions.Clear();

            for (int i = 0; i < level.MapLayout.SizeY; i++)
            {
                grdPlayField.RowDefinitions.Add(new RowDefinition());
            }
            for (int j = 0; j < level.MapLayout.SizeX; j++)
            {
                grdPlayField.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < grdPlayField.RowDefinitions.Count(); i++)
            {
                for (int j = 0; j < grdPlayField.ColumnDefinitions.Count(); j++)
                {
                    Border border = new();
                    Image image = new();
                    border.Child = image;
                    border.Background = new SolidColorBrush(Colors.Black);
                    image.Margin = new(3);

                    grdPlayField.Children.Add(border);
                    Grid.SetColumn(border, i);
                    Grid.SetRow(border, j);
                }
            }
        }
    }
}
