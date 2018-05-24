using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using Voxelated.Engine.Console;

namespace Voxelated.Network.Messages {
    /// <summary>
    /// Allows for the client to send a command for the
    /// server to parse.
    /// </summary>
    public class CommandMessage : NetMessage {
        #region Properties
        /// <summary>
        /// The type of message it is.
        /// </summary>
        public override NetMessageType Type {
            get { return NetMessageType.Command; }
        }

        /// <summary>
        /// The category of the message
        /// </summary>
        public override NetMessageCategory Category {
            get { return NetMessageCategory.Lobby; }
        }

        /// <summary>
        /// The command that the client wants performed
        /// </summary>
        public Command Command { get; private set; }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new command message to send to
        /// the server.
        /// </summary>
        public CommandMessage(Command command) {
            if(command == null) {
                throw new ArgumentException("Argument: command is invalid.");
            }

            Command = command;
        }

        /// <summary>
        /// Decode a command message that was recieved
        /// from over the network.
        /// </summary>
        public CommandMessage(NetIncomingMessage inMsg) : base() {
          //  Command = DecodeObject(inMsg) as Command;
        }
        #endregion

        #region Overrides
        ///// <summary>
        ///// Pack up the command for sending out over
        ///// the network.
        ///// </summary>
        //protected override void EncodeContent(NetOutgoingMessage outMsg) {
        //    EncodeObject(outMsg, Command);
        //}
        #endregion
    }
}
