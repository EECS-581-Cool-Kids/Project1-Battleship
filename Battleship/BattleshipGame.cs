/*
 *   Module Name: BattleshipGame.cs
 *   Purpose: This module is the main game class for the Battleship game.
 *            It is responsible for managing all other subordinate manager objects needed to run the game.
 *   Inputs: None
 *   Output: None
 *   Additional code sources:
 *   Developers: Derek Norton, Ethan Berkley, Jacob Wilkus, Mo Morgan, and Richard Moser
 *   Date: 09/11/2024
 *   Last Modified: 09/14/2024
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
    /// <summary>
    /// The main game class for the Battleship game.
    /// Manages all game logic and object management.
    /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleshipGame"/> class.
        ///</summary>
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
            // Set the window size.
            _graphics.IsFullScreen = false; // Set to true to enable fullscreen mode.
            _graphics.PreferredBackBufferWidth = Constants.SQUARE_SIZE * Constants.GRID_SIZE * 2 * Constants.SCALE; // Set the window width to fit two grids. Scaled by Constants.SCALE.
            _graphics.PreferredBackBufferHeight = Constants.SQUARE_SIZE * Constants.GRID_SIZE * Constants.SCALE; // Set the window height to fit one grid. Scaled by Constants.SCALE.
            _graphics.ApplyChanges(); // Apply the changes to the window size.

            Window.Title = "Battleship"; // Set the window title.

            _player1grid = new Grid(Constants.GRID_SIZE, Constants.PLAYER_1_OFFSET); // Initialize the player 1 grid object.
            _player2grid = new Grid(Constants.GRID_SIZE, Constants.PLAYER_2_OFFSET); // Initialize the player 2 grid object.
            _shipManager = new ShipManager(5);  /* Initialize the ship manager with the number of ships.
                                                 * The parameter will eventually be a constant int property whose value
                                                 * is determined by the number of ships chosen at the main menu.
                                                 */

            _turnManager = new TurnManager(); // Initialize the turn manager object.

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
            _spriteBatch = new SpriteBatch(GraphicsDevice); // Initialize the sprite batch object. This gives the ability to draw objects to the screen by accessing the graphics device.

            /* Load the content for the grid objects, cursor, and ship manager objects.
             * The Content property is inherited from the base Game class. It is used to load content from the Content.mgcb file.
             */
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

            // Check if the left mouse button is released. If it is, indicate that the click has been read.
            if (Mouse.GetState().LeftButton == ButtonState.Released) // If the left mouse button is released, set the read click to true.
            {
                _shipManager!.ReadClick = true;
            }
            // Check if the game is waiting for the players to swap turns and if the read click is true.
            // If so, progress the game by acknowledging the turn swap has been completed.
            else if (_turnManager!.SwapWaiting && _shipManager!.ReadClick)
            {
                _shipManager!.ReadClick = false;
                _turnManager.SwapWaiting = false; // Setting this to false ends the turn swap delay.
            }
            else
            {
                // Update the ship manager object while the players are in ship placing mode.
                if (_shipManager!.IsPlayer1Placing && _player1grid.CurrentTile is not null)
                    _shipManager.UpdateWhilePlacing(_player1grid.CurrentTile, _cursor.Orientation, 1);
                if (_shipManager!.IsPlayer2Placing && _player2grid.CurrentTile is not null)
                    _shipManager.UpdateWhilePlacing(_player2grid.CurrentTile, _cursor.Orientation, 2);

                // Handle shooting logic if the players are not in ship placing mode.
                if (!_shipManager.IsPlacingShips)
                {
                    HandleShooting();
                }

            }

            // Hide both players ships if transitioning between player turns 
            // or hide the ships of the player who is not currently taking their turn.
            _shipManager!.HideP1Ships = _turnManager!.SwapWaiting || !_turnManager.IsP1sTurn;
            _shipManager.HideP2Ships = _turnManager!.SwapWaiting || _turnManager.IsP1sTurn;

            base.Update(gameTime); // Ensures the framerwork-level logic in the base class is updated.
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
        
            MouseState mouseState = Mouse.GetState(); // Get the current mouse state.

            // If the left mouse button is pressed and the read click is true, shoot the tile.
            if (_shipManager!.ReadClick && mouseState.LeftButton == ButtonState.Pressed)
            {
                _shipManager.ReadClick = false; // Set the read click to false to prevent multiple shots per click.
                bool? success = false; // This variable will store the result of the shot. Initialized to false.

                // Shoot the tile for the player whose turn it is.
                if (_turnManager!.IsP1sTurn) 
                {
                    success = _player2grid!.Shoot();
                }
                else
                {
                    success = _player1grid!.Shoot();
                }

                // If the shot was valid (a hit or a miss), move to the next turn and hide the ships of the player who is not taking their turn.
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