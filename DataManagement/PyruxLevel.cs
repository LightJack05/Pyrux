using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyrux.DataManagement
{
    internal partial class PyruxLevel
    {
        public string LevelName { get; }
        public string Task { get; }
        public string Hint { get; }
        public bool IsBuiltIn { get; }
        public PyruxLevelMapLayout MapLayout { get; set; }

        public PyruxLevel(string levelName, string task, bool isBuiltIn, PyruxLevelMapLayout mapLayout)
        {
            LevelName = levelName;
            Task = task;
            IsBuiltIn = isBuiltIn;
            MapLayout = mapLayout;
        }

        public PyruxLevel Copy()
        {
            return new PyruxLevel(
                LevelName, 
                Task, 
                IsBuiltIn, 
                new PyruxLevelMapLayout(
                    MapLayout.WallLayout,
                    MapLayout.CollectablesLayout,
                    new PositionVector2(
                        MapLayout.StartPosition.X,
                        MapLayout.StartPosition.Y
                        ),
                    MapLayout.StartPlayerDirection
                    )
                );
        }
    }
}
