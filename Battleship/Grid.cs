using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
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
                    Point squarePosition = new Point(colNum * SQUARE_SIZE * SCALE, rowNum * SQUARE_SIZE * SCALE);
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
    }
}
