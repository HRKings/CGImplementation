using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CGImplementation.Algorithms
{
    public static class DDA
    {
        public static void DrawLine(SpriteBatch batch, Point start, Point end, Color color)
        {
            // Calculate dX & dY
            var dX = end.X - start.X;
            var dY = end.Y - start.Y;

            // Calculate the steps required for generating pixels
            var steps = Math.Abs(dX) > Math.Abs(dY) ? Math.Abs(dX) : Math.Abs(dY);

            // Calculate the increment of x & y for each steps
            var xIncrement = dX / (float) steps;
            var yIncrement = dY / (float) steps;

            // Put pixel for each step
            float drawX = start.X;
            float drawY = start.Y;
            
            for (var i = 0; i <= steps; i++)
            {
                Utils.GeneralUtils.DrawPixel(batch,new Point((int)Math.Round(drawX),
                    (int)Math.Round(drawY)), color);
                drawX += xIncrement; // Increment X at each step
                drawY += yIncrement; // Increment Y at each step
            }
        }
    }
}