using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Battleship
{
    public class Cursor
    {
        /// <summary>
        /// Texture for the left half of the cursor when it is horizontal.
        /// </summary>
        public Texture2D? CursorLeftHalfTexture { get; set; }

        /// <summary>
        /// Texture for the right half of the cursor when it is horizontal.
        /// </summary>
        public Texture2D? CursorRightHalfTexture { get; set; }

        /// <summary>
        /// Texture for the top half of the cursor when it is vertical.
        /// </summary>
        public Texture2D? CursorTopHalfTexture { get; set; }

        /// <summary>
        /// Texture for the bottom half of the cursor when it is vertical.
        /// </summary>
        public Texture2D? CursorBottomHalfTexture { get; set; }

        /// <summary>
        /// The first rectangle for the cursor, represents the starting edge.
        /// </summary>
        public Rectangle? CursorStartRectangle { get; set; }

        /// <summary>
        /// The second rectangle for the cursor, represents the ending edge.
        /// </summary>
        public Rectangle? CursorEndRectangle { get; set; }

        /// <summary>
        /// The orientation of the cursor.
        /// </summary>
        public CursorOrientation Orientation { get; set; } = CursorOrientation.HORIZONTAL;

        /// <summary>
        /// The timeout for rotating the cursor.
        /// </summary>
        private Timer? _rotateTimeout;

        /// <summary>
        /// LoadContent for the cursor.
        /// </summary>
        public void LoadContent(ContentManager content)
        {
            CursorLeftHalfTexture = content.Load<Texture2D>("cursor_left");
            CursorRightHalfTexture = content.Load<Texture2D>("cursor_right");
            CursorTopHalfTexture = content.Load<Texture2D>("cursor_top");
            CursorBottomHalfTexture = content.Load<Texture2D>("cursor_bottom");
        }

        /// <summary>
        /// Update for the cursor while in ship placement mode
        /// </summary>
        /// <param name="currentTile">The current GridTile the mouse is over.</param>
        /// <param name="tileLocation">The location of the given GridTile in the GridArray</param>
        /// <param name="shipSize">The size of the currently selected ship</param>
        public void UpdateWhilePlacing(GridTile? currentTile, Tuple<int, int> tileLocation, int shipSize)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.R) && (_rotateTimeout is null || !_rotateTimeout.Enabled))
            {
                FlipOrientation();
                _rotateTimeout = new Timer(100);
                _rotateTimeout.Elapsed += OnTimeoutEvent!;
                _rotateTimeout.Start();
            }

            if (currentTile is not null)
            {
                if (Orientation.Equals(CursorOrientation.HORIZONTAL))
                    UpdateRectangles(currentTile.GetCursorLeftHalfLocation(tileLocation.Item1, shipSize),
                                     currentTile.GetCursorRightHalfLocation(tileLocation.Item1, shipSize),
                                     currentTile.GetCursorAdjustedHorizontalSize());
                else
                    UpdateRectangles(currentTile.GetCursorTopHalfLocation(tileLocation.Item2, shipSize),
                                     currentTile.GetCursorBottomHalfLocation(tileLocation.Item2, shipSize),
                                     currentTile.GetCursorAdjustedVerticalSize());
            }
            else if (CursorStartRectangle is not null || CursorEndRectangle is not null)
                RemoveCursor();
        }

        /// <summary>
        /// Updates the cursor while in play mode.
        /// </summary>
        /// <param name="currentTile">The current GridTile the mouse is over.</param>
        /// <param name="tileLocation">The location of currentTile in the GridArray</param>
        public void UpdateWhilePlaying(GridTile? currentTile, int tileLocation)
        {
            if (Orientation.Equals(CursorOrientation.VERTICAL))
                FlipOrientation();

            if (currentTile is not null)
                UpdateRectangles(currentTile.GetCursorLeftHalfLocation(tileLocation, 1),
                                 currentTile.GetCursorRightHalfLocation(tileLocation, 1),
                                 currentTile.GetCursorAdjustedHorizontalSize());
            else if (CursorStartRectangle is not null || CursorEndRectangle is not null)
                RemoveCursor();
        }

        /// <summary>
        /// Draw for the Cursor.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture1;
            Texture2D texture2;
            if (Orientation.Equals(CursorOrientation.HORIZONTAL))
            {
                texture1 = CursorLeftHalfTexture!;
                texture2 = CursorRightHalfTexture!;
            }
            else
            {
                texture1 = CursorTopHalfTexture!;
                texture2 = CursorBottomHalfTexture!;
            }

            if (CursorStartRectangle is not null && CursorEndRectangle is not null)
            {
                spriteBatch.Draw(texture1, (Rectangle)CursorStartRectangle, Color.White);
                spriteBatch.Draw(texture2, (Rectangle)CursorEndRectangle, Color.White);
            }
        }

        /// <summary>
        /// Updates the info for the rectangle objects.
        /// </summary>
        /// <param name="startLocation">The start coordinates for the cursor rectangle.</param>
        /// <param name="endLocation">The end coordinates for the cursor rectangle.</param>
        /// <param name="size">The size of the rectangles.</param>
        private void UpdateRectangles(Point startLocation, Point endLocation, Point size)
        {
            if (CursorStartRectangle is null || CursorStartRectangle.Value.X != startLocation.X || CursorStartRectangle.Value.Y != startLocation.Y ||
                CursorEndRectangle is null || CursorEndRectangle.Value.X != endLocation.X || CursorEndRectangle.Value.Y != endLocation.Y)
            {
                CursorStartRectangle = new Rectangle(startLocation, size);
                CursorEndRectangle = new Rectangle(endLocation, size);
            }
        }

        /// <summary>
        /// Removes the cursor when the mouse is no longer over the board.
        /// </summary>
        private void RemoveCursor()
        {
            CursorStartRectangle = null;
            CursorEndRectangle = null;
        }

        /// <summary>
        /// Flips the orientation of the cursor
        /// </summary>
        private void FlipOrientation()
        {
            Orientation = Orientation.Equals(CursorOrientation.HORIZONTAL) ? CursorOrientation.VERTICAL : CursorOrientation.HORIZONTAL;
        }

        /// <summary>
        /// Event called when the rotate timer times out.
        /// </summary>
        private static void OnTimeoutEvent(object source, ElapsedEventArgs e)
        {
            Timer timer = (Timer)source;
            timer.Dispose();
        }
    }
}
