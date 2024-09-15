/*
 *   Module Name: ExtensionMethods.cs
 *   Purpose: This module contains the extension methods for the Battleship game. It contains the CoordinatesOf method which finds the x and y coordinate of a given value in the matrix.
 *   Inputs: None
 *   Output: None
 *   Additional code sources:
 *   Developers: Derek Norton, Ethan Berkley, Jacob Wilkus, Mo Morgan, and Richard Moser
 *   Date: 09/11/2024
 *   Last Modified: 09/14/2024
 */
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
        /// Method obtained from StackOverflow
        /// </summary>
        /// <returns>
        /// Tuple of the x and y coordinate of the object in the matrix or (-1,-1) if the object isn't found in the matrix.
        ///</returns>
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
