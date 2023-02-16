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
        public PositionVector2 StartPosition { get; set; }
        public PositionVector2 CurrentPlayerPosition { get; set; }
        public uint StartPlayerDirection { get; set; }
        public uint CurrentPlayerDirection { get; set; }
        public int SizeX { get => WallLayout.GetLength(0); }
        public int SizeY { get => WallLayout.GetLength(1); }

        public PyruxLevelMapLayout(bool[,] wallLayout, int[,] collectablesLayout, PositionVector2 startPosition, uint startPlayerDirection)
        {
            WallLayout = wallLayout;
            CollectablesLayout = collectablesLayout;
            StartPosition = startPosition;
            CurrentPlayerPosition = startPosition;
            StartPlayerDirection = startPlayerDirection;
            CurrentPlayerDirection = startPlayerDirection;
        }
    }
}
