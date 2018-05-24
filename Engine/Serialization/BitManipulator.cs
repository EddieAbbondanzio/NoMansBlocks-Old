using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Utilities;

// Eddie Abbondanzio 4-26-2018
// Avoid rewriting if possible. Already invested two weeks...
namespace Voxelated.Serialization {
    /// <summary>
    /// Helper class for working at the bit level. Allows for abstracting
    /// away the individual bytes in a byte array and can write / read starting at
    /// any desired bit index. Should idealy only be called by the SerializeUtil class.
    /// </summary>
    public static class BitManipulator {
        #region Read / Write
        /// <summary>
        /// Returns a single bit's value.
        /// </summary>
        /// <param name="bytes">The byte to retrieve the bit from.</param>
        /// <param name="bitIndex">The index in the byte of the bit.</param>
        /// <returns>0, or 1 depending on the bits value.</returns>
        public static byte ReadBit(byte b, int bitIndex) {
            if (bitIndex < 0) { throw new ArgumentOutOfRangeException("BitIndex index must be greater than zero!"); }
            if (bitIndex > 8) { throw new ArgumentOutOfRangeException("BitIndex index cannot be outside of the byte!"); }

            return (byte)((b & (1 << bitIndex)) >> bitIndex);
        }

        /// <summary>
        /// Returns a single bit's value.
        /// </summary>
        /// <param name="bytes">The byte array to retrieve the bit from.</param>
        /// <param name="bitIndex">The index in the array of the bit.</param>
        /// <returns>0, or 1 depending on the bits value.</returns>
        public static byte ReadBit(byte[] bytes, int bitIndex) {
            int bitLength = bytes.Length * 8;

            //Validate input.
            if (bytes == null) { throw new ArgumentNullException("Byte array is null!"); }
            if (bitIndex < 0)  { throw new ArgumentOutOfRangeException("BitIndex index must be greater than zero!"); }
            if (bitIndex >= bitLength) { throw new ArgumentOutOfRangeException("BitIndex index cannot be outside of the byte array!"); }

            int byteIndex = bitIndex / 8;
            int bitOffset = bitIndex % 8;

            return (byte)((bytes[byteIndex] & (1 << bitOffset)) >> bitOffset);
        }

        /// <summary>
        /// Read the desired number of bits from the byte array starting
        /// at any bit index.
        /// </summary>
        /// <param name="bytes">The byte array to read from/</param>
        /// <param name="startBit">The starting bit (Inclusive, starts at 0).</param>
        /// <param name="bitCount">The number of bits to read.</param>
        /// <returns>The bits read from the array.</returns>
        public static byte[] ReadBits(byte[] bytes, int startBit, int bitCount) {
            int bitLength = bytes.Length * 8;

            //Validate the input
            if (bytes == null) { throw new ArgumentNullException("Byte array is null!"); }
            if (startBit < 0)  { throw new ArgumentOutOfRangeException("StartBit index must be greater than zero!"); }
            if (startBit >= bitLength) { throw new ArgumentOutOfRangeException("StartBit index cannot be outside of the byte array!"); }
            if (bitCount < 0) { throw new ArgumentOutOfRangeException("BitCount must be greater than zero!"); }
            if (bitCount + startBit > bitLength) { throw new ArgumentOutOfRangeException("Byte array does not contain enough elements!"); }

            //The local offset of the bit in it's byte
            int startByte = startBit / 8;
            int bitOffset = startBit % 8;

            //Reading from a single byte.
            if ((bitOffset + bitCount) <= 8) {
                //Read the bits then shift them.
                byte value = ReadBits(bytes, startByte, bitCount, bitOffset);
                value >>= bitOffset;

                return new byte[] { value };
            }
            //Reading from multiple bytes
            else {
                //Figure out how many bytes to read from.
                int byteCount = bitCount / 8;
                int remainderBits = bitCount % 8;   //The bits of the last byte being written to.

                if (remainderBits > 0) {
                    byteCount++;
                }

                //Create the new array to return.
                byte[] values = new byte[byteCount];

                //Start getting the byte values
                for (int b = 0; b < byteCount; b++) {
                    int currBitCount = (b == byteCount - 1 && remainderBits != 0) ? remainderBits : 8;

                    //Easy case.
                    if (bitOffset == 0) {
                        values[b] = ReadBits(bytes, startByte + b, currBitCount, 0);
                    }
                    //Gotta pull bits from 2 seperate bytes.
                    else {
                        //Now figure out the counts
                        int nextByteBitCount, currByteBitCount;

                        //Gotta do a little extra math
                        if(bitOffset + currBitCount > 8) {
                            nextByteBitCount = bitOffset + currBitCount - 8;
                            currByteBitCount = 8 - bitOffset;
                        }
                        else {
                            nextByteBitCount = 0;
                            currByteBitCount = currBitCount;
                        }

                        //Read some of the bits in.
                        int currBits = ReadBits(bytes, startByte + b, currByteBitCount, bitOffset) >> bitOffset;

                        //Read the rest in (if any)
                        if (nextByteBitCount > 0) {
                            currBits += ReadBits(bytes, startByte + b + 1, nextByteBitCount, 0) << currByteBitCount;
                        }

                        values[b] = (byte)currBits;
                    }
                }

                return values;
            }
        }

        /// <summary>
        /// Write a single bit to a byte.
        /// </summary>
        /// <param name="b">The byte to write it to.</param>
        /// <param name="value">The bit value to write.</param>
        /// <param name="bitIndex">The position in the byte to write it at.</param>
        public static void WriteBit(ref byte b, byte value, int bitIndex) {
            if (bitIndex < 0)  { throw new ArgumentOutOfRangeException("BitIndex index must be greater than zero!"); }
            if (bitIndex >= 8) { throw new ArgumentOutOfRangeException("BitIndex index cannot be outside of the byte array!"); }

            value &= 1;
            byte bitMask = (byte)(1 << bitIndex);

            //Apply the bit.
            b &= (byte)~bitMask;
            b |= (byte)(value << bitIndex);
        }

        /// <summary>
        /// Write a single bit to a byte array.
        /// </summary>
        /// <param name="b">The byte to write it to.</param>
        /// <param name="value">The bit value to write.</param>
        /// <param name="bitIndex">The position in the array to write it at.</param>
        public static void WriteBit(byte[] bytes, byte value, int bitIndex) {
            int bitLength = bytes.Length * 8;

            //Input validation.
            if (bytes == null) { throw new ArgumentNullException("Byte array is null!"); }
            if (bitIndex < 0)  { throw new ArgumentOutOfRangeException("BitIndex index must be greater than zero!"); }
            if (bitIndex >= bitLength) { throw new ArgumentOutOfRangeException("BitIndex index cannot be outside of the byte array!"); }

            value &= 1;
            byte bitMask = (byte)(1 << bitIndex);

            //The local offset of the bit in it's byte
            int startByte = bitIndex / 8;
            int bitOffset = bitIndex % 8;

            //Apply the bit.
            bytes[startByte] &= (byte)~bitMask;
            bytes[startByte] |= (byte)(value << bitOffset);
        }

        /// <summary>
        /// Write the desired number of bits from the values array into the
        /// bytes array.
        /// </summary>
        /// <param name="bytes">The array to write into.</param>
        /// <param name="values">The bits to copy.</param>
        /// <param name="startBit">The first bit to write at (inclusive).</param>
        /// <param name="bitCount">The number of bits to copy.</param>
        public static void WriteBits(byte[] bytes, byte[] values, int startBit, int bitCount) {
            int dstBitLength = bytes.Length * 8;
            int valBitLength = values.Length * 8;

            //Validate the input
            if (bytes == null) { throw new ArgumentNullException("Byte array is null!"); }
            if (values == null) { throw new ArgumentNullException("Values array is null!"); }
            if (startBit < 0) { throw new ArgumentOutOfRangeException("StartBit index must be greater than zero!"); }
            if (startBit >= dstBitLength) { throw new ArgumentOutOfRangeException("StartBit index cannot be outside of the byte array!"); }
            if (bitCount < 0) { throw new ArgumentOutOfRangeException("BitCount must be greater than zero!"); }
            if (bitCount + startBit > dstBitLength) { throw new ArgumentOutOfRangeException("Byte array does not contain enough elements!"); }
            if (bitCount > valBitLength) { throw new ArgumentOutOfRangeException("Value array does not contain enough elements!"); }

            //The local offset of the bit in it's byte
            int startByte = startBit / 8;
            int bitOffset = startBit % 8;

            //Writing into a single bit case
            if ((bitOffset + bitCount) <= 8) {
                WriteBits(bytes, values[0], startByte, bitCount, bitOffset);
            }
            //Multi-byte write operation
            else {
                //Figure out how many bytes to write to.
                int byteCount = bitCount / 8;
                int remainderBits = bitCount % 8;   //The bits of the last byte being written to.

                if (remainderBits > 0) {
                    byteCount++;
                }

                for (int b = 0; b < byteCount; b++) {
                    int currBitCount = (b == byteCount - 1 && remainderBits != 0) ? remainderBits : 8;

                    //Easy 1 to  1 case.
                    if (bitOffset == 0) {
                        WriteBits(bytes, values[b], startByte + b, currBitCount, 0);
                    }
                    //Harder pulling from two bytes, then writing to two bytes.
                    else {
                        int currByteBitCount = 8 - bitOffset;
                        int nextByteBitCount = currBitCount - currByteBitCount;

                        if (currByteBitCount > 0) {
                            byte currBits = ReadBits(values, b, currByteBitCount, 0);
                            WriteBits(bytes, currBits, startByte + b, currByteBitCount, bitOffset);
                        }

                        //If there is any to write do so.
                        if (nextByteBitCount > 0) {
                            byte nextBits = (byte)(ReadBits(values, b, nextByteBitCount, currByteBitCount) >> currByteBitCount);
                            WriteBits(bytes, nextBits, startByte + b + 1, nextByteBitCount, 0);

                        }
                    }
                }
            }
        }
        #endregion

        #region Helpers
        /// <summary>
        /// Reads the desired number of bits from the byte within the byte array.
        /// </summary>
        /// <param name="bytes">The byte array to read from.</param>
        /// <param name="byteIndex">The byte index to read. (0 to bytes.length - 1).</param>
        /// <param name="bitCount">The numebr of bits to read. (1 to 8).</param>
        /// <param name="bitOffset">The bits offset. (0 to 7).</param>
        public static byte ReadBits(byte[] bytes, int byteIndex, int bitCount, int bitOffset) {
            //Input Validation
            if (bytes == null) { throw new ArgumentNullException("Bytes cannot be null!"); }

            //Validate the bitCount
            if (bitCount > 8) {
                bitCount = 8;
            }
            else if (bitCount < 1) {
                bitCount = 1;
            }

            if (bitOffset + bitCount > 8) {
                throw new ArgumentOutOfRangeException("Attempting to read too many bits!");
            }

            byte bitMask = (byte)(((1 << bitCount) - 1) << bitOffset);
            return (byte)(bytes[byteIndex] & bitMask);
        }

        /// <summary>
        /// Write the desired number of bits into the byte at the specified
        /// location within the byte array.
        /// </summary>
        /// <param name="bytes">The byte array that the byte being written to exists in.</param>
        /// <param name="value">The value to write to the byte.</param>
        /// <param name="byteIndex">The bytes index in the array. (0 to bytes.length - 1)</param>
        /// <param name="bitCount">The number of bits to write. (1 to values.length * 8 - 1)</param>
        /// <param name="bitOffset">The position of the first bit. (Inclusive) (0 to 7)</param>
        public static void WriteBits(byte[] bytes, byte value, int byteIndex, int bitCount, int bitOffset) {
            //Input Validation
            if (bytes == null) { throw new ArgumentNullException("Bytes cannot be null!"); }

            //Validate the bitCount
            if (bitCount > 8) {
                bitCount = 8;
            }
            else if (bitCount < 1) {
                bitCount = 1;
            }

            if (bitOffset + bitCount > 8) {
                throw new ArgumentOutOfRangeException("Attempting to write too many bits!");
            }

            //Generate the bit mask first. This looks like 0110 0000 if bitCount = 2, and bitOffset = 6.
            byte bitMask = (byte)(((1 << bitCount) - 1) << bitOffset);

            //Wipe the old bits. Then write the new ones.
            bytes[byteIndex] &= (byte)~bitMask;
            bytes[byteIndex] |= (byte)((value << bitOffset) & bitMask);
        }

        /// <summary>
        /// Shift an entire byte array to the left by shiftIndex
        /// bits. Does not wrap around values. Array size is
        /// kept constant and does not account for overflow.
        /// </summary>
        /// <param name="bytes">The bytes to shift.</param>
        /// <param name="shiftCount">The number of bits to shift
        /// it left.</param>
        public static void LeftBitShift(byte[] bytes, int shiftCount) {
            //Input validation first.
            if (bytes == null) { throw new ArgumentNullException("Bytes cannot be null!"); }

            //Don't waste CPU cycles on junk.
            if (shiftCount <= 0) {
                return;
            }

            int currShift = shiftCount > 8 ? 8 : shiftCount;

            byte carryOver = 0;
            byte carryMask = (byte)(((1 << currShift) - 1) << 8 - currShift);

            for (int b = 0; b < bytes.Length; b++) {
                //Figure out what value to carry in.
                if (b < bytes.Length - 1) {
                    carryOver = (byte)((bytes[b + 1] & carryMask) >> 8 - currShift);
                }
                else {
                    carryOver = 0;
                }

                //Move the current values over
                bytes[b] <<= currShift;
                bytes[b] |= carryOver;
            }

            //Recursive step
            if (shiftCount > 8) {
                LeftBitShift(bytes, shiftCount - 8);
            }
        }

        /// <summary>
        /// Shift an entire byte array to the right by shiftIndex
        /// bits. Does not wrap around bits or account for
        /// overflow.
        /// </summary>
        /// <param name="bytes">The bytes to shift.</param>
        /// <param name="shiftCount">The number of bits to shift
        /// it right.</param>
        public static void RightBitShift(byte[] bytes, int shiftCount) {
            //Input validation first.
            if (bytes == null) { throw new ArgumentNullException("Bytes cannot be null!"); }

            //Don't waste CPU cycles on junk.
            if (shiftCount <= 0) {
                return;
            }

            int currShift = shiftCount > 8 ? 8 : shiftCount;

            byte carryOver = 0;
            byte carryMask = (byte)((1 << currShift) - 1);

            for (int b = bytes.Length - 1; b >= 0; b--) {
                //Figure out what value to carry in.
                if (b > 0) {
                    carryOver = (byte)(bytes[b - 1] & carryMask);
                }
                else {
                    carryOver = 0;
                }

                //Move the current values over
                bytes[b] >>= currShift;
                bytes[b] |= (byte)(carryOver << (8 - currShift));
            }

            //Recursively work it haha.
            if (shiftCount > 8) {
                RightBitShift(bytes, shiftCount - 8);
            }
        }

        /// <summary>
        /// Returns how many bytes are needed to fit the
        /// specific number of bits.
        /// </summary>
        /// <param name="bits">How many desired bits to store.</param>
        /// <returns>The number of bytes needed to fit them.</returns>
        public static int ByteCountForBits(int bits) {
            //Prevent bad results.
            if (bits == 0) {
                return 0;
            }

            int bytes = bits / 8;
            if (bits % 8 != 0) {
                bytes++;
            }

            return bytes;
        }
        #endregion
    }
}
