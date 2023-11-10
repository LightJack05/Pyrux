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
        public VirtualKeyModifiers Modifier { get; set; }
        public VirtualKey Key { get; set; }
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

    }
}
