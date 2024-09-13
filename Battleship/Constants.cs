namespace Battleship
{
    public static class Constants
    {
        /// <summary>
        /// The number of pixels for the width and height of each square.
        /// </summary>
        public const int SQUARE_SIZE = 16;
        
        /// <summary>
        /// The scale factor between the texture and actual display.
        /// </summary>
        public const int SCALE = 5;
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
        public const int PLAYER_2_OFFSET = 880; // should be SQUARE_SIZE * GRID_SIZE * 2 * SCALE, i.e. 16 * 11 * 2 * scale / 2
    }
}