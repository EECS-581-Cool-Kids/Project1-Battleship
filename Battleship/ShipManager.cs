using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Battleship
{
    public class ShipManager
    {
        /// <summary>
        /// The number of pixels for the width and height of each square.
        /// </summary>
        // private const int SQUARE_SIZE = 9;
        private const int SQUARE_SIZE = 16;
        
        /// <summary>
        /// The scale factor between the texture and actual display.
        /// </summary>
        private const int SCALE = 5;

        /// <summary>
        /// The texture for the 1x1 ship.
        /// </summary>
        public Texture2D? ShipTexture1x1 { get; set; }

        /// <summary>
        /// The texture for the 1x2 ship.
        /// </summary>
        public Texture2D? ShipTexture1x2 { get; set; }

        /// <summary>
        /// The texture for the 1x3 ship.
        /// </summary>
        public Texture2D? ShipTexture1x3 { get; set; }

        /// <summary>
        /// The texture for the 1x4 ship.
        /// </summary>
        public Texture2D? ShipTexture1x4 { get; set; }

        /// <summary>
        /// The texture for the 1x5 ship.
        /// </summary>
        public Texture2D? ShipTexture1x5 { get; set; }
        
        /// <summary>
        /// The texture for the 1x1 ship vertical rotation.
        /// </summary>
        public Texture2D? ShipTexture1x1vert { get; set; }
        
        /// <summary>
        /// <summary>
        /// The texture for the 1x2 ship vertical rotation.
        /// </summary>
        public Texture2D? ShipTexture1x2vert { get; set; }
        
        /// <summary>
        /// The texture for the 1x3 ship vertical rotation.
        /// </summary>
        public Texture2D? ShipTexture1x3vert { get; set; }
        
        /// <summary>
        /// The texture for the 1x4 ship vertical rotation.
        /// </summary>
        public Texture2D? ShipTexture1x4vert { get; set; }
        
        /// <summary>
        /// The texture for the 1x5 ship vertical rotation.
        /// </summary>
        public Texture2D? ShipTexture1x5vert { get; set; }

        /// <summary>
        /// The collection of Player 1 ships.
        /// </summary>
        public List<Ship> Player1Ships { get; set; } = new();

        /// <summary>
        /// The collection of Player 2 ships.
        /// </summary>
        public List<Ship> Player2Ships { get; set; } = new();

        /// <summary>
        /// Whether or not the game is in player 1's ship placement mode.
        /// Currently defaults to true for testing.
        /// </summary>
        public bool IsPlayer1Placing { get; set; } = true;

        /// <summary>
        /// Whether or not the game is in player 2's ship placement mode.
        /// Currently defaults to true for testing.
        /// </summary>
        public bool IsPlayer2Placing { get; set; } = false;

        /// <summary>
        /// The number of ships for each player.
        /// </summary>
        public int NumShips { get; set; }

        /// <summary>
        /// Size of the currently selected ship.
        /// </summary>
        public int CurrentShipSize { get; set; } = 1;

        /// <summary>
        /// The timeout for ship placement.
        /// </summary>
        private Timer? _placementTimeout;

        /// <summary>
        /// Event called when a ship is placed in the player 1 grid.
        /// </summary>
        public Action<GridTile, Ship, CursorOrientation>? OnPlayer1ShipPlaced;

        /// <summary>
        /// Event called when a ship is placed in the player 2 grid.
        /// </summary>
        public Action<GridTile, Ship, CursorOrientation>? OnPlayer2ShipPlaced;

        /// <summary>
        /// Event called to check if a position on player 1's board is valid.
        /// </summary>
        public Func<GridTile, int, CursorOrientation, bool>? IsPlayer1PlacementValid;

        /// <summary>
        /// Event called to check if a position on player 2's board is valid.
        /// </summary>
        public Func<GridTile, int, CursorOrientation, bool>? IsPlayer2PlacementValid;

        /// <summary>
        /// Event called when a tile needs adjustment in the player 1 grid.
        /// </summary>
        public Func<GridTile, int, CursorOrientation, GridTile>? OnPlayer1AdjustedTileRequested;

        /// <summary>
        /// Event called when a tile needs adjustment in the player 2 grid.
        /// </summary>
        public Func<GridTile, int, CursorOrientation, GridTile>? OnPlayer2AdjustedTileRequested;

        public ShipManager(int numShips) 
        {
            NumShips = numShips;
        }

        /// <summary>
        /// Load content for the ShipManager.
        /// </summary>
        public void LoadContent(ContentManager content)
        {
            ShipTexture1x1 = content.Load<Texture2D>("ship1x1");
            ShipTexture1x2 = content.Load<Texture2D>("ship1x2");
            ShipTexture1x3 = content.Load<Texture2D>("ship1x3");
            ShipTexture1x4 = content.Load<Texture2D>("ship1x4");
            ShipTexture1x5 = content.Load<Texture2D>("ship1x5");
            ShipTexture1x1vert = content.Load<Texture2D>("ship1x1vert");
            ShipTexture1x2vert = content.Load<Texture2D>("ship1x2vert");
            ShipTexture1x3vert = content.Load<Texture2D>("ship1x3vert");
            ShipTexture1x4vert = content.Load<Texture2D>("ship1x4vert");
            ShipTexture1x5vert = content.Load<Texture2D>("ship1x5vert");
        }

        /// <summary>
        /// Update for the ship manager.
        /// </summary>
        public void UpdateWhilePlacing(GridTile currentTile, CursorOrientation orientation, int playerNum)
        {
            MouseState mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed && (_placementTimeout is null || !_placementTimeout.Enabled))
            {
                if (OnPlayer1AdjustedTileRequested is not null && playerNum == 1)
                    currentTile = OnPlayer1AdjustedTileRequested.Invoke(currentTile, CurrentShipSize, orientation);
                else if (OnPlayer2AdjustedTileRequested is not null && playerNum == 2)
                    currentTile = OnPlayer2AdjustedTileRequested.Invoke(currentTile, CurrentShipSize, orientation);

                bool isShipPlacementValid = playerNum == 1 ? IsPlayer1PlacementValid!.Invoke(currentTile, CurrentShipSize, orientation)
                                                           : IsPlayer2PlacementValid!.Invoke(currentTile, CurrentShipSize, orientation);

                if (!isShipPlacementValid)
                    return;

                Point size;
                if (orientation.Equals(CursorOrientation.HORIZONTAL))
                    size = new Point(SCALE * SQUARE_SIZE * CurrentShipSize, SCALE * SQUARE_SIZE);
                else
                    size = new Point(SCALE * SQUARE_SIZE, SCALE * SQUARE_SIZE * CurrentShipSize);

                Ship ship = new Ship(currentTile.GetLocation(), size, CurrentShipSize);
                currentTile.Ship = ship;

                ship.ShipTexture = CurrentShipSize switch
                {
                    2 => ShipTexture1x2,
                    3 => ShipTexture1x3,
                    4 => ShipTexture1x4,
                    5 => ShipTexture1x5,
                    _ => ShipTexture1x1,
                };

                if (IsPlayer1Placing)
                    Player1Ships.Add(ship);
                else
                    Player2Ships.Add(ship);
                CurrentShipSize++;

                if (CurrentShipSize > NumShips && IsPlayer1Placing)
                {
                    IsPlayer1Placing = false;
                    IsPlayer2Placing = true;
                    CurrentShipSize = 1;
                }
                else if (CurrentShipSize > NumShips && IsPlayer2Placing)
                {
                    IsPlayer2Placing = false;
                }

                if (_placementTimeout is not null)
                {
                    _placementTimeout.Stop();
                    _placementTimeout.Dispose();
                }

                _placementTimeout = new Timer(1000);
                _placementTimeout.Elapsed += OnTimeoutEvent!;
                _placementTimeout.Start();

                if (OnPlayer1ShipPlaced is not null && playerNum == 1)
                    OnPlayer1ShipPlaced.Invoke(currentTile, ship, orientation);
                else if (OnPlayer2ShipPlaced is not null && playerNum == 2)
                    OnPlayer2ShipPlaced.Invoke(currentTile, ship, orientation);
            }
        }

        /// <summary>
        /// Draw for the ship manager.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Ship ship in Player1Ships)
                spriteBatch.Draw(ship.ShipTexture, ship.ShipRectangle, Color.White);
            foreach (Ship ship in Player2Ships)
                spriteBatch.Draw(ship.ShipTexture, ship.ShipRectangle, Color.White);
        }

        /// <summary>
        /// Event called when the placement timer times out.
        /// </summary>
        private static void OnTimeoutEvent(object source, ElapsedEventArgs e)
        {
            Timer timer = (Timer)source;
            timer.Stop();
            timer.Dispose();
        }
    }
}
