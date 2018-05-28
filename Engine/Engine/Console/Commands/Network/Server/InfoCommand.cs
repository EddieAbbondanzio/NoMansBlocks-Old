using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Network;
using Voxelated.Network.Server;
using Voxelated.Utilities;

namespace Voxelated.Engine.Console.Commands {
    /// <summary>
    /// Command that prints out some info on the current
    /// state of the game lobby.
    /// </summary>
    [Serializable]
    public class InfoCommand : Command {
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
            get { return "info"; }
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
            get { return "Displays some info about the lobby."; }
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Performs the operation to begin hosting a new lobby.
        /// </summary>
        protected override void ExecuteCommand(params string[] arguments) {
            NetServerManager server = VoxelatedEngine.Engine.NetManager as NetServerManager;

            if(server != null) {
                LoggerUtils.Log("Console: Current Lobby Info:", LogLevel.Release);
                LoggerUtils.Log("Console: Number of Players: " + server.Lobby.PlayerCount, LogLevel.Release);
            }
        }
        #endregion
    }
}
