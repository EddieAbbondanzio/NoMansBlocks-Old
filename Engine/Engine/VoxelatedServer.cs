﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Engine;
using Voxelated.Network;
using Voxelated.Network.Server;

namespace Voxelated {
    /// <summary>
    /// Server instance of the engine.
    /// </summary>
    public class VoxelatedServer : VoxelatedEngine {
        #region Components
        /// <summary>
        /// The network manager of the server.
        /// </summary>
        public override NetManager NetManager { get; protected set; }
        #endregion

        #region Properties
        /// <summary>
        /// The server is never rendering.
        /// </summary>
        public override bool IsRenderingEnabled {
            get { return false; }
        }

        /// <summary>
        /// Default settings of the server, will allow them
        /// to be changed later on.
        /// </summary>
        public override VoxelatedSettings Settings {
            get { return VoxelatedSettings.ServerSettings; }
        }

        /// <summary>
        /// Time manager of the server.
        /// </summary>
        protected override Time Time {
            get {
                return time;
            }
        }
        #endregion

        #region Members
        /// <summary>
        /// The time of the server.
        /// </summary>
        private Time time;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new headless server instance
        /// of the voxelated game engine
        /// </summary>
        public VoxelatedServer() : base() {
            NetManager = new NetServerManager(new NetServerSettings("Test Server", "Please Ignore.", 32, NetPermissions.Player));
            time = new Time();
        }
        #endregion
    }
}
