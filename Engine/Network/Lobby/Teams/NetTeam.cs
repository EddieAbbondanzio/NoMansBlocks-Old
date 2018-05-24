using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Serialization;
using Voxelated.Utilities;

namespace Voxelated.Network.Lobby {
    /// <summary>
    /// Represents a team in the net lobby. This is
    /// a group of players working together towards
    /// a common goal. 4/4/18
    /// </summary>
    public class NetTeam : SerializableObject {
        #region Properties
        /// <summary>
        /// The color of the team
        /// </summary>
        public NetTeamColor Color { get; private set; }

        /// <summary>
        /// The current size of the team
        /// </summary>
        public int Size { get { return MemberIds.Count; } }

        /// <summary>
        /// The current score of the team.
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// The player ids of the team members.
        /// </summary>
        private List<byte> MemberIds { get; set; }

        /// <summary>
        /// Indicator flag for rebuilding from bytes.
        /// </summary>
        protected override SerializableType Type {
            get { return SerializableType.NetTeam; }
        }

        /// <summary>
        /// Optimize the serialize method by predefining
        /// a buffer size.
        /// </summary>
        protected override int ByteSize {
            get { return Size * 8 + 5; }
        }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new team of the following color.
        /// </summary>
        public NetTeam(NetTeamColor color) {
            Color     = color;
            Score     = 0;
            MemberIds = new List<byte>();
        }

        /// <summary>
        /// Recreate a net team from it's serialized
        /// byte data.
        /// </summary>
        /// <param name="bytes">The bytes to extract it from.</param>
        /// <param name="startBit">The first bit index.</param>
        public NetTeam(byte[] bytes, int startBit) {
            ByteBuffer buffer = GetContent(bytes, startBit, Type);
            Color           = (NetTeamColor) buffer.ReadByte(3);
            int playerCount = buffer.ReadInt(5);
            Score           = buffer.ReadInt();

            //Read in the team member id's of the players.
            byte[] ids = buffer.ReadBytes(playerCount * 8);
            MemberIds = ids.ToList();
        }

        /// <summary>
        /// Rebuild the NetTeam from a byte buffer
        /// containing it's values.
        /// </summary>
        /// <param name="buffer">The buffer to read from.</param>
        public NetTeam(ByteBuffer buffer) {
            buffer.SkipReadingBits(32);

            Color = (NetTeamColor)buffer.ReadByte(3);
            int playerCount = buffer.ReadInt(5);
            Score = buffer.ReadInt();

            //Read in the team member id's of the players.
            byte[] ids = buffer.ReadBytes(playerCount * 8);
            MemberIds = ids.ToList();
        }
        #endregion

        #region Publics
        /// <summary>
        /// Add a new player to the team
        /// </summary>
        public void AddMember(NetPlayer player) {
            //Ensure it's a real player first, and not in the team
            if(player == null || MemberIds.Contains(player.Id)) {
                return;
            }

            player.SetTeam(Color);
            MemberIds.Add(player.Id);
        }

        /// <summary>
        /// Checks if the team contains a member that
        /// matches up to the inputted player.
        /// </summary>
        /// <param name="player">The player to look for.</param>
        /// <returns>True if the team contains that player.</returns>
        public bool ContainsMember(NetPlayer player) {
            if(player != null) {
                return MemberIds.Contains(player.Id);
            }
            else {
                return false;
            }
        }

        /// <summary>
        /// Remove a player from the team
        /// </summary>
        public void RemoveMember(NetPlayer player) {
            //Ensure it's a real player first, and in the team
            if (player == null || !MemberIds.Contains(player.Id)) {
                return;
            }

            player.SetTeam(NetTeamColor.Spectator);
            MemberIds.Remove(player.Id);
        }

        /// <summary>
        /// Checks if two teams are equivalent.
        /// </summary>
        /// <param name="obj">The other object to compare against.</param>
        /// <returns>True if both are the smae.</returns>
        public override bool Equals(object obj) {
            if(obj == null) {
                return false;
            }

            NetTeam other = obj as NetTeam;

            if(other == null) {
                return false;
            }

            return Color == other.Color
                && MemberIds.SequenceEqual(other.MemberIds)
                && Score == other.Score;
        }

        /// <summary>
        /// Generates a unique integer to 
        /// represent the object.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            return base.GetHashCode();
        }
        #endregion

        #region Serialization
        /// <summary>
        /// Serialize the content of the team to rebuild it
        /// later on.
        /// </summary>
        /// <param name="buffer">The buffer to write content to.</param>
        protected override void SerializeContent(ByteBuffer buffer) {
            buffer.Write((byte) Color, 3);
            buffer.Write((byte)  Size, 5);
            buffer.Write(Score);
            buffer.Write(MemberIds);
        }
        #endregion
    }
}
