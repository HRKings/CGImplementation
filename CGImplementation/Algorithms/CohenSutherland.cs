using Microsoft.Xna.Framework;

namespace CGImplementation.Algorithms
{
    public class CohenSutherland
    {
        // Bytes used to partition the space into 9 regions
        private const byte INSIDE = 0; // 0000

        private const byte LEFT = 1; // 0001
        private const byte RIGHT = 2; // 0010
        private const byte BOTTOM = 4; // 0100
        private const byte TOP = 8; // 1000

        private static byte ComputeOutCode(Rectangle clipWindow, double x, double y)
        {
            // The first code is to be inside of the clip window
            var code = INSIDE;

            // If it is on the left
            if (x < clipWindow.Left)
                code |= LEFT;
            // If it is on the right
            else if (x > clipWindow.Right)
                code |= RIGHT;
            // If it is bellow
            if (y < clipWindow.Bottom)
                code |= BOTTOM;
            // If it is above
            else if (y > clipWindow.Top)
                code |= TOP;

            return code;
        }

        public static (Point, Point) ClipLine(Rectangle clipWindow, Point start, Point end)
        {
            double startX = start.X;
            double startY = start.Y;
            double endX = end.X;
            double endY = end.Y;

            // compute outcodes for the start and end of the line
            var startOutcode = ComputeOutCode(clipWindow, startX, startY);
            var endOutcode = ComputeOutCode(clipWindow, endX, endY);
            var accept = false;

            while (true)
            {
                // Bitwise OR is 0. Trivially accept and get out of loop
                if ((startOutcode | endOutcode) == 0)
                {
                    accept = true;
                    break;
                }

                // Bitwise AND is not 0. Trivially reject and get out of loop
                if ((startOutcode & endOutcode) != 0)
                {
                    break;
                }

                // If it fails both tests, calculate the line segment to clip
                double x = 0;
                double y = 0;

                // Pick one endpoint that is outside the clip window
                var outcodeOut = (startOutcode != 0) ? startOutcode : endOutcode;

                // Find the intersection point

                // Point is above the clip window
                if ((outcodeOut & TOP) != 0)
                {
                    x = startX + (endX - startX) * (clipWindow.Top - startY) / (endY - startY);
                    y = clipWindow.Top;
                }
                // Point is bellow
                else if ((outcodeOut & BOTTOM) != 0)
                {
                    x = startX + (endX - startX) * (clipWindow.Bottom - startY) / (endY - startY);
                    y = clipWindow.Bottom;
                }
                // Point is on the right
                else if ((outcodeOut & RIGHT) != 0)
                {
                    y = startY + (endY - startY) * (clipWindow.Right - startX) / (endX - startX);
                    x = clipWindow.Right;
                }
                // Point is to the left
                else if ((outcodeOut & LEFT) != 0)
                {
                    y = startY + (endY - startY) * (clipWindow.Left - startX) / (endX - startX);
                    x = clipWindow.Left;
                }

                // Outside of the clip window
                if (outcodeOut == startOutcode)
                {
                    startX = x;
                    startY = y;
                    startOutcode = ComputeOutCode(clipWindow, startX, startY);
                }
                else
                {
                    endX = x;
                    endY = y;
                    endOutcode = ComputeOutCode(clipWindow, endX, endY);
                }
            }

            // return the clipped line
            return (new Point((int) startX, (int) startY),
                    new Point((int) endX, (int) endY));

        }
    }
}