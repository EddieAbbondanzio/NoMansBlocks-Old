using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Serialization;
using Voxelated.Utilities;

namespace Voxelated.Network.Lobby {
    /// <summary>
    /// Player class of a client. Contains info
    /// such as stats, name and more.
    /// </summary>
    public class NetPlayer : SerializableObject {
        #region Properties
        /// <summary>
        /// The unique id of the player. This
        /// is used to relate the client connection
        /// to it's player.
        /// </summary>
        public byte Id { get; private set; }

        /// <summary>
        /// The name to show the player as 
        /// in the lobby.
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// The color of the player.
        /// </summary>
        public NetTeamColor Team { get; set; }

        /// <summary>
        /// Various values being tracked
        /// on the player.
        /// </summary>
        public NetPlayerStats Stats { get; private set; }

        /// <summary>
        /// Flag to help identify the object when it's in 
        /// byte format.
        /// </summary>
        protected override SerializableType Type {
            get { return SerializableType.NetPlayer; }
        }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new instance of a player with
        /// team set to spectator.
        /// </summary>
        /// <param name="id">The player's id.</param>
        /// <param name="name">The nickname of the player.</param>
        public NetPlayer(byte id, string name) {
            if (id == byte.MaxValue) {
                throw new ArgumentOutOfRangeException("Id of 255 is reserved and cannot be used!");
            }

            Id = id;
            NickName = name;
            Team = NetTeamColor.Spectator;
            Stats = new NetPlayerStats();
        }

        /// <summary>
        /// Create a new instance of a player with
        /// a custom team color.
        /// </summary>
        /// <param name="id">The player's id.</param>
        /// <param name="name">The nickname of the player.</param>
        /// <param name="team">The player's team color.</param>
        public NetPlayer(byte id, string name, NetTeamColor team) {
            if(id == byte.MaxValue) {
                throw new ArgumentOutOfRangeException("Id of 255 is reserved and cannot be used!");
            }

            Id = id;
            NickName = name;
            Team = team;
            Stats = new NetPlayerStats();
        }

        /// <summary>
        /// Create a netplayer from it's serialized byte array.
        /// </summary>
        /// <param name="bytes">The encoded player.</param>
        /// <param name="startBit">The starting bit position.</param>
        public NetPlayer(byte[] bytes, int startBit) {
            ByteBuffer buffer = GetContent(bytes, startBit, Type);
            Id       = buffer.ReadByte();
            Team    = (NetTeamColor)buffer.ReadByte();
            NickName = buffer.ReadString();
            Stats    = buffer.ReadSerializableObject() as NetPlayerStats;
        }

        /// <summary>
        /// Recreate a NetPlayer from it's bytes that are being
        /// stored in a buffer.
        /// </summary>
        /// <param name="buffer">The buffer to retrieve the values from.</param>
        public NetPlayer(ByteBuffer buffer) {
            buffer.SkipReadingBits(32);

            Id       = buffer.ReadByte();
            Team    = (NetTeamColor)buffer.ReadByte();
            NickName = buffer.ReadString();
            Stats    = buffer.ReadSerializableObject() as NetPlayerStats;
        }
        #endregion

        #region Publics
        /// <summary>
        /// Set the team color of the player.
        /// </summary>
        public void SetTeam(NetTeamColor color) {
            Team = color;
        }

        /// <summary>
        /// Tests if two players are equivalent.
        /// </summary>
        /// <param name="obj">The other player to test.</param>
        /// <returns>True if the players are the same.</returns>
        public override bool Equals(object obj) {
            if(obj == null) {
                return false;
            }

            NetPlayer otherPlayer = obj as NetPlayer;

            if(otherPlayer == null) {
                return false;
            }

            return Id == otherPlayer.Id
                && NickName == otherPlayer.NickName
                && Team == otherPlayer.Team
                && Stats.Equals(otherPlayer.Stats);
        }

        /// <summary>
        /// Returns a hashcode of the player.
        /// </summary>
        /// <returns>The 'unique' hashcode.</returns>
        public override int GetHashCode() {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns the players information
        /// in a print friendly string format.
        /// </summary>
        /// <returns>The player represented as a string.</returns>
        public override string ToString() {
            return string.Format("(NetPlayer) Id: {0} Name: {1} Team: {2}", Id, NickName, Team);
        }
        #endregion

        #region Serialization
        /// <summary>
        /// Pack up the info needed to rebuild
        /// the net player later on.
        /// </summary>
        /// <param name="buffer">The buffer to write to.</param>
        protected override void SerializeContent(ByteBuffer buffer) {
            buffer.Write(Id);
            buffer.Write((byte)Team);
            buffer.Write(NickName);
            buffer.Write(Stats);
        }
        #endregion
    }
}
