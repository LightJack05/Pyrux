using Windows.System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Pyrux.Pages.SettingsPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShortcutSettings : Page
    {
        private List<ComboBox> _keyComboBoxes = new();
        private List<ComboBox> _keyModifierComboBoxes = new();

        private Dictionary<uint, string> _virtualKeyModifierToString = new()
        {
            {0 , "None" },
            {1, "Control" },
            {2, "Alt" },
            {3, "Control + Alt" },
            {4, "Shift" },
            {5, "Control + Shift"},
            {6, "Alt + Shift" },
            {7, "Control + Alt + Shift" }
        };
        private Dictionary<string, uint> _stringToVirtualKeyModifier = new()
        {
            {"None", 0 },
            {"Control",1 },
            {"Alt",2 },
            {"Control + Alt",3 },
            {"Shift",4 },
            {"Control + Shift",5},
            {"Alt + Shift",6 },
            {"Control + Alt + Shift",7 }
        };
        private List<KeyboardShortcut> _keyboardShortcuts { get; set; } = new()
        {
            PyruxSettings.Keybinds.StartShortcut,
            PyruxSettings.Keybinds.StepShortcut,
            PyruxSettings.Keybinds.ResetShortcut,
            PyruxSettings.Keybinds.PeakGoalLayoutShortcut,
            PyruxSettings.Keybinds.SaveShortcut,
            PyruxSettings.Keybinds.ExportShortcut,
            PyruxSettings.Keybinds.WallToolShortcut,
            PyruxSettings.Keybinds.ChipToolShortcut,
            PyruxSettings.Keybinds.PlayerToolShortcut,
            PyruxSettings.Keybinds.RotatePlayerShortcut,
            PyruxSettings.Keybinds.ImportLevelShortcut,
            PyruxSettings.Keybinds.NewLevelShortcut
        };
        public ShortcutSettings()
        {
            this.InitializeComponent();
        }

        private void ComboBoxModifier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((KeyboardShortcut)((ComboBox)sender).DataContext).Modifier = (VirtualKeyModifiers)_stringToVirtualKeyModifier[e.AddedItems[0].ToString()];
            PyruxSettings.SaveSettings();
        }

        private void ComboBoxKey_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((KeyboardShortcut)((ComboBox)sender).DataContext).Key = Enum.Parse<VirtualKey>(e.AddedItems[0].ToString());
            PyruxSettings.SaveSettings();
        }

        private void ComboBoxModifier_Loaded(object sender, RoutedEventArgs e)
        {
            ((ComboBox)sender).SelectedItem = _virtualKeyModifierToString[(uint)((KeyboardShortcut)((ComboBox)sender).DataContext).Modifier];
            _keyModifierComboBoxes.Add((ComboBox)sender);

        }

        private void ComboBoxKey_Loaded(object sender, RoutedEventArgs e)
        {
            ((ComboBox)sender).SelectedItem = ((KeyboardShortcut)((ComboBox)sender).DataContext).Key.ToString();
            _keyComboBoxes.Add((ComboBox)sender);
        }

        private void ItemsRepeater_ElementPrepared(ItemsRepeater sender, ItemsRepeaterElementPreparedEventArgs args)
        {
            if (args.Element is FrameworkElement element)
            {
                element.DataContext = _keyboardShortcuts[args.Index];
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PyruxSettings.Keybinds.Reset();
            PyruxSettings.SaveSettings();
            foreach (ComboBox comboBox in _keyComboBoxes)
            {
                comboBox.SelectedItem = ((KeyboardShortcut)comboBox.DataContext).Key.ToString();

            }
            foreach (ComboBox comboBox in _keyModifierComboBoxes)
            {
                comboBox.SelectedItem = _virtualKeyModifierToString[(uint)((KeyboardShortcut)comboBox.DataContext).Modifier];
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _keyComboBoxes.Clear();
            _keyModifierComboBoxes.Clear();
        }
    }
}
