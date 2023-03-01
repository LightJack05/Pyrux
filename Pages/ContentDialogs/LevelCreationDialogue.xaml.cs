// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Pyrux.Pages.ContentDialogs
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LevelCreationDialogue : Page
    {
        public static int PlayingFieldSize = 2;
        public static string LevelName = "";
        string previousNameTextValue = "";
        public LevelCreationDialogue()
        {
            this.InitializeComponent();
            PlayingFieldSize = 2;
            LevelName = "";
        }

        private void TextBox_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            if(sender.Text.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0) 
            {
                sender.Text = previousNameTextValue;
                sender.SelectionStart = sender.Text.Length;
                sender.SelectionLength = 0;
                LevelName = sender.Text;
            }
            else
            {
                previousNameTextValue = sender.Text;
                LevelName = sender.Text;
            }
        }

        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            PlayingFieldSize = (int)e.NewValue;
        }
    }
}
