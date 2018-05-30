﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Network;
using Voxelated.Utilities;
using Voxelated.Network.Messages;

namespace Voxelated.Engine {
    /// <summary>
    /// Engine time. Used to sync clients with lobbies and run
    /// physics calculations with.
    /// </summary>
    public class Time {
        #region Statics
        /// <summary>
        /// The time on the local instance
        /// </summary>
        public static double LocalTime {
            get {
                return instance?.localTime ?? 0;
            }
        }

        /// <summary>
        /// The time on the server.
        /// </summary>
        public static double ServerTime {
            get {
                if(instance != null) {
                    return instance.localTime + instance.serverOffset;
                }
                else {
                    return 0;
                }
            }
        }

        /// <summary>
        /// The current time that has passed for this tick
        /// of the engine.
        /// </summary>
        public static double DeltaTime {
            get {
                return instance?.deltaTime ?? 0;
            }
        }
        #endregion

        #region Members
        /// <summary>
        /// The singleton reference of time.
        /// </summary>
        private static Time instance;

        /// <summary>
        /// The time of the local engine.
        /// </summary>
        private double localTime;

        /// <summary>
        /// Offset used to calculate server
        /// time based on local time.
        /// </summary>
        private double serverOffset;

        /// <summary>
        /// How much time has passed since
        /// the last frame.
        /// </summary>
        private double deltaTime;

        /// <summary>
        /// Handles maintaing an up to date
        /// server time for the client. And
        /// sending out time syncs for the server.
        /// </summary>
        private TimeSynchronizer timeSyncer;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new time object.
        /// </summary>
        public Time() {
            localTime    = 0d;
            serverOffset = 0d;

            instance = this;
            timeSyncer = new TimeSynchronizer(this);
        }
        #endregion

        #region Publics
        /// <summary>
        /// Increment time by the amount of time
        /// that has passed this frame.
        /// </summary>
        /// <param name="deltaTime">The amount of 
        /// seconds that has passed since the last frame.</param>
        public void Update(float deltaTime) {
            localTime += deltaTime;
            this.deltaTime = deltaTime;

            timeSyncer.Update(deltaTime);
        }

        /// <summary>
        /// Set the server offset from the local 
        /// time.
        /// </summary>
        /// <param name="offset">The offset of the
        /// server.</param>
        public void SetServerOffset(double offset) {
            serverOffset = offset;
        }
        #endregion
    }
}