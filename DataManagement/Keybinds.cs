
using Microsoft.UI.Xaml.Input;
using Windows.System;

namespace Pyrux.DataManagement
{
    public class Keybinds
    {
        public KeyboardShortcut StartShortcut { get; set; }
        public KeyboardShortcut StepShortcut { get; set; }
        public KeyboardShortcut ResetShortcut { get; set; }
        public KeyboardShortcut PeakGoalLayoutShortcut { get; set; }
        public KeyboardShortcut SaveShortcut { get; set; }
        public KeyboardShortcut ExportShortcut { get; set; }
        public KeyboardShortcut WallToolShortcut { get; set; }
        public KeyboardShortcut ChipToolShortcut { get; set; }
        public KeyboardShortcut PlayerToolShortcut { get; set; }
        public KeyboardShortcut RotatePlayerShortcut { get; set; }
        public KeyboardShortcut ImportLevelShortcut { get; set; }
        public KeyboardShortcut NewLevelShortcut { get; set; }
        public Keybinds()
        {
            StartShortcut = new() { Modifier = VirtualKeyModifiers.None, Key = VirtualKey.F5 };
            StepShortcut = new() { Modifier = VirtualKeyModifiers.Control, Key = VirtualKey.F5 };
            ResetShortcut = new() { Modifier = VirtualKeyModifiers.Control, Key = VirtualKey.R };
            PeakGoalLayoutShortcut = new() { Key = VirtualKey.Space };
            SaveShortcut = new() { Modifier = VirtualKeyModifiers.Control, Key = VirtualKey.S };
            ExportShortcut = new() { Modifier = (VirtualKeyModifiers)5, Key = VirtualKey.S };
            WallToolShortcut = new() { Modifier = VirtualKeyModifiers.Menu, Key = VirtualKey.Number1 };
            ChipToolShortcut = new() { Modifier = VirtualKeyModifiers.Menu, Key = VirtualKey.Number2 };
            PlayerToolShortcut = new() { Modifier = VirtualKeyModifiers.Menu, Key = VirtualKey.Number3 };
            RotatePlayerShortcut = new() { Modifier = VirtualKeyModifiers.Menu, Key= VirtualKey.Number4 };
            ImportLevelShortcut = new() { Modifier= VirtualKeyModifiers.Control, Key= VirtualKey.O };
            NewLevelShortcut = new() { Modifier = VirtualKeyModifiers.Control, Key = VirtualKey.N }; 
        }
    }
}
