using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixedTestMonoGameProject
{
    public class Grid
    {
        /// <summary>
        /// The number of pixels for the width and height of each square.
        /// </summary>
        private const int _squareSize = 9;

        /// <summary>
        /// The scale factor between the texture and actual display.
        /// </summary>
        private const int _scale = 5;

        /// <summary>
        /// The 2D Array representing and storing the grid.
        /// </summary>
        public GridTile[,] GridArray { get; set; }

        /// <summary>
        /// The texture used to indicate when the mouse is hovering over a square.
        /// </summary>
        public Texture2D SquareSelectedTexture { get; set; }

        /// <summary>
        /// The texture used to indicate a square was a "miss".
        /// </summary>
        public Texture2D SquareMissedTexture { get; set; }

        /// <summary>
        /// The texture used to indicate a square was a "hit".
        /// </summary>
        public Texture2D SquareHitTexture { get; set; }

        /// <summary>
        /// The size of width and height of the grid.
        /// </summary>
        public int Size { get; set; }

        public Grid(int size)
        {
            // Initialize the 2D Array
            GridArray = new GridTile[size, size];
            Size = size;

            // Initialize each GridTile
            for (int rowNum = 0; rowNum < size; rowNum++)
            {
                for (int colNum = 0; colNum < size; colNum++)
                {
                    Point squarePosition = new Point(colNum * _squareSize * _scale, rowNum * _squareSize * _scale);
                    Point squareSize = new Point(_squareSize * _scale, _squareSize * _scale);

                    GridArray[rowNum, colNum] = new GridTile(squarePosition, squareSize);
                }
            }
        }
    }
}
