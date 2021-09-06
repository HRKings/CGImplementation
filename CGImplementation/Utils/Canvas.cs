using System;
using System.Collections.Generic;
using System.Linq;
using CGImplementation.Algorithms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CGImplementation.Utils
{
    public class CanvasObject
    {
        public Rectangle BoundingBox { get; set; }
    }
    
    public class CanvasLine : CanvasObject
    {
        public Point Start { get; set; }
        public Point End { get; set; }
    }
    
    public class CanvasCircle : CanvasObject
    {
        public Point Center { get; set; }
        public int Radius { get; set; }
    }

    public enum EnumRasterAlgorithm
    {
        DDA = 0,
        Bresenham = 1
    }
    
    public static class Canvas
    {
        public static EnumRasterAlgorithm Raster = EnumRasterAlgorithm.Bresenham;
        public static List<CanvasLine> Lines = new ();
        public static List<CanvasCircle> Circles = new ();

        public static CanvasLine SelectedLine = null;
        public static CanvasLine ClippedLine = null;
        public static CanvasCircle SelectedCircle = null;

        public static void AddLine(Point start, Point end) 
            => Lines.Add(new CanvasLine
            {
                BoundingBox = GeneralUtils.CreateRectangle(start, end),
                Start = start,
                End = end,
            });

        public static void AddCircle(Point center, int radius)
            => Circles.Add(new CanvasCircle
            {
                Center = center,
                Radius = radius,
                BoundingBox = new Rectangle(center - new Point(radius), new Point(radius*2)),
            });

        public static void SelectCanvasObject(Point mousePosition)
        {
            if(!GeneralUtils.isLeftButtonPressed)
                return;

            var selectLine = Lines.FirstOrDefault(x => x.BoundingBox.Contains(mousePosition));
            var selectCircle = Circles.FirstOrDefault(x => x.BoundingBox.Contains(mousePosition));

            if (selectLine is not null)
            {
                SelectedCircle = null;
                SelectedLine = selectLine;
                return;
            }

            if (selectCircle is not null)
            {
                SelectedLine = null;
                SelectedCircle = selectCircle;
            }
        }

        public static void ApplyTranslate(Keys pressedKey)
        {
            var translate = pressedKey switch
            {
                Keys.Up => new Point(0, -10),
                Keys.Down => new Point(0, 10),
                Keys.Left => new Point(-10, 0),
                Keys.Right => new Point(10, 0),
                _ => new Point(0)
            };
            
            if (SelectedLine is not null)
            {
                SelectedLine.Start = Transform.Translate(SelectedLine.Start, translate);
                SelectedLine.End = Transform.Translate(SelectedLine.End, translate);
                SelectedLine.BoundingBox = GeneralUtils.CreateRectangle(SelectedLine.Start, SelectedLine.End);
                return;
            }

            if (SelectedCircle is not null)
            {
                SelectedCircle.Center = Transform.Translate(SelectedCircle.Center, translate);
                SelectedCircle.BoundingBox = new Rectangle(SelectedCircle.Center - new Point(SelectedCircle.Radius),
                    new Point(SelectedCircle.Radius * 2));
            }
        }

        public static void ApplyRotation(bool inverse = false)
        {
            if (SelectedLine is not null)
            {
                SelectedLine.End = Transform.Rotate(SelectedLine.End, SelectedLine.Start, inverse ? -5 : 5);
                SelectedLine.BoundingBox = GeneralUtils.CreateRectangle(SelectedLine.Start, SelectedLine.End);
            }
        }

        public static void ApplyClip()
        {
            if(SelectedLine is null)
                return;

            var clippedLine = CohenSutherland.ClipLine(
                GeneralUtils.CreateRectangle(GeneralUtils.LeftPosition, GeneralUtils.RightPosition),
                SelectedLine.Start,
                SelectedLine.End);
 
            ClippedLine = new CanvasLine
            {
                BoundingBox = new Rectangle(),
                Start = clippedLine.Item1,
                End = clippedLine.Item2
            };
        }
    }
}