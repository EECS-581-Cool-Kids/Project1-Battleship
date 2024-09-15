/*
 *   Module Name: GridTile.cs
 *   Purpose: This module is the GridTile class for the Battleship game. It represents a single tile on the grid.
 *   Inputs: None
 *   Output: None
 *   Additional code sources:
 *   Developers: Derek Norton, Ethan Berkley, Jacob Wilkus, Mo Morgan, and Richard Moser
 *   Date: 09/11/2024
 *   Last Modified: 09/14/2024
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class GridTile
    {
        /// <summary>
        /// The rectangle object that stores the texture.
        /// </summary>
        public Rectangle GridRectangle { get; set; }

        /// <summary>
        /// The texture object for the tile.
        /// </summary>
        public Texture2D? GridTexture { get; set; }

        /// <summary>
        /// The ship over the grid tile.
        /// </summary>
        public Ship? Ship { get; set; }

        /// <summary>
        /// Whether or not the mouse is hovering over the tile.
        /// </summary>
        public bool MouseOver { get; set; } = false;

        /// <summary>
        /// Whether or not the tile can be selected.
        /// Used to ignore the tiles not in the actual 10x10.
        /// </summary>
        public bool CanSelect { get; set; } = false;

        /// <summary>
        /// Whether or not the grid tile has a ship over it.
        /// </summary>
        public bool HasShip
        {
            get
            {
                return Ship is not null;
            }
        }

        /// <summary>
        /// Once the tile has been shot, prevent it from being shot again.
        /// Set by the Grid object that owns this instance.
        /// </summary>
        public bool IsShot { get; set;} = false;

        /// <summary>
        /// Whether or not the grid tile has been hit.
        /// </summary>
        public bool IsHit { get; set; } = false;

        public GridTile(Point location, Point size)
        {
            GridRectangle = new Rectangle(location, size);
        }

        /// <summary>
        /// Gets the location for the left half of a horizontal cursor.
        /// </summary>
        /// <param name="squareCoord">The X coordinate of this tile</param>
        /// <param name="shipSize">The currently selected ship size.</param>
        public Point GetCursorLeftHalfLocation(int squareCoord, int shipSize)
        {
            // validates the ship size. The maximum ship size is 5.
            if (shipSize > 5)
                shipSize = 5;

            int squareAdjust = 0; // a variable to adjust the x coordinate of the cursor

            // this while loop sets the squareAdjust variable to the offset needed to keep the cursor within the grid
            while (squareCoord + shipSize > 11)
            {
                squareAdjust++;
                shipSize--;
            }

            int xAdjust = Constants.SCALE * Constants.SQUARE_SIZE * squareAdjust; // setting the xAdjust variable to the offset needed to keep the cursor within the grid

            return new Point(GridRectangle.X - Constants.SCALE - xAdjust, GridRectangle.Y - Constants.SCALE); // returns the location of the left half of the cursor using the calculated offset.
        }

        /// <summary>
        /// Gets the location for the right half of a horizontal cursor.
        /// </summary>
        /// <param name="squareCoord">The X coordinate of this tile</param>
        /// <param name="shipSize">The currently selected ship size.</param>
        /// <returns></returns>
        public Point GetCursorRightHalfLocation(int squareCoord, int shipSize)
        {
            // validates the ship size. The maximum ship size is 5.
            if (shipSize > 5)
                shipSize = 5;

            // sets the variable that offsets how far from the left cursor the right cursor should be
            // scale factor * pixels per square * ship size + half of the scaled square size - 3 pixels to center it
            int xAdjust = - Constants.SCALE * Constants.SQUARE_SIZE * shipSize + Constants.SCALE * Constants.SQUARE_SIZE / 2 - 3;

            return new Point(GridRectangle.X - Constants.SCALE - xAdjust, GridRectangle.Y - Constants.SCALE);
        }

        /// <summary>
        /// Gets the location for the top half of a vertical cursor.
        /// </summary>
        /// <param name="squareCoord">The Y coordinate of this tile</param>
        /// <param name="shipSize">The currently selected ship size.</param>
        /// <returns></returns>
        public Point GetCursorTopHalfLocation(int squareCoord, int shipSize)
        {
            // validates the ship size. The maximum ship size is 5.
            if (shipSize > 5)
                shipSize = 5; // The ship size will be used to determine where the cursor should be

            int squareAdjust = 0; // a variable to adjust the y coordinate of the cursor, if needed

            // this while loop sets the squareAdjust variable to the offset needed to keep the cursor within the grid
            while (squareCoord + shipSize > 11)
            {
                squareAdjust++; 
                shipSize--;
            }

            int yAdjust = Constants.SCALE * Constants.SQUARE_SIZE * squareAdjust; // how far from the top cursor the bottom cursor should be

            return new Point(GridRectangle.X - Constants.SCALE, GridRectangle.Y - Constants.SCALE - yAdjust); // returns the location of the top half of the cursor using the calculated offset.
        }

        /// <summary>
        /// Gets the location for the bottom half of a vertical cursor.
        /// </summary>
        /// <param name="squareCoord">The Y coordinate of this tile</param>
        /// <param name="shipSize">The currently selected ship size.</param>
        /// <returns></returns>
        public Point GetCursorBottomHalfLocation(int squareCoord, int shipSize)
        {
            // validates the ship size. The maximum ship size is 5.
            if (shipSize > 5)
                shipSize = 5;

            // sets how far from the top cursor the bottom cursor should be
            // scale factor * pixels per square * ship size + half of the scaled square size - 3 pixels to center it
            int yAdjust = - Constants.SCALE * Constants.SQUARE_SIZE * shipSize + Constants.SCALE * Constants.SQUARE_SIZE / 2 - 3;

            return new Point(GridRectangle.X - Constants.SCALE, GridRectangle.Y - Constants.SCALE - yAdjust); // returns the location of the bottom half of the cursor using the calculated offset.
        }

        /// <summary>
        /// Gets the adjusted size for a horizontal cursor.
        /// </summary>
        public Point GetCursorAdjustedHorizontalSize()
        {
            return new Point((Constants.SQUARE_SIZE + 1) * Constants.SCALE / 2, (Constants.SQUARE_SIZE + 1) * Constants.SCALE);
        }

        /// <summary>
        /// Gets the adjusted size for a vertical cursor.
        /// </summary>
        public Point GetCursorAdjustedVerticalSize()
        {
            return new Point((Constants.SQUARE_SIZE + 1) * Constants.SCALE, (Constants.SQUARE_SIZE + 1) * Constants.SCALE / 2);
        }

        /// <summary>
        /// The location of the grid tile.
        /// </summary>
        public Point GetLocation()
        {
            return new Point(GridRectangle.X, GridRectangle.Y);
        }
    }
}
