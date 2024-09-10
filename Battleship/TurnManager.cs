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
    public class TurnManager
    {
        /// <summary>
        /// The rectangle object that stores the texture.
        /// </summary>
        public Rectangle TurnIndicatorRectangle { get; set; }

        /// <summary>
        /// The texture object for the P1 indicator.
        /// </summary>
        public Texture2D? P1Texture { get; set; }
        /// <summary>
        /// The texture object for the P2 indicator.
        /// </summary>
        public Texture2D? P2Texture { get; set; }

        /// <summary>
        /// If it is currently P1's turn.
        /// </summary>
        public bool IsP1sTurn = true;


        public TurnManager(Point location, Point size) {
            TurnIndicatorRectangle = new Rectangle(location, size);
        }

        public void LoadContent(ContentManager content)
        {
            P1Texture = content.Load<Texture2D>("P1");
            P2Texture = content.Load<Texture2D>("P2");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsP1sTurn)
            {
                spriteBatch.Draw(P1Texture, TurnIndicatorRectangle, Color.White);
            }
            else
            {
                spriteBatch.Draw(P2Texture, TurnIndicatorRectangle, Color.White);
            }

        }

        /// <summary>
        /// Toggle's IsP1sTurn.
        /// </summary>
        public void NextTurn()
        {
            IsP1sTurn = !IsP1sTurn;
        }
    }
}
