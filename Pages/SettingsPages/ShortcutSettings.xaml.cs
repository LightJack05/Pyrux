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
using Windows.System;
using Windows.UI.Core;
using System.Linq;
using Microsoft.Scripting.Utils;
using IronPython.Compiler.Ast;
using System.Collections.ObjectModel;
using System.Diagnostics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Pyrux.Pages.SettingsPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShortcutSettings : Page
    {

        private List<KeyboardShortcut> _keyboardShortcuts { get; set; } = new()
        {
            new KeyboardShortcut(VirtualKeyModifiers.Control, VirtualKey.S)
            //PyruxSettings.Keybinds.StartShortcut,
            //PyruxSettings.Keybinds.StepShortcut,
            //PyruxSettings.Keybinds.ResetShortcut,
            //PyruxSettings.Keybinds.PeakGoalLayoutShortcut,
            //PyruxSettings.Keybinds.SaveShortcut
        };
        public ShortcutSettings()
        {
            this.InitializeComponent();
        }

        private void ComboBoxModifier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void ComboBoxKey_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBoxModifier_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void ComboBoxKey_Loaded(object sender, RoutedEventArgs e)
        {
            //((ComboBox)sender).SelectedItem = ((KeyboardShortcut)((ComboBox)sender).DataContext).Key.ToString();
        }

    }
}
