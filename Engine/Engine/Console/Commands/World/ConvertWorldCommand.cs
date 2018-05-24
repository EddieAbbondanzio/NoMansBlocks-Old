using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Network;
using Voxelated.Terrain;

namespace Voxelated.Engine.Console.Commands {
    /// <summary>
    /// Command to convert a world from .vxl into
    /// the game and set it active.
    /// </summary>
    [Serializable]
    public class ConvertWorldCommand : Command {
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
            get { return "convert"; }
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
            get { return "Converts a world from .vxl format."; }
        }
        #endregion

        /// <summary>
        /// Performs the operation to convert the world.
        /// </summary>
        protected override void ExecuteCommand(params string[] arguments) {
            string fileName = arguments[0].Trim();

            VoxelatedEngine.Engine.World.WorldHandler.Load(WorldType.Converted, fileName);
        }
    }
}
