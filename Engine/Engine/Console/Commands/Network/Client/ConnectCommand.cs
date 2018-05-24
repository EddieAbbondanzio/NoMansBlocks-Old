using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Network;
using Voxelated.Utilities;

namespace Voxelated.Engine.Console.Commands {
    /// <summary>
    /// Connect to a server, input required is the connect
    /// end point.
    /// </summary>
    [Serializable]
    public class ConnectCommand : Command {
        #region Properties
        /// <summary>
        /// How many commands it expects.
        /// </summary>
        public override int ArgumentCount {
            get { return 1; }
        }

        /// <summary>
        /// The opcode of the command
        /// </summary>
        public override string Keyword {
            get { return "connect"; }
        }

        /// <summary>
        /// The permissions level required to call it.
        /// </summary>
        public override NetPermissions PermissionRequired {
            get { return NetPermissions.Guest; }
        }

        /// <summary>
        /// Only clients can call this.
        /// </summary>
        public override CommandType Type {
            get { return CommandType.Client; }
        }

        /// <summary>
        /// The message to display when help is called.
        /// </summary>
        public override string HelpMessage {
            get { return "Connect to a server."; }
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Attempt to connect. Don't worry about checking if
        /// arguments is correct length, base class does that.
        /// </summary>
        protected override void ExecuteCommand(params string[] arguments) {
            IPEndPoint serverAddress = NetUtils.ParseIPEndPoint(arguments[0]);

            if(serverAddress != null) {
                NetClientManager client = VoxelatedEngine.Engine.NetManager as NetClientManager;
                client.Connect(serverAddress);
            }
            else {
                LoggerUtils.LogError("Unable to parse server address.");
            }
        }
        #endregion
    }
}
