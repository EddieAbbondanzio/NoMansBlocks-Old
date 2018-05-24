using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Voxelated.Utilities {
    /// <summary>
    /// Helper match methods. Only really covers the basics.
    /// </summary>
    public static class MathUtils {
        /// <summary>
        /// Random number generator. Call this instead
        /// of a creating a new one when ever needed.
        /// </summary>
        public static System.Random Random = new System.Random();

        #region Range Checks
        /// <summary>
        /// Checks if the byte value is within the min (inclusive), and
        /// max (exclusive).
        /// </summary>
        /// <param name="min">The minimum limit.</param>
        /// <param name="max">The maximum limit.</param>
        /// <param name="val">The value to test.</param>
        /// <returns>True if the value is within the range.</returns>
        public static bool InRange(byte min, byte max, byte val) {
            return val >= min && val < max;
        }

        /// <summary>
        /// Checks if the int value is within the min (inclusive), and
        /// max (exclusive).
        /// </summary>
        /// <param name="min">The minimum limit.</param>
        /// <param name="max">The maximum limit.</param>
        /// <param name="val">The value to test.</param>
        /// <returns>True if the value is within the range.</returns>
        public static bool InRange(int min, int max, int val) {
            return val >= min && val < max;
        }

        /// <summary>
        /// Checks if the uint value is within the min (inclusive), and
        /// max (exclusive).
        /// </summary>
        /// <param name="min">The minimum limit.</param>
        /// <param name="max">The maximum limit.</param>
        /// <param name="val">The value to test.</param>
        /// <returns>True if the value is within the range.</returns>
        public static bool InRange(uint min, uint max, uint val) {
            return val >= min && val < max;
        }

        /// <summary>
        /// Checks if the float value is within the min (inclusive), and
        /// max (exclusive).
        /// </summary>
        /// <param name="min">The minimum limit.</param>
        /// <param name="max">The maximum limit.</param>
        /// <param name="val">The value to test.</param>
        /// <returns>True if the value is within the range.</returns>
        public static bool InRange(float min, float max, float val) {
            return val >= min && val < max;
        }

        /// <summary>
        /// Checks if the Vect2Int value is within the min (inclusive), and
        /// max (exclusive).
        /// </summary>
        /// <param name="min">The minimum limit.</param>
        /// <param name="max">The maximum limit.</param>
        /// <param name="val">The value to test.</param>
        /// <returns>True if the value is within the range.</returns>
        public static bool InRange(Vect2Int min, Vect2Int max, Vect2Int val) {
            return val.X >= min.X
                && val.X < max.X
                && val.Y >= min.Y
                && val.Y < max.Y;
        }

        /// <summary>
        /// Checks if the Vect3Int value is within the min (inclusive), and
        /// max (exclusive).
        /// </summary>
        /// <param name="min">The minimum limit.</param>
        /// <param name="max">The maximum limit.</param>
        /// <param name="val">The value to test.</param>
        /// <returns>True if the value is within the range.</returns>
        public static bool InRange(Vect3Int min, Vect3Int max, Vect3Int val) {
            return val.X >= min.X
                && val.X < max.X
                && val.Y >= min.Y
                && val.Y < max.Y
                && val.Z >= min.Z
                && val.Z < max.Z;
        }
        #endregion

        #region Clamping
        /// <summary>
        /// Clamps a byte between a max and min value. Minimum
        /// and Maximum are inclusive.
        /// </summary>
        /// <param name="value">The value to clamp</param>
        /// <param name="min">The minimum limit.</param>
        /// <param name="max">The maximum limit.</param>
        /// <returns>The value clamped to it's limits.</returns>
        public static byte Clamp(this byte value, byte min, byte max) {
            //If they passed the parameters in the wrong order, pull the ole' switcharo on em.
            if(min > max) {
                byte temp = min;
                min = max;
                max = temp;
            }

            if(value <= min) {
                return min;
            }
            else if(value > max) {
                return max;
            }
            else {
                return value;
            }
        }

        /// <summary>
        /// Clamps an int between a max and min value.  Minimum
        /// and Maximum are inclusive.
        /// </summary>
        /// <param name="value">The value to clamp</param>
        /// <param name="min">The minimum limit.</param>
        /// <param name="max">The maximum limit.</param>
        /// <returns>The value clamped to it's limits.</returns>
        public static int Clamp(this int value, int min, int max) {
            //If they passed the parameters in the wrong order, pull the ole' switcharo on em.
            if (min > max) {
                int temp = min;
                min = max;
                max = temp;
            }

            if (value <= min) {
                return min;
            }
            else if (value > max) {
                return max;
            }
            else {
                return value;
            }
        }

        /// <summary>
        /// Clamps an float between a max and min value.  Minimum
        /// and Maximum are inclusive.
        /// </summary>
        /// <param name="value">The value to clamp</param>
        /// <param name="min">The minimum limit.</param>
        /// <param name="max">The maximum limit.</param>
        /// <returns>The value clamped to it's limits.</returns>
        public static float Clamp(this float value, float min, float max) {
            //If they passed the parameters in the wrong order, pull the ole' switcharo on em.
            if (min > max) {
                float temp = min;
                min = max;
                max = temp;
            }

            if (value <= min) {
                return min;
            }
            else if (value > max) {
                return max;
            }
            else {
                return value;
            }
        }
        #endregion

        #region Min Tests
        /// <summary>
        /// Returns the smaller of the two bytes.
        /// </summary>
        /// <param name="a">The first byte.</param>
        /// <param name="b">The second byte.</param>
        /// <returns>The smaller of the two values.</returns>
        public static byte Min(byte a, byte b) {
            if(a > b) {
                return b;
            }
            else {
                return a;
            }
        }

        /// <summary>
        /// Returns the smaller of the two ints.
        /// </summary>
        /// <param name="a">The first int.</param>
        /// <param name="b">The second int.</param>
        /// <returns>The smaller of the two values.</returns>
        public static int Min(int a, int b) {
            if (a > b) {
                return b;
            }
            else {
                return a;
            }
        }

        /// <summary>
        /// Returns the smaller of the two floats.
        /// </summary>
        /// <param name="a">The first float.</param>
        /// <param name="b">The second float.</param>
        /// <returns>The smaller of the two values.</returns>
        public static float Min(float a, float b) {
            if (a > b) {
                return b;
            }
            else {
                return a;
            }
        }
        #endregion

        #region Max Tests
        /// <summary>
        /// Returns the larger of the two bytes.
        /// </summary>
        /// <param name="a">The first byte.</param>
        /// <param name="b">The second byte.</param>
        /// <returns>The larger of the two values.</returns>
        public static byte Max(byte a, byte b) {
            if (a > b) {
                return a;
            }
            else {
                return b;
            }
        }

        /// <summary>
        /// Returns the larger of the two int.
        /// </summary>
        /// <param name="a">The first int.</param>
        /// <param name="b">The second int.</param>
        /// <returns>The larger of the two values.</returns>
        public static int Max(int a, int b) {
            if (a > b) {
                return a;
            }
            else {
                return b;
            }
        }

        /// <summary>
        /// Returns the larger of the two float.
        /// </summary>
        /// <param name="a">The first float.</param>
        /// <param name="b">The second float.</param>
        /// <returns>The larger of the two values.</returns>
        public static float Max(float a, float b) {
            if (a > b) {
                return a;
            }
            else {
                return b;
            }
        }
        #endregion
    }
}