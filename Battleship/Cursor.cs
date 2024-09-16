/*
 *   Module Name: Cursor.cs
 *   Purpose: This module is the Cursor class for the Battleship game. It is responsible for managing the cursor object in the game.
 *   Inputs: None
 *   Output: None
 *   Additional code sources:
 *   Developers: Derek Norton, Ethan Berkley, Jacob Wilkus, Mo Morgan, and Richard Moser
 *   Date: 09/11/2024
 *   Last Modified: 09/14/2024
 */

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
    /// <summary>
    /// <c>Cursor</c> class is used to represent the cursor object in the game.
    /// </summary>
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
        /// Sets the textures for the cursor.
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
            // Rotate the cursor if the R key is pressed and the timer isn't running.
            if (Keyboard.GetState().IsKeyDown(Keys.R) && (_rotateTimeout is null || !_rotateTimeout.Enabled))
            {
                FlipOrientation();
                _rotateTimeout = new Timer(200); // Set the timer to 200ms.
                _rotateTimeout.Elapsed += OnTimeoutEvent!; // Call the OnTimeoutEvent method when the timer times out. Prevents the cursor from rotating more than 1 time per 200ms.
                _rotateTimeout.Start(); // Start the timer
            }

            // Update the rectangles based on the current tile to make the cursor the same size as the ship it is placing.
            // Otherwise, remove the cursor.
            if (currentTile is not null)
            {
                if (Orientation.Equals(CursorOrientation.HORIZONTAL)) // If the cursor is horizontal
                    UpdateRectangles(currentTile.GetCursorLeftHalfLocation(tileLocation.Item1, shipSize), // Update the rectangles based on where the left and right half of the cursor is.
                                     currentTile.GetCursorRightHalfLocation(tileLocation.Item1, shipSize),
                                     currentTile.GetCursorAdjustedHorizontalSize());
                else // Otherwise, do the same for the vertical cursor.
                    UpdateRectangles(currentTile.GetCursorTopHalfLocation(tileLocation.Item2, shipSize),
                                     currentTile.GetCursorBottomHalfLocation(tileLocation.Item2, shipSize),
                                     currentTile.GetCursorAdjustedVerticalSize());
            }

            // Otherwise, if either end of the rectangle is null, remove the cursor.
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
            // If the cursor is vertical, flip it to horizontal. The shot cursor is always horizontal.
            if (Orientation.Equals(CursorOrientation.VERTICAL))
                FlipOrientation();

            // Update the rectangles based on the current tile. The second argument is always 1 for the shot cursor, so that only one tile is highlighted and shot.
            if (currentTile is not null)
                UpdateRectangles(currentTile.GetCursorLeftHalfLocation(tileLocation, 1),
                                 currentTile.GetCursorRightHalfLocation(tileLocation, 1),
                                 currentTile.GetCursorAdjustedHorizontalSize());

            // Otherwise, remove the cursor if either end of the cursor's rectangle is null.
            else if (CursorStartRectangle is not null || CursorEndRectangle is not null)
                RemoveCursor();
        }

        /// <summary>
        /// Draw for the Cursor.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            // Initialize the textures for the cursor halves.
            Texture2D texture1;
            Texture2D texture2;

            // Set the textures based on the orientation of the cursor.
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

            // Draw the cursor if the end rectangles are on the grid.
            if (CursorStartRectangle is not null && CursorEndRectangle is not null)
            {
                spriteBatch.Draw(texture1, (Rectangle)CursorStartRectangle, Color.White);
                spriteBatch.Draw(texture2, (Rectangle)CursorEndRectangle, Color.White);
            }
        }

        /// <summary>
        /// Updates start and end locations for the rectangle objects.
        /// </summary>
        /// <param name="startLocation">The start coordinates for the cursor rectangle.</param>
        /// <param name="endLocation">The end coordinates for the cursor rectangle.</param>
        /// <param name="size">The size of the rectangles.</param>
        private void UpdateRectangles(Point startLocation, Point endLocation, Point size)
        {
            // If the rectangles are null or the start and end locations are different from startLocation and endLocation, update the rectangles.
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
            // Set the start and end rectangles to null, which prevents them from being drawn.
            CursorStartRectangle = null;
            CursorEndRectangle = null;
        }

        /// <summary>
        /// Flips the orientation of the cursor.
        /// </summary>
        private void FlipOrientation()
        {
            Orientation = Orientation.Equals(CursorOrientation.HORIZONTAL) ? CursorOrientation.VERTICAL : CursorOrientation.HORIZONTAL;
        }

        /// <summary>
        /// Event called when the rotate timer times out.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private static void OnTimeoutEvent(object source, ElapsedEventArgs e)
        {
            // Type cast the source of the event to a Timer object and dispose of it. The source will always be a Timer object, since this event is only called when a timer times out.
            // However, coding the logic this way is more robust and prevents potential errors.
            Timer timer = (Timer)source;
            timer.Dispose(); // Dispose of the timer
        }
    }
}
