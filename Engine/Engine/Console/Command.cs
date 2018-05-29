using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Network;
using Voxelated.Serialization;
using Voxelated.Utilities;

namespace Voxelated.Engine.Console {
    /// <summary>
    /// Base class for commands. Provides some default
    /// functionality.
    /// </summary>
    public abstract class Command {
        #region Properties
        /// <summary>
        /// The op code of the command
        /// </summary>
        public abstract string Keyword { get; }

        /// <summary>
        /// The number of arguments required for the command
        /// </summary>
        public abstract int ArgumentCount { get; }

        /// <summary>
        /// The permissions required to call the command
        /// </summary>
        public abstract NetPermissions PermissionRequired { get; }

        /// <summary>
        /// Controls where the command can be called from.
        /// </summary>
        public abstract CommandType Type { get; }

        /// <summary>
        /// The message to display when /help is called.
        /// </summary>
        public abstract string HelpMessage { get; }
        #endregion

        #region Publics
        /// <summary>
        /// Execute the command.
        /// </summary>
        public bool Execute(params string[] arguments) {
            NetPermissions callerPerms = NetPermissions.Server;//VoxelatedEngine.NetManager.Permissions;

            //If the caller's permissions aren't high enough. Cancel
            if(callerPerms < PermissionRequired) {
                LoggerUtils.LogError("Your permission level isn't high enough for the " + Keyword + " command.");
                return false;
            }

            //Check if the arg count is correct
            if(arguments.Length < ArgumentCount) {
                LoggerUtils.LogError("Incorrect number of arguments recieved. The " + Keyword + " command requires " + ArgumentCount);
                return false;
            }

            //Now perform the action
            ExecuteCommand(arguments);
            return true;
        }
        #endregion

        #region Privates
        /// <summary>
        /// This is the method that actually does the command. It's hidden
        /// to the base class to ensure validation is performed
        /// first.
        /// </summary>
        protected abstract void ExecuteCommand(params string[] arguments);
        #endregion

        #region Overrides
        /// <summary>
        /// Convert the command into a byte array
        /// that can be later used to rebuild the command.
        /// </summary>
        /// <returns>The command encoded in a byte array.</returns>
        public byte[] Serialize() {
            byte[] bytes = null; // SerializeUtils.Serialize(Keyword);
            return bytes;
        }
        #endregion
    }
}
