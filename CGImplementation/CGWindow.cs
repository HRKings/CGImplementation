using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CGImplementation.Algorithms;
using CGImplementation.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CGImplementation
{
    public class CGWindow : Game
    {
        // The renderer
        private readonly GraphicsDeviceManager _graphicsDevice;
        
        // The GPU draw call batch
        private SpriteBatch _spriteBatch;
        
        // Get the initial mouse and keyboard state
        private MouseState _lastMouseState = Mouse.GetState();
        private KeyboardState _lastKeyboardState = Keyboard.GetState();

        public CGWindow(string[] args)
        {
            _graphicsDevice = new GraphicsDeviceManager(this);

            // Change the assets folder
            Content.RootDirectory = "Content";

            IsMouseVisible = true;
        }
        
        protected override void Initialize()
        {
            // Sets the window size
            _graphicsDevice.PreferredBackBufferWidth = 1280;
            _graphicsDevice.PreferredBackBufferHeight = 720;

            _graphicsDevice.ApplyChanges();

            // Init the GUI buttons hashmap
            GeneralUtils.GraphButtons = new Dictionary<string, FunctionButton>();

            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Loads the font
            GeneralUtils.Font = Content.Load<SpriteFont>("Arial");

            // Creates the white pixel
            GeneralUtils.Pixel = new Texture2D(GraphicsDevice, 1, 1);
            GeneralUtils.Pixel.SetData(new[] { Color.White });

            // Creates a button
            GeneralUtils.GraphButtons.Add("addLine", new FunctionButton("Adicionar Linha",
                10, 710, Color.Aquamarine, Color.Aqua, Color.Black));
            
            // Declares a custom function to be executed when the button is clicked
            GeneralUtils.GraphButtons["addLine"].OnPress = () =>
            {
                Canvas.AddLine(GeneralUtils.LeftPosition, GeneralUtils.RightPosition);
                GeneralUtils.GraphButtons["addLine"].IsActive = false;
                return true;
            };
            
            // Creates a button
            GeneralUtils.GraphButtons.Add("addCircle", new FunctionButton("Adicionar Circulo",
                10 + 10 + GeneralUtils.GraphButtons["addLine"].Area.Width + GeneralUtils.GraphButtons["addLine"].Area.X,
                710, Color.Aquamarine, Color.Aqua, Color.Black));
            
            // Declares a custom function to be executed when the button is clicked
            GeneralUtils.GraphButtons["addCircle"].OnPress = () =>
            {
                Canvas.AddCircle(GeneralUtils.LeftPosition, 100);
                GeneralUtils.GraphButtons["addCircle"].IsActive = false;
                return true;
            };

            // Creates a button
            GeneralUtils.GraphButtons.Add("rasterDDA", new FunctionButton("Ativar DDA",
                10 + GeneralUtils.GraphButtons["addCircle"].Area.Width + GeneralUtils.GraphButtons["addCircle"].Area.X, 
                710, Color.Aquamarine, Color.Red, Color.Black));

            GeneralUtils.GraphButtons["rasterDDA"].OnPress = () =>
            {
                if (Canvas.Raster is EnumRasterAlgorithm.Bresenham)
                {
                    Canvas.Raster = EnumRasterAlgorithm.DDA;
                    GeneralUtils.GraphButtons["rasterDDA"].IsActive = true;
                    return true;
                }
                
                Canvas.Raster = EnumRasterAlgorithm.Bresenham;
                GeneralUtils.GraphButtons["rasterDDA"].IsActive = false;
                return true;
            }; 
            
            // Creates a button
            GeneralUtils.GraphButtons.Add("clip", new FunctionButton("Cohen Clip",
                10 + GeneralUtils.GraphButtons["rasterDDA"].Area.Width + GeneralUtils.GraphButtons["rasterDDA"].Area.X, 
                710, Color.Aquamarine, Color.Red, Color.Black));
            
            GeneralUtils.GraphButtons["clip"].OnPress = () =>
            {
                Canvas.ApplyClip();
                GeneralUtils.GraphButtons["clip"].IsActive = false;
                return true;
            }; 
            
            // Creates a button
            GeneralUtils.GraphButtons.Add("moveUp", new FunctionButton("Acima",
                10 + GeneralUtils.GraphButtons["clip"].Area.Width + GeneralUtils.GraphButtons["clip"].Area.X, 
                710, Color.Aquamarine, Color.Red, Color.Black));
            
            GeneralUtils.GraphButtons["moveUp"].OnPress = () =>
            {
                Canvas.ApplyTranslate(Keys.Up);
                GeneralUtils.GraphButtons["moveUp"].IsActive = false;
                return true;
            }; 
            
            // Creates a button
            GeneralUtils.GraphButtons.Add("moveDown", new FunctionButton("Abaixo",
                10 + GeneralUtils.GraphButtons["moveUp"].Area.Width + GeneralUtils.GraphButtons["moveUp"].Area.X, 
                710, Color.Aquamarine, Color.Red, Color.Black));
            
            GeneralUtils.GraphButtons["moveDown"].OnPress = () =>
            {
                Canvas.ApplyTranslate(Keys.Down);
                GeneralUtils.GraphButtons["moveDown"].IsActive = false;
                return true;
            }; 
            
            // Creates a button
            GeneralUtils.GraphButtons.Add("moveLeft", new FunctionButton("Esquerda",
                10 + GeneralUtils.GraphButtons["moveDown"].Area.Width + GeneralUtils.GraphButtons["moveDown"].Area.X, 
                710, Color.Aquamarine, Color.Red, Color.Black));
            
            GeneralUtils.GraphButtons["moveLeft"].OnPress = () =>
            {
                Canvas.ApplyTranslate(Keys.Left);
                GeneralUtils.GraphButtons["moveLeft"].IsActive = false;
                return true;
            }; 
            
            // Creates a button
            GeneralUtils.GraphButtons.Add("moveRight", new FunctionButton("Direita",
                10 + GeneralUtils.GraphButtons["moveLeft"].Area.Width + GeneralUtils.GraphButtons["moveLeft"].Area.X, 
                710, Color.Aquamarine, Color.Red, Color.Black));
            
            GeneralUtils.GraphButtons["moveRight"].OnPress = () =>
            {
                Canvas.ApplyTranslate(Keys.Right);
                GeneralUtils.GraphButtons["moveRight"].IsActive = false;
                return true;
            }; 
            
            // Creates a button
            GeneralUtils.GraphButtons.Add("rotateLeft", new FunctionButton("R: <-",
                10 + GeneralUtils.GraphButtons["moveRight"].Area.Width + GeneralUtils.GraphButtons["moveRight"].Area.X, 
                710, Color.Aquamarine, Color.Red, Color.Black));
            
            GeneralUtils.GraphButtons["rotateLeft"].OnPress = () =>
            {
                Canvas.ApplyRotation(true);
                GeneralUtils.GraphButtons["rotateLeft"].IsActive = false;
                return true;
            }; 
            
            // Creates a button
            GeneralUtils.GraphButtons.Add("rotateRight", new FunctionButton("R: ->",
                10 + GeneralUtils.GraphButtons["rotateLeft"].Area.Width + GeneralUtils.GraphButtons["rotateLeft"].Area.X, 
                710, Color.Aquamarine, Color.Red, Color.Black));
            
            GeneralUtils.GraphButtons["rotateRight"].OnPress = () =>
            {
                Canvas.ApplyRotation();
                GeneralUtils.GraphButtons["rotateRight"].IsActive = false;
                return true;
            }; 
            
        }
        
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Verify if the left or right mouse button was pressed
            var currentMouseState = Mouse.GetState();
            GeneralUtils.isLeftButtonPressed = _lastMouseState.LeftButton is ButtonState.Released 
                                              && currentMouseState.LeftButton is ButtonState.Pressed;
            GeneralUtils.isRightButtonPressed = _lastMouseState.RightButton is ButtonState.Released 
                                               && currentMouseState.RightButton is ButtonState.Pressed;

            // Only change the the cursor position if it isn't on any button
            if (GeneralUtils.GraphButtons.Any(x
                => x.Value.Area.Contains(currentMouseState.Position)) == false)
            {
                if (GeneralUtils.isLeftButtonPressed)
                    GeneralUtils.LeftPosition = currentMouseState.Position;

                if (GeneralUtils.isRightButtonPressed)
                    GeneralUtils.RightPosition = currentMouseState.Position;

            }

            // Update the GUI buttons
            GeneralUtils.UpdateButtons(currentMouseState.Position);
            
            // Select the canvas object
            Canvas.SelectCanvasObject(currentMouseState.Position);

            // Update the state of the mouse and keyboard
            _lastMouseState = Mouse.GetState();

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            // Clear the GPU and set the background to white
            GraphicsDevice.Clear(Color.Black);

            // Begin drawing
            _spriteBatch.Begin();

            // Draw the HUD
            _spriteBatch.DrawString(GeneralUtils.Font, $"FPS: {1 / gameTime.ElapsedGameTime.TotalSeconds:00.00}",
                new Vector2(0), Color.White);
            
            _spriteBatch.DrawString(GeneralUtils.Font, $"Linhas desenhadas com: {Canvas.Raster}",
                new Vector2(0, GeneralUtils.Font.MeasureString("FPS").Y), Color.White);
            
            // Draw the GUI
            GeneralUtils.DrawButtons(_spriteBatch);
            
            // Draw the mouse clicks
            GeneralUtils.DrawPixel(_spriteBatch, GeneralUtils.LeftPosition, Color.Blue, 7);
            GeneralUtils.DrawPixel(_spriteBatch, GeneralUtils.RightPosition, Color.Orange, 7);
            
            // Draw the bounding box for the selected object
            if(Canvas.SelectedLine is not null || Canvas.SelectedCircle is not null){
                var outerBoundingBox = Canvas.SelectedLine is not null
                    ? Canvas.SelectedLine.BoundingBox
                    : Canvas.SelectedCircle.BoundingBox;
                _spriteBatch.Draw(GeneralUtils.Pixel, outerBoundingBox, null, Color.White);
                var innerBoundingBox = new Rectangle(outerBoundingBox.Location, outerBoundingBox.Size);
                innerBoundingBox.Inflate(-1, -1);
                _spriteBatch.Draw(GeneralUtils.Pixel, innerBoundingBox, null, Color.Black);
            }
            
            // Draw the lines
            if(Canvas.Raster is EnumRasterAlgorithm.DDA)
                foreach (var canvasLine in Canvas.Lines)
                    DDA.DrawLine(_spriteBatch, canvasLine.Start, canvasLine.End, Color.White);
            else
                foreach (var canvasLine in Canvas.Lines)
                    Bresenham.DrawLine(_spriteBatch, canvasLine.Start, canvasLine.End, Color.Red);
            
            if(Canvas.ClippedLine is not null)
                DDA.DrawLine(_spriteBatch, Canvas.ClippedLine.Start, Canvas.ClippedLine.End, Color.Blue);
                
            // Draw the circles
            foreach (var canvasCircle in Canvas.Circles)
                Bresenham.DrawCircle(_spriteBatch, canvasCircle.Center, canvasCircle.Radius, Color.Crimson);
            
            // End drawing
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}