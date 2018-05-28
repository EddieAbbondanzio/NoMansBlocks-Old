using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Network;
using Voxelated.Utilities;
using LiteNetLib;
using Voxelated.Network.Client;

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
            string[] splitAddress = arguments[0].Split(':');

            if(splitAddress.Length == 2) {
                int port = 0;
                
                if(int.TryParse(splitAddress[1], out port)) {
                    NetEndPoint serverAddress = new NetEndPoint(splitAddress[0], port);

                    if (serverAddress != null) {
                        NetClientManager client = VoxelatedEngine.Engine.NetManager as NetClientManager;
                        //LoggerUtils.Log("Connect to: " + serverAddress.ToString());

                        if(client != null) {
                            client.Connect(serverAddress);
                        }
                        else {
                            LoggerUtils.LogError("ConnectCommand: NetClient was null.");
                        }
                    }
                }
            }
            else {
                LoggerUtils.LogError("Unable to parse server address.");
            }
        }
        #endregion
    }
}
