using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyrux.DataManagement
{
    internal partial class PyruxLevel
    {
        /// <summary>
        /// The name of the level.
        /// </summary>
        public string LevelName { get; }
        /// <summary>
        /// The string describing the task of the level.
        /// </summary>
        public string Task { get; }
        /// <summary>
        /// A hint for completing the level. Can use MarkDown and is displayed on the "Hint" page.
        /// </summary>
        public string Hint { get; }
        /// <summary>
        /// Determines whether the level is built in, or user created.
        /// </summary>
        public bool IsBuiltIn { get; }
        /// <summary>
        /// The maplayout of the level. Contains walls, playerposition, playerdirection, collectable layout etc.
        /// </summary>
        public PyruxLevelMapLayout MapLayout { get; set; }
        /// <summary>
        /// The script running on the level.
        /// </summary>
        public string Script { get; set; } = "";

        /// <summary>
        /// Initialize a new PyruxLevel.
        /// </summary>
        /// <param name="levelName">The name of the level.</param>
        /// <param name="task">The string describing the task of the level.</param>
        /// <param name="isBuiltIn">Determines whether the level is built in, or user created.</param>
        /// <param name="mapLayout"> The maplayout of the level. Contains walls, playerposition, playerdirection, collectable layout etc. Should be an instance of the PyruxMapLayout class.</param>
        public PyruxLevel(string levelName, string task, bool isBuiltIn, PyruxLevelMapLayout mapLayout, string script)
        {
            LevelName = levelName;
            Task = task;
            IsBuiltIn = isBuiltIn;
            MapLayout = mapLayout;
            Script = script;
        }
        /// <summary>
        /// Creates a copy of the PyruxLevel that is not linked to the original.
        /// </summary>
        /// <returns>A non-reference copy of the class instance it is called on.</returns>
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
                    ),
                Script
                );
        }
    }
}
