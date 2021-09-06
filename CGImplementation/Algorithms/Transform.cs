using System;
using Microsoft.Xna.Framework;

namespace CGImplementation.Algorithms
{
    public static class Transform
    {
        public static Point Translate(Point original, Point translation)
            => original + translation;
        
        public static Point Scale(Point original, Point scalate)
            => original * scalate;
        
        public static Point Rotate(Point original, Point center, double angle)
        {
            var sin = Math.Sin(angle);
            var cosing = Math.Cos(angle);

            // Move point to origin
            original -= center;

            // Rotate point
            var newX = (int)Math.Round(original.X * cosing - original.Y * sin);
            var newY = (int)Math.Round(original.X * sin + original.Y * cosing);

            // Translate point back
            original.X = newX + center.X;
            original.Y = newY + center.Y;
            
            return original;
        }

        public static Point Mirror(Point original, Point origin)
            => Rotate(origin, origin, 180);
    }
}