/*
 *   Module Name: TurnManager.cs
 *   Purpose: This module is the TurnManager class that handles the turn indicator and the logic for switching turns.
 *   Inputs: None
 *   Output: None
 *   Additional code sources: None
 *   Developers: Derek Norton, Ethan Berkley, Jacob Wilkus, Mo Morgan, and Richard Moser
 *   Date: 09/11/2024
 *   Last Modified: 09/15/2024
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace Battleship
{
    /// <summary>
    /// The TurnManager class handles the turn indicator and the logic for switching turns.
    /// </summary>
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
        /// The texture object for the swap indicator.
        /// </summary>
        public Texture2D? SwapTexture { get; set; }

        /// <summary>
        /// If it is currently P1's turn.
        /// </summary>
        public bool IsP1sTurn = true;

        /// <summary>
        /// When turn is swapped, this is true and no ships render at first.
        /// Once player has confirmed they are ready by clicking again, this goes back to false which renders the current players ships and sunken opponent's ships.
        /// </summary>
        public bool SwapWaiting = false;

        /// <summary>
        /// Initializes an instance of the TurnManager class.
        /// The Rectangle is set to the top leftmost corner of the P2's grid.
        /// </summary>  
        public TurnManager() {
            TurnIndicatorRectangle = new Rectangle(new Point(Constants.PLAYER_2_OFFSET, 0), 
                                                   new Point(Constants.SQUARE_SIZE * Constants.SCALE, Constants.SQUARE_SIZE * Constants.SCALE));
        }

        /// <summary>
        /// Load content for the ship manager.
        /// </summary>
        public void LoadContent(ContentManager content)
        {
            // Load the textures for the turn indicators.
            P1Texture = content.Load<Texture2D>("P1");
            P2Texture = content.Load<Texture2D>("P2");
            SwapTexture = content.Load<Texture2D>("swap");
        }

        /// <summary>
        /// Draw for the ship manager.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the indicator for the swap state, if the SwapWaiting flag is set.
            if (SwapWaiting)
                spriteBatch.Draw(SwapTexture, TurnIndicatorRectangle, Color.White);

            // Draw the indicator for the current player's turn.
            else if (IsP1sTurn)
                spriteBatch.Draw(P1Texture, TurnIndicatorRectangle, Color.White);
            else
                spriteBatch.Draw(P2Texture, TurnIndicatorRectangle, Color.White);
        }

        /// <summary>
        /// Toggle's IsP1sTurn.
        /// </summary>
        public void NextTurn()
        {
            // toggle the P1sTurn flag, and set the SwapWaiting flag.
            IsP1sTurn = !IsP1sTurn;
            SwapWaiting = true;

        }
    }
}
