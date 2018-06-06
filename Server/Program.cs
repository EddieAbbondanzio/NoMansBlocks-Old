using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Network;
using Voxelated.Network.Messages;
using Voxelated.Network.Server;
using Voxelated.Utilities;

namespace Voxelated.Server {
    /// <summary>
    /// The entry point for the applicaiton. The server app itself
    /// does nothing more than process commands.
    /// </summary>
    public class Program {
        #region Properties
        /// <summary>
        /// Easy to save reference of the server
        /// </summary>
        private static VoxelatedServer server;
        #endregion

        public static void Main(string[] args) {
            PrintHeader();
            LoggerUtils.SetLogProfile(LogProfile.ConsoleDebug);

            server = new VoxelatedServer();
            server.Start();
            server.OnStop += OnStop;

            ITimer testTimer = Time.CreateNewTimer(15.0f);

            server.Console.Parse("/host");

            //While it's running, keep accepting commands
            while (server.IsRunning) {
                string input = Console.ReadLine();
                server.Console.Parse(input);
            }
        }

        /// <summary>
        /// Called when the voxelated engine is shutting down.
        /// </summary>
        private static void OnStop(object sender, EventArgs e) {
            if (server.NetManager != null) {
                NetServerManager serverManager = server.NetManager as NetServerManager;
                serverManager.Stop();
            }

            LoggerUtils.Log("Server stopped. Press any key to close window...");
            Console.ReadKey();
        }

        /// <summary>
        /// Prints out some useful(ish) info to the console.
        /// </summary>
        private static void PrintHeader() {
            Console.WriteLine("======================================================");
            Console.WriteLine("|               Voxelated Game Server                |");
            Console.WriteLine("|                     v0.1.4.18                      |");
            Console.WriteLine("|                                                    |");
            Console.WriteLine("|                /help if your stuck                 |");
            Console.WriteLine("======================================================\n");
        }
    }
}
