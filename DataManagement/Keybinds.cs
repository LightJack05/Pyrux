
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
            StartShortcut = new() { Modifier = VirtualKeyModifiers.None, Key = VirtualKey.F5, Description = "Execute Code" };
            StepShortcut = new() { Modifier = VirtualKeyModifiers.Control, Key = VirtualKey.F5, Description = "Step through code" };
            ResetShortcut = new() { Modifier = VirtualKeyModifiers.Control, Key = VirtualKey.R, Description = "Reset Layout" };
            PeakGoalLayoutShortcut = new() { Key = VirtualKey.Space , Description = "Peak Goal Layout"};
            SaveShortcut = new() { Modifier = VirtualKeyModifiers.Control, Key = VirtualKey.S , Description = "Save Level"};
            ExportShortcut = new() { Modifier = (VirtualKeyModifiers)5, Key = VirtualKey.S , Description = "Export Level"};
            WallToolShortcut = new() { Modifier = VirtualKeyModifiers.Menu, Key = VirtualKey.Number1 , Description = "Select Wall Tool"};
            ChipToolShortcut = new() { Modifier = VirtualKeyModifiers.Menu, Key = VirtualKey.Number2 , Description = "Select Chip Tool"};
            PlayerToolShortcut = new() { Modifier = VirtualKeyModifiers.Menu, Key = VirtualKey.Number3 , Description = "Select Move Player Tool"};
            RotatePlayerShortcut = new() { Modifier = VirtualKeyModifiers.Menu, Key= VirtualKey.Number4 , Description = "Rotate Player"};
            ImportLevelShortcut = new() { Modifier= VirtualKeyModifiers.Control, Key= VirtualKey.O , Description = "Import Level"};
            NewLevelShortcut = new() { Modifier = VirtualKeyModifiers.Control, Key = VirtualKey.N , Description = "Create new Level"}; 
        }

        public void Reset()
        {
            StartShortcut.Modifier = VirtualKeyModifiers.None; StartShortcut.Key = VirtualKey.F5;
            StepShortcut.Modifier = VirtualKeyModifiers.Control; StepShortcut.Key = VirtualKey.F5;
            ResetShortcut.Modifier = VirtualKeyModifiers.Control; ResetShortcut.Key = VirtualKey.R;
            PeakGoalLayoutShortcut.Key = VirtualKey.Space;
            SaveShortcut.Modifier = VirtualKeyModifiers.Control; SaveShortcut.Key = VirtualKey.S;
            ExportShortcut.Modifier = (VirtualKeyModifiers)5; ExportShortcut.Key = VirtualKey.S;
            WallToolShortcut.Modifier = VirtualKeyModifiers.Menu; WallToolShortcut.Key = VirtualKey.Number1;
            ChipToolShortcut.Modifier = VirtualKeyModifiers.Menu; ChipToolShortcut.Key = VirtualKey.Number2;
            PlayerToolShortcut.Modifier = VirtualKeyModifiers.Menu; PlayerToolShortcut.Key = VirtualKey.Number3;
            RotatePlayerShortcut.Modifier = VirtualKeyModifiers.Menu; RotatePlayerShortcut.Key = VirtualKey.Number4;
            ImportLevelShortcut.Modifier = VirtualKeyModifiers.Control; ImportLevelShortcut.Key = VirtualKey.O;
            NewLevelShortcut.Modifier = VirtualKeyModifiers.Control; NewLevelShortcut.Key = VirtualKey.N;
        }
    }
}
