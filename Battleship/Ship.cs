/*
 *   Module Name: Ship.cs
 *   Purpose: This module is the Ship class for the Battleship game. It represents a ship.
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

        /// <summary>
        /// Whether or not the ship has been sunk.
        /// </summary>
        public bool IsSunk
        {
            get
            {
                foreach (GridTile tile in ShipTiles)
                {
                    if (!tile.IsHit)
                    {
                        return false;           
                    } 
                }
                return true;
            }
        }

        /// <summary>
        /// A list containing the grid tiles the ship is placed on.
        /// </summary>
        public List<GridTile> ShipTiles { get; set; } = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="Ship"/> class.
        /// </summary>
        public Ship(Point position, Point size, int length)
        {
            ShipRectangle = new Rectangle(position, size);
            Length = length;
        }
    }
}
