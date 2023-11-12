using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;

namespace Pyrux.DataManagement
{
    public class KeyboardShortcut
    {
        public static List<string> AvailableVirtualKeyCodes { get; } = Enum.GetValues<VirtualKey>().Select<VirtualKey, string>((key) => { return key.ToString(); }).ToList();
        public static List<string> AvailableVirtualKeyModifiers { get; } =
            Enum.GetValues<VirtualKeyModifiers>()
            .Select<VirtualKeyModifiers, string>(
                (key) => key.ToString()
            )
            .Append("Control + Shift")
            .Append("Control + Alt")
            .Append("Alt + Shift")
            .Select<string, string>(
                (key) => key.Replace("Menu", "Alt")
            )
            .Where(
                (key) => !key.Contains("Windows")
            )
            .Order()
            .ToList();

        public VirtualKeyModifiers Modifier { get; set; }
        public VirtualKey Key { get; set; }
        public string Description { get; set; }
        public KeyboardShortcut() 
        {
            Key = VirtualKey.None;
            Modifier = VirtualKeyModifiers.None;
        }
        public KeyboardShortcut(VirtualKey key)
        {
            Key = key;
            Modifier = VirtualKeyModifiers.None;
        }
        public KeyboardShortcut(VirtualKeyModifiers modifier, VirtualKey key)
        {
            Key = key;
            Modifier = modifier;
        }

        public override string ToString()
        {
            return $"({Modifier} + {Key})";
        }

    }
}
