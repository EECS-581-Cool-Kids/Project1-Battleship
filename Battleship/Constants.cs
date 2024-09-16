/*
 *   Module Name: Constants.cs
 *   Purpose: This module is the Constants class for the Battleship game. It contains all the constant values used in the game.
 *   Inputs: None
 *   Output: None
 *   Additional code sources: None
 *   Developers: Derek Norton, Ethan Berkley, Jacob Wilkus, Mo Morgan, and Richard Moser
 *   Date: 09/11/2024
 *   Last Modified: 09/15/2024
 */

namespace Battleship
{
    /// <summary>
    /// The Constants class contains all the constant values used in the game.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The number of pixels for the width and height of each square.
        /// </summary>
        public const int SQUARE_SIZE = 16;
        
        /// <summary>
        /// The scale factor between the texture and actual display.
        /// </summary>
        public const int SCALE = 4;

        /// <summary>
        /// Internal grid object.
        /// The grid size
        /// </summary>
        public const int GRID_SIZE = 11;

        ///<summary>
        /// Player 1 grid offset value.
        /// </summary>
        public const int PLAYER_1_OFFSET = 0;

        ///<summary>
        /// Player 2 grid offset value.
        /// </summary>
        public const int PLAYER_2_OFFSET = SQUARE_SIZE * GRID_SIZE * SCALE; // should be SQUARE_SIZE * GRID_SIZE * 2 * SCALE, i.e. 16 * 11 * 2 * scale / 2
    }
}