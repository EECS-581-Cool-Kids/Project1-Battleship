/*
 *   Module Name: BattleshipGame.cs
 *   Purpose: This module is the main game class for the Battleship game.
 *            It is responsible for managing all other subordinate manager objects needed to run the game.
 *   Inputs: None
 *   Output: None
 *   Additional code sources: ChatGPT for getting the size of an array
 *   Developers: Derek Norton, Ethan Berkley, Jacob Wilkus, Mo Morgan, Richard Moser, Michael Oliver, Peter Pham
 *   Date: 09/03/2024
 *   Last Modified: 09/23/2024
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.IO.Pipes;

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

        /// <summary>
        /// Settings menu object.
        /// </summary>
        private SettingsMenu SettingsMenu;

        /// <summary>
        /// Creates object to store the selected difficulty for the AI; Sets the default state to disabled
        /// </summary>
        public DifficultyState selectedDifficulty = DifficultyState.Disabled;

        //private Postgame postgame;

        /// <summary>
        /// Object containing the font used in menu
        /// </summary>
        private SpriteFont font;

        /// <summary>
        /// Variable containing the number of ships used in game.
        /// </summary>
        public int shipCount;

        /// <summary>
        /// The total number of hits needed to sink all ships for player 1.
        ///</summary>
        public int P1HitLimit;

        /// <summary>
        /// The total number of hits needed to sink all ships for player 2.
        ///</summary>
        public int P2HitLimit;

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
        /// Textures and fonts used while switching players - Added by Mikey (Sep 22)
        ///</summary>
        private Texture2D SwapTexture;
        private SpriteFont feedbackFont; 
        private Texture2D backgroundTexture; 

        /// <summary>
        /// Array for holding coords which should be priorited in attacks by the AI
        ///</summary>
        public List<(int,int)> priorityAttacks;

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

            priorityAttacks = new List<(int, int)>(); // Initializes the empty list
            base.Initialize(); // Ensures the framerwork-level logic in the base class is initialized.
        }

        /// <summary>
        /// Loads all texture content.
        /// Called once at startup.
        /// </summary>
        protected override void LoadContent()
        {
            // If the game hasn't started, load the font and content for the main menu and ship selection menu.
            if (!inGame)
            {
                _spriteBatch = new SpriteBatch(GraphicsDevice);
                font = Content.Load<SpriteFont>("defaultFont");

                // Initialize the main menu and ship selection menu
                menu = new Menu(font);
                SettingsMenu = new SettingsMenu(font);
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

            /* Load the content for the grid objects, cursor, and ship manager objects.
             * The Content property is inherited from the base Game class. It is used to load content from the Content.mgcb file.
             */
            _player1grid!.LoadContent(Content);
            _player2grid!.LoadContent(Content);
            _shipManager!.LoadContent(Content);
            _cursor.LoadContent(Content);
            _turnManager!.LoadContent(Content);

            SwapTexture = Content.Load<Texture2D>("swap"); // extra textures for when switching players
            feedbackFont = Content.Load<SpriteFont>("feedbackFont"); 
            backgroundTexture = Content.Load<Texture2D>("clear"); 
            
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

            // If the game hasn't started, update the main menu and ship selection menu.
            if (!inGame)
            {
                switch (currentGameState)
                {
                    // Update the main menu if the current game state is the main menu.
                    case GameState.MainMenu:
                        menu.Update(); // Update the main menu.
                        if (menu.SelectedState == GameState.ShipSelection) // If the "Play Game" button is clicked, transition to ship selection menu.
                        {
                            currentGameState = GameState.ShipSelection; // Transition to the ship selection menu.
                        }
                        else if (menu.SelectedState == GameState.Settings) // If the "Settings" button is clicked, go to the settings menu.
                        {
                            currentGameState = GameState.Settings; // Transition to the settings menu.
                        }
                        else if (menu.SelectedState == GameState.Exit) // If the "Exit" button is clicked, exit the game.
                        {
                            Exit();
                        }
                        break;

                    // Update the ship selection menu if the current game state is the ship selection menu.
                    case GameState.ShipSelection:
                        shipSelectionMenu.Update(); // Update the ship selection menu.

                        // When "Start Game" is clicked, transition to playing
                        if (shipSelectionMenu.IsSelectionMade)
                        {
                            shipCount = shipSelectionMenu.SelectedShipCount;  // Store the selected ship count
                            P1HitLimit = shipCount * (shipCount + 1) / 2; // Calculate the hit limit for player 1. This equation gives the total number of hits needed to sink all ships.
                            P2HitLimit = shipCount * (shipCount + 1) / 2; // Calculate the hit limit for player 2.
                            inGame = true; // Set the game to be in progress. This will skip the main menu and ship selection menu logic from all subsequent calls to Update().
                            currentGameState = GameState.Playing;  // Transition to the gameplay state
                            base.Initialize();
                            _shipManager!.ReadClick = false; // Set the read click to false ensure catching the positive end of the next click.
                        }
                        else if (shipSelectionMenu.back && Mouse.GetState().LeftButton == ButtonState.Released) // If the back button is clicked, return to the main menu.
                        {
                            currentGameState = GameState.MainMenu; // Transition back to main menu
                            base.Initialize();
                        }
                        break;

                    case GameState.Playing:
                        // Add your game logic here
                        // When the game is over, reset to main menu
                        break;

                    // When "Settings" is clicked, transition to the settings menu
                    case GameState.Settings:
                        SettingsMenu.Update();
                        
                        selectedDifficulty = SettingsMenu.SelectedDifficulty; // Updates the global difficulty to what was chosen in the settings menu
                        if (SettingsMenu.back && Mouse.GetState().LeftButton == ButtonState.Released) // If the back button is clicked, return to the main menu.
                        {
                            // Update the game's difficulty based on what was selected within the settings menu when the player returns to the main menu.
                            currentGameState = GameState.MainMenu; // Transition back to main menu
                            base.Initialize();
                        }
                        break;

                    case GameState.Exit:
                        Exit();
                        break;
                }

                base.Update(gameTime);
                return;
            }

            // If the start game button is pressed without choosing a ship count, return to the main menu.
            if (P1HitLimit == 0 || P2HitLimit == 0)
            {
                inGame = false; // reset the inGame variable to false to enable the main menu and ship selection menu to be displayed.
                currentGameState = GameState.MainMenu; // Transition back to the main menu.
                base.Initialize();
                return;
            }

            // Uses system random class to randomly pick grids to place ships for the AI
            Random random = new Random();

            // Update the grid objects
            _player1grid!.Update();
            _player2grid!.Update();

            // Get the current tile location for each player.
            Tuple<int, int> currentPlayer1TileLocation = _player1grid.GridArray.CoordinatesOf(_player1grid.CurrentTile);
            Tuple<int, int> currentPlayer2TileLocation = _player2grid.GridArray.CoordinatesOf(_player2grid.CurrentTile);

            // If the AI is disabled, continue the game as originally coded
            if (selectedDifficulty == DifficultyState.Disabled)
            {

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
            }
            // If the AI is enabled, proceed with randomly placing the ships
            else
            {
                // Update the cursor object depending on if player 1 is placing ships or shooting tiles.
                if (_shipManager!.IsPlayer1Placing && _player1grid.CurrentTile is not null)
                    _cursor.UpdateWhilePlacing(_player1grid.CurrentTile, currentPlayer1TileLocation, _shipManager.CurrentShipSize);
                else if (_player1grid.CurrentTile is not null)
                    _cursor.UpdateWhilePlaying(_player1grid.CurrentTile, currentPlayer1TileLocation.Item1);

                // Update the cursor object depending on if player 2 is placing ships or shooting tiles.
                if (_shipManager!.IsPlayer2Placing)
                {
                    // Gets random X and Y coords and modifies player 2's CurrentTile to the randomly chosen one
                    int gridSize = _player2grid.GridArray.GetLength(0); // Used ChatGPT here to know how to get the size of an array
                    int randomTileX = random.Next(1, gridSize);
                    int randomTileY = random.Next(1, gridSize);

                    _player2grid.CurrentTile = _player2grid.GridArray[randomTileX,randomTileY];
                    // Shows randomized ship placement
                    // _cursor.UpdateWhilePlacing(_player2grid.CurrentTile, currentPlayer2TileLocation, _shipManager.CurrentShipSize);
                    
                    // Changes settings to proceed without needing an input for the AI 
                    _turnManager.SwapWaiting = false;
                    _shipManager!.ReadClick = true;
                }
                else if (_player2grid.CurrentTile is not null)
                    _cursor.UpdateWhilePlaying(_player2grid.CurrentTile, currentPlayer2TileLocation.Item1);

                // Check if the left mouse button is released. If it is, indicate that the click has been read.
                if (Mouse.GetState().LeftButton == ButtonState.Released) // If the left mouse button is released, set the read click to true.
                {
                    _shipManager!.ReadClick = true;
                }
                // Check if the game is waiting for the players to swap turns and if the read click is true.
                // If so, progress the game by acknowledging the turn swap has been completed.
                else if (_turnManager!.SwapWaiting)
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

                _shipManager!.HideP1Ships = _turnManager!.SwapWaiting || !_turnManager.IsP1sTurn;
                _shipManager.HideP2Ships = true;
            }

            base.Update(gameTime); // Ensures the framework-level logic in the base class is updated.
        }

        /// <summary>
        /// Draws objects to the screen. Called constantly in a loop.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        protected override void Draw(GameTime gameTime)
        {
            // If the game hasn't started, draw the main menu and ship selection menu.
            if (!inGame)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue); // Clear the screen with a blue color.
                _spriteBatch!.Begin(); // Begin the sprite batch for drawing.

                if (currentGameState! == GameState.MainMenu)
                {
                    menu.Draw(_spriteBatch);
                }
                else if (currentGameState == GameState.ShipSelection)
                {
                    shipSelectionMenu.Draw(_spriteBatch);
                }
                else if (currentGameState == GameState.Settings)
                {
                    SettingsMenu.Draw(_spriteBatch);
                    SettingsMenu.SelectedDifficulty = selectedDifficulty;
                }

                _spriteBatch.End(); // End the sprite batch for drawing.
                base.Draw(gameTime); // Ensures the framework-level logic in the base class is drawn.
                return;
            }

            // If the game has started, clear the screen and draw the game elements.
            GraphicsDevice.Clear(Color.CornflowerBlue); // Clear the screen with a blue color.
            _spriteBatch!.Begin(samplerState: SamplerState.PointClamp);

            // Draw the grid objects and other elements
            _player1grid!.DrawBackground(_spriteBatch);
            _player2grid!.DrawBackground(_spriteBatch);
            _shipManager!.Draw(_spriteBatch);
            _player1grid!.DrawForeground(_spriteBatch);
            _player2grid!.DrawForeground(_spriteBatch);
            _cursor.Draw(_spriteBatch);
            _turnManager!.Draw(_spriteBatch);

            // Check if a turn swap is waiting and draw the texture and feedback message
            if (_turnManager.SwapWaiting && selectedDifficulty == DifficultyState.Disabled)
            {
                _spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                //_spriteBatch.Draw(SwapTexture, new Vector2((GraphicsDevice.Viewport.Width - SwapTexture.Width) / 2,
                //                                            (GraphicsDevice.Viewport.Height - SwapTexture.Height) / 2), 
                //                Color.White);

                string feedbackMessage = _turnManager.IsP1sTurn ? "Player 2's Turn Finished!\nClick to Switch Player" : "Player 1's Turn Finished!\nClick to Switch Player";
                Vector2 messageSize = feedbackFont.MeasureString(feedbackMessage);
                Vector2 messagePosition = new Vector2((GraphicsDevice.Viewport.Width - messageSize.X) / 2,
                                                    (GraphicsDevice.Viewport.Height - SwapTexture.Height) / 2 + SwapTexture.Height);
                _spriteBatch.DrawString(feedbackFont, feedbackMessage, messagePosition, Color.Red);
            }
            _spriteBatch.End(); // End the sprite batch for drawing.
            base.Draw(gameTime); // Ensures the framework-level logic in the base class is drawn.
        }


        /// <summary>
        /// Handles shooting logic for the game.
        /// </summary>
        private void HandleShooting()
        {
            // If the game is not in progress, return because there's nothing to shoot.
            if (!inGame)
            {
                return;
            }
                
            MouseState mouseState = Mouse.GetState(); // Get the current mouse state.

            // If the left mouse button is pressed and the read click is true, shoot the tile.
            if (selectedDifficulty == DifficultyState.Disabled) // Continues as normal if the AI is disabled
            {
                if (_shipManager!.ReadClick && mouseState.LeftButton == ButtonState.Pressed)
                {
                    _shipManager.ReadClick = false; // Set the read click to false to prevent multiple shots per click.
                    bool? success = false; // This variable will store the result of the shot. Initialized to false.

                    // Shoot the tile for the player whose turn it is.
                    if (_turnManager!.IsP1sTurn)
                    {
                        success = _player2grid!.Shoot();
                        if (success == true)
                        {
                            P2HitLimit = P2HitLimit - 1; // Decrement the hit limit for player 2 if the shot was successful.
                        }
                    }
                    else
                    {
                        success = _player1grid!.Shoot();
                        if (success == true)
                        {
                            P1HitLimit = P1HitLimit - 1; // Decrement the hit limit for player 1 if the shot was successful.
                        }
                    }

                    // If the shot was valid (a hit or a miss), move to the next turn and hide the ships of the player who is not taking their turn.
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
            // Easy difficulty: randomly selects tiles to attack
            else if (selectedDifficulty == DifficultyState.Easy)
            {
                if (_shipManager!.ReadClick && ((mouseState.LeftButton == ButtonState.Pressed) || !_turnManager!.IsP1sTurn))
                {
                    _shipManager.ReadClick = false; // Set the read click to false to prevent multiple shots per click.
                    bool? success = false; // This variable will store the result of the shot. Initialized to false.

                    // Shoot the tile for the player whose turn it is.
                    if (_turnManager!.IsP1sTurn)
                    {
                        success = _player2grid!.Shoot();
                        if (success == true)
                        {
                            P2HitLimit = P2HitLimit - 1; // Decrement the hit limit for player 2 if the shot was successful.
                        }
                    }
                    // If it is the AI's turn to attack:
                    else
                    {
                        // Repeats until a valid tile is randomly selected to be attacked
                        while (true)
                        {
                            Random random = new Random();
                            int gridSize = _player2grid.GridArray.GetLength(0); // Used ChatGPT here to know how to get the size of an array
                            int randomTileX = random.Next(1, gridSize);
                            int randomTileY = random.Next(1, gridSize);   
                            // Updates the AI's attacking tile to that which was randomly chosen
                            _player1grid.CurrentTile = _player1grid.GridArray[randomTileX,randomTileY];

                            success = _player1grid!.Shoot();
                            if (success is not null)
                                break;
                        }
                        if (success == true)
                        {
                            P1HitLimit = P1HitLimit - 1; // Decrement the hit limit for player 1 if the shot was successful.
                        }
                    }

                    // If the shot was valid (a hit or a miss), move to the next turn and hide the ships of the player who is not taking their turn.
                    if (success is not null)
                    {
                        _turnManager.NextTurn();
                        // Reverts changes made in the NextTurn() function so that the user does not need to click to proceed on behalf of the AI
                        _turnManager.SwapWaiting = false;
                        _shipManager.ReadClick = true;
                        _shipManager!.HideP1Ships = !_turnManager.IsP1sTurn;
                        _shipManager.HideP2Ships = _turnManager.IsP1sTurn;
                    }
                    if (P1HitLimit == 0)
                    {
                        inGame = false;
                        // Changes SwapWaiting and ReadClick back once the game ends to avoid glitches when returning to the main menu screen.
                        _turnManager.SwapWaiting = true;
                        _shipManager.ReadClick = false;
                        currentGameState = GameState.MainMenu;
                        base.Initialize();
                    }
                    else if (P2HitLimit == 0)
                    {
                        inGame = false;
                        currentGameState = GameState.MainMenu;
                        _turnManager.SwapWaiting = true;
                        _shipManager.ReadClick = false;
                        base.Initialize();
                    }
                }
            }
            // Medium AI Difficulty: Attacks orthogonal tiles when hitting a ship
            else if (selectedDifficulty == DifficultyState.Medium)
            {
                if (_shipManager!.ReadClick && ((mouseState.LeftButton == ButtonState.Pressed) || !_turnManager!.IsP1sTurn))
                {
                    _shipManager.ReadClick = false; // Set the read click to false to prevent multiple shots per click.
                    bool? success = false; // This variable will store the result of the shot. Initialized to false.

                    // Shoot the tile for the player whose turn it is.
                    if (_turnManager!.IsP1sTurn)
                    {
                        success = _player2grid!.Shoot();
                        if (success == true)
                        {
                            P2HitLimit = P2HitLimit - 1; // Decrement the hit limit for player 2 if the shot was successful.
                        }
                    }
                    // If it is the AI's turn to attack:
                    else
                    {
                        // Repeats until a valid tile is randomly selected to be attacked
                        while (true)
                        {
                            // Randomly generates coords to attack
                            Random random = new Random();
                            int gridSize = _player2grid.GridArray.GetLength(0); // Used ChatGPT here to know how to get the size of an array
                            int tileX = random.Next(1, gridSize);
                            int tileY = random.Next(1, gridSize);   

                            // If priority attacks exist, use their coords and remove the tuple from the list
                            if (priorityAttacks.Count > 0)
                            {
                                var priorityAttack = priorityAttacks[0];
                                tileX = priorityAttack.Item1;
                                tileY = priorityAttack.Item2;
                                priorityAttacks.RemoveAt(0);
                            }
                            // Sets the CurrentTile to the priority attack if it exists, or the random attack if it doesn't
                            _player1grid.CurrentTile = _player1grid.GridArray[tileX,tileY];
                            // if (_player1grid.CurrentTile.HasShip)

                            success = _player1grid!.Shoot();
                            if (success is not null)
                            {
                                // If a ship tile was attacked, add the adjacent tiles to the priority attack list
                                if (success is true)
                                {
                                    if (tileX-1 >= 1)
                                        priorityAttacks.Add((tileX-1, tileY));
                                    if (tileX+1 <= 10)
                                        priorityAttacks.Add((tileX+1, tileY));
                                    if (tileY-1 >= 1)
                                        priorityAttacks.Add((tileX, tileY-1));
                                    if (tileY+1 <= 10)
                                        priorityAttacks.Add((tileX, tileY+1));
                                }
                                break;
                            }
                                
                        }
                        if (success == true)
                        {
                            P1HitLimit = P1HitLimit - 1; // Decrement the hit limit for player 1 if the shot was successful.
                        }
                    }

                    // If the shot was valid (a hit or a miss), move to the next turn and hide the ships of the player who is not taking their turn.
                    if (success is not null)
                    {
                        _turnManager.NextTurn();
                        // Reverts changes made in the NextTurn() function so that the user does not need to click to proceed on behalf of the AI
                        _turnManager.SwapWaiting = false;
                        _shipManager.ReadClick = true;
                        _shipManager!.HideP1Ships = !_turnManager.IsP1sTurn;
                        _shipManager.HideP2Ships = _turnManager.IsP1sTurn;
                    }
                    if (P1HitLimit == 0)
                    {
                        inGame = false;
                        // Changes SwapWaiting and ReadClick back once the game ends to avoid glitches when returning to the main menu screen.
                        _turnManager.SwapWaiting = true;
                        _shipManager.ReadClick = false;
                        currentGameState = GameState.MainMenu;
                        base.Initialize();
                    }
                    else if (P2HitLimit == 0)
                    {
                        inGame = false;
                        currentGameState = GameState.MainMenu;
                        _turnManager.SwapWaiting = true;
                        _shipManager.ReadClick = false;
                        base.Initialize();
                    }
                }
            }
            // Hard Difficulty: Lands a hit every turn
            else if (selectedDifficulty == DifficultyState.Hard)
            {
                if (_shipManager!.ReadClick && ((mouseState.LeftButton == ButtonState.Pressed) || !_turnManager!.IsP1sTurn))
                {
                    _shipManager.ReadClick = false; // Set the read click to false to prevent multiple shots per click.
                    bool? success = false; // This variable will store the result of the shot. Initialized to false.

                    // Shoot the tile for the player whose turn it is.
                    if (_turnManager!.IsP1sTurn)
                    {
                        success = _player2grid!.Shoot();
                        if (success == true)
                        {
                            P2HitLimit = P2HitLimit - 1; // Decrement the hit limit for player 2 if the shot was successful.
                        }
                    }
                    // If it is the AI's turn to attack:
                    else
                    {
                        // Repeats until a valid tile is randomly selected to be attacked
                        while (true)
                        {
                            Random random = new Random();
                            int gridSize = _player2grid.GridArray.GetLength(0); // Used ChatGPT here to know how to get the size of an array
                            int randomTileX = random.Next(1, gridSize);
                            int randomTileY = random.Next(1, gridSize);   
                            // Updates the AI's attacking tile to that which was randomly chosen
                            _player1grid.CurrentTile = _player1grid.GridArray[randomTileX,randomTileY];
                            if (!_player1grid.CurrentTile.HasShip)
                                continue;
                            success = _player1grid!.Shoot();
                            if (success is not null)
                                break;
                        }
                        if (success == true)
                        {
                            P1HitLimit = P1HitLimit - 1; // Decrement the hit limit for player 1 if the shot was successful.
                        }
                    }

                    // If the shot was valid (a hit or a miss), move to the next turn and hide the ships of the player who is not taking their turn.
                    if (success is not null)
                    {
                        _turnManager.NextTurn();
                        // Reverts changes made in the NextTurn() function so that the user does not need to click to proceed on behalf of the AI
                        _turnManager.SwapWaiting = false;
                        _shipManager.ReadClick = true;
                        _shipManager!.HideP1Ships = !_turnManager.IsP1sTurn;
                        _shipManager.HideP2Ships = _turnManager.IsP1sTurn;
                    }
                    if (P1HitLimit == 0)
                    {
                        inGame = false;
                        // Changes SwapWaiting and ReadClick back once the game ends to avoid glitches when returning to the main menu screen.
                        _turnManager.SwapWaiting = true;
                        _shipManager.ReadClick = false;
                        currentGameState = GameState.MainMenu;
                        base.Initialize();
                    }
                    else if (P2HitLimit == 0)
                    {
                        inGame = false;
                        currentGameState = GameState.MainMenu;
                        _turnManager.SwapWaiting = true;
                        _shipManager.ReadClick = false;
                        base.Initialize();
                    }
                }
            }
        }
    }
}