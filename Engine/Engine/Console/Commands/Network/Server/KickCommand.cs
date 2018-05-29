using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Network;
using Voxelated.Network.Lobby;
using Voxelated.Network.Server;
using Voxelated.Utilities;

namespace Voxelated.Engine.Console.Commands {
    /// <summary>
    /// Command to kick a player from the game. Expects an
    /// argument of the players nickname
    /// </summary>
    [Serializable]
    public class KickCommand : Command {
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
            get { return "kick"; }
        }

        /// <summary>
        /// The permissions level required to call it.
        /// </summary>
        public override NetPermissions PermissionRequired {
            get { return NetPermissions.Trusted; }
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
            get { return "Kick a player from the lobby."; }
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Kicks a player
        /// </summary>
        protected override void ExecuteCommand(params string[] arguments) {
            NetServerManager server = VoxelatedEngine.Engine.NetManager as NetServerManager;

            if(server != null) {
                string name = arguments[0].Trim();

                //See if a reason was given
                if(arguments.Length > 1) {
                    StringBuilder sb = new StringBuilder();
                    for(int i = 1; i < arguments.Length; i++) {
                        sb.Append(arguments[i]);

                        if (i < arguments.Length - 1) {
                            sb.Append(" ");
                        }
                    }

                    string reason = sb.ToString();
                    server.ConnectionHandler.KickConnectionByPlayerName(name, reason);
                }
                else {
                    server.ConnectionHandler.KickConnectionByPlayerName(name, string.Empty);
                }
            }
        }
        #endregion
    }
}
