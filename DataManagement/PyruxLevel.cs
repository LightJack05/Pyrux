using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyrux.DataManagement
{
    internal class PyruxLevel
    {
        public string LevelName { get; set; }
        public string Task { get; set; }
        public string Hint { get; set; }
        public bool IsBuiltIn { get; set; }
        public PyruxLevelMapLayout MapLayout { get; set; }

        public PyruxLevel(string levelName, string task, bool isBuiltIn, PyruxLevelMapLayout mapLayout)
        {
            LevelName = levelName;
            Task = task;
            IsBuiltIn = isBuiltIn;
            MapLayout = mapLayout;
        }
    }
}
