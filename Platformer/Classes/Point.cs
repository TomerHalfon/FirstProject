using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer.Classes
{
   public class Point
        // a simple point class (used to define location on the canvas)
    {
        public double Top { get; set; }
        public double Left { get; set; }
        public Point()
        {
            Top = 0;
            Left = 0;
        }
        public Point(double top, double left)
        {
            Top = top;
            Left = left;
        }
        public override string ToString()
        {
            return $"Top:{Top}, Left:{Left}";
        }
    }
}
