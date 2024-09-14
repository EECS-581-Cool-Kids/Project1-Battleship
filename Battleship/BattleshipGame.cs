/*   
 *   Module Name: BattleshipGame.cs  
 *   Purpose: This module is the main game class for the Battleship game. 
 *            It is responsible for managing all other subordinate manager objects needed to run the game.Authors: Derek Norton, Ethan Berkley, Jacob Wilkus, Mo Morgan, and Richard Moser
 *   Inputs: None 
 *   Output: None
 *   Additional code sources:
 *   Developers: Derek Norton, Ethan Berkley, Jacob Wilkus, Mo Morgan, and Richard Moser
 *   Date: 09/11/2024
 *   Last Modified: 09/13/2024
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
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

        /// <summary>
        /// The internal turn manager object.
        /// </summary>
        private TurnManager? _turnManager;

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
            // Set the window size. Enable the game in windowed mode.
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = Constants.SQUARE_SIZE * Constants.GRID_SIZE * 2 * Constants.SCALE;
            _graphics.PreferredBackBufferHeight = Constants.SQUARE_SIZE * Constants.GRID_SIZE * Constants.SCALE;
            _graphics.ApplyChanges();

            Window.Title = "Battleship"; // Set the window title.

            _player1grid = new Grid(Constants.GRID_SIZE, Constants.PLAYER_1_OFFSET);
            _player2grid = new Grid(Constants.GRID_SIZE, Constants.PLAYER_2_OFFSET);
            _shipManager = new ShipManager(5);  // Initialize the ship manager with the number of ships.
                                                // The parameter will eventually be a constant int property whose value
            _turnManager = new TurnManager();
            // add the event handlers for ship placement, tile adjustment, and ship placement validation for both players.
            _shipManager.OnPlayer1ShipPlaced = _player1grid.ShipPlaced;
            _shipManager.OnPlayer2ShipPlaced = _player2grid.ShipPlaced;
            _shipManager.OnPlayer1AdjustedTileRequested = _player1grid.GetAdjustedCurrentTile;
            _shipManager.OnPlayer2AdjustedTileRequested = _player2grid.GetAdjustedCurrentTile;
            _shipManager.IsPlayer1PlacementValid = _player1grid.IsShipPlacementValid;
            _shipManager.IsPlayer2PlacementValid = _player2grid.IsShipPlacementValid;
            _shipManager.OnPlayerChange = _turnManager.NextTurn;

            base.Initialize(); // Ensures the framerwork-level logic in the base class is initialized.
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
            _turnManager!.LoadContent(Content);
        }
        
        /// <summary>
        /// Checks if any game logic has updated. Called constantly in a loop.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        protected override void Update(GameTime gameTime)
        {
            // Exit the game if the back button is pressed or the escape key is pressed.
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Update the grid objects.
            _player1grid!.Update();
            _player2grid!.Update();

            // Get the current tile location for each player.
            Tuple<int, int> currentPlayer1TileLocation = _player1grid.GridArray.CoordinatesOf(_player1grid.CurrentTile);
            Tuple<int, int> currentPlayer2TileLocation = _player2grid.GridArray.CoordinatesOf(_player2grid.CurrentTile);

            // Update the cursor object depending on if player 1 is placing ships or shooting tiles.
            if (_shipManager!.IsPlayer1Placing && _player1grid.CurrentTile is not null)
                _cursor.UpdateWhilePlacing(_player1grid.CurrentTile, currentPlayer1TileLocation, _shipManager.CurrentShipSize);
            else if (_player1grid.CurrentTile is not null)
                _cursor.UpdateWhilePlaying(_player1grid.CurrentTile, currentPlayer1TileLocation.Item1);
            
            // Update the cursor object depending on if player 2 is placing ships or shooting tiles.
            if (_shipManager!.IsPlayer2Placing && _player2grid.CurrentTile is not null)
                _cursor.UpdateWhilePlacing(_player2grid.CurrentTile, currentPlayer2TileLocation, _shipManager.CurrentShipSize);
            else if (_player2grid.CurrentTile is not null)
                _cursor.UpdateWhilePlaying(_player2grid.CurrentTile, currentPlayer2TileLocation.Item1);

            if (Mouse.GetState().LeftButton == ButtonState.Released)
            {
                _shipManager!.ReadClick = true;
            } else if (_turnManager!.SwapWaiting && _shipManager!.ReadClick)
            {
                _shipManager!.ReadClick = false;
                _turnManager.SwapWaiting = false;
            }
            else
            {
                if (_shipManager!.IsPlayer1Placing && _player1grid.CurrentTile is not null)
                    _shipManager.UpdateWhilePlacing(_player1grid.CurrentTile, _cursor.Orientation, 1);
                if (_shipManager!.IsPlayer2Placing && _player2grid.CurrentTile is not null)
                    _shipManager.UpdateWhilePlacing(_player2grid.CurrentTile, _cursor.Orientation, 2);

            // Update the ship manager object while the players are in ship placing mode.
            if (_shipManager!.IsPlayer1Placing && _player1grid.CurrentTile is not null)
                _shipManager.UpdateWhilePlacing(_player1grid.CurrentTile, _cursor.Orientation, 1);
            if (_shipManager!.IsPlayer2Placing && _player2grid.CurrentTile is not null)
                _shipManager.UpdateWhilePlacing(_player2grid.CurrentTile, _cursor.Orientation, 2);



                if (!_shipManager.IsPlacingShips)
                {
                    HandleShooting();
                }

            }

            // Hide if (waiting for player swap) or (its p2's turn)
            _shipManager!.HideP1Ships = _turnManager!.SwapWaiting || !_turnManager.IsP1sTurn;
            _shipManager.HideP2Ships = _turnManager!.SwapWaiting || _turnManager.IsP1sTurn;

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws objects to the screen. Called constantly in a loop.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue); // Clear the screen with a blue background color. 

            // Draws the grid objects, cursor, and ship manager objects.
            // The various Draw commands are batched together by being enclosed in a _spriteBatch!.Begin/End() block.
            _spriteBatch!.Begin(samplerState: SamplerState.PointClamp);
            _player1grid!.DrawBackground(_spriteBatch);
            _player2grid!.DrawBackground(_spriteBatch);

            _shipManager!.Draw(_spriteBatch);

            _player1grid!.DrawForeground(_spriteBatch);
            _player2grid!.DrawForeground(_spriteBatch);

            _cursor.Draw(_spriteBatch);
            _turnManager!.Draw(_spriteBatch);
            _spriteBatch!.End();

            base.Draw(gameTime); // Ensures the framerwork-level logic in the base class is drawn.
        }
        /// <summary>
        /// Handles shooting logic for the game.
        /// </summary>
        private void HandleShooting()
        {
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                Point mousePoint = new Point(mouseState.X, mouseState.Y);
                bool? hit = _player2grid!.Shoot(mousePoint);
            }
        }
        /// <summary>
        /// Handles shooting logic for the game.
        /// </summary>
        private void HandleShooting()
        {
            MouseState mouseState = Mouse.GetState();
            if (_shipManager!.ReadClick && mouseState.LeftButton == ButtonState.Pressed)
            {
                _shipManager.ReadClick = false;
                bool? success = false;
                if (_turnManager!.IsP1sTurn)
                {
                    success = _player2grid!.Shoot();
                }
                else
                {
                    success = _player1grid!.Shoot();
                }
                if (success is not null)
                {
                    _turnManager.NextTurn();
                    _shipManager!.HideP1Ships = !_turnManager.IsP1sTurn;
                    _shipManager.HideP2Ships = _turnManager.IsP1sTurn;
                }
            }
        }
    }
}