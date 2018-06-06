using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Network;
using Voxelated.Utilities;
using Voxelated.Network.Messages;

namespace Voxelated {
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

        #region Properties
        /// <summary>
        /// Handles maintaing an up to date
        /// server time for the client. And
        /// sending out time syncs for the server.
        /// </summary>
        internal TimeSynchronizer TimeSyncer { get; private set; }

        /// <summary>
        /// The factory controller that handles giving out
        /// and creating new timers for use by the engine.
        /// </summary>
        internal TimerFactory TimerFactory { get; set; }
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
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new time object.
        /// </summary>
        public Time() {
            localTime    = 0d;
            serverOffset = 0d;

            instance = this;
            TimerFactory = new TimerFactory();
            TimeSyncer = new TimeSynchronizer(this, TimerFactory);
        }
        #endregion

        #region Publics
        /// <summary>
        /// Increment time by the amount of time
        /// that has passed this frame.
        /// </summary>
        /// <param name="deltaTime">The amount of 
        /// seconds that has passed since the last frame.</param>
        public void Update(double deltaTime) {
            localTime += deltaTime;
            this.deltaTime = deltaTime;

            TimeSyncer.Update(deltaTime);
            TimerFactory.Update(deltaTime);
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

        /// <summary>
        /// Get a new timer from the Time synchronizer
        /// that is kept synced across the network.
        /// </summary>
        /// <param name="duration">How many seconds
        /// to run the timer for.</param>
        /// <returns>An interface reference back to the timer.</returns>
        public static ITimer CreateNewTimer(double duration) {
            if(instance != null) {
                return instance.TimerFactory.CreateNewTimer(duration);
            }
            else {
                return null;
            }
        }
        #endregion
    }
}
