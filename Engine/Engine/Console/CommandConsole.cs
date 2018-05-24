using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Network;
using Voxelated.Utilities;

namespace Voxelated.Engine.Console {
    /// <summary>
    /// Command console for parsing inputs from Unity
    /// </summary>
    public class CommandConsole {
        #region Constants
        /// <summary>
        /// The char that commands need to be started with.
        /// </summary>
        public const char EscapeChar = '/';
        #endregion

        #region Properties
        /// <summary>
        /// The list of available commands.
        /// </summary>
        public List<Command> Commands { get; private set; }
        #endregion

        #region Constructor(s)
        public CommandConsole() {
            Commands = GetCommands();
        }
        #endregion

        #region Publics
        /// <summary>
        /// Parses a string for a command
        /// </summary>
        public void Parse(string input) {
            //Check if it starts with the escape char
            if(string.IsNullOrEmpty(input) || input[0] != EscapeChar) {
                LoggerUtils.Log("Invalid Command. Type /help for assistance.");
                return;
            }

            string[] splitInput = input.Split(' ');
            string keyword = splitInput[0].TrimStart(EscapeChar);

            //Find the command and execute if it's not null
            Command command = Commands.Find(c => c.Keyword == keyword);

            if(command != null) {
                if(command.ArgumentCount > 0) {
                    string[] arguments = splitInput.Skip(1).ToArray();
                    command.Execute(arguments);
                }
                else {
                    command.Execute();
                }
            }
            else {
                LoggerUtils.Log("/" + keyword + " is not recognized. Type /help for assistances.");
            }
        }
        #endregion

        #region Statics
        /// <summary>
        /// Get a list of the commands that can be used.
        /// </summary>
        public static List<Command> GetCommands() {
            var commands = typeof(Command).Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(Command)) && !t.IsAbstract)
                .Select(t => (Command)Activator.CreateInstance(t));

            return commands.ToList();
        }
        
        /// <summary>
        /// Find a command based off it's keyword.
        /// </summary>
        /// <param name="keyWord">The command keyword to look for.</param>
        /// <returns>The command that matches the keyword. (If any).</returns>
        public static Command GetCommand(string keyWord) {
            return GetCommands().Find(c => c.Keyword == keyWord);
        }
        #endregion
    }
}
