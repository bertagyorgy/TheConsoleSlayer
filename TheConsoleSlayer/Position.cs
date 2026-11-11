using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheConsoleSlayer
{
    internal class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }
        public static Position Add(Position p_1, Position p_2)
        {
            return new Position(p_1.X + p_2.X, p_1.Y + p_2.Y);
        }
        public static double Distance(Position p_1, Position p_2)
        {
            return Math.Sqrt(Math.Pow(p_2.X - p_1.X, 2) + Math.Pow(p_2.Y - p_1.Y, 2));
        }
    }
}
