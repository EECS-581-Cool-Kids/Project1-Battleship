using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Battleship
{
    public class Grid
    {
        /// <summary>
        /// The number of pixels for the width and height of each square.
        /// </summary>
        private const int SQUARE_SIZE = 9;

        /// <summary>
        /// The scale factor between the texture and actual display.
        /// </summary>
        private const int SCALE = 5;

        /// <summary>
        /// The 2D Array representing and storing the grid.
        /// </summary>
        public GridTile[,] GridArray { get; set; }

        /// <summary>
        /// The texture used to indicate a square was a "miss".
        /// </summary>
        public Texture2D? SquareMissedTexture { get; set; }

        /// <summary>
        /// The texture used to indicate a square was a "hit".
        /// </summary>
        public Texture2D? SquareHitTexture { get; set; }

        /// <summary>
        /// The current tile the mouse is hovering over.
        /// Set in the update loop.
        /// </summary>
        public GridTile? CurrentTile { get; set; }

        /// <summary>
        /// The size of width and height of the grid.
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// The horizontal offset value used for drawing the grid.
        /// </summary>
        private int _offset { get; set; }

        public Grid(int size, int offset)
        {
            // Initialize the 2D Array. 
            GridArray = new GridTile[size, size];
            Size = size;
            _offset = offset;

            // Initialize each GridTile
            for (int rowNum = 0; rowNum < size; rowNum++)
            {
                for (int colNum = 0; colNum < size; colNum++)
                {
                    Point squarePosition = new Point(colNum * SQUARE_SIZE * SCALE + _offset, rowNum * SQUARE_SIZE * SCALE);
                    Point squareSize = new Point(SQUARE_SIZE * SCALE, SQUARE_SIZE * SCALE);

                    GridArray[rowNum, colNum] = new GridTile(squarePosition, squareSize);
                }
            }
        }

        /// <summary>
        /// LoadContent for the grid.
        /// </summary>
        public void LoadContent(ContentManager content)
        {
            GridArray[0, 0].GridTexture = content.Load<Texture2D>("top_corner");

            for (int tileNum = 1; tileNum < Size; tileNum++)
            {
                GridArray[0, tileNum].GridTexture = content.Load<Texture2D>($"column_{tileNum}");
                GridArray[tileNum, 0].GridTexture = content.Load<Texture2D>($"row_{tileNum}");
            }

            for (int colNum = 1; colNum < Size; colNum++)
            {
                for (int rowNum = 1; rowNum < Size; rowNum++)
                {
                    GridArray[colNum, rowNum].GridTexture = content.Load<Texture2D>("square");
                    GridArray[colNum, rowNum].CanSelect = true;
                }
            }

            SquareMissedTexture = content.Load<Texture2D>("square_miss");
            SquareHitTexture = content.Load<Texture2D>("square_hit");
        }

        /// <summary>
        /// Update for the grid while in ship placement mode.
        /// </summary>
        public void Update()
        {
            MouseState mouseState = Mouse.GetState();
            Point mousePoint = new Point(mouseState.X, mouseState.Y);

            // Get which square the mouse is inside of
            foreach (GridTile tile in GridArray)
            {
                if (tile.GridRectangle.Contains(mousePoint) && tile.CanSelect)
                    CurrentTile = tile;
                else if (!tile.GridRectangle.Contains(mousePoint) && (CurrentTile?.Equals(tile) ?? false))
                    CurrentTile = null;
            }
        }

        /// <summary>
        /// Draw for the grid.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (GridTile tile in GridArray)
                spriteBatch.Draw(tile.GridTexture, tile.GridRectangle, Color.White);
        }

        /// <summary>
        /// Handles updating all grid tiles to indicate that a ship has been placed upon it.
        /// </summary>
        /// <param name="tile">The tile that was selected.</param>
        /// <param name="ship">The ship placed.</param>
        /// <param name="orientation">The current cursor orientation.</param>
        public void ShipPlaced(GridTile tile, Ship ship, CursorOrientation orientation)
        {
            Tuple<int, int> currentTileLocation = GridArray.CoordinatesOf(tile);

            for (int tileNum = 1; tileNum < ship.Length; tileNum++)
            {
                GridTile nextTile;
                if (orientation.Equals(CursorOrientation.HORIZONTAL))
                    nextTile = GridArray[currentTileLocation.Item2, currentTileLocation.Item1 + tileNum]; 
                else
                    nextTile = GridArray[currentTileLocation.Item2 + tileNum, currentTileLocation.Item1];

                nextTile.Ship = ship;
            }
        }

        /// <summary>
        /// Returns a new "current tile" based on a required adjustment.
        /// This is for when the ship is being placed on squares close to the edge.
        /// </summary>
        /// <param name="currentTile">The current tile the mouse is over.</param>
        /// <param name="shipLength">The length of the ship being placed.</param>
        /// <param name="orientation">The cursor's orientation</param>
        public GridTile GetAdjustedCurrentTile(GridTile currentTile, int shipLength, CursorOrientation orientation)
        {
            Tuple<int, int> currentTileLocation = GridArray.CoordinatesOf(currentTile);

            if (orientation.Equals(CursorOrientation.HORIZONTAL) && currentTileLocation.Item1 + shipLength >= Size)
                return GridArray[currentTileLocation.Item2, Size - shipLength];
            else if (orientation.Equals(CursorOrientation.VERTICAL) && currentTileLocation.Item2 + shipLength >= Size)
                return GridArray[Size - shipLength, currentTileLocation.Item1];
            else
                return currentTile;
        }

        /// <summary>
        /// Confirms a ship placement is valid.
        /// </summary>
        /// <param name="selectedTile"></param>
        /// <param name="shipLength"></param>
        /// <param name="orientation"></param>
        /// <returns></returns>
        public bool IsShipPlacementValid(GridTile selectedTile, int shipLength, CursorOrientation orientation)
        {
            if (selectedTile.HasShip)
                return false;

            Tuple<int, int> currentTileLocation = GridArray.CoordinatesOf(selectedTile);

            List<GridTile> tiles = new();

            for (int tileNum = 1; tileNum < shipLength; tileNum++)
            {
                if (orientation.Equals(CursorOrientation.HORIZONTAL))
                    tiles.Add(GridArray[currentTileLocation.Item2, currentTileLocation.Item1 + tileNum]);
                else
                    tiles.Add(GridArray[currentTileLocation.Item2 + tileNum, currentTileLocation.Item1]);
            }

            foreach (GridTile tile in tiles)
            {
                if (tile.HasShip)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// This method returns True if the GridTile clicked on is a hit, return False if it is a miss.
        /// Also changes the GridTile texture to show the result of the shot.
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public bool Shoot(Point position)
        {
            // Convert the position to a GridTile.
            // To do this, we need to find the row and column of the GridTile.
            // The column is the X position minus the offset divided by the size of the GridTile.
            // The row is the Y position divided by the size of the GridTile.
            int col = ((int)position.X - _offset) / (SQUARE_SIZE * SCALE);
            int row = position.Y / (SQUARE_SIZE * SCALE);

            // Need to validate row and col to ensure they are within the bounds of the GridArray.
            // This is because the mouse can be clicked outside of the grid. And if that happens without this check, the game will crash.
            // We don't want to throw an exception here, so we just return false if the row or column is out of bounds.
            if (row < 0 || row >= Size || col < 0 || col >= Size)
                return false;

            GridTile targetTile = GridArray[row, col];

            // Only allow shooting on the 10x10 grid.
            if (targetTile.CanSelect)
            {
                if (targetTile.HasShip)
                {
                    targetTile.GridTexture = SquareHitTexture;
                    return true;
                }
                else
                {
                    targetTile.GridTexture = SquareMissedTexture;
                    return false;
                }
            }
            return false; // If the tile is not selectable, return false. Might want to consider throwing an exception or returning null here.
        }
    }
}
