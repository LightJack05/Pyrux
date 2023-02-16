using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Pyrux.DataManagement
{
    /// <summary>
    /// A class that represents a 2-dimensional position on the playing field.
    /// Only accepts integers as coordinates.
    /// </summary>
    internal class PositionVector2 : IEquatable<PositionVector2>, IAdditionOperators<PositionVector2, PositionVector2, PositionVector2>
    {
        /// <summary>
        /// Returns a PositionVector2 pointing right.
        /// </summary>
        public static PositionVector2 Right { get => new PositionVector2(1,0); }
        /// <summary>
        /// Returns a PositionVector2 pointing down.
        /// </summary>
        public static PositionVector2 Down { get => new PositionVector2(0,1); }
        /// <summary>
        /// Returns a PositionVector2 pointing left.
        /// </summary>
        public static PositionVector2 Left { get => new PositionVector2(-1,0); }
        /// <summary>
        /// Returns a PositionVector2 pointing up.
        /// </summary>
        public static PositionVector2 Up { get => new PositionVector2(0,-1); }

        /// <summary>
        /// The X-value of the PositionVector2 (horizontal, positive towards right)
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// The Y-value of the PositionVector2 (vertical, positive towards down)
        /// </summary>
        public int Y { get; set; }
        /// <summary>
        /// Initializes an empty instance of the PositionVector2 class. (X=0, Y=0)
        /// </summary>
        public PositionVector2()
        {
            X = 0;
            Y = 0;
        }
        /// <summary>
        /// Initializes an instance of the PositionVector2 class.
        /// </summary>
        /// <param name="x">The X-value of the instance.</param>
        /// <param name="y">The Y-value of the instnace.</param>
        public PositionVector2(int x , int y)
        {
            X = x;
            Y = y;
        }
        /// <summary>
        /// Checks whether 2 PositionVector2 instnaces are equal.
        /// </summary>
        /// <param name="other">The instance to compare to.</param>
        /// <returns>True if the two instances are equal, false if the instances are not equal.</returns>
        public bool Equals(PositionVector2 other)
        {
            return (other.X == this.X && other.Y == this.Y);
        }
        public static PositionVector2 operator +(PositionVector2 left, PositionVector2 right)
        {
            return new PositionVector2(left.X + right.X, left.Y + right.Y);
        }
        /// <summary>
        /// Creates a copy of the PositionVector2 that is not linked to the original.
        /// </summary>
        /// <returns>An independant copy of the PositionVector2 it is called on.</returns>
        public PositionVector2 Copy() => new PositionVector2(X, Y);
        public override string ToString() => $"({X},{Y})";
    }
}
