using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxelated.Serialization;
using Voxelated.Utilities;

namespace Voxelated {
    /// <summary>
    /// Integer version of Unitys Vector2. Provides
    /// some operator overloads and conversions.
    /// </summary>
    public struct Vect2Int {
        #region Static Instances
        /// <summary>
        /// Unit Vect2Int in the East direction.
        /// </summary>
        public static Vect2Int East = new Vect2Int(1, 0);

        /// <summary>
        /// Unit Vect2Int in the West direction.
        /// </summary>
        public static Vect2Int West = new Vect2Int(-1, 0);

        /// <summary>
        /// Unit Vect2Int in the Up direction.
        /// </summary>
        public static Vect2Int Up = new Vect2Int(0, 1);

        /// <summary>
        /// Unit Vect2Int in the Down direction.
        /// </summary>
        public static Vect2Int Down = new Vect2Int(0, -1);

        /// <summary>
        /// Vect2Int with an X and Y component of 1.
        /// </summary>
        public static Vect2Int One = new Vect2Int(1, 1);

        /// <summary>
        /// Vect2Int with an X and Y component of 0.
        /// </summary>
        public static Vect2Int Zero = new Vect2Int(0, 0);
        #endregion

        #region
        /// <summary>
        /// The X component.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// The Y component.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Array access to the coordinate components
        /// of the Vect2Int.
        /// </summary>
        /// <param name="i">The component index.</param>
        /// <returns>The desired component value.</returns>
        public int this[int i] {
            get {
                switch (i) {
                    case 0:
                        return X;
                    case 1:
                        return Y;
                    default:
                        throw new Exception("Vector2Int. Invalid component access.");
                }
            }
            set {
                switch (i) {
                    case 0:
                        X = value;
                        break;
                    case 1:
                        Y = value;
                        break;
                    default:
                        throw new Exception("Vector2Int. Invalid component set.");
                }
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new Vect2Int with the respective
        /// values for it's two components
        /// </summary>
        /// <param name="x">The x coordinate</param>
        /// <param name="y">The y coordinate</param>
        public Vect2Int(int x, int y) {
            this.X = x;
            this.Y = y;
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Checks if two Vect2Ints are equivalent.
        /// </summary>
        /// <param name="obj">The other Vect2Int.</param>
        /// <returns>True if both Xs, and Ys are the same.</returns>
        public override bool Equals(object obj) {
            if (!(obj is Vect2Int)) {
                return false;
            }

            Vect2Int other = (Vect2Int)obj;
            return X == other.X && Y == other.Y;
        }

        /// <summary>
        /// Generate a unique hash code based off the components
        /// of the Vect2Int.
        /// </summary>
        /// <returns>The hashcode.</returns>
        public override int GetHashCode() {
            int hash = 23;
            hash = hash * 31 + X;
            hash = hash * 31 + Y;

            return hash;
        }
        #endregion

        #region Operator Overloads
        /// <summary>
        /// Cast a Vect2Int into a Unity Vector2.
        /// </summary>
        /// <param name="pos">The converted Vector2.</param>
        public static explicit operator Vector2(Vect2Int pos) {
            return pos.ToVector2();
        }

        /// <summary>
        /// Convert into a Unity float based Vector.
        /// </summary>
        /// <param name="pos">The converted Vector3.</param>
        public static explicit operator Vector3(Vect2Int pos) {
            return new Vector3(pos.X, pos.Y, 0.0f);
        }

        /// <summary>
        /// Convert into a Vect3Int equivalent vector
        /// where Z is set to 0.
        /// </summary>
        /// <param name="pos">The converted Vect3Int.</param>
        public static explicit operator Vect3Int(Vect2Int pos) {
            return new Vect3Int(pos.X, pos.Y, 0);
        }

        /// <summary>
        /// Convert Unity's int based Vector 2 into this format.
        /// </summary>
        /// <param name="pos">The converted position.</param>
        public static explicit operator Vect2Int(Vector2Int pos) {
            return new Vect2Int(pos.x, pos.y);
        }

        /// <summary>
        /// Test if two Vect2Ints are equal. (If X == X, and Y == Y).
        /// </summary>
        /// <param name="posA">The first Vect2Int.</param>
        /// <param name="posB">The second Vect2Int.</param>
        /// <returns>True if the Vect2Ints are the same.</returns>
        public static bool operator ==(Vect2Int posA, Vect2Int posB) {
            return posA.Equals(posB);
        }

        /// <summary>
        /// Test if two Vect2Ints are not equivalent.
        /// If the X components do not match, and or the Y components
        /// do not match.
        /// </summary>
        /// <param name="posA">The first Vect2Int.</param>
        /// <param name="posB">The second Vect2Int.</param>
        /// <returns>True if the Vect2Ints are different.</returns>
        public static bool operator !=(Vect2Int posA, Vect2Int posB) {
            return !posA.Equals(posB);
        }

        /// <summary>
        /// Add two Vect2Ints together. This does a.X + b.X, and
        /// a.Y + b.Y.
        /// </summary>
        /// <param name="posA">The first Vect2Int.</param>
        /// <param name="posB">The second Vect2Int.</param>
        /// <returns>The resultant Vect2Int of the two added.</returns>
        public static Vect2Int operator +(Vect2Int posA, Vect2Int posB) {
            posA.X += posB.X;
            posA.Y += posB.Y;

            return posA;
        }

        /// <summary>
        /// Subtract posB from posA. In the manner of posA.X - posB.X,
        /// and posA.Y - posB.y.
        /// </summary>
        /// <param name="posA">The first Vect2Int.</param>
        /// <param name="posB">The second Vect2Int.</param>
        /// <returns>The resultant Vect2Int of posB subtracted from posA.</returns>
        public static Vect2Int operator -(Vect2Int posA, Vect2Int posB) {
            posA.X -= posB.X;
            posA.Y -= posB.Y;

            return posA;
        }

        /// <summary>
        /// Multiply the Vect2Int by a scalar. This multiplies
        /// both components and returns the result.
        /// </summary>
        /// <param name="pos">The Vect2Int to multiply.</param>
        /// <param name="scalar">The scalar to multiply by.</param>
        /// <returns>The resultant of the position being multiplied by the scalar.</returns>
        public static Vect2Int operator *(Vect2Int pos, int scalar) {
            pos.X *= scalar;
            pos.Y *= scalar;

            return pos;
        }

        /// <summary>
        /// Divide the Vect2Int by the scalar. This divides it from
        /// both components.
        /// </summary>
        /// <param name="pos">The Vect2Int to divide.</param>
        /// <param name="scalar">The scalar to divide by.</param>
        /// <returns>The resulting remainder.</returns>
        public static Vect2Int operator /(Vect2Int pos, int scalar) {
            pos.X /= scalar;
            pos.Y /= scalar;

            return pos;
        }

        /// <summary>
        /// Perform modulo on the Vect2Int components by the scalar amount.
        /// </summary>
        /// <param name="pos">The Vect2Int to apply modulo to.</param>
        /// <param name="scalar">The modulo by value.</param>
        /// <returns>The resultant.</returns>
        public static Vect2Int operator %(Vect2Int pos, int scalar) {
            pos.X %= scalar;
            pos.Y %= scalar;

            return pos;
        }
        #endregion

        #region Conversions
        /// <summary>
        /// Convert the Vect2Int into a Unity float
        /// based Vector2.
        /// </summary>
        /// <returns>The converted Vect2Int</returns>
        public Vector2 ToVector2() {
            return new Vector2(X, Y);
        }

        /// <summary>
        /// Generate a print friendly string of the Vect2Int.
        /// </summary>
        /// <returns>The string version of the Vect2Int.</returns>
        public override string ToString() {
            return "(" + this.X + ", " + this.Y + " )";
        }

        /// <summary>
        /// Convert the Vect2Int into a int
        /// array of 2 ints.
        /// </summary>
        /// <returns>An int array in the form {x, y}.</returns>
        public int[] ToArray() {
            return new int[] { X, Y };
        }
        #endregion
    }
}