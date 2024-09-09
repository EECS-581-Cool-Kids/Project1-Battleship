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
        /// Internal grid object.
        /// </summary>
        private Grid? _grid;

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
            _graphics.PreferredBackBufferWidth = 495;
            _graphics.PreferredBackBufferHeight = 495;
            _graphics.ApplyChanges();

            Window.Title = "Battleship";

            _grid = new Grid(11);
            _shipManager = new ShipManager(5);

            base.Initialize();
        }

        /// <summary>
        /// Loads all texture content.
        /// Called once at startup.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _grid!.LoadContent(Content);
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

            _grid!.Update();

            Tuple<int, int> currentTileLocation = _grid.GridArray.CoordinatesOf(_grid.CurrentTile);
            if (_shipManager!.IsShipPlacementMode)
                _cursor.UpdateWhilePlacing(_grid.CurrentTile, currentTileLocation, _shipManager.CurrentShipSize);
            else
                _cursor.UpdateWhilePlaying(_grid.CurrentTile, currentTileLocation.Item1);

            if (_shipManager!.IsShipPlacementMode && _grid.CurrentTile is not null)
                _shipManager.UpdateWhilePlacing(_grid.CurrentTile, _cursor.Orientation);

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
            _grid!.Draw(_spriteBatch);
            _cursor.Draw(_spriteBatch);
            _shipManager!.Draw(_spriteBatch);
            _spriteBatch!.End();

            base.Draw(gameTime);
        }
    }
}
