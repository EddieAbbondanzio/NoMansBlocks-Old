using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxelated.Utilities {
    /// <summary>
    /// Class containing various commonly needed array methods.
    /// </summary>
    public static class ArrayUtils {
        #region Insert
        /// <summary>
        /// Copies the source array into the destination array. Ensure destination is long enough to
        /// accept the copy array starting at startIndex or an exception will be thrown.
        /// </summary>
        /// <typeparam name="T">The array type.</typeparam>
        /// <param name="source">The array to copy.</param>
        /// <param name="destination">The array to copy into.</param>
        /// <param name="startIndex">Where in the destination array to start at.</param>
        public static void CopyInto<T>(T[] source, T[] destination, int startIndex = 0) {
            //Double check it will fit first
            if(destination.Length < source.Length + startIndex) {
                throw new ArgumentException("Destination array does not have enough space to insert at index " + startIndex);
            }

            //Copy it in.
            for(int i = 0; i < source.Length; i++) {
                destination[startIndex + i] = source[i];
            }
        }

        /// <summary>
        /// Copies the source list into the destination array. Ensure destination is long enough to
        /// accept the copy array starting at startIndex or an exception will be thrown.
        /// </summary>
        /// <typeparam name="T">The array type.</typeparam>
        /// <param name="source">The list to copy.</param>
        /// <param name="destination">The array to copy into.</param>
        /// <param name="startIndex">Where in the destination array to start at.</param>
        public static void CopyInto<T>(List<T> source, T[] destination, int startIndex = 0) {
            //Double check it will fit first
            if (destination.Length < source.Count + startIndex) {
                throw new ArgumentException("Destination array does not have enough space to insert at index " + startIndex);
            }

            //Copy it in.
            for(int i = 0; i < source.Count; i++) {
                destination[startIndex + i] = source[i];
            }
        }

        /// <summary>
        /// Copies a fixed number of elements from the source array into
        /// the destination array.
        /// </summary>
        /// <typeparam name="T">The type of array.</typeparam>
        /// <param name="source">The array to copy from.</param>
        /// <param name="destination">The array to copy into.</param>
        /// <param name="startIndex">Where to start copying from.</param>
        /// <param name="length">The number of elements to copy.</param>
        public static void CopyInto<T>(T[] source, T[] destination, int startIndex, int length) {
            //Double check source array is even long enough
            if(source.Length < length) {
                throw new ArgumentException("Source array is not large enough!");
            }

            //Double check it will fit first
            if (destination.Length < length + startIndex) {
                throw new ArgumentException("Destination array does not have enough space to insert at index " + startIndex);
            }

            //Copy it in.
            for(int i = 0; i < length; i++) {
                destination[startIndex + i] = source[i];
            }
        }

        /// <summary>
        /// Copies a fixed number of elements from the source list into
        /// the destination array.
        /// </summary>
        /// <typeparam name="T">The type of array.</typeparam>
        /// <param name="source">The list to copy from.</param>
        /// <param name="destination">The array to copy into.</param>
        /// <param name="startIndex">Where to start copying from.</param>
        /// <param name="length">The number of elements to copy.</param>
        public static void CopyInto<T>(List<T> source, T[] destination, int startIndex, int length) {
            //Double check source array is even long enough
            if (source.Count < length) {
                throw new ArgumentException("Source array is not large enough!");
            }

            //Double check it will fit first
            if (destination.Length < length + startIndex) {
                throw new ArgumentException("Destination array does not have enough space to insert at index " + startIndex);
            }

            //Copy the values in
            for (int i = 0; i < length; i++) {
                destination[startIndex + i] = source[i];
            }
        }
        #endregion

        #region Index Checks
        /// <summary>
        /// Checks if a 1d array contains the desired index.
        /// </summary>
        /// <typeparam name="T">The type of array.</typeparam>
        /// <param name="array">The array to check.</param>
        /// <param name="x">The desired index.</param>
        /// <returns>True if the array contains the index [x].</returns>
        public static bool ContainsIndex<T>(this T[] array, int x) {
            return x >= 0 && x < array.GetLength(0);
        }

        /// <summary>
        /// Checks if a 2d array contains the desired index.
        /// </summary>
        /// <typeparam name="T">The type of array.</typeparam>
        /// <param name="array">The array to check.</param>
        /// <param name="x">The x coordinate of the index.</param>
        /// <param name="y">The y coordinate of the index.</param>
        /// <returns>True if the array contains index [x,y].</returns>
        public static bool ContainsIndex<T>(this T[,] array, int x, int y) {
            return x >= 0 && x < array.GetLength(0)
                && y >= 0 && y < array.GetLength(1);
        }

        /// <summary>
        /// Checks if a 3d array contains the desired index.
        /// </summary>
        /// <typeparam name="T">The type of array.</typeparam>
        /// <param name="array">The array to check.</param>
        /// <param name="x">The x coordinate of the index.</param>
        /// <param name="y">The y coordinate of the index.</param>
        /// <param name="z">The z coordinate of the index.</param>
        /// <returns>True if the array contains index [x,y,z].</returns>
        public static bool ContainsIndex<T>(this T[,,] array, int x, int y, int z) {
            return x >= 0 && x < array.GetLength(0)
                && y >= 0 && y < array.GetLength(1)
                && z >= 0 && z < array.GetLength(2);
        }

        /// <summary>
        /// Checks if a 4d array contains the desired index.
        /// </summary>
        /// <typeparam name="T">The type of array.</typeparam>
        /// <param name="array">The array to check.</param>
        /// <param name="x">The x coordinate of the index.</param>
        /// <param name="y">The y coordinate of the index.</param>
        /// <param name="z">The z coordinate of the index.</param>
        /// <param name="d">The d coordinate of the index.</param>
        /// <returns>True if the array contains index [x,y,z,d].</returns>
        public static bool ContainsIndex<T>(this T[,,,] array, int x, int y, int z, int d) {
            return x >= 0 && x < array.GetLength(0)
                && y >= 0 && y < array.GetLength(1)
                && z >= 0 && z < array.GetLength(2)
                && d >= 0 && d < array.GetLength(3);
        }
        #endregion

        #region Extensions
        /// <summary>
        /// Gets a subarray from the inputted array.
        /// </summary>
        /// <typeparam name="T">The type of array.</typeparam>
        /// <param name="data">The array to take the sub array from.</param>
        /// <param name="index">The start index.</param>
        /// <param name="length">How long of a sub array to take.</param>
        /// <returns>The resultant sub array.</returns>
        public static T[] SubArray<T>(this T[] data, int index, int length) {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        /// <summary>
        /// Gets a subarray from the inputted array.
        /// </summary>
        /// <typeparam name="T">The type of array.</typeparam>
        /// <param name="data">The array to take the sub array from.</param>
        /// <param name="index">The start index.</param>
        /// <returns>The resultant sub array.</returns>
        public static T[] SubArray<T>(this T[] data, int index) {
            int length = data.Length - index ;
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        /// <summary>
        /// Convert an array into it's list equivalent.
        /// </summary>
        /// <typeparam name="T">The type of the array.</typeparam>
        /// <param name="data">The array values</param>
        /// <returns>The list collection variant.</returns>
        public static List<T> ToList<T>(this T[] data) {
            List<T> list = new List<T>();
            list.AddRange(data);

            return list;
        }
        #endregion
    }
}