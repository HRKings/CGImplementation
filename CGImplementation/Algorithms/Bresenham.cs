using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CGImplementation.Algorithms
{
    public class Bresenham
    {
        // All cases Bresenham Line
        public static void DrawLine(SpriteBatch batch, Point start, Point end, Color color)
        {
            // Calculate the width and height of the line, by using deltaPos
            var width = end.X - start.X;
            var height = end.Y - start.Y;

            var incrementOne = new Point(0);
            var incrementTwo = new Point(0);

            // Handle the cases where the width is negative
            incrementOne.X = width switch
            {
                < 0 => -1,
                > 0 => 1,
                _ => incrementOne.X
            };

            // Handle the case where the height is negativa
            incrementOne.Y = height switch
            {
                < 0 => -1,
                > 0 => 1,
                _ => incrementOne.Y
            };
            
            // Handle the second case when the width is negative
            incrementTwo.X = width switch
            {
                < 0 => -1,
                > 0 => 1,
                _ => incrementTwo.X
            };
            
            // Get the longest and shortest size
            var longest = Math.Abs(width);
            var shortest = Math.Abs(height);
            
            // If the longest is less than the shortest, invert the cases
            if (!(longest > shortest))
            {
                longest = Math.Abs(height);
                shortest = Math.Abs(width);
                incrementTwo.Y = height switch
                {
                    < 0 => -1,
                    > 0 => 1,
                    _ => incrementTwo.Y
                };
                incrementTwo.X = 0;
            }

            // Calculate the numerator by shifting by one bit
            var numerator = longest >> 1;
            
            // For every step, do
            for (var i = 0; i <= longest; i++)
            {
                // Draw the pixel at the point
                Utils.GeneralUtils.DrawPixel(batch, new Point(start.X, start.Y), color);
                
                // Increase the numerator
                numerator += shortest;
                
                // If the numerator is greater than the longest, use case one
                if (!(numerator < longest))
                {
                    numerator -= longest;
                    start.X += incrementOne.X;
                    start.Y += incrementOne.Y;
                }
                // If not, use the case 2
                else
                {
                    start.X += incrementTwo.X;
                    start.Y += incrementTwo.Y;
                }
            }
        }
        
        // Function to draw the 8 points of a circle
        private static void DrawCirclePoints(SpriteBatch batch, Point center, Point pos, Color color)
        {
            Utils.GeneralUtils.DrawPixel(batch, new Point(center.X+pos.X, center.Y+pos.Y), color);
            Utils.GeneralUtils.DrawPixel(batch, new Point(center.X-pos.X, center.Y+pos.Y), color);
            Utils.GeneralUtils.DrawPixel(batch, new Point(center.X+pos.X, center.Y-pos.Y), color);
            Utils.GeneralUtils.DrawPixel(batch, new Point(center.X-pos.X, center.Y-pos.Y), color);
            Utils.GeneralUtils.DrawPixel(batch, new Point(center.X+pos.Y, center.Y+pos.X), color);
            Utils.GeneralUtils.DrawPixel(batch, new Point(center.X-pos.Y, center.Y+pos.X), color);
            Utils.GeneralUtils.DrawPixel(batch, new Point(center.X+pos.Y, center.Y-pos.X), color);
            Utils.GeneralUtils.DrawPixel(batch, new Point(center.X-pos.Y, center.Y-pos.X), color);
        }

        // Bresenham circle
        public static void DrawCircle(SpriteBatch batch, Point center, int radius, Color color)
        {
            // Set the initial point
            var drawPoint = new Point(0, radius);
            
            // Calculate the decision value
            var decision = 3 - 2 * radius;
            
            // Draw the first points
            DrawCirclePoints(batch, center, drawPoint, color);
            
            // While we have points to draw
            while (drawPoint.Y >= drawPoint.X)
            {
                // Increase X, to draw the full circle
                drawPoint.X++;

                // Check for decision parameter and update it, X and Y
                if (decision > 0)
                {
                    drawPoint.Y--;
                    decision = decision + 4 * (drawPoint.X - drawPoint.Y) + 10;
                }
                else
                    decision = decision + 4 * drawPoint.X + 6;

                // For each pixel we draw all eight pixels
                DrawCirclePoints(batch, center, drawPoint, color);
            }
        }
    }
}