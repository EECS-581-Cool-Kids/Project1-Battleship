using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class Ship
    {
        /// <summary>
        /// The rectangle object that stores the texture.
        /// </summary>
        public Rectangle ShipRectangle { get; set; }

        /// <summary>
        /// The texture object for the tile.
        /// </summary>
        public Texture2D? ShipTexture { get; set; }

        /// <summary>
        /// The length of the ship.
        /// </summary>
        public int Length { get; set; }

        public Ship(Point position, Point size, int length)
        {
            ShipRectangle = new Rectangle(position, size);
            Length = length;
        }
    }
}
