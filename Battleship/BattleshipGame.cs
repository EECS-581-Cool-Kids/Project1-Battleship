using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Diagnostics.Tracing;

namespace Battleship
{
    public class BattleshipGame : Game
    {
        /// <summary>
        /// Internal grid object.
        /// The grid size
        /// </summary>
        private const int GRID_SIZE = 11;

        ///<summary>
        /// Player 1 grid offset value.
        /// </summary>
        private const int PLAYER_1_OFFSET = 0;

        ///<summary>
        /// Player 2 grid offset value.
        /// </summary>
        private const int PLAYER_2_OFFSET = 500;

        /// <summary>
        /// The MonoGame Graphics Device Manager.
        /// </summary>
        private GraphicsDeviceManager _graphics;

        /// <summary>
        /// The MonoGame sprit batch object.
        /// </summary>
        private SpriteBatch? _spriteBatch;

        /// <summary>
        /// The player's cursor.
        /// </summary>
        private Cursor _cursor = new();

        /// <summary>
        /// Player 1 grid object.
        /// </summary>
        private Grid? _player1grid;

        /// <summary>
        /// Player 2 grid object.
        /// </summary>
        private Grid? _player2grid;

        /// <summary>
        /// The internal ship manager object.
        /// </summary>
        private ShipManager? _shipManager;

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

            _player1grid = new Grid(GRID_SIZE, PLAYER_1_OFFSET);
            _player2grid = new Grid(GRID_SIZE, PLAYER_2_OFFSET);
            _shipManager = new ShipManager(5);

            // add event handlers
            _shipManager.OnPlayer1ShipPlaced = _player1grid.ShipPlaced;
            _shipManager.OnPlayer1AdjustedTileRequested = _player1grid.GetAdjustedCurrentTile;
            _shipManager.OnPlayer2ShipPlaced = _player2grid.ShipPlaced;
            _shipManager.OnPlayer2AdjustedTileRequested = _player2grid.GetAdjustedCurrentTile;

            base.Initialize();
        }

        /// <summary>
        /// Loads all texture content.
        /// Called once at startup.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _player1grid!.LoadContent(Content);
            _player2grid!.LoadContent(Content);
            _shipManager!.LoadContent(Content);
            _cursor.LoadContent(Content);
        }
        
        /// <summary>
        /// Checks if any game logic has updated. Called constantly in a loop.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _player1grid!.Update();
            _player2grid!.Update();

            Tuple<int, int> currentPlayer1TileLocation = _player1grid.GridArray.CoordinatesOf(_player1grid.CurrentTile);
            Tuple<int, int> currentPlayer2TileLocation = _player1grid.GridArray.CoordinatesOf(_player2grid.CurrentTile);

            if (_shipManager!.IsPlayer1Placing && _player1grid.CurrentTile is not null)
                _cursor.UpdateWhilePlacing(_player1grid.CurrentTile, currentPlayer1TileLocation, _shipManager.CurrentShipSize);
            else if (_player1grid.CurrentTile is not null)
                _cursor.UpdateWhilePlaying(_player1grid.CurrentTile, currentPlayer1TileLocation.Item1);
            
            if (_shipManager!.IsPlayer2Placing && _player2grid.CurrentTile is not null)
                _cursor.UpdateWhilePlacing(_player2grid.CurrentTile, currentPlayer2TileLocation, _shipManager.CurrentShipSize);
            else if (_player2grid.CurrentTile is not null)
                _cursor.UpdateWhilePlaying(_player2grid.CurrentTile, currentPlayer2TileLocation.Item1);

            if (_shipManager!.IsPlayer1Placing && _player1grid.CurrentTile is not null)
                _shipManager.UpdateWhilePlacing(_player1grid.CurrentTile, _cursor.Orientation, 1);
            if (_shipManager!.IsPlayer2Placing && _player2grid.CurrentTile is not null)
                _shipManager.UpdateWhilePlacing(_player2grid.CurrentTile, _cursor.Orientation, 2);

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws objects to the screen. Called constantly in a loop.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch!.Begin(samplerState: SamplerState.PointClamp);
            _player1grid!.Draw(_spriteBatch);
            _player2grid!.Draw(_spriteBatch);
            _cursor.Draw(_spriteBatch);
            _shipManager!.Draw(_spriteBatch);
            _spriteBatch!.End();

            base.Draw(gameTime);
        }
    }
}