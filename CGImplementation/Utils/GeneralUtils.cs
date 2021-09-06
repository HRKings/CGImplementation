using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CGImplementation.Utils
{
    public class FunctionButton
    {
        public readonly string Name;
        public bool IsActive;
        public Rectangle Area;
        public Color Colour;
        public Color Highlight;
        public Color Text;
        public Func<bool> OnPress;

        public FunctionButton(string name, int x, int y, Color cor, Color active, Color text)
        {
            Name = name;
            IsActive = false;
            var (tX, ty) = GeneralUtils.Font.MeasureString(name);
            Area = new Rectangle(x, y - (int)ty, (int)tX+10, (int)ty);
            Colour = cor;
            Highlight = active;
            Text = text;
        }

        public void Toggle()
        {
            IsActive = !IsActive;
        }
    }

    public static class GeneralUtils
    {
        // An empty pixel texture
        public static Texture2D Pixel;
        public static SpriteFont Font;

        // The mouse buttons
        public static bool isLeftButtonPressed = false;
        public static bool isRightButtonPressed = false;

        // The mouse position to use
        public static Point LeftPosition = Point.Zero;
        public static Point RightPosition = Point.Zero;

        // The entire of the GUI buttons
        public static Dictionary<string, FunctionButton> GraphButtons;

        public static void DrawPixel(SpriteBatch spriteBatch, Point position, Color color, int size = 1)
        {
            var rectangle = new Rectangle(position, new Point(size));
            spriteBatch.Draw(Pixel, rectangle, null, color);
        }

        public static void DrawLine(SpriteBatch spriteBatch, Vector2 begin, Vector2 end, Color color,
            int width = 1)
        {
            var r = new Rectangle((int) begin.X, (int) begin.Y, (int) (end - begin).Length() + width, width);
            var v = Vector2.Normalize(begin - end);
            var angle = (float) Math.Acos(Vector2.Dot(v, -Vector2.UnitX));
            if (begin.Y > end.Y) angle = MathHelper.TwoPi - angle;

            spriteBatch.Draw(Pixel, r, null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
        }

        public static void UpdateButtons(Point mousePosition)
        {
            if (!isLeftButtonPressed)
                return;

            foreach (var (key, _) in GraphButtons
                .Where(button => button.Value.Area.Contains(mousePosition)))
            {
                GraphButtons[key].Toggle();

                if (GraphButtons[key].OnPress is not null)
                    GraphButtons[key].OnPress();
            }
        }

        public static void DrawButtons(SpriteBatch spriteBatch)
        {
            foreach (var button in GraphButtons.Values)
            {
                spriteBatch.Draw(Pixel, new Vector2(button.Area.X, button.Area.Y), button.Area,
                    button.IsActive ? button.Highlight : button.Colour);
                spriteBatch.DrawString(Font, button.Name, new Vector2(button.Area.X + 5, button.Area.Y),
                    button.Text);
            }
        }

        public static void DrawLineString(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 begin,
            Vector2 end, Color color, float scale = 1f)
        {
            var v = Vector2.Normalize(begin - end);
            var angle = (float) Math.Acos(Vector2.Dot(v, -Vector2.UnitX));
            var spriteEffects = SpriteEffects.None;
            if (begin.Y > end.Y)
            {
                angle = MathHelper.TwoPi - angle;
                spriteEffects = SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally;
            }

            var posX = (begin.X + end.X) / 2;
            var posY = (begin.Y + end.Y) / 2;
            spriteBatch.DrawString(font, text, new Vector2(posX, posY), color, angle,
                Vector2.Zero, scale, spriteEffects, 1f);
        }

        public static Rectangle CreateRectangle(Point start, Point end)
        {
            // Get the size of the rectangle
            var size = end - start;

            // If the Height is positive, return the rectangle 
            if (size.Y >= 0) 
                return new Rectangle(start, size);
            
            // If the height is negative, get the absolute height and subtract it from the start.Y
            size.Y = Math.Abs(size.Y);
            start.Y -= size.Y;
            return new Rectangle(start, size);
        }

        public static bool IsKeyPressed(Keys key, KeyboardState lastState, KeyboardState currentState) 
            => lastState[key] == KeyState.Up && currentState[key] == KeyState.Down;
    }
}
