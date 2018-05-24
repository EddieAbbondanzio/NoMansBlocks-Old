using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using Voxelated.Network.Lobby;
using Voxelated.Utilities;

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
        /// Decode a player joined message that was recieved from
        /// the network
        /// </summary>
        /// <param name="inMsg">The lidgren message with the joined message in it.</param>
        public PlayerJoinedMessage(NetIncomingMessage inMsg) : base(inMsg) {
            PlayerId = buffer.ReadByte();
            PlayerName = buffer.ReadString();
        }
        #endregion
    }
}
