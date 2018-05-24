using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Network;
using Voxelated.Terrain;
using Voxelated.Utilities;

namespace Voxelated.Engine.Console.Commands {
    /// <summary>
    /// Command to generate a world from
    /// randomness.
    /// </summary>
    [Serializable]
    public class GenerateWorldCommand : Command {
        #region Properties
        /// <summary>
        /// How many commands it expects.
        /// </summary>
        public override int ArgumentCount {
            get { return 2; }
        }

        /// <summary>
        /// The opcode of the command
        /// </summary>
        public override string Keyword {
            get { return "generate"; }
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
            get { return "Generates a new world from random noise."; }
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Performs the operation to convert the world.
        /// </summary>
        protected override void ExecuteCommand(params string[] arguments) {
            string typeString = arguments[0].Trim();
            string nameString = arguments[1].Trim();

            //Lol stupid way to cap 1st letter.
            typeString = char.ToUpper(typeString[0]) + typeString.Substring(1);

            //Try to get world type from it
            WorldType type;
            if(Enum.TryParse(typeString, out type)) {
                VoxelatedEngine.Engine.World.WorldHandler.Load(type, nameString);
            }
            else {
                LoggerUtils.LogError("Command Console: Invalid world type specified.");
            }
        }
        #endregion
    }
}
