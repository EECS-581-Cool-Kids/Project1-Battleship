using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Battleship
{
    public class BattleshipGame : Game
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
        /// One for each player.
        /// </summary>
        private Grid _player1grid;
        private Grid _player2grid;

        /// <summary>
        /// Constructor for the Battleship game.
        /// </summary>
        public BattleshipGame()
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
            _graphics.PreferredBackBufferWidth = 1000; // Increased width to fit both grids.
            _graphics.PreferredBackBufferHeight = 495;
            _graphics.ApplyChanges();

            Window.Title = "Battleship";

            _player1grid = new Grid(11);
            _player2grid = new Grid(11);

            base.Initialize();
        }

        /// <summary>
        /// Loads all texture content.
        /// Called once at startup.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _player1grid.LoadContent(Content);
            _player2grid.LoadContent(Content);
        }

        /// <summary>
        /// Checks if any game logic has updated. Called constantly in a loop.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _player1grid.Update();
            _player2grid.Update();

            base.Update(gameTime);
        }
        /// <summary>
        /// Helper method to handle mouse interaction with a grid using the grid's position offset.
        /// </summary>
        /// <param name="_grid">A grid object.</param>
        private void HandleGridInteraction(Grid _grid, Vector2 _position)
        {
            // Get which square the mouse is inside of
            MouseState mouseState = Mouse.GetState();
            Point moustPoint = new Point(mouseState.X - (int)_position.X, mouseState.Y - (int)_position.Y); // Offset the mouse position by the grid's position
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
        }

        /// <summary>
        /// Draws objects to the screen. Called constantly in a loop.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _player1grid.Draw(_spriteBatch);
            _player2grid.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
        /// <summary>
        ///  Helper method to draw a grid at a specified position.
        /// </summary>
        /// <param name="_grid"></param>
        /// <param name="postition"></param>
        private void DrawGrid(Grid _grid, Vector2 postition)
        {
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

                // Draw each tile, offset by the grid's position
                _spriteBatch.Draw(texture, new Rectangle(tile.GridRectangle.X + (int)postition.X, tile.GridRectangle.Y + (int)postition.Y, tile.GridRectangle.Width, tile.GridRectangle.Height), Color.White);
            }
        }
    }
}