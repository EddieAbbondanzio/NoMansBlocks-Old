using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Utilities;

namespace Voxelated {
    /// <summary>
    /// Controller for the timers that are ran
    /// in the engine.
    /// </summary>
    internal class TimerFactory {
        /// <summary>
        /// Timers are kept hidden to the Time Synchronizer to
        /// allow for the factory pattern to be followed. Timers start
        /// automatically since I don't want to sync them starting.
        /// </summary>
        private class Timer : ITimer {
            #region Events
            /// <summary>
            /// The event that goes off when the timer's 
            /// remaining time is 0.
            /// </summary>
            public event EventHandler OnTimerFinished;
            #endregion

            #region Properties
            /// <summary>
            /// If the timer is currently active, and counting
            /// down to zero.
            /// </summary>
            public bool IsActive { get { return TimeRemaining > 0; } }

            /// <summary>
            /// The unique id that can be used to 
            /// retrieve the timer from the synchronizer.
            /// </summary>
            public byte Id { get; private set; }

            /// <summary>
            /// The time that the timer began on.
            /// </summary>
            public double StartTime { get; private set; }

            /// <summary>
            /// How long the timer is running for.
            /// </summary>
            public double Duration { get; private set; }

            /// <summary>
            /// How much time is left until the timer goes off.
            /// </summary>
            public double TimeRemaining { get; private set; }
            #endregion

            #region Members
            /// <summary>
            /// Reference back to the synchronizer.
            /// </summary>
            private TimerFactory timerFactory;

            private bool hasGoneOff;
            #endregion

            #region Constructor(s)
            /// <summary>
            /// Create a new timer that can begin instantly.
            /// </summary>
            /// <param name="timerFactory">The time factory parent of the timer.</param>
            /// <param name="id">The unique id of the timer.</param>
            /// <param name="duration">How long the timer will run for.</param>
            public Timer(TimerFactory timerFactory, byte id, double duration) {
                this.timerFactory = timerFactory;
                Id = id;
                Duration = duration;
                TimeRemaining = duration;
                StartTime = Time.ServerTime;
                hasGoneOff = false;
            }

            /// <summary>
            /// Use this to rebuild a timer from the 
            /// network.
            /// </summary>
            /// <param name="id">The unique id of the timer.</param>
            /// <param name="startTime">When the timer started according
            /// to server time.</param>
            /// <param name="duration">How long the timer is running for.</param>
            public Timer(byte id, double startTime, double duration) {
                Id = id;
                StartTime = startTime;
                Duration = duration;
            }
            #endregion

            #region Publics
            /// <summary>
            /// End the timer early, and don't set off the
            /// on timer finished event.
            /// </summary>
            public void Stop() {
                //Null out any existing events waiting on this
                //timer. Where gonna remove every reference
                //so it get's collected by GC.
                timerFactory.RemoveTimer(this);
            }

            /// <summary>
            /// Increment the timer with how much time
            /// has passed.
            /// </summary>
            /// <param name="deltaTime">Time since the last update.</param>
            public void Update(float deltaTime) {
                if (IsActive) {
                    TimeRemaining -= deltaTime;
                }
                else if (!hasGoneOff) {
                    hasGoneOff = true;

                    if (OnTimerFinished != null) {
                        OnTimerFinished(this, null);
                    }
                }
            }
            #endregion
        }

        #region Constants
        /// <summary>
        /// The maximum number of timers allowed
        /// in the factory at once.
        /// </summary>
        private const int MaxTimerCount = 32;
        #endregion

        #region Members
        /// <summary>
        /// The timers being kept updated in the factory.
        /// </summary>
        private List<Timer> timers;

        /// <summary>
        /// The timer ids that are free to be used.
        /// </summary>
        private ThreadableQueue<byte> availableTimerIds;
        #endregion

        #region Constructor(s)
        public TimerFactory() {
            timers = new List<Timer>();
            availableTimerIds = new ThreadableQueue<byte>();

            for (byte b = 0; b < MaxTimerCount; b++) {
                availableTimerIds.Enqueue(b);
            }
        }
        #endregion

        #region Publics
        /// <summary>
        /// Get a new timer that is kept synced across the network.
        /// </summary>
        /// <param name="duration">How many seconds
        /// to run the timer for.</param>
        /// <returns>An interface reference back to the timer.</returns>
        public ITimer CreateNewTimer(double duration) {
            Timer timer = new Timer(this, availableTimerIds.Dequeue(), duration);
            timers.Add(timer);

            return timer as ITimer;
        }

        /// <summary>
        /// Update all the timers in the list.
        /// </summary>
        /// <param name="deltaTime">How much time has passed
        /// since the last frame.</param>
        public void Update(float deltaTime) {
            for(int i = 0; i < timers.Count(); i++) {
                timers[i].Update(deltaTime);

                //Remove inactive timers.
                if (!timers[i].IsActive) {
                    RemoveTimer(timers[i]);
                }
            }
        }
        #endregion

        #region Helpers
        /// <summary>
        /// Remove a timer from the factory. It has
        /// either gone off or is no longer needed.
        /// </summary>
        /// <param name="timer">The timer to remove.</param>
        private void RemoveTimer(Timer timer) {
            if(timer != null) {
                availableTimerIds.Enqueue(timer.Id);
                timers.Remove(timer);

                LoggerUtils.Log("removed timer!");
            }
        }
        #endregion
    }
}
