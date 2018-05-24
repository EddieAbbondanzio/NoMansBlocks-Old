using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Voxelated.Serialization;
using Voxelated.Utilities;

namespace Voxelated {
    /// <summary>
    /// An integer based version of the standard Vector3.
    /// </summary>
    public struct Vect3Int {
        #region Static Instances
        /// <summary>
        /// Unit Vect3Int in the North direction.
        /// </summary>
        public static Vect3Int North = new Vect3Int(0, 0, 1);

        /// <summary>
        /// Unit Vect3Int in the South direction.
        /// </summary>
        public static Vect3Int South = new Vect3Int(0, 0, -1);

        /// <summary>
        /// Unit Vect3Int in the East direction.
        /// </summary>
        public static Vect3Int East = new Vect3Int(1, 0, 0);

        /// <summary>
        /// Unit Vect3Int in the West direction
        /// </summary>
        public static Vect3Int West = new Vect3Int(-1, 0, 0);

        /// <summary>
        /// Unit Vect3Int in the Up direction
        /// </summary>
        public static Vect3Int Up = new Vect3Int(0, 1, 0);

        /// <summary>
        /// Unit Vect3Int in the Down direction.
        /// </summary>
        public static Vect3Int Down = new Vect3Int(0, -1, 0);

        /// <summary>
        /// Vect3Int with a X, Y, and Z component of 1.
        /// </summary>
        public static Vect3Int One = new Vect3Int(1, 1, 1);

        /// <summary>
        /// Vect3Int with a X, Y, and Z component of 0.
        /// </summary>
        public static Vect3Int Zero = new Vect3Int(0, 0, 0);
        #endregion

        #region Propeties
        /// <summary>
        /// The X component.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// The Y component.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// The Z component.
        /// </summary>
        public int Z { get; set; }

        /// <summary>
        /// Array access to the coordinate components
        /// of the Vect3Int.
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
                    case 2:
                        return Z;
                    default:
                        throw new Exception("Vector3Int. Invalid component access.");
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
                    case 2:
                        Z = value;
                        break;
                    default:
                        throw new Exception("Vector3Int. Invalid component set.");
                }
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new Vect3Int with the following
        /// values to represent each of it's components.
        /// </summary>
        /// <param name="x">The x coordinate</param>
        /// <param name="y">The y coordinate</param>
        /// <param name="z">The z coordinate</param>
        public Vect3Int(int x, int y, int z) {
            X = x;
            Y = y;
            Z = z;
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Checks to see if the Vect3Int is equivalent 
        /// with another Vect3Int.
        /// </summary>
        /// <param name="obj">The other Vect3Int to test.</param>
        /// <returns>True if both are equivalent in each component.</returns>
        public override bool Equals(object obj) {
            if (!(obj is Vect3Int)) {
                return false;
            }

            Vect3Int other = (Vect3Int)obj;
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        /// <summary>
        /// Generate a unique hash code based off the components
        /// of the Vect3Int.
        /// </summary>
        /// <returns>The hashcode.</returns>
        public override int GetHashCode() {
            int hash = 23;
            hash = hash * 31 + X;
            hash = hash * 31 + Y;
            hash = hash * 31 + Z;

            return hash;
        }
        #endregion

        #region Operator Overloads
        /// <summary>
        /// Cast a Vect3Int into a Unity Vector2.
        /// </summary>
        /// <param name="pos">The converted Vector2.</param>
        public static explicit operator Vector2(Vect3Int pos) {
            return new Vector2(pos.X, pos.Y);
        }

        /// <summary>
        /// Convert into a Unity float based Vector3.
        /// </summary>
        /// <param name="pos">The converted Vector3.</param>
        public static explicit operator Vector3(Vect3Int pos) {
            return pos.ToVector3();
        }

        /// <summary>
        /// Convert into a Unity integer based Vector2.
        /// </summary>
        /// <param name="pos">The converted Vector2Int.</param>
        public static explicit operator Vector2Int(Vect3Int pos) {
            return new Vector2Int(pos.X, pos.Y);
        }

        /// <summary>
        /// Convert into a Unity based Vector3.
        /// </summary>
        /// <param name="pos">The resultant vector3Int.</param>
        public static explicit operator Vector3Int(Vect3Int pos) {
            return new Vector3Int(pos.X, pos.Y, pos.Z);
        }

        /// <summary>
        /// Convert Unitys format to my
        /// (superior) format. heh.
        /// </summary>
        /// <param name="pos">The resultant Vect3Int.</param>
        public static explicit operator Vect3Int(Vector3Int pos) {
            return new Vect3Int(pos.x, pos.y, pos.z);
        }

        /// <summary>
        /// Convert the Vect3Int into a Vect2Int. This basically
        /// just removes the Z component.
        /// </summary>
        /// <param name="pos">The converted Vect2Int.</param>
        public static explicit operator Vect2Int(Vect3Int pos) {
            return new Vect2Int(pos.X, pos.Y);
        }

        /// <summary>
        /// Tests if two Vect3Ints are equivalent.
        /// </summary>
        /// <param name="posA">The first position.</param>
        /// <param name="posB">The second position.</param>
        /// <returns>True if both have equivalent components.</returns>
        public static bool operator ==(Vect3Int posA, Vect3Int posB) {
            return posA.Equals(posB);
        }


        /// <summary>
        /// Tests if two Vect3Ints are different.
        /// </summary>
        /// <param name="posA">The first position.</param>
        /// <param name="posB">The second position.</param>
        /// <returns>True if both have 1 or more different components.</returns>
        public static bool operator !=(Vect3Int posA, Vect3Int posB) {
            return !posA.Equals(posB);
        }

        /// <summary>
        /// Add two Vect3Ints together.
        /// </summary>
        /// <param name="posA">The first position.</param>
        /// <param name="posB">The second position.</param>
        /// <returns>The resultant Vect3Int.</returns>
        public static Vect3Int operator +(Vect3Int posA, Vect3Int posB) {
            posA.X += posB.X;
            posA.Y += posB.Y;
            posA.Z += posB.Z;

            return posA;
        }

        /// <summary>
        /// Subtract Vect3Int posB from posA.
        /// </summary>
        /// <param name="posA">The position to subtract from.</param>
        /// <param name="posB">The position to subtract.</param>
        /// <returns>The resultant of the subtraction.</returns>
        public static Vect3Int operator -(Vect3Int posA, Vect3Int posB) {
            posA.X -= posB.X;
            posA.Y -= posB.Y;
            posA.Z -= posB.Z;

            return posA;
        }

        /// <summary>
        /// Multiply the Vect3Ints components by the scalar.
        /// </summary>
        /// <param name="pos">The position to multiply.</param>
        /// <param name="scalar">The value to multiply the components by.</param>
        /// <returns>The resultant of the multiplication.</returns>
        public static Vect3Int operator *(Vect3Int pos, int scalar) {
            pos.X *= scalar;
            pos.Y *= scalar;
            pos.Z *= scalar;

            return pos;
        }

        /// <summary>
        /// Divide the Vect3Ints components by the scalar.
        /// </summary>
        /// <param name="pos">The position to divide.</param>
        /// <param name="scalar">The value to divide each component by.</param>
        /// <returns>The resultant Vect3Int of the operation.</returns>
        public static Vect3Int operator /(Vect3Int pos, int scalar) {
            pos.X /= scalar;
            pos.Y /= scalar;
            pos.Z /= scalar;

            return pos;
        }

        /// <summary>
        /// Perform the modulo operation on the position.
        /// </summary>
        /// <param name="pos">The position to operate on.</param>
        /// <param name="scalar">The value to modulo it by.</param>
        /// <returns>The resultant Vect3Int of the operation.</returns>
        public static Vect3Int operator %(Vect3Int pos, int scalar) {
            pos.X %= scalar;
            pos.Y %= scalar;
            pos.Z %= scalar;

            return pos;
        }
        #endregion

        #region Conversions
        /// <summary>
        /// Convert the Vect3Int into a Unity float
        /// based Vector3.
        /// </summary>
        /// <returns>The converted Vect3Int</returns>
        public Vector3 ToVector3() {
            return new Vector3(X, Y, Z);
        }

        /// <summary>
        /// Generate a print friendly string of the Vect3Int.
        /// </summary>
        /// <returns>The string version of the Vect3Int.</returns>
        public override string ToString() {
            return "(" + X + ", " + Y + ", " + Z + " )";
        }

        /// <summary>
        /// Convert the Vect3Int into a int
        /// array of 3 ints.
        /// </summary>
        /// <returns>An int array in the form {x, y, z}.</returns>
        public int[] ToArray() {
            return new int[] { X, Y, Z };
        }
        #endregion
    }
}