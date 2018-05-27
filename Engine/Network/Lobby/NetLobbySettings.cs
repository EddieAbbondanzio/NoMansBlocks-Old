using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Serialization;
using Voxelated.Utilities;

namespace Voxelated.Network.Lobby {
    /// <summary>
    /// Various settings of the NetLobby class.
    /// Helps keep them containted in one spot
    /// and easier to serialize.
    /// </summary>
    public class NetLobbySettings : SerializableObject {
        #region Constants
        /// <summary>
        /// Maximum character length for the name.
        /// </summary>
        public const int NameLengthLimit = 24;

        /// <summary>
        /// Maximum character length for the description.
        /// </summary>
        public const int DescriptionLengthLimit = 176;
        #endregion

        #region Properties
        /// <summary>
        /// The name to display the lobby.
        /// </summary>
        public string Name { get; private set; } = string.Empty;

        /// <summary>
        /// The text description of the lobby.
        /// </summary>
        public string Description { get; private set; } = string.Empty;

        /// <summary>
        /// How many seconds to wait between games. Defaults out
        /// to 90 seconds.
        /// </summary>
        public float IntermissionTime { get; private set; } = 90.0f;

        /// <summary>
        /// Flag to help identify the object when it's in 
        /// byte format.
        /// </summary>
        protected override SerializableType Type {
            get { return SerializableType.NetLobbySettings; }
        }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Settings for a networked lobby. Intermission time
        /// is kept at default.
        /// </summary>
        /// <param name="lobbyName">The name of the lobby (Max length = 24).</param>
        /// <param name="lobbyDesc">The description of the lobby (Max length = 176).</param>
        public NetLobbySettings(string lobbyName, string lobbyDesc) {
            Name        = StringUtils.Clamp(lobbyName, NameLengthLimit);
            Description = StringUtils.Clamp(lobbyDesc, DescriptionLengthLimit);
        }

        /// <summary>
        /// Settings for a networked lobby with a custom time
        /// between matches setting.
        /// </summary>
        /// <param name="lobbyName">The name of the lobby (Max length = 24).</param>
        /// <param name="lobbyDesc">The description of the lobby (Max length = 176).</param>
        /// <param name="intermissionTime">How long the lobby sits at menu before next match.</param>
        public NetLobbySettings(string lobbyName, string lobbyDesc, float intermissionTime) {
            Name        = StringUtils.Clamp(lobbyName, NameLengthLimit);
            Description = StringUtils.Clamp(lobbyDesc, DescriptionLengthLimit);

            IntermissionTime = intermissionTime;
        }

        /// <summary>
        /// Recreate lobby settings from their byte values.
        /// </summary>
        /// <param name="bytes">The encoded settings.</param>
        /// <param name="startBit">The first bit of the settings.</param>
        public NetLobbySettings(byte[] bytes, int startBit) {
            ByteBuffer buffer = GetContent(bytes, startBit, Type);

            Name             = buffer.ReadString();
            Description      = buffer.ReadString();
            IntermissionTime = buffer.ReadFloat();
        }

        /// <summary>
        /// Recreate the lobby settings from a
        /// byte buffer that was passed in.
        /// </summary>
        /// <param name="buffer">The buffer containing the settings.</param>
        public NetLobbySettings(ByteBuffer buffer) {
            buffer.SkipReadingBits(32);

            Name             = buffer.ReadString();
            Description      = buffer.ReadString();
            IntermissionTime = buffer.ReadFloat();
        }
        #endregion

        #region Serialization
        /// <summary>
        /// Serialize the contents of the settings
        /// so they can be re built later on.
        /// </summary>
        /// <param name="buffer">The buffer to write to.</param>
        protected override void SerializeContent(ByteBuffer buffer) {
            buffer.Write(Name);
            buffer.Write(Description);
            buffer.Write(IntermissionTime);
        }
        #endregion
    }
}
