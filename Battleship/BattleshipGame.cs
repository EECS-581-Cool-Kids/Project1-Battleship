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

        /// <summary>
        /// Boolean representing if user is in game or in the menu.
        /// </summary>
        private bool inGame = false;

        /// <summary>
        /// Game state object.
        /// </summary>
        private GameState currentGameState;

        /// <summary>
        /// Main menu object.
        /// </summary>
        private Menu menu;

        /// <summary>
        /// Ship selection menu object.
        /// </summary>
        private ShipSelectionMenu shipSelectionMenu;

        //private Postgame postgame;

        /// <summary>
        /// Object containing the font used in menu
        /// </summary>
        private SpriteFont font;

        /// <summary>
        /// Variable containing the number of ships used in game.
        /// </summary>
        public int shipCount;

        public int P1HitLimit;
        public int P2HitLimit;

        SpriteBatch spriteBatch;

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
            _graphics.PreferredBackBufferWidth = Constants.SQUARE_SIZE * Constants.GRID_SIZE * 2 * Constants.SCALE;
            _graphics.PreferredBackBufferHeight = Constants.SQUARE_SIZE * Constants.GRID_SIZE * Constants.SCALE;
            _graphics.ApplyChanges();

            Window.Title = "Battleship";

            _player1grid = new Grid(Constants.GRID_SIZE, Constants.PLAYER_1_OFFSET);
            _player2grid = new Grid(Constants.GRID_SIZE, Constants.PLAYER_2_OFFSET);
            _shipManager = new ShipManager(5);
            _turnManager = new TurnManager();
            // add event handlers
            _shipManager.OnPlayer1ShipPlaced = _player1grid.ShipPlaced;
            _shipManager.OnPlayer2ShipPlaced = _player2grid.ShipPlaced;
            _shipManager.OnPlayer1AdjustedTileRequested = _player1grid.GetAdjustedCurrentTile;
            _shipManager.OnPlayer2AdjustedTileRequested = _player2grid.GetAdjustedCurrentTile;
            _shipManager.IsPlayer1PlacementValid = _player1grid.IsShipPlacementValid;
            _shipManager.IsPlayer2PlacementValid = _player2grid.IsShipPlacementValid;
            _shipManager.OnPlayerChange = _turnManager.NextTurn;

            base.Initialize();
        }

        /// <summary>
        /// Loads all texture content.
        /// Called once at startup.
        /// </summary>
        protected override void LoadContent()
        {
            if (!inGame)
            {
                _spriteBatch = new SpriteBatch(GraphicsDevice);
                font = Content.Load<SpriteFont>("defaultFont");

                // Initialize the main menu and ship selection menu
                menu = new Menu(font);
                shipSelectionMenu = new ShipSelectionMenu(font);
                return;
            }
            _player1grid = new Grid(Constants.GRID_SIZE, Constants.PLAYER_1_OFFSET);
            _player2grid = new Grid(Constants.GRID_SIZE, Constants.PLAYER_2_OFFSET);
            _shipManager = new ShipManager(shipCount);
            _turnManager = new TurnManager();
            // add event handlers
            _shipManager.OnPlayer1ShipPlaced = _player1grid.ShipPlaced;
            _shipManager.OnPlayer2ShipPlaced = _player2grid.ShipPlaced;
            _shipManager.OnPlayer1AdjustedTileRequested = _player1grid.GetAdjustedCurrentTile;
            _shipManager.OnPlayer2AdjustedTileRequested = _player2grid.GetAdjustedCurrentTile;
            _shipManager.IsPlayer1PlacementValid = _player1grid.IsShipPlacementValid;
            _shipManager.IsPlayer2PlacementValid = _player2grid.IsShipPlacementValid;
            _shipManager.OnPlayerChange = _turnManager.NextTurn;
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

            if (!inGame)
            {
                switch (currentGameState)
                {
                    case GameState.MainMenu:
                        menu.Update();
                        if (menu.SelectedState == GameState.ShipSelection)
                        {
                            currentGameState = GameState.ShipSelection;
                        }
                        else if (menu.SelectedState == GameState.Exit)
                        {
                            Exit();
                        }
                        break;

                    case GameState.ShipSelection:
                        shipSelectionMenu.Update();

                        // When "Start Game" is clicked, transition to playing
                        if (shipSelectionMenu.IsSelectionMade)
                        {
                            shipCount = shipSelectionMenu.SelectedShipCount;  // Store the selected ship count
                            P1HitLimit = shipCount * (shipCount + 1) / 2;
                            P2HitLimit = shipCount * (shipCount + 1) / 2;
                            inGame = true;
                            currentGameState = GameState.Playing;  // Transition to the gameplay state
                            base.Initialize();
                        }
                        else if (shipSelectionMenu.back)
                        {
                            currentGameState = GameState.MainMenu; //Transistions back to main menu
                            base.Initialize();
                        }
                        break;

                    case GameState.Playing:
                        // Add your game logic here
                        // When the game is over, reset to main menu

                        break;

                    case GameState.Settings:
                        // Add settings logic here (not implemented)
                        break;

                    case GameState.Exit:

                        Exit();

                        break;
                }

                base.Update(gameTime);
                return;
            }
            if (P1HitLimit == 0 || P2HitLimit == 0)
            {
                inGame = false;
                currentGameState = GameState.MainMenu;
                base.Initialize();
                return;
            }

            

            _player1grid!.Update();
            _player2grid!.Update();

            Tuple<int, int> currentPlayer1TileLocation = _player1grid.GridArray.CoordinatesOf(_player1grid.CurrentTile);
            Tuple<int, int> currentPlayer2TileLocation = _player2grid.GridArray.CoordinatesOf(_player2grid.CurrentTile);

            if (_shipManager!.IsPlayer1Placing && _player1grid.CurrentTile is not null)
                _cursor.UpdateWhilePlacing(_player1grid.CurrentTile, currentPlayer1TileLocation, _shipManager.CurrentShipSize);
            else if (_player1grid.CurrentTile is not null)
                _cursor.UpdateWhilePlaying(_player1grid.CurrentTile, currentPlayer1TileLocation.Item1);

            if (_shipManager!.IsPlayer2Placing && _player2grid.CurrentTile is not null)
                _cursor.UpdateWhilePlacing(_player2grid.CurrentTile, currentPlayer2TileLocation, _shipManager.CurrentShipSize);
            else if (_player2grid.CurrentTile is not null)
                _cursor.UpdateWhilePlaying(_player2grid.CurrentTile, currentPlayer2TileLocation.Item1);

            if (Mouse.GetState().LeftButton == ButtonState.Released)
            {
                _shipManager!.ReadClick = true;
            }
            else if (_turnManager!.SwapWaiting && _shipManager!.ReadClick)
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
            if (!inGame)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);

                _spriteBatch.Begin();

                if (currentGameState == GameState.MainMenu)
                {
                    menu.Draw(_spriteBatch);
                }
                else if (currentGameState == GameState.ShipSelection)
                {
                    shipSelectionMenu.Draw(_spriteBatch);
                }
                else if (currentGameState == GameState.Playing)
                {
                    // Add drawing code for the game here
                }

                _spriteBatch.End();

                base.Draw(gameTime);
                return;
            }
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch!.Begin(samplerState: SamplerState.PointClamp);
            _player1grid!.DrawBackground(_spriteBatch);
            _player2grid!.DrawBackground(_spriteBatch);

            _shipManager!.Draw(_spriteBatch);

            _player1grid!.DrawForeground(_spriteBatch);
            _player2grid!.DrawForeground(_spriteBatch);

            _cursor.Draw(_spriteBatch);
            _turnManager!.Draw(_spriteBatch);
            _spriteBatch!.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Handles shooting logic for the game.
        /// </summary>
        private void HandleShooting()
        {
            if (!inGame)
            {
                return;
            }
            MouseState mouseState = Mouse.GetState();
            if (_shipManager!.ReadClick && mouseState.LeftButton == ButtonState.Pressed)
            {
                _shipManager.ReadClick = false;
                bool? success = false;
                if (_turnManager!.IsP1sTurn)
                {
                    success = _player2grid!.Shoot();
                    if (success == true)
                    {
                        P2HitLimit = P2HitLimit - 1;
                    }
                }
                else
                {
                    success = _player1grid!.Shoot();
                    if (success == true)
                    {
                        P1HitLimit = P1HitLimit - 1;
                    }
                }
                if (success is not null)
                {
                    _turnManager.NextTurn();
                    _shipManager!.HideP1Ships = !_turnManager.IsP1sTurn;
                    _shipManager.HideP2Ships = _turnManager.IsP1sTurn;
                }
                if (P1HitLimit == 0)
                {
                    inGame = false;
                    currentGameState = GameState.MainMenu;
                    base.Initialize();
                }
                else if (P2HitLimit == 0)
                {
                    inGame = false;
                    currentGameState = GameState.MainMenu;
                    base.Initialize();
                }

            }
        }
    }
}