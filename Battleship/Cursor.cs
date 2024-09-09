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
        public Texture2D? CursorLeftHalfTexture { get; set; }

        public Texture2D? CursorRightHalfTexture { get; set; }

        public Texture2D? CursorTopHalfTexture { get; set; }

        public Texture2D? CursorBottomHalfTexture { get; set; }

        public Rectangle? CursorStartRectangle { get; set; }

        public Rectangle? CursorEndRectangle { get; set; }

        public CursorOrientation Orientation { get; set; } = CursorOrientation.VERTICAL;

        private Timer? _rotateTimeout;

        public void LoadContent(ContentManager content)
        {
            CursorLeftHalfTexture = content.Load<Texture2D>("cursor_left");
            CursorRightHalfTexture = content.Load<Texture2D>("cursor_right");
            CursorTopHalfTexture = content.Load<Texture2D>("cursor_top");
            CursorBottomHalfTexture = content.Load<Texture2D>("cursor_bottom");
        }


        public void Update(GridTile? currentTile, Tuple<int, int> tileLocation, int shipSize)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.R) && (_rotateTimeout is null || !_rotateTimeout.Enabled))
            {
                FlipOrientation();
                _rotateTimeout = new Timer(100);
                _rotateTimeout.Elapsed += OnTimeoutEvent;
                _rotateTimeout.Start();
            }

            if (currentTile is not null)
            {
                if (Orientation.Equals(CursorOrientation.HORIZONTAL))
                    UpdateRectangles(currentTile.GetLeftHalfLocation(tileLocation.Item1, shipSize),
                                     currentTile.GetRightHalfLocation(tileLocation.Item1, shipSize),
                                     currentTile.GetAdjustedHorizontalSize());
                else
                    UpdateRectangles(currentTile.GetTopHalfLocation(tileLocation.Item2, shipSize),
                                     currentTile.GetBottomHalfLocation(tileLocation.Item2, shipSize),
                                     currentTile.GetAdjustedVerticalSize());
            }
            else if (CursorStartRectangle is not null)
                RemoveCursor();
        }

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


        private void UpdateRectangles(Point startLocation, Point endLocation, Point size)
        {
            if (CursorStartRectangle is null || CursorStartRectangle.Value.X != startLocation.X || CursorStartRectangle.Value.Y != startLocation.Y ||
                CursorEndRectangle is null || CursorEndRectangle.Value.X != endLocation.X || CursorEndRectangle.Value.Y != endLocation.Y)
            {
                CursorStartRectangle = new Rectangle(startLocation, size);
                CursorEndRectangle = new Rectangle(endLocation, size);
            }
        }


        private void RemoveCursor()
        {
            CursorStartRectangle = null;
        }


        private void FlipOrientation()
        {
            if (Orientation.Equals(CursorOrientation.HORIZONTAL))
                Orientation = CursorOrientation.VERTICAL;
            else
                Orientation = CursorOrientation.HORIZONTAL;
        }

        private static void OnTimeoutEvent(object source, ElapsedEventArgs e)
        {
            Timer timer = (Timer)source;
            timer.Dispose();
        }
    }
}
