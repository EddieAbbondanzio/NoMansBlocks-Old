using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Network;
using Voxelated.Utilities;

namespace Voxelated.Engine.Console.Commands {
    /// <summary>
    /// Allows a server to begin hosting a new lobby.
    /// </summary>
    public class HostCommand : Command {
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
            get { return "host"; }
        }

        /// <summary>
        /// The permissions level required to call it.
        /// </summary>
        public override NetPermissions PermissionRequired {
            get { return NetPermissions.Server; }
        }

        /// <summary>
        /// Client or server can call this.
        /// </summary>
        public override CommandType Type {
            get { return CommandType.Common; }
        }

        /// <summary>
        /// The message to display when help is called.
        /// </summary>
        public override string HelpMessage {
            get { return "Hosts a new server lobby."; }
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Performs the operation to begin hosting a new lobby.
        /// </summary>
        protected override void ExecuteCommand(params string[] arguments) {
            NetServerManager server = VoxelatedEngine.Engine.NetManager as NetServerManager;

            if(server != null) {
                server.Host();
            }
            else {
                LoggerUtils.LogError("Console: Unable to start server!");
            }
        }
        #endregion
    }
}
