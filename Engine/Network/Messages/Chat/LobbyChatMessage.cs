using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Network.Messages {
    /// <summary>
    /// Chat message that's going to every client currently
    /// connected in the lobby.
    /// </summary>
    public class LobbyChatMessage : NetMessage {
        #region Properties
        /// <summary>
        /// The type of message it is.
        /// </summary>
        public override NetMessageType Type {
            get { return NetMessageType.LobbyChat; }
        }

        /// <summary>
        /// The category of the message
        /// </summary>
        public override NetMessageCategory Category {
            get { return NetMessageCategory.Chat; }
        }

        /// <summary>
        /// The nick name of the sender of
        /// the message.
        /// </summary>
        public string SenderName { get; private set; }

        /// <summary>
        /// The chat message being sent.
        /// </summary>
        public string Message { get; private set; }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new outgoing message.
        /// </summary>
        /// <param name="senderName">The nickname of who sent it.</param>
        /// <param name="message">The text of the chat message.</param>
        public LobbyChatMessage(string senderName, string message) : base() {
            SenderName = senderName;
            Message    = message;

            buffer.Write(senderName);
            buffer.Write(message);
        }

        /// <summary>
        /// Rebuild an incoming chat messsage.
        /// </summary>
        /// <param name="inMsg">The message it arrived on.</param>
        public LobbyChatMessage(NetIncomingMessage inMsg) : base(inMsg) {
            SenderName = buffer.ReadString();
            Message    = buffer.ReadString();
        }
        #endregion

        #region Publics
        /// <summary>
        /// Convert the lobby chat message into a print
        /// friendly format for displaying.
        /// </summary>
        /// <returns>The message as a string.</returns>
        public override string ToString() {
            return string.Format("({0}) {1}: {2}", "Lobby", SenderName, Message);
        }
        #endregion
    }
}
