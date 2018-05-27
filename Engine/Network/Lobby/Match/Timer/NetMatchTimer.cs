using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Network.Lobby.Match {
    /// <summary>
    /// Timer for a NetMatch. These come in two flavors
    /// infinite, and count down. An infinite timer
    /// counts up for ever, whereas a count down timer starts
    /// at some limit.
    /// </summary>
    public class NetMatchTimer {
        #region Events
        /// <summary>
        /// Fired when the timer runs out of time.
        /// This only occurs on count down timers.
        /// </summary>
        public event EventHandler OnStop;
        #endregion

        #region Properties
        /// <summary>
        /// The mode that the timer is complying to.
        /// </summary>
        public NetMatchTimerMode Mode { get; private set; }

        /// <summary>
        /// How long the timer will run for. (If any).
        /// </summary>
        public double TimeLimit { get; private set; }

        /// <summary>
        /// How long the timer has been
        /// running for.
        /// </summary>
        public double CurrentTime { get; private set; }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new timer that never ends.
        /// </summary>
        public NetMatchTimer() {
            Mode = NetMatchTimerMode.Infinite;
            TimeLimit = -1;
        }

        /// <summary>
        /// Create a new timer that runs for
        /// a fixed number of seconds.
        /// </summary>
        /// <param name="timeLimit">The number of seconds
        /// to tick for.</param>
        public NetMatchTimer(double timeLimit) {
            Mode = NetMatchTimerMode.CountDown;
            TimeLimit = timeLimit;
        }
        #endregion

        #region Publics
        /// <summary>
        /// Start the timer.
        /// </summary>
        public void Start() {

        }

        /// <summary>
        /// Update the timer based on how much time has passed.
        /// </summary>
        /// <param name="deltaTime">The amount of seconds
        /// that has passed so far.</param>
        public void Update(double deltaTime) {
            switch (Mode) {
                case NetMatchTimerMode.CountDown:
                    CurrentTime -= deltaTime;

                    if(CurrentTime > TimeLimit) {
                        Stop();
                    }

                    break;
                case NetMatchTimerMode.Infinite:
                    CurrentTime += deltaTime;
                    break;
            }
        }

        /// <summary>
        /// Stop the timer early, before it
        /// reaches the end.
        /// </summary>
        public void Stop() {

            //Fire off the event if anyone is listening.
            if(OnStop != null) {
                OnStop(this, null);
            }
        }
        #endregion
    }
}
