// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Pyrux.Pages.ContentDialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Pyrux.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GoalPageView : Page
    {
        private Image _charImage;
        private static int SelectedToolIndex { get; set; }
        private PyruxLevelMapLayout _displayedMapLayout;
        PyruxLevel ActiveLevel { 
            get => StaticDataStore.ActiveLevel; 
            set => StaticDataStore.ActiveLevel = value; 
        }

        public GoalPageView()
        {
            InitializeComponent();
        }
        private void grdPlayFieldBoundary_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double size = Math.Min(grdPlayFieldBoundary.ActualWidth, grdPlayFieldBoundary.ActualHeight);
            grdPlayField.Height = size;
            grdPlayField.Width = size;
        }

        private void LoadLevelIntoPage()
        {
            StaticDataStore.OriginalActiveLevelMapLayout = ActiveLevel.GoalMapLayout.Copy();
            
            BuildPlayGrid();
            FullDisplayRedraw();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (ActiveLevel != null)
            {
                if (ActiveLevel.GoalMapLayout == null)
                {
                    ActiveLevel.GoalMapLayout = ActiveLevel.MapLayout.Copy();
                }
                LoadLevelIntoPage();
                StaticDataStore.OriginalActiveLevelMapLayout = ActiveLevel.MapLayout.Copy();
                FullDisplayRedraw();
                PrepareToolSelection();
            }
        }

        private void PrepareToolSelection()
        {
            if (ActiveLevel.IsBuiltIn)
            {
                btnScrewTool.IsEnabled = false;
                btnRotate.IsEnabled = false;
                btnPlayerTool.IsEnabled = false;
                SelectedToolIndex = -1;
            }
            else
            {
                btnScrewTool.IsEnabled = false;
                SelectedToolIndex = 1;
            }
        }

        /// <summary>
        /// Build the Grid for the level that is loaded.
        /// </summary>
        private void BuildPlayGrid()
        {
            grdPlayField.Children.Clear();
            grdPlayField.RowDefinitions.Clear();
            grdPlayField.ColumnDefinitions.Clear();

            for (int i = 0; i < ActiveLevel.GoalMapLayout.SizeY; i++)
            {
                grdPlayField.RowDefinitions.Add(new RowDefinition());
            }
            for (int j = 0; j < ActiveLevel.GoalMapLayout.SizeX; j++)
            {
                grdPlayField.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < grdPlayField.RowDefinitions.Count(); i++)
            {
                for (int j = 0; j < grdPlayField.ColumnDefinitions.Count(); j++)
                {
                    Border border = new();
                    Image image = new();
                    image.PointerPressed += Tile_Clicked;
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
            charImage.RenderTransform = new RotateTransform { Angle = 90d * ActiveLevel.GoalMapLayout.CurrentPlayerDirection };
            grdPlayField.Children.Add(charImage);
            Grid.SetColumn(charImage, ActiveLevel.GoalMapLayout.StartPosition.X);
            Grid.SetRow(charImage, ActiveLevel.GoalMapLayout.StartPosition.Y);
            _charImage = charImage;
        }

        public void FullDisplayRedraw()
        {
            PyruxLevelMapLayout mapLayout = ActiveLevel.GoalMapLayout;
            Image charImage = (Image)grdPlayField.Children.Last();
            charImage.RenderTransform = new RotateTransform { Angle = 90 * ActiveLevel.GoalMapLayout.CurrentPlayerDirection };
            Grid.SetColumn(charImage, ActiveLevel.GoalMapLayout.CurrentPlayerPosition.X);
            Grid.SetRow(charImage, ActiveLevel.GoalMapLayout.CurrentPlayerPosition.Y);


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
            _displayedMapLayout = mapLayout.Copy();
        }
        /// <summary>
        /// Update the grid that is displaying the playing field so it matches the background data.
        /// </summary>
        public void UpdateDisplay()
        {
            if (_displayedMapLayout == null)
            {
                FullDisplayRedraw();
                return;
            }

            PyruxLevelMapLayout mapLayout = ActiveLevel.GoalMapLayout;
            Image charImage = (Image)grdPlayField.Children.Last();
            charImage.RenderTransform = new RotateTransform { Angle = 90 * ActiveLevel.GoalMapLayout.CurrentPlayerDirection };
            Grid.SetColumn(charImage, ActiveLevel.GoalMapLayout.CurrentPlayerPosition.X);
            Grid.SetRow(charImage, ActiveLevel.GoalMapLayout.CurrentPlayerPosition.Y);


            for (int i = 0; i < mapLayout.WallLayout.GetLength(0); i++)
            {
                for (int j = 0; j < mapLayout.WallLayout.GetLength(1); j++)
                {
                    Border border = (Border)grdPlayField.Children[i * grdPlayField.ColumnDefinitions.Count() + j];
                    Image image = (Image)border.Child;
                    if (mapLayout.WallLayout[j, i])
                    {
                        if ((mapLayout.WallLayout[j, i] != _displayedMapLayout.WallLayout[j, i]))
                        {
                            image.Source = new BitmapImage(new Uri("ms-appx:///Assets/Textures/Wall.png"));
                        }
                    }
                    else if ((!mapLayout.WallLayout[j, i] && (mapLayout.CollectablesLayout[j, i] == 0)))
                    {
                        if (mapLayout.WallLayout[j, i] != _displayedMapLayout.WallLayout[j, i] || mapLayout.CollectablesLayout[j, i] != _displayedMapLayout.CollectablesLayout[j, i])
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
                    }
                    else
                    {
                        if (mapLayout.CollectablesLayout[j, i] != _displayedMapLayout.CollectablesLayout[j, i])
                        {
                            //TODO: Add light mode collectables.
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

            _displayedMapLayout = mapLayout.Copy();

        }
        private void Tile_Clicked(object sender, RoutedEventArgs e)
        {

            Border clickedBorder = VisualTreeHelper.GetParent((Image)sender) as Border;
            switch (SelectedToolIndex)
            {
                case 1:
                    ChangeScrewsTool(new PositionVector2(Grid.GetColumn(clickedBorder), Grid.GetRow(clickedBorder)));
                    break;
                case 2:
                    PositionVector2 newPlayerPosition = new(Grid.GetColumn(clickedBorder), Grid.GetRow(clickedBorder));
                    MovePlayer(newPlayerPosition);
                    break;
                default:
                    break;
            }

        }

        private void btnScrewTool_Click(object sender, RoutedEventArgs e)
        {
            SelectedToolIndex = 1;
            btnScrewTool.IsEnabled = false;

            btnPlayerTool.IsEnabled = true;
        }

        private void btnPlayerTool_Click(object sender, RoutedEventArgs e)
        {
            SelectedToolIndex = 2;
            btnPlayerTool.IsEnabled = false;

            btnScrewTool.IsEnabled = true;
        }

        private void btnRotate_Click(object sender, RoutedEventArgs e)
        {
            TurnPlayer();
        }

        

        private void MovePlayer(PositionVector2 position)
        {
            if (!ActiveLevel.GoalMapLayout.WallLayout[position.Y, position.X])
            {
                ActiveLevel.GoalMapLayout.CurrentPlayerPosition = position.Copy();
            }
            UpdateDisplay();
            SaveNewLayout();
        }

        private void TurnPlayer()
        {
            ActiveLevel.GoalMapLayout.CurrentPlayerDirection++;
            UpdateDisplay();
            SaveNewLayout();
        }

        private void SaveNewLayout()
        {
            StaticDataStore.OriginalActiveLevelMapLayout = ActiveLevel.GoalMapLayout.Copy();
        }


        private void ChangeScrewsTool(PositionVector2 position)
        {
            if (!ActiveLevel.GoalMapLayout.WallLayout[position.Y, position.X])
            {
                PlaceScrewsDialog.Position = position;
                ShowScrewNumberChangeDialog();
            }
        }

        private async void ShowScrewNumberChangeDialog()
        {
            ContentDialog screwNumberChangeDialog = new();
            screwNumberChangeDialog.XamlRoot = this.Content.XamlRoot;
            screwNumberChangeDialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            screwNumberChangeDialog.Title = "Screws";
            screwNumberChangeDialog.PrimaryButtonText = "Save";
            screwNumberChangeDialog.SecondaryButtonText = "Cancel";
            screwNumberChangeDialog.DefaultButton = ContentDialogButton.Primary;
            screwNumberChangeDialog.Content = new PlaceScrewsDialog();

            ContentDialogResult result = await screwNumberChangeDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                ScrewNumberChangeDialogFinished();
            }

        }
        private void ScrewNumberChangeDialogFinished()
        {
            UpdateScrewCount(PlaceScrewsDialog.Position, PlaceScrewsDialog.ScrewNumber);
        }

        private void UpdateScrewCount(PositionVector2 position, int count)
        {
            ActiveLevel.GoalMapLayout.SetScrewNumberAtPosition(position, count);
            SaveNewLayout();
            UpdateDisplay();
        }
    }
}
