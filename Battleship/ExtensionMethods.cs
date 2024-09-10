using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Finds the x and y coordinate of a given value in the matrix.
        /// Returns -1, -1 if the object does not exist within the matrix.
        /// Method obtained from StackOverflow
        /// </summary>
        public static Tuple<int, int> CoordinatesOf<T>(this T[,] matrix, T value)
        {
            int width = matrix.GetLength(0);
            int height = matrix.GetLength(1);

            for (int y = 0; y < width; ++y)
            {
                for (int x = 0; x < height; ++x)
                {
                    if (matrix[y, x]!.Equals(value))
                        return Tuple.Create(x, y);
                }
            }

            return Tuple.Create(-1, -1);
        }
    }
}
