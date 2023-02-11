using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyrux.DataManagement
{
    internal class PyruxLevelMapLayout
    {
        public bool[,] WallLayout { get; set; }
        public int[,] CollectablesLayout { get; set; }
        PositionVector2 startPosition { get; set; }
        PositionVector2 currentPlayerPosition { get; set; }
        public int SizeX { get => WallLayout.GetLength(0); }
        public int SizeY { get => WallLayout.GetLength(1); }

        public PyruxLevelMapLayout(bool[,] wallLayout, int[,] collectablesLayout, PositionVector2 startPosition, PositionVector2 currentPlayerPosition)
        {
            WallLayout = wallLayout;
            CollectablesLayout = collectablesLayout;
            this.startPosition = startPosition;
            this.currentPlayerPosition = currentPlayerPosition;
        }
    }
}
