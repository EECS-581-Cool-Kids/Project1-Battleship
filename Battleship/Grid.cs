using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Battleship
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

        ///<summary>
        ///The number associated with the grid's player.
        ///i.e. 1 for player 1, 2 for player 2.
        /// </summary>
        public int playerNum { get; set; }

        ///<summary>
        /// Static property to keep track of the current player.
        /// Increments by 1 each time a new player is created.
        /// </summary>
        private static int _playerCounter = 0;

        /// <summary>
        /// The 
        /// </summary>
        private Vector2 _position;

        /// <summary>
        /// The previous mouse point from the last update cycle.
        /// </summary>
        private Point _previousMousePoint;

        /// <summary>
        /// Whether or not the mouse has updated.
        /// </summary>
        private bool _mouseUpdated;

        public Grid(int size)
        {
            // Initialize the 2D Array. 
            GridArray = new GridTile[size, size];
            Size = size;
            this.playerNum = Interlocked.Increment(ref _playerCounter); // Increment the player counter and assign the value to playerNum. Thread-safe.
            this._position = this.playerNum == 1 ? new Vector2(0, 0) : new Vector2(500, 0); // Set the position of the grid based on the player number.


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

            SquareSelectedTexture = content.Load<Texture2D>("square_selected");
            SquareMissedTexture = content.Load<Texture2D>("square_miss");
            SquareHitTexture = content.Load<Texture2D>("square_hit");
        }

        /// <summary>
        /// Update for the grid.
        /// </summary>
        public void Update()
        {
            MouseState mouseState = Mouse.GetState();
            Point mousePoint = new Point(mouseState.X - (int)_position.X, mouseState.Y - (int)_position.Y); // Offset the mouse position by the grid's position 
            
            // Skip update loop if the mouse has not moved
            if (mousePoint.X == _previousMousePoint.X && mousePoint.Y == _previousMousePoint.Y)
            {
                _mouseUpdated = false;
                return;
            }
            else
            {
                _mouseUpdated = true;
            }

            // Get which square the mouse is inside of
            foreach (GridTile tile in GridArray)
            {
                if (tile.GridRectangle.Contains(mousePoint) && tile.CanSelect)
                {
                    tile.MouseOver = true;

                    if (mouseState.LeftButton == ButtonState.Pressed && !tile.IsMiss && !tile.IsHit)
                        tile.IsMiss = true;
                    if (mouseState.RightButton == ButtonState.Pressed && !tile.IsMiss && !tile.IsHit)
                        tile.IsHit = true;
                }
                else if (!tile.GridRectangle.Contains(mousePoint) && tile.MouseOver)
                {
                    tile.MouseOver = false;
                }
            }
        }

        /// <summary>
        /// Draw for the grid.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (!_mouseUpdated)
                return;

            foreach (GridTile tile in GridArray)
            {
                Texture2D texture;
                if (tile.IsMiss)
                    texture = SquareMissedTexture;
                else if (tile.IsHit)
                    texture = SquareHitTexture;
                else if (tile.MouseOver)
                    texture = SquareSelectedTexture;
                else
                    texture = tile.GridTexture;

                spriteBatch.Draw(texture, new Rectangle(tile.GridRectangle.X +(int)_position.X, tile.GridRectangle.Y + (int)_position.Y, tile.GridRectangle.Width, tile.GridRectangle.Height), Color.White);
            }
        }
    }
}
