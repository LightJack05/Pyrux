using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyrux.DataManagement
{
    internal class PositionVector2 : IEquatable<PositionVector2>
    {
        public int X { get; set; }
        public int Y { get; set; }
        public PositionVector2()
        {
            X = 0;
            Y = 0;
        }
        public PositionVector2(int x , int y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(PositionVector2 other)
        {
            return (other.X == this.X && other.Y == this.Y);
        }

        public override string ToString() => $"({X},{Y})";
    }
}
