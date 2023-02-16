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
using System.Reflection.Metadata.Ecma335;
using Windows.ApplicationModel.Wallet;
using Windows.UI.ViewManagement;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Newtonsoft.Json;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Pyrux.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExercisePage : Page
    {

        public static ExercisePage Instance { get; private set; }
        private Image CharImage;
        private PyruxLevel _activeLevel;
        internal PyruxLevel ActiveLevel
        {
            get => _activeLevel;
            set
            {
                _activeLevel = value;
                BuildPlayGrid();
                LoadLevelIntoPage();
                UpdateDisplay();
            }
        }
        public ExercisePage()
        {
            this.InitializeComponent();
            Instance = this;
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
                        new bool[,] {
                        {true, true, true, false, false, false, false, false, false},
                        {false, true, false, true, false, false, false, false, false },
                        {false, false, false, false, false, false, false, false, false },
                        {false, false, false, false, false, false, false, false, false },
                        {false, false, false, false, false, false, false, false, false },
                        {false, false, false, false, false, false, false, false, false },
                        {false, false, false, false, false, false, false, false, false },
                        {false, false, false, false, false, false, false, false, false },
                        {false, false, false, false, false, false, false, false, false }
                        },

                        new int[,] {
                            {0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0},
                            {1,0,0,0,0,0,0,0,0},
                            {1,1,4,0,0,0,0,0,0},
                            {0,0,0,6,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0},
                            {0,0,0,15,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0}
                        },

                        new PositionVector2(5, 5),
                        1
                    );

            ActiveLevel = new PyruxLevel("TestlevelModified", "Test your shit, it works!", true, levelLayout);
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            ArbitraryCodeExecution();
        }

        /// <summary>
        /// Load a level into the exercise page
        /// </summary>
        void LoadLevelIntoPage()
        {
            expTaskExpander.Header = ActiveLevel.LevelName;
            txtTaskBox.Text = ActiveLevel.Task;
            BuildPlayGrid();
        }

        void LoadCodeIntoEditor()
        {
            txtCodeEditor.Text = ActiveLevel.Script;
        }

        /// <summary>
        /// Build the Grid for the level that is loaded.
        /// </summary>
        void BuildPlayGrid()
        {
            grdPlayField.Children.Clear();
            grdPlayField.RowDefinitions.Clear();
            grdPlayField.ColumnDefinitions.Clear();

            for (int i = 0; i < ActiveLevel.MapLayout.SizeY; i++)
            {
                grdPlayField.RowDefinitions.Add(new RowDefinition());
            }
            for (int j = 0; j < ActiveLevel.MapLayout.SizeX; j++)
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

            Image charImage = new();
            charImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/Textures/Player.png"));
            charImage.RenderTransformOrigin = new(.5, .5);
            charImage.RenderTransform = new RotateTransform { Angle = 90d * ActiveLevel.MapLayout.CurrentPlayerDirection };
            grdPlayField.Children.Add(charImage);
            Grid.SetColumn(charImage, ActiveLevel.MapLayout.StartPosition.X);
            Grid.SetRow(charImage, ActiveLevel.MapLayout.StartPosition.Y);
            CharImage = charImage;
        }

        public void UpdateDisplay()
        {
            PyruxLevelMapLayout mapLayout = ActiveLevel.MapLayout;
            Image charImage = (Image)grdPlayField.Children.Last();
            charImage.RenderTransform = new RotateTransform { Angle = 90 * ActiveLevel.MapLayout.CurrentPlayerDirection };


            for (int i = 0; i < mapLayout.WallLayout.GetLength(0); i++)
            {
                for (int j = 0; j < mapLayout.WallLayout.GetLength(1); j++)
                {
                    Border border = (Border)grdPlayField.Children[i * grdPlayField.ColumnDefinitions.Count() + j];
                    Image image = (Image)border.Child;
                    if (mapLayout.WallLayout[j, i])
                    {
                        image.Source = new BitmapImage(new Uri("ms-appx:///Assets/Textures/Wall.png"));
                    }
                    else if (!mapLayout.WallLayout[j, i] && (mapLayout.CollectablesLayout[j, i] == 0))
                    {
                        if (Application.Current.RequestedTheme == ApplicationTheme.Dark)
                        {
                            image.Source = new BitmapImage(new Uri("ms-appx:///Assets/Textures/Background/dark.png"));
                        }
                        else
                        {
                            image.Source = new BitmapImage(new Uri("ms-appx:///Assets/Textures/Background/light.png"));
                        }
                    }
                    else
                    {
                        if (mapLayout.CollectablesLayout[j, i] <= 9 && mapLayout.CollectablesLayout[j, i] > 0)
                        {
                            image.Source = new BitmapImage(new Uri($"ms-appx:///Assets/Textures/Collectables/Collectables{mapLayout.CollectablesLayout[j, i]}.png"));
                        }
                        else
                        {
                            image.Source = new BitmapImage(new Uri($"ms-appx:///Assets/Textures/Collectables/Collectables9.png"));
                        }
                    }

                }
            }

        }


        void ArbitraryCodeExecution()
        {
            //TODO: Add references for other methods.
            //TODO: Add a mockup library for the code editor's Autocomplete/Intellisense.
            //TODO: Add non-execution block that is used to import the mockup library that is removed once the file is imported into the program.

            string pythonCode = ActiveLevel.Script;
            ScriptEngine scriptEngine = Python.CreateEngine();
            ScriptScope scriptScope = scriptEngine.CreateScope();
            scriptScope.SetVariable("TurnLeft", () => this.ActiveLevel.TurnLeft());

            Task.Factory.StartNew(() =>
            {
                scriptEngine.Execute(pythonCode, scriptScope);
            });
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            Pyrux.LevelIO.LevelSaving.Save(ActiveLevel);
        }


    }
}
