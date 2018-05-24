using System;
using System.Collections.Generic;
using System.Linq;
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
    public class DisconnectCommand : Command {
        #region Properties
        /// <summary>
        /// How many commands it expects.
        /// </summary>
        public override int ArgumentCount {
            get { return 0; }
        }

        /// <summary>
        /// The opcode of the command
        /// </summary>
        public override string Keyword {
            get { return "disconnect"; }
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
            get { return "Disconnect from a server."; }
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Attempt to connect. Don't worry about checking if
        /// arguments is correct length, base class does that.
        /// </summary>
        protected override void ExecuteCommand(params string[] arguments) {
            NetClientManager client = VoxelatedEngine.Engine.NetManager as NetClientManager;

            if (client != null) {
                client.Disconnect();
            }
            else {
                LoggerUtils.LogError("Can't disconnect from nothing!");
            }
        }
        #endregion
    }
}
