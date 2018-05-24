using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Voxelated.Serialization;
using Voxelated.Utilities;

namespace Voxelated {
    /// <summary>
    /// 16 bit color, useful for blocks and
    /// stuff that doesn't need as much precision
    /// as a color32. RGB Components are from 0-64,
    /// and Alpha is 0 or 1.
    /// </summary>
    public struct Color16 {
        #region Static Instances
        /// <summary>
        /// Bluish green
        /// </summary>
        public static Color16 Turquoise    = new Color16( 3, 23, 19);     //rgb( 26, 188, 156)

        /// <summary>
        /// Light green
        /// </summary>
        public static Color16 Emerald      = new Color16( 6, 25, 14);     //rgb( 46, 204, 113)

        /// <summary>
        /// Light blue
        /// </summary>
        public static Color16 PeterRiver   = new Color16( 6, 19, 27);     //rgb( 52, 152, 219)

        /// <summary>
        /// Light purple
        /// </summary>
        public static Color16 Amethyst     = new Color16(19, 11, 27);     //rgb(155,  89, 182)

        /// <summary>
        /// Darkish gray blue
        /// </summary>
        public static Color16 WetAsphalt   = new Color16( 6,  9, 12);     //rgb( 52,  73,  94)

        /// <summary>
        /// Darker than turquiose, kinda a greenish blue
        /// </summary>
        public static Color16 GreenSea     = new Color16( 3, 20, 12);     //rgb( 22, 160, 133)

        /// <summary>
        /// Dark green
        /// </summary>
        public static Color16 Nephritis    = new Color16( 5, 21, 12);     //rgb( 39, 174,  96)

        /// <summary>
        /// Dark blue
        /// </summary>
        public static Color16 BelizeHole   = new Color16( 5, 12, 23);     //rgb( 41, 128, 185)

        /// <summary>
        /// Dark purple
        /// </summary>
        public static Color16 Wisteria     = new Color16(17,  8, 21);     //rgb(142,  68, 173)

        /// <summary>
        /// Dark navy blue
        /// </summary>
        public static Color16 MidnightBlue = new Color16( 6,  8, 10);     //rgb( 44,  62,  80)

        /// <summary>
        /// Yellow
        /// </summary>
        public static Color16 SunFlower    = new Color16(30, 24,  2);     //rgb(241, 196,  15)

        /// <summary>
        /// Orange
        /// </summary>
        public static Color16 Carrot       = new Color16(28, 12, 13);     //rgb(230, 126,  34)

        /// <summary>
        /// Ligh red
        /// </summary>
        public static Color16 Alizarin     = new Color16(28, 10,  7);     //rgb(231,  76,  60)

        /// <summary>
        /// Light gray
        /// </summary>
        public static Color16 Clouds       = new Color16(29, 29, 29);     //rgb(236, 240, 241)

        /// <summary>
        /// Medium gray
        /// </summary>
        public static Color16 Concrete     = new Color16(18, 20, 20);     //rgb(149, 165, 166)

        /// <summary>
        /// Orange. No shit.
        /// </summary>
        public static Color16 Orange       = new Color16(30, 19,  3);     //rgb(243, 156,  18)

        /// <summary>
        /// Dark orange
        /// </summary>
        public static Color16 Pumpkin      = new Color16(26, 10,  0);     //rgb(211,  84,   0)

        /// <summary>
        /// Dark red
        /// </summary>
        public static Color16 Pomegranate  = new Color16(23, 10,  5);     //rgb(192,  57,  43)

        /// <summary>
        /// Darker than concrete gray
        /// </summary>
        public static Color16 Silver       = new Color16(23, 24, 24);     //rgb(189, 195, 199)

        /// <summary>
        /// Darish gray
        /// </summary>
        public static Color16 Asbestos     = new Color16(12, 18, 17);     //rgb(127, 140, 141)

        /// <summary>
        /// Black
        /// </summary>
        public static Color16 BlackPerl    = new Color16( 3,  5,  5);     //rgb( 30,  39,  46)

        /// <summary>
        /// Brown
        /// </summary>
        public static Color16 Bark         = new Color16(12,  8,  2);     //rgb( 96,  64,  13)
        #endregion

        #region BitMasks
        /// <summary>
        /// Bit mask for the red component of the color.
        /// </summary>
        private const ushort RBitMask = 0xF800;  // 1111 1000 0000 0000

        /// <summary>
        /// Bit mask for the green component of the color.
        /// </summary>
        private const ushort GBitMask = 0x7C0;   // 0000 0111 1100 0000

        /// <summary>
        /// Bit mask for the blue component of the color.
        /// </summary>
        private const ushort BBitMask = 0x3E;    // 0000 0000 0011 1110

        /// <summary>
        /// Bit mask for the alpha component of the color.
        /// </summary>
        private const ushort ABitMask = 0x1;     // 0000 0000 0000 0001
        #endregion

        #region Members
        /// <summary>
        /// This is the underlying data of the color.
        /// </summary>
        private ushort data;
        #endregion

        #region Color Components
        /// <summary>
        /// The red component of the color. This should
        /// be between 0-31.
        /// </summary>
        public byte R {
            get {
                return (byte)((data & RBitMask) >> 11);
            }
            set {
                //Clamp value first
                value &= 0x1F;

                data =  (ushort)(data & ~RBitMask);
                data |= (ushort)(value << 11);
            }
        }

        /// <summary>
        /// The green component of the color. This
        /// should be between 0-31.
        /// </summary>
        public byte G {
            get {
                return (byte)((data & GBitMask) >> 6);
            }
            set {
                //Clamp value first
                value &= 0x1F;

                data =  (ushort)(data & ~GBitMask);
                data |= (ushort)(value << 6);
            }
        }

        /// <summary>
        /// The blue component of the color. 
        /// This should be between 0-31.
        /// </summary>
        public byte B {
            get {
                return (byte)((data & BBitMask) >> 1);
            }
            set {
                //Clamp value first
                value &= 0x1F;

                data =  (ushort)(data & ~BBitMask);
                data |= (ushort)(value << 1);
            }
        }

        /// <summary>
        /// The alpha component. 0 is invisible.
        /// 1 is Visible.
        /// </summary>
        public byte A {
            get {
                return (byte)(data & ABitMask);
            }
            set {
                //Clamp value first
                value &= 0x01;

                data = (ushort)(data & ~ABitMask);
                data |= value;
            }
        }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new Color16. This is a 2 byte
        /// representation of colors.
        /// </summary>
        /// <param name="r">The red component. 0 - 63.</param>
        /// <param name="g">The green component. 0 - 63.</param>
        /// <param name="b">The blue component. 0 - 63.</param>
        /// <param name="a">The alpha component. 0 - 1.</param>
        public Color16(byte r, byte g, byte b, byte a = 1) {
            data = 0;

            R = r;
            G = g;
            B = b;
            A = a;
        }

        /// <summary>
        /// Rebuild a Color16 from it's unique 16 bit integer.
        /// </summary>
        /// <param name="value">The integer that represents
        /// the color16.</param>
        public Color16(ushort value) {
            data = value;
        }
        #endregion

        #region Conversions
        /// <summary>
        /// Cast the Color32 into a Color16. Some precision
        /// is lost here.
        /// </summary>
        /// <param name="color">The converted color in 16 bit form.</param>
        public static implicit operator Color16(Color32 color) {
            byte r = (byte)(color.r / 255.0f * 31.0f);
            byte g = (byte)(color.g / 255.0f * 31.0f);
            byte b = (byte)(color.b / 255.0f * 31.0f);

            return new Color16(r, g, b, 1);
        }

        /// <summary>
        /// Cast the Color16 into a Color32.
        /// </summary>
        /// <param name="color">The color as a Unity Color32</param>
        public static implicit operator Color32(Color16 color) {
            return color.ToColor32();
        }

        /// <summary>
        /// Cast the Color16 into a Color.
        /// </summary>
        /// <param name="color">The color as a Unity Color.</param>
        public static implicit operator Color(Color16 color) {
            return color.ToColor();
        }

        /// <summary>
        /// Convert the color into a Unity Color32.
        /// </summary>
        /// <returns>The newly created Color32.</returns>
        public Color32 ToColor32() {
            return new Color32((byte)(R * 8), (byte)(G * 8), (byte)(B * 8), (byte)(A * 255));
        }

        /// <summary>
        /// Convert the color into a Unity Color.
        /// </summary>
        /// <returns>The newly created Color.</returns>
        public Color ToColor() {
            return new Color(R / 31.0f, G / 31.0f, B / 31.0f, A / 1.0f);
        }

        /// <summary>
        /// Converts the color into a hex string.
        /// </summary>
        /// <returns>The hex string representation of the color.</returns>
        public string ToHex() {
            StringBuilder hex = new StringBuilder(7);

            hex.Append("#");
            hex.AppendFormat("{0:x2}", R * 8);
            hex.AppendFormat("{0:x2}", G * 8);
            hex.AppendFormat("{0:x2}", B * 8);

            return hex.ToString();
        }
        #endregion

        #region Operators
        /// <summary>
        /// Compare the components of color a, with those
        /// in color b.
        /// </summary>
        /// <param name="a">The first color.</param>
        /// <param name="b">The second color.</param>
        /// <returns>True if both colors are equivalent.</returns>
        public static bool operator ==(Color16 a, Color16 b) {
            return a.Equals(b);
        }

        /// <summary>
        /// Compare the components of color a with color b.
        /// </summary>
        /// <param name="a">The first color.</param>
        /// <param name="b">The second color.</param>
        /// <returns>True if colors do not match.</returns>
        public static bool operator !=(Color16 a, Color16 b) {
            return !a.Equals(b);
        }

        /// <summary>
        /// Checks if two colors are equivalent.
        /// </summary>
        /// <param name="obj">The other color to check.</param>
        /// <returns>True if obj is a color, and matches.</returns>
        public override bool Equals(object obj) {
            //Check it's actually a color first
            if(!(obj is Color16)) {
                return false;
            }

            Color16 color = (Color16)obj;
            return data == color.data;
        }

        /// <summary>
        /// Unique integer to represent the color.
        /// </summary>
        /// <returns>The hashcode.</returns>
        public override int GetHashCode() {
            return data.GetHashCode();
        }

        /// <summary>
        /// Converts the color into a print friendly string
        /// </summary>
        /// <returns>The color as a string</returns>
        public override string ToString() {
            return string.Format("(RGBA) ({0}, {1}, {2}, {3})", R, G, B, A);
        }

        /// <summary>
        /// Returns the colors underlying data struct.
        /// </summary>
        /// <returns>The unique ushort that represents
        /// the color.</returns>
        public ushort ToUShort() {
            return data;
        }
        #endregion
    }
}
