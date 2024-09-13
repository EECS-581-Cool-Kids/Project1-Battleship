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

        /// <summary>
        /// Whether or not the ship has been placed.
        /// </summary>
        public bool IsPlaced { get; set; }

        ///<summary>
        ///Private backing field for IsSunk
        /// </summary>
        private bool _isSunk = false;

        /// <summary>
        /// Whether or not the ship has been sunk.
        /// </summary>
        public bool IsSunk
        {
            get
            {
                _isSunk = true;
                foreach (GridTile tile in ShipTiles)
                {
                    if (!tile.IsHit)
                    {
                        _isSunk = false;
                        break;
                    } 
                }
                
                return _isSunk;
            }
            set {}
        }

        /// <summary>
        /// A list containing the grid tiles the ship is placed on.
        /// </summary>
        public List<GridTile> ShipTiles { get; set; } = new();

        public Ship(Point position, Point size, int length)
        {
            ShipRectangle = new Rectangle(position, size);
            Length = length;
        }
    }
}
