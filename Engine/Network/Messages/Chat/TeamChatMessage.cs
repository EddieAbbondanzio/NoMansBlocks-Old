using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Network.Lobby;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Voxelated.Network.Messages {
    /// <summary>
    /// Chat message that's going to all clients in a team.
    /// </summary>
    public class TeamChatMessage : NetMessage {
        #region Properties
        /// <summary>
        /// The type of the message
        /// </summary>
        public override NetMessageType Type {
            get { return NetMessageType.TeamChat; }
        }

        /// <summary>
        /// The category of message it is.
        /// </summary>
        public override NetMessageCategory Category {
            get { return NetMessageCategory.Chat; }
        }

        /// <summary>
        /// The team of who sent it.
        /// </summary>
        public NetTeamColor Team { get; private set; }

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
        /// Create a new outgoing team chat message.
        /// </summary>
        /// <param name="team">The color of the team to send it to.</param>
        /// <param name="senderName">The name of who sent it.</param>
        /// <param name="message">The message being sent.</param>
        public TeamChatMessage(NetTeamColor team, string senderName, string message) {
            Team       = team;
            SenderName = senderName;
            Message    = message;

            buffer.Write((byte)team);
            buffer.Write(senderName);
            buffer.Write(message);
        }

        /// <summary>
        /// Rebuild an incoming team chat message.
        /// </summary>
        /// <param name="inMsg">The message it arrived on.</param>
        public TeamChatMessage(NetPeer sender, NetDataReader reader) : base(sender, reader) {
            Team       = (NetTeamColor)buffer.ReadByte();
            SenderName = buffer.ReadString();
            Message    = buffer.ReadString();
        }
        #endregion

        #region Publics
        /// <summary>
        /// Convert the team chat message into a print
        /// friendly format for displaying.
        /// </summary>
        /// <returns>The message as a string.</returns>
        public override string ToString() {
            return string.Format("({0}) {1}: {2}", Team, SenderName, Message);
        }
        #endregion
    }
}
