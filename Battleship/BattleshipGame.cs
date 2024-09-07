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
        /// The grid size
        /// </summary>
        private const int _GRIDSIZE = 11;

        ///<summary>
        /// Player 1 grid offset value.
        /// </summary>
        private const int _PLAYER1OFFSET = 0;

        ///<summary>
        /// Player 2 grid offset value.
        /// </summary>
        private const int _PLAYER2OFFSET = 500;

        /// <summary>
        /// Player 1 grid object.
        /// </summary>
        private Grid _player1grid;

        /// <summary>
        /// Player 2 grid object.
        /// </summary>
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

            _player1grid = new Grid(_GRIDSIZE, _PLAYER1OFFSET);
            _player2grid = new Grid(_GRIDSIZE, _PLAYER2OFFSET);

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
    }
}