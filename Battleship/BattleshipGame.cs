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
        /// Two internal grid objects. One for player and one for opponent.
        /// </summary>
        private Grid _playerGrid;
        private Grid _opponentGrid;

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

            _playerGrid = new Grid(11);
            _opponentGrid = new Grid(11);

            base.Initialize();
        }

        /// <summary>
        /// Loads all texture content.
        /// Called once at startup.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load the player grid textures
            LoadGridTextures(_playerGrid, "player");
            LoadGridTextures(_opponentGrid, "opponent");
        }

        /// <summary>
        /// Helper method to load textures for a specific grid.
        /// </summary>
        /// <param name="grid">A grid object.</param>
        /// <param name="gridOwner">The player the grid belongs to.</param>
        private void LoadGridTextures(Grid _grid, string _gridOwner)
        {
            GridTile[,] gridArray = _grid.GridArray;
            gridArray[0, 0].GridTexture = Content.Load<Texture2D>("top_corner");

            for (int tileNum = 1; tileNum < _grid.Size; tileNum++)
            {
                gridArray[0, tileNum].GridTexture = Content.Load<Texture2D>($"column_{tileNum}");
                gridArray[tileNum, 0].GridTexture = Content.Load<Texture2D>($"row_{tileNum}");
            }

            for (int colNum = 1; colNum < _grid.Size; colNum++)
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

            // Handle mouse interaction of player grid
            HandleGridInteraction(_playerGrid, new Vector2(0,0));

            // Handle mouse interaction with opponent grid
            HandleGridInteraction(_opponentGrid, new Vector2(500,0));

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

            // Draw the player grid starting at (50,50) and the opponent grid starting at (550,50)
            DrawGrid(_playerGrid, new Vector2(0, 0));
            DrawGrid(_opponentGrid, new Vector2(500, 0));

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