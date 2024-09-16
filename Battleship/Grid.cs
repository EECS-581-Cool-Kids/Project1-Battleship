/*
 *   Module Name: Grid.cs
 *   Purpose: This module is the Grid class for the Battleship game. It represents a grid of tiles that a player places their ships on.
 *   Inputs: None
 *   Output: None
 *   Additional code sources:
 *   Developers: Derek Norton, Ethan Berkley, Jacob Wilkus, Mo Morgan, and Richard Moser
 *   Date: 09/11/2024
 *   Last Modified: 09/14/2024
 */
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size">The size of the grid.</param>
        /// <param name="offset">The horizontal offset value used for drawing the grid.</param>
        public Grid(int size, int offset)
        {
            // Initialize some properties 
            GridArray = new GridTile[size, size]; // Initialize the 2D array that holds the gridtiles
            Size = size; // Set the size of the grid. The grid is always square, so only one size is needed.
            _offset = offset; // Set the horizontal offset value used for drawing the grid.

            // Initialize each GridTile
            for (int rowNum = 0; rowNum < size; rowNum++)
            {
                for (int colNum = 0; colNum < size; colNum++)
                {
                    // Calculate and assign the position and size of the GridTile
                    Point squarePosition = new Point(colNum * Constants.SQUARE_SIZE * Constants.SCALE + _offset, rowNum * Constants.SQUARE_SIZE * Constants.SCALE);
                    Point squareSize = new Point(Constants.SQUARE_SIZE * Constants.SCALE, Constants.SQUARE_SIZE * Constants.SCALE);

                    GridArray[rowNum, colNum] = new GridTile(squarePosition, squareSize); // Create a new GridTile object at the index of the 2D array
                }
            }
        }

        /// <summary>
        /// LoadContent for the grid.
        /// </summary>
        /// <param name="content">The content manager.</param>
        public void LoadContent(ContentManager content)
        {
            GridArray[0, 0].GridTexture = content.Load<Texture2D>("top_corner"); // Load the top left corner texture

            // Load the textures for the top row and left column
            for (int tileNum = 1; tileNum < Size; tileNum++)
            {
                GridArray[0, tileNum].GridTexture = content.Load<Texture2D>($"column_{tileNum}");
                GridArray[tileNum, 0].GridTexture = content.Load<Texture2D>($"row_{tileNum}");
            }

            // Load the textures for the rest of the grid
            for (int colNum = 1; colNum < Size; colNum++)
            {
                for (int rowNum = 1; rowNum < Size; rowNum++)
                {
                    GridArray[colNum, rowNum].GridTexture = content.Load<Texture2D>("square");
                    GridArray[colNum, rowNum].CanSelect = true;
                }
            }

            // Load the textures for the "miss" and "hit" squares
            SquareMissedTexture = content.Load<Texture2D>("square_miss");
            SquareHitTexture = content.Load<Texture2D>("square_hit");
        }

        /// <summary>
        /// Update for the grid while in ship placement mode.
        /// </summary>
        public void Update()
        {
            // Get the current mouse state and position
            MouseState mouseState = Mouse.GetState();
            Point mousePoint = new Point(mouseState.X, mouseState.Y);

            // Get which tile the mouse is inside of
            foreach (GridTile tile in GridArray)
            {
                // If the mouse is inside the tile, set the current tile to that tile, otherwise set it to null
                if (tile.GridRectangle.Contains(mousePoint) && tile.CanSelect)
                    CurrentTile = tile;
                else if (!tile.GridRectangle.Contains(mousePoint) && (CurrentTile?.Equals(tile) ?? false))
                    CurrentTile = null;
            }
        }

        /// <summary>
        /// First draw pass for the grid.
        /// Draws the base grid texture, which happens behinds the ships.
        /// Should be called before ships are drawn.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to draw with.</param>
        public void DrawBackground(SpriteBatch spriteBatch)
        {
            // If the tile is not hit, draw the background grid texture
            foreach (GridTile tile in GridArray)
                if (!tile.IsHit)
                    spriteBatch.Draw(tile.GridTexture, tile.GridRectangle, Color.White);
        }

        /// <summary>
        /// Second draw pass for the grid.
        /// Draws the base grid texture, which happens in front of the ships.
        /// Should be called afer ships are drawn.
        /// </summary>
        public void DrawForeground(SpriteBatch spriteBatch)
        {
            // If the tile is hit, draw the foreground grid texture
            foreach (GridTile tile in GridArray)
                if (tile.IsHit)
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
            Tuple<int, int> currentTileLocation = GridArray.CoordinatesOf(tile); // Get the coordinates of the selected tile

            // Update the grid tiles to indicate that a ship has been placed on them
            for (int tileNum = 0; tileNum < ship.Length; tileNum++)
            {
                GridTile nextTile; // The next tile to be updated

                // If the orientation is horizontal, asign the tile to the right of the current tile.
                // If the orientation is vertical, assign the tile below the current tile.
                if (orientation.Equals(CursorOrientation.HORIZONTAL))
                    nextTile = GridArray[currentTileLocation.Item2, currentTileLocation.Item1 + tileNum];
                else
                    nextTile = GridArray[currentTileLocation.Item2 + tileNum, currentTileLocation.Item1];

                nextTile.Ship = ship; // Assign the ship to the next tile
                ship.ShipTiles.Add(nextTile); // Add the next tile to the ship's list of tiles that it occupies
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
            Tuple<int, int> currentTileLocation = GridArray.CoordinatesOf(currentTile); // Get the coordinates of the current tile

            // If the ship is too close to the edge, adjust the current tile to be the edge tile
            if (orientation.Equals(CursorOrientation.HORIZONTAL) && currentTileLocation.Item1 + shipLength >= Size) // If the ship is too close to the right edge
                return GridArray[currentTileLocation.Item2, Size - shipLength]; // Set the current tile to the right edge tile
            else if (orientation.Equals(CursorOrientation.VERTICAL) && currentTileLocation.Item2 + shipLength >= Size) // If the ship is too close to the bottom edge
                return GridArray[Size - shipLength, currentTileLocation.Item1]; // Set the current tile to the bottom edge tile
            else
                return currentTile; // Otherwise, return the current tile
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
            // If the selected tile already has a ship, return false
            if (selectedTile.HasShip)
                return false;

            Tuple<int, int> currentTileLocation = GridArray.CoordinatesOf(selectedTile); // Get the coordinates of the selected tile

            List<GridTile> tiles = new(); // Create a list of tiles to check if they have ships on them

            // Add the tiles that the ship will occupy to the list depending on the orientation of the cursor
            for (int tileNum = 1; tileNum < shipLength; tileNum++)
            {
                if (orientation.Equals(CursorOrientation.HORIZONTAL))
                    tiles.Add(GridArray[currentTileLocation.Item2, currentTileLocation.Item1 + tileNum]);
                else
                    tiles.Add(GridArray[currentTileLocation.Item2 + tileNum, currentTileLocation.Item1]);
            }

            // Check if any of the tiles have ships on them. If so, return false.
            foreach (GridTile tile in tiles)
            {
                if (tile.HasShip)
                    return false;
            }

            return true; // If no tiles have ships on them, return true
        }

        /// <summary>
        /// This method returns True if the GridTile clicked on is a hit, return False if it is a miss.
        /// Returns null if the tile is not selectable.
        /// Also changes the GridTile texture to show the result of the shot.
        /// </summary>
        /// <returns>
        /// True if the GridTile clicked on is a hit, return False if it is a miss, return null if the current tile has already been shot, is null, or isn't on the 10x10 grid.
        ///</returns>
        public bool? Shoot()
        {
            if (CurrentTile is null)
                return null;

            // Only allow shooting on the 10x10 grid.
            if (!CurrentTile.CanSelect)
                return null;

            // Don't allow shooting the same spot again.
            if (CurrentTile.IsShot)
                return null;
            else
            {
                // Mark the tile as shot and change the texture to show the result of the shot.
                CurrentTile.IsShot = true;
                if (CurrentTile.HasShip)
                {
                    CurrentTile.IsHit = true;
                    CurrentTile.GridTexture = SquareHitTexture;
                    return true;
                }
                else
                {
                    CurrentTile.GridTexture = SquareMissedTexture;
                    return false;
                }
            }
        }
    }
}
