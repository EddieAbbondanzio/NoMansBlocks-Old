using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Network.Lobby.Match;
using Voxelated.Serialization;
using Voxelated.Utilities;

namespace Voxelated.Network.Lobby {
    /// <summary>
    /// Various settings of the NetLobby class.
    /// Helps keep them containted in one spot
    /// and easier to serialize.
    /// </summary>
    public class NetLobbySettings : SerializableObject {
        #region Properties
        /// <summary>
        /// How many seconds to wait between games. Defaults out
        /// to 90 seconds.
        /// </summary>
        public float IntermissionTime { get; private set; } = 90.0f;

        /// <summary>
        /// If matches should be started automatically.
        /// </summary>
        public bool AutoStartMatches { get; private set; } = true;

        /// <summary>
        /// How matches are picked by the lobby to play.
        /// </summary>
        public SelectorMode MatchSelectionMode { get; private set; }

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
        public NetLobbySettings() {
        }

        /// <summary>
        /// Settings for a networked lobby with a custom time
        /// between matches setting.
        /// </summary>
        /// <param name="intermissionTime">How long the lobby sits at menu before next match.</param>
        public NetLobbySettings(float intermissionTime) {
            IntermissionTime = intermissionTime;
        }

        /// <summary>
        /// Recreate lobby settings from their byte values.
        /// </summary>
        /// <param name="bytes">The encoded settings.</param>
        /// <param name="startBit">The first bit of the settings.</param>
        public NetLobbySettings(byte[] bytes, int startBit) {
            ByteBuffer buffer = GetContent(bytes, startBit, Type);

            IntermissionTime = buffer.ReadFloat();
            AutoStartMatches = buffer.ReadBool();
            
        }

        /// <summary>
        /// Recreate the lobby settings from a
        /// byte buffer that was passed in.
        /// </summary>
        /// <param name="buffer">The buffer containing the settings.</param>
        public NetLobbySettings(ByteBuffer buffer) {
            buffer.SkipReadingBits(32);

            IntermissionTime = buffer.ReadFloat();
            AutoStartMatches = buffer.ReadBool();
        }
        #endregion

        #region Serialization
        /// <summary>
        /// Serialize the contents of the settings
        /// so they can be re built later on.
        /// </summary>
        /// <param name="buffer">The buffer to write to.</param>
        protected override void SerializeContent(ByteBuffer buffer) {
            buffer.Write(IntermissionTime);
            buffer.Write(AutoStartMatches);
            buffer.Write((byte)MatchSelectionMode);
        }
        #endregion
    }
}
