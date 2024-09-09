using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class Cursor
    {
        public Texture2D? CursorTexture { get; set; }

        public Rectangle? CursorRectangle { get; set; }


        public void LoadContent(ContentManager content)
        {
            CursorTexture = content.Load<Texture2D>("cursor");
        }


        public void Update(Point location, Point size)
        {
            if (CursorRectangle is null || CursorRectangle.Value.X != location.X || CursorRectangle.Value.Y != location.Y)
                CursorRectangle = new Rectangle(location, size);
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            if (CursorRectangle != null)
                spriteBatch.Draw(CursorTexture, (Rectangle)CursorRectangle, Color.White);
        }


        public void RemoveCursor()
        {
            CursorRectangle = null;
        }
    }
}
