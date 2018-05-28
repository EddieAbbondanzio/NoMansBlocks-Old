using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Voxelated.Network.Messages {
    /// <summary>
    /// Message to indicate clients that a player has
    /// left the lobby.
    /// </summary>
    public class PlayerLeftMessage : NetMessage {
        #region Properties
        /// <summary>
        /// The type of message it is.
        /// </summary>
        public override NetMessageType Type {
            get { return NetMessageType.PlayerLeft; }
        }

        /// <summary>
        /// The category of the message
        /// </summary>
        public override NetMessageCategory Category {
            get { return NetMessageCategory.Lobby; }
        }

        /// <summary>
        /// The id of the player leaving
        /// </summary>
        public byte PlayerId { get; private set; }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new player left alert to send out.
        /// Player id is the id of the leaving player.
        /// </summary>
        /// <param name="playerId">The id of the player that left.</param>
        public PlayerLeftMessage(byte playerId) : base() {
            if (playerId == byte.MaxValue) {
                throw new ArgumentOutOfRangeException("PlayerId", "Id of 255 is reserved and cannot be used!");
            }

            PlayerId = playerId;
            buffer.Write(PlayerId);

        }

        /// <summary>
        /// Decode a message of a player leaving that
        /// was recieved from the network.
        /// </summary>
        /// <param name="inMsg">The lidgren message with the left message in it.</param>
        public PlayerLeftMessage(NetPeer sender, NetDataReader reader) : base(sender, reader) {
            PlayerId = buffer.ReadByte();
        }
        #endregion
    }
}
