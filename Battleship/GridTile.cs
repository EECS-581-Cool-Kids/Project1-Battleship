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


        public Point GetAdjustedLocation()
        {
            return new Point(GridRectangle.X - SCALE, GridRectangle.Y - SCALE);
        }


        public Point GetAdjustedSize()
        {
            return new Point((SQUARE_SIZE + 1) * SCALE, (SQUARE_SIZE + 1) * SCALE);
        }


        public Point GetLocation()
        {
            return new Point(GridRectangle.X, GridRectangle.Y);
        }
    }
}
