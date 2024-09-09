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
        /// The number of pixels for the width and height of each square.
        /// </summary>
        private const int SQUARE_SIZE = 9;

        /// <summary>
        /// The scale factor between the texture and actual display.
        /// </summary>
        private const int SCALE = 5;

        /// <summary>
        /// The rectangle object that stores the texture.
        /// </summary>
        public Rectangle GridRectangle { get; set; }

        /// <summary>
        /// The texture object for the tile.
        /// </summary>
        public Texture2D? GridTexture { get; set; }

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
        /// Whether or not the tile is a "miss".
        /// </summary>
        public bool IsMiss { get; set; } = false;

        /// <summary>
        /// Whether or not the tile is a "hit".
        /// </summary>
        public bool IsHit { get; set; } = false;

        public GridTile(Point location, Point size)
        {
            GridRectangle = new Rectangle(location, size);
        }


        public Point GetLeftHalfLocation(int squareCoord, int shipSize)
        {
            if (shipSize > 5)
                shipSize = 5;

            int squareAdjust = 0;
            while (squareCoord + shipSize > 11)
            {
                squareAdjust++;
                shipSize--;
            }

            int xAdjust = SCALE * SQUARE_SIZE * squareAdjust;

            return new Point(GridRectangle.X - SCALE - xAdjust, GridRectangle.Y - SCALE);
        }


        public Point GetRightHalfLocation(int squareCoord, int shipSize)
        {
            if (shipSize > 5)
                shipSize = 5;

            while (squareCoord + shipSize > 11)
                shipSize--;

            int xPos = GridRectangle.X + (SCALE * 4);
            xPos += (shipSize - 1) * SCALE * SQUARE_SIZE;

            return new Point(xPos, GridRectangle.Y - SCALE);
        }


        public Point GetTopHalfLocation(int squareCoord, int shipSize)
        {
            if (shipSize > 5)
                shipSize = 5;

            int squareAdjust = 0;
            while (squareCoord + shipSize > 11)
            {
                squareAdjust++;
                shipSize--;
            }

            int yAdjust = SCALE * SQUARE_SIZE * squareAdjust;

            return new Point(GridRectangle.X - SCALE, GridRectangle.Y - SCALE - yAdjust);
        }


        public Point GetBottomHalfLocation(int squareCoord, int shipSize)
        {
            if (shipSize > 5)
                shipSize = 5;

            while (squareCoord + shipSize > 11)
                shipSize--;

            int yPos = GridRectangle.Y + (SCALE * 4);
            yPos += (shipSize - 1) * SCALE * SQUARE_SIZE;

            return new Point(GridRectangle.X - SCALE, yPos);
        }


        public Point GetAdjustedHorizontalSize()
        {
            return new Point((SQUARE_SIZE + 1) * SCALE / 2, (SQUARE_SIZE + 1) * SCALE);
        }


        public Point GetAdjustedVerticalSize()
        {
            return new Point((SQUARE_SIZE + 1) * SCALE, (SQUARE_SIZE + 1) * SCALE / 2);
        }


        public Point GetLocation()
        {
            return new Point(GridRectangle.X, GridRectangle.Y);
        }
    }
}
