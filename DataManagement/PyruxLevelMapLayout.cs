namespace Pyrux.DataManagement
{
    internal class PyruxLevelMapLayout : IEquatable<PyruxLevelMapLayout>
    {
        /// <summary>
        /// The layout of Walls in the map.
        /// </summary>
        public bool[,] WallLayout { get; set; }
        /// <summary>
        /// The layout of the collectables in the map.
        /// </summary>
        public int[,] CollectablesLayout { get; set; }
        /// <summary>
        /// The position the player started at.
        /// </summary>
        public PositionVector2 StartPosition { get; set; }
        /// <summary>
        /// The current position of the player.
        /// </summary>
        public PositionVector2 CurrentPlayerPosition { get; set; }
        /// <summary>
        /// The direction the player started in.
        /// 0 = Right
        /// 1 = Down
        /// 2 = Left
        /// 3 = Up
        /// </summary>
        public byte StartPlayerDirection { get; set; }
        /// <summary>
        /// The direction the player is currently facing.
        /// 0 = Right
        /// 1 = Down
        /// 2 = Left
        /// 3 = Up
        /// </summary>
        public byte CurrentPlayerDirection { get; set; }
        /// <summary>
        /// Number of screws in the players inventory.
        /// </summary>
        public int PlayerScrewInventory { get; set; }
        /// <summary>
        /// Size of the Map in X-direction.
        /// </summary>
        public int SizeX { get => WallLayout.GetLength(1); }
        /// <summary>
        /// Size of the Map in Y-direction.
        /// </summary>
        public int SizeY { get => WallLayout.GetLength(0); }
        /// <summary>
        /// Initializes a new maplayout. Should be bound to a PyruxLevel instance.
        /// </summary>
        /// <param name="wallLayout">The layout of walls in the map.</param>
        /// <param name="collectablesLayout">The layout of the collectables in the map.</param>
        /// <param name="startPosition">The position the player should start at.</param>
        /// <param name="startPlayerDirection">The direction the player should start in.</param>
        public PyruxLevelMapLayout(bool[,] wallLayout, int[,] collectablesLayout, PositionVector2 startPosition, byte startPlayerDirection)
        {
            WallLayout = wallLayout;
            CollectablesLayout = collectablesLayout;
            StartPosition = startPosition;
            CurrentPlayerPosition = startPosition;
            StartPlayerDirection = startPlayerDirection;
            CurrentPlayerDirection = startPlayerDirection;
        }
        /// <summary>
        /// Get the number of screws at a specific position.
        /// </summary>
        /// <param name="position">The position to check.</param>
        /// <returns>Number of screws at the given position.</returns>
        public int GetScrewNumberAtPosition(PositionVector2 position)
        {
            return CollectablesLayout[position.Y, position.X];
        }
        /// <summary>
        /// Set the screw number at a given position.
        /// </summary>
        /// <param name="position">The position to set.</param>
        /// <param name="number">The value to set the position to.</param>
        public void SetScrewNumberAtPosition(PositionVector2 position, int number)
        {
            CollectablesLayout[position.Y, position.X] = number;
        }
        /// <summary>
        /// Check if there is a wall at a given position.
        /// </summary>
        /// <param name="position">The position to check.</param>
        /// <returns>True if there is a wall at the position given, otherwise false.</returns>
        public bool IsWallAtPosition(PositionVector2 position)
        {
            return WallLayout[position.Y, position.X];
        }

        public PyruxLevelMapLayout Copy()
        {
            bool[,] wallLayoutCopy = new bool[WallLayout.GetLength(0), WallLayout.GetLength(1)];
            for (int i = 0; i < WallLayout.GetLength(0); i++)
            {
                for (int j = 0; j < WallLayout.GetLength(1); j++)
                {
                    wallLayoutCopy[i, j] = WallLayout[i, j];
                }
            }

            int[,] collectablesCopy = new int[CollectablesLayout.GetLength(0), CollectablesLayout.GetLength(1)];
            for (int i = 0; i < CollectablesLayout.GetLength(0); i++)
            {
                for (int j = 0; j < CollectablesLayout.GetLength(1); j++)
                {
                    collectablesCopy[i, j] = CollectablesLayout[i, j];
                }
            }

            PyruxLevelMapLayout newLayout = new(
                wallLayoutCopy,
                collectablesCopy,
                StartPosition.Copy(),
                StartPlayerDirection);
            newLayout.CurrentPlayerPosition = CurrentPlayerPosition.Copy();
            newLayout.CurrentPlayerDirection = CurrentPlayerDirection;
            return newLayout;
        }

        public bool Equals(PyruxLevelMapLayout other)
        {
            if (other == null) return false;

            if (other.SizeX != this.SizeX || other.SizeY != this.SizeY)
            {
                return false;
            }

            for (int i = 0; i < CollectablesLayout.GetLength(0); i++)
            {
                for (int j = 0; j < CollectablesLayout.GetLength(1); j++)
                {
                    if (this.CollectablesLayout[i, j] != other.CollectablesLayout[i, j])
                    {
                        return false;
                    }
                }
            }

            for (int i = 0; i < WallLayout.GetLength(0); i++)
            {
                for (int j = 0; j < WallLayout.GetLength(1); j++)
                {
                    if (this.WallLayout[i, j] != other.WallLayout[i, j])
                    {
                        return false;
                    }
                }
            }

            if (PlayerScrewInventory != other.PlayerScrewInventory)
            {
                return false;
            }

            if (CurrentPlayerDirection * 90 % 360 != other.CurrentPlayerDirection * 90 % 360)
            {
                return false;
            }

            if (!CurrentPlayerPosition.Equals(other.CurrentPlayerPosition))
            {
                return false;
            }

            return true;
        }

        public bool MatchGoalLayout( PyruxLevelMapLayout GoalLayout)
        {
            if (GoalLayout == null) return false;

            if (GoalLayout.SizeX != this.SizeX || GoalLayout.SizeY != this.SizeY)
            {
                return false;
            }

            for (int i = 0; i < CollectablesLayout.GetLength(0); i++)
            {
                for (int j = 0; j < CollectablesLayout.GetLength(1); j++)
                {
                    if (this.CollectablesLayout[i, j] != GoalLayout.CollectablesLayout[i, j])
                    {
                        return false;
                    }
                }
            }

            for (int i = 0; i < WallLayout.GetLength(0); i++)
            {
                for (int j = 0; j < WallLayout.GetLength(1); j++)
                {
                    if (this.WallLayout[i, j] != GoalLayout.WallLayout[i, j])
                    {
                        return false;
                    }
                }
            }

            if (CurrentPlayerDirection * 90 % 360 != GoalLayout.CurrentPlayerDirection * 90 % 360)
            {
                return false;
            }

            if (!CurrentPlayerPosition.Equals(GoalLayout.CurrentPlayerPosition))
            {
                return false;
            }

            return true;
        }
    }
}
