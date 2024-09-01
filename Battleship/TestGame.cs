using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace FixedTestMonoGameProject
{
    public class TestGame : Game
    {
        /// <summary>
        /// The MonoGame Graphics Device Manager.
        /// </summary>
        private GraphicsDeviceManager _graphics;

        /// <summary>
        /// The MonoGame sprit batch object.
        /// </summary>
        private SpriteBatch _spriteBatch;

        /// <summary>
        /// Internal grid object.
        /// </summary>
        private Grid _grid;

        public TestGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Initializes the relevant objects and window. 
        /// Called once at startup.
        /// </summary>
        protected override void Initialize()
        {
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = 495;
            _graphics.PreferredBackBufferHeight = 495;
            _graphics.ApplyChanges();

            Window.Title = "Battleship";

            _grid = new Grid(11);

            base.Initialize();
        }

        /// <summary>
        /// Loads all texture content.
        /// Called once at startup.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load the grid textures
            GridTile[,] gridArray = _grid.GridArray;

            gridArray[0, 0].GridTexture = Content.Load<Texture2D>("top_corner");

            for (int tileNum = 1; tileNum < _grid.Size; tileNum++)
            {
                gridArray[0, tileNum].GridTexture = Content.Load<Texture2D>($"column_{tileNum}");
                gridArray[tileNum, 0].GridTexture = Content.Load<Texture2D>($"row_{tileNum}");
            }

            for (int colNum = 1; colNum <  _grid.Size; colNum++)
            {
                for (int rowNum = 1; rowNum < _grid.Size; rowNum++)
                {
                    gridArray[colNum, rowNum].GridTexture = Content.Load<Texture2D>("square");
                    gridArray[colNum, rowNum].CanSelect = true;
                }
            }

            _grid.SquareSelectedTexture = Content.Load<Texture2D>("square_selected");
            _grid.SquareMissedTexture = Content.Load<Texture2D>("square_miss");
            _grid.SquareHitTexture = Content.Load<Texture2D>("square_hit");
        }

        /// <summary>
        /// Checks if any game logic has updated. Called constantly in a loop.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Get which square the mouse is inside of
            MouseState mouseState = Mouse.GetState();
            Point moustPoint = new Point(mouseState.X, mouseState.Y);
            foreach (GridTile tile in _grid.GridArray)
            {
                if (tile.GridRectangle.Contains(moustPoint) && tile.CanSelect)
                {
                    tile.MouseOver = true;

                    if (mouseState.LeftButton == ButtonState.Pressed && !tile.IsMiss && !tile.IsHit)
                        tile.IsMiss = true;
                    if (mouseState.RightButton == ButtonState.Pressed && !tile.IsMiss && !tile.IsHit)
                        tile.IsHit = true;
                }
                else if (!tile.GridRectangle.Contains(moustPoint) && tile.MouseOver)
                {
                    tile.MouseOver = false;
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws objects to the screen. Called constantly in a loop.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            // Draw the grid
            GridTile[,] gridArray = _grid.GridArray;
            foreach (GridTile tile in gridArray)
            {
                Texture2D texture;
                if (tile.IsMiss)
                    texture = _grid.SquareMissedTexture;
                else if (tile.IsHit)
                    texture = _grid.SquareHitTexture;
                else if (tile.MouseOver)
                    texture = _grid.SquareSelectedTexture;
                else
                    texture = tile.GridTexture;

                _spriteBatch.Draw(texture, tile.GridRectangle, Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
