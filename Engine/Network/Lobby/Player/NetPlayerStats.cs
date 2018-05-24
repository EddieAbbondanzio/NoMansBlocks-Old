using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Serialization;
using Voxelated.Utilities;

namespace Voxelated.Network.Lobby {
    /// <summary>
    /// Container of various stats to track
    /// about players.
    /// </summary>
    public class NetPlayerStats : SerializableObject {
        #region Properties
        /// <summary>
        /// How many kills the player has earned.
        /// </summary>
        public ushort Kills { get; set; }

        /// <summary>
        /// How many times the player has died.
        /// </summary>
        public ushort Deaths { get; set; }

        /// <summary>
        /// How many blocks the player has placed.
        /// </summary>
        public uint BlocksPlaced { get; set; }

        /// <summary>
        /// How many blocks the player has destroyed.
        /// </summary>
        public uint BlocksDestroyed { get; set; }

        /// <summary>
        /// Points the player has earned.
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Flag to identify what it is when serialzied.
        /// </summary>
        protected override SerializableType Type {
            get { return SerializableType.NetPlayerStats; }
        }

        /// <summary>
        /// The number of bytes needed to serialize it.
        /// </summary>
        protected override int ByteSize {
            get { return 16; }
        }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new player stats of default
        /// values.
        /// </summary>
        public NetPlayerStats() {
            Kills  = 0;
            Deaths = 0;
            Score  = 0;
            BlocksPlaced    = 0;
            BlocksDestroyed = 0;
        }

        /// <summary>
        /// Create a player stats object from it's
        /// serialized byte array.
        /// </summary>
        /// <param name="bytes">The stats serialized as a byte array.</param>
        /// <param name="startBit">The first bit to get it from.</param>
        public NetPlayerStats(byte[] bytes, int startBit = 0) {
            ByteBuffer buffer = GetContent(bytes, startBit, Type);
            Kills           = buffer.ReadUShort();
            Deaths          = buffer.ReadUShort();
            BlocksPlaced    = buffer.ReadUInt();
            BlocksDestroyed = buffer.ReadUInt();
            Score           = buffer.ReadInt();
        }

        /// <summary>
        /// Create a player stats object from it's bytes. This 
        /// method is more efficient and should be used when possible.
        /// </summary>
        /// <param name="buffer">The buffer containing
        /// the bytes of the stats.</param>
        public NetPlayerStats(ByteBuffer buffer) {
            buffer.SkipReadingBits(32);

            Kills           = buffer.ReadUShort();
            Deaths          = buffer.ReadUShort();
            BlocksPlaced    = buffer.ReadUInt();
            BlocksDestroyed = buffer.ReadUInt();
            Score           = buffer.ReadInt();
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Tests if two player stats are equivalent.
        /// </summary>
        /// <param name="obj">The other player stats.</param>
        /// <returns>True if the stats match.</returns>
        public override bool Equals(object obj) {
            if(obj == null) {
                return false;
            }

            NetPlayerStats other = obj as NetPlayerStats;

            if(other == null) {
                return false;
            }

            return Kills == other.Kills
                && Deaths == other.Deaths
                && BlocksPlaced == other.BlocksPlaced
                && BlocksDestroyed == other.BlocksDestroyed
                && Score == other.Score;
        }

        /// <summary>
        /// Returns a hashcode of the stats.
        /// </summary>
        /// <returns>The 'unique' hashcode.</returns>
        public override int GetHashCode() {
            return base.GetHashCode();
        }
        #endregion

        #region Serialization
        /// <summary>
        /// Write the contents of the stats to the buffer
        /// that will contain all of it's data.
        /// </summary>
        /// <param name="buffer">The buffer to write to.</param>
        protected override void SerializeContent(ByteBuffer buffer) {
            buffer.Write(Kills);
            buffer.Write(Deaths);
            buffer.Write(BlocksPlaced);
            buffer.Write(BlocksDestroyed);
            buffer.Write(Score);
        }
        #endregion
    }
}
