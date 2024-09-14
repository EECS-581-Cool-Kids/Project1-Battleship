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
        /// <returns></returns>
        public Point GetCursorLeftHalfLocation(int squareCoord, int shipSize)
        {
            if (shipSize > 5)
                shipSize = 5;

            int squareAdjust = 0;
            while (squareCoord + shipSize > 11)
            {
                squareAdjust++;
                shipSize--;
            }

            int xAdjust = Constants.SCALE * Constants.SQUARE_SIZE * squareAdjust;

            return new Point(GridRectangle.X - Constants.SCALE - xAdjust, GridRectangle.Y - Constants.SCALE);
        }

        /// <summary>
        /// Gets the location for the right half of a horizontal cursor.
        /// </summary>
        /// <param name="squareCoord">The X coordinate of this tile</param>
        /// <param name="shipSize">The currently selected ship size.</param>
        /// <returns></returns>
        public Point GetCursorRightHalfLocation(int squareCoord, int shipSize)
        {
            if (shipSize > 5)
                shipSize = 5;
            // sets how far from the left cursor the right cursor should be
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
            if (shipSize > 5)
                shipSize = 5;

            int squareAdjust = 0;
            while (squareCoord + shipSize > 11)
            {
                squareAdjust++;
                shipSize--;
            }

            int yAdjust = Constants.SCALE * Constants.SQUARE_SIZE * squareAdjust;

            return new Point(GridRectangle.X - Constants.SCALE, GridRectangle.Y - Constants.SCALE - yAdjust);
        }

        /// <summary>
        /// Gets the location for the bottom half of a vertical cursor.
        /// </summary>
        /// <param name="squareCoord">The Y coordinate of this tile</param>
        /// <param name="shipSize">The currently selected ship size.</param>
        /// <returns></returns>
        public Point GetCursorBottomHalfLocation(int squareCoord, int shipSize)
        {
            if (shipSize > 5)
                shipSize = 5;
            // sets how far from the top cursor the bottom cursor should be
            // scale factor * pixels per square * ship size + half of the scaled square size - 3 pixels to center it
            int yAdjust = - Constants.SCALE * Constants.SQUARE_SIZE * shipSize + Constants.SCALE * Constants.SQUARE_SIZE / 2 - 3;

            return new Point(GridRectangle.X - Constants.SCALE, GridRectangle.Y - Constants.SCALE - yAdjust);
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
