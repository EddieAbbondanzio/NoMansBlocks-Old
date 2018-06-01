using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Network.Lobby;
using Voxelated.Utilities;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Voxelated.Network.Messages {
    /// <summary>
    /// Message to alert other players in the lobby
    /// that a new player has joined
    /// </summary>
    public class PlayerJoinedMessage : NetMessage {
        #region Properties
        /// <summary>
        /// The type of message it is.
        /// </summary>
        public override NetMessageType Type {
            get { return NetMessageType.PlayerJoined; }
        }

        /// <summary>
        /// The category of the message
        /// </summary>
        public override NetMessageCategory Category {
            get { return NetMessageCategory.Lobby; }
        }

        /// <summary>
        /// The id of the player joining
        /// </summary>
        public byte PlayerId { get; private set; }

        /// <summary>
        /// The name of the player joining.
        /// </summary>
        public string PlayerName { get; private set; }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a message for sending out to otber
        /// players to alert a new player has joined.
        /// </summary>
        /// <param name="playerId">The id of the new player.</param>
        /// <param name="playerName">The visible nickname of the player.</param>
        public PlayerJoinedMessage(byte playerId, string playerName) : base() {
            if (playerId == byte.MaxValue) {
                throw new ArgumentOutOfRangeException("PlayerId", "Id of 255 is reserved and cannot be used!");
            }

            if (!StringUtils.IsAlphaNumeric(playerName)) {
                throw new ArgumentException("PlayerName", "Cannot be null or only contain whitespace");
            }

            PlayerId = playerId;
            PlayerName = playerName;

            buffer.Write(playerId);
            buffer.Write(playerName);
        }

        /// <summary>
        /// Rebuild an incoming player message alert from the
        /// server.
        /// </summary>
        /// <param name="sender">The server.</param>
        /// <param name="reader">The data of the message.</param>
        public PlayerJoinedMessage(NetPeer sender, NetDataReader reader) : base(sender, reader) {
            PlayerId = buffer.ReadByte();
            PlayerName = buffer.ReadString();
        }
        #endregion
    }
}
