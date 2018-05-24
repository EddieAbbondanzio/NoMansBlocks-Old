using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Network;

namespace Voxelated.Engine.Console.Commands {
    /// <summary>
    /// Shuts down the game. 
    /// </summary>
    [Serializable]
    public class StopCommand : Command {
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
            get { return "stop"; }
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
            get { return CommandType.Server; }
        }

        /// <summary>
        /// The message to display when help is called.
        /// </summary>
        public override string HelpMessage {
            get { return "Shuts down the network manager."; }
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Performs the operation to stop the engine
        /// </summary>
        protected override void ExecuteCommand(params string[] arguments) {
            VoxelatedEngine.Engine?.Stop();
        }
        #endregion
    }
}
