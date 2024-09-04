using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class ShipManager
    {
        /// <summary>
        /// The texture for the 1x1 ship.
        /// </summary>
        public Texture2D ShipTexture1x1 { get; set; }

        /// <summary>
        /// The texture for the 1x2 ship.
        /// </summary>
        public Texture2D ShipTexture1x2 { get; set; }

        /// <summary>
        /// The texture for the 1x3 ship.
        /// </summary>
        public Texture2D ShipTexture1x3 { get; set; }

        /// <summary>
        /// The texture for the 1x4 ship.
        /// </summary>
        public Texture2D ShipTexture1x4 { get; set; }

        /// <summary>
        /// The texture for the 1x5 ship.
        /// </summary>
        public Texture2D ShipTexture1x5 { get; set; }

        /// <summary>
        /// The collection of Player 1 ships.
        /// </summary>
        public List<Ship> Player1Ships { get; set; } = new();

        /// <summary>
        /// The collection of Player 2 ships.
        /// </summary>
        public List<Ship> Player2Ships { get; set; } = new();

        /// <summary>
        /// Load content for the ShipManager.
        /// </summary>
        public void LoadContent(ContentManager content)
        {
            ShipTexture1x1 = content.Load<Texture2D>("ship1x1");
            ShipTexture1x2 = content.Load<Texture2D>("ship1x2");
            ShipTexture1x3 = content.Load<Texture2D>("ship1x3");
            ShipTexture1x4 = content.Load<Texture2D>("ship1x4");
            ShipTexture1x5 = content.Load<Texture2D>("ship1x5");
        }


        public void Update()
        {

        }


        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
