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
        public IntermissionDuration IntermissionTime { get; private set; }

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
        /// Create a new instance of the lobby settings.
        /// </summary>
        /// <param name="intermissionTime">How long between matches.</param>
        /// <param name="matchSelectionMode">How the next match is picked.</param>
        public NetLobbySettings(IntermissionDuration intermissionTime, SelectorMode matchSelectionMode) {
            IntermissionTime   = intermissionTime;
            MatchSelectionMode = matchSelectionMode;
        }

        /// <summary>
        /// Recreate lobby settings from their byte values.
        /// </summary>
        /// <param name="bytes">The encoded settings.</param>
        /// <param name="startBit">The first bit of the settings.</param>
        public NetLobbySettings(byte[] bytes, int startBit) {
            ByteBuffer buffer = GetContent(bytes, startBit, Type);

            IntermissionTime   = (IntermissionDuration)buffer.ReadByte();
            MatchSelectionMode = (SelectorMode)buffer.ReadByte();
        }

        /// <summary>
        /// Recreate the lobby settings from a
        /// byte buffer that was passed in.
        /// </summary>
        /// <param name="buffer">The buffer containing the settings.</param>
        public NetLobbySettings(ByteBuffer buffer) {
            buffer.SkipReadingBits(32);

            IntermissionTime   = (IntermissionDuration)buffer.ReadByte();
            MatchSelectionMode = (SelectorMode)buffer.ReadByte();
        }
        #endregion

        #region Serialization
        /// <summary>
        /// Serialize the contents of the settings
        /// so they can be re built later on.
        /// </summary>
        /// <param name="buffer">The buffer to write to.</param>
        protected override void SerializeContent(ByteBuffer buffer) {
            buffer.Write((byte)IntermissionTime);
            buffer.Write((byte)MatchSelectionMode);
        }
        #endregion
    }
}
