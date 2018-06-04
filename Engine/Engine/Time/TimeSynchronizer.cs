using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Network;
using Voxelated.Network.Messages;
using Voxelated.Utilities;

namespace Voxelated {
    /// <summary>
    /// Handles keeping time synced up with
    /// the network server. (When connected
    /// to one).
    /// </summary>
    public class TimeSynchronizer {
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
            private TimeSynchronizer timeSyncer;
            #endregion

            #region Constructor(s)
            /// <summary>
            /// Create a new timer that can begin instantly.
            /// </summary>
            /// <param name="timeSyncer">The time synchronizer parent of the timer.</param>
            /// <param name="id">The unique id of the timer.</param>
            /// <param name="duration">How long the timer will run for.</param>
            public Timer(TimeSynchronizer timeSyncer, byte id, double duration) {
                this.timeSyncer = timeSyncer;
                Id = id;
                Duration = duration;
                TimeRemaining = duration;
                StartTime = Time.ServerTime;
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
                Id        = id;
                StartTime = startTime;
                Duration  = duration;
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
                OnTimerFinished = null;
                timeSyncer.timers.Remove(this);
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
            }
            #endregion
        }

        #region Constants
        /// <summary>
        /// How long to wait between sending out syncs.
        /// </summary>
        private const float TimeBetweenSyncs = 15.0f;
        #endregion

        #region Members
        /// <summary>
        /// Quick reference back to the network manager.
        /// </summary>
        private NetManager netManager;

        /// <summary>
        /// The time of the engine.
        /// </summary>
        private Time time;

        /// <summary>
        /// When the time sync was sent.
        /// </summary>
        private double timeSyncSentAt;

        /// <summary>
        /// How long it's been since the
        /// last time sync.
        /// </summary>
        private double timeSinceLastSync;

        /// <summary>
        /// The timer ids that are free to be used.
        /// </summary>
        private ThreadableQueue<byte> availableTimerIds;

        /// <summary>
        /// The active timers being managed.
        /// </summary>
        private List<Timer> timers;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new time synchronizer that
        /// maintains net time.
        /// </summary>
        /// <param name="time">The time to maintain.</param>
        public TimeSynchronizer(Time time) {
            netManager = VoxelatedEngine.Engine.NetManager;
            this.time         = time;
            timeSinceLastSync = 0;
            timeSyncSentAt    = 0;

            NetMessageListener.OnTimeMessage += OnTimeMessage;
            if (!netManager.IsServer) {
               NetMessageListener.OnConnectionMessage += OnConnectionMessage;
            }
        }

        /// <summary>
        /// Free up resources.
        /// </summary>
        ~TimeSynchronizer() {
            NetMessageListener.OnTimeMessage -= OnTimeMessage;
            if (!netManager.IsServer) {
               NetMessageListener.OnConnectionMessage -= OnConnectionMessage;
            }
        }
        #endregion

        #region Message Handlers
        /// <summary>
        /// Whenever the net manager recieves a connection message.
        /// This is used to know when to send a time request message.
        /// </summary>
        /// <param name="sender">NetMessageListener</param>
        /// <param name="e">The connection message recieved.</param>
        private void OnConnectionMessage(object sender, NetMessageArgs e) {
            switch (e.Message?.Type) {
                //When a new server is connected to. Send it a time request.
                case NetMessageType.ConnectionAccepted:
                    SendSyncRequest();
                    break;

                //Disconnected from server. Wipe offset.
                case NetMessageType.Disconnected:
                    time.SetServerOffset(0);
                    break;
            }
        }

        /// <summary>
        /// When a time request, or sync message has been recieved.
        /// </summary>
        /// <param name="sender">NetMessageListener</param>
        /// <param name="e">The time message recieved.</param>
        private void OnTimeMessage(object sender, NetMessageArgs e) {
            switch (e.Message?.Type) {
                //Time sync message was recieved.
                case NetMessageType.TimeSync:
                    TimeSyncMessage incomingSync = e.Message as TimeSyncMessage;

                    if (incomingSync != null) {
                        time.SetServerOffset(incomingSync.ServerTime - timeSyncSentAt);
                    }

                    break;

                //Client is requesting a time sync message.
                case NetMessageType.TimeSyncRequest:
                    LiteNetLib.NetPeer msgSender = e.Message.Sender;
                    if (msgSender != null) {
                        TimeSyncMessage outgoingSync = new TimeSyncMessage(Time.LocalTime);
                        netManager.SendMessage(outgoingSync, msgSender, LiteNetLib.SendOptions.ReliableOrdered);
                    }
                    break;
            }
        }
        #endregion

        #region Publics
        /// <summary>
        /// Get a new timer from the Time synchronizer
        /// that is kept synced across the network.
        /// </summary>
        /// <param name="duration">How many seconds
        /// to run the timer for.</param>
        /// <returns>An interface reference back to the timer.</returns>
        public ITimer CreateNewTimer(double duration) {
            return null;
        }



        /// <summary>
        /// Increment counter, and check to see if another time sync needs to be sent.
        /// </summary>
        /// <param name="deltaTime">The time that's passed since
        /// the last frame.</param>
        public void Update(float deltaTime) {
            if (netManager.IsServer) {
                return;
            }

            timeSinceLastSync += deltaTime;

            if(timeSinceLastSync > TimeBetweenSyncs) {
                SendSyncRequest();
                timeSinceLastSync = 0;
            }
        }
        #endregion

        #region Helpers
        /// <summary>
        /// Send out a sync request for time to the
        /// server.
        /// </summary>
        private void SendSyncRequest() {
            TimeSyncRequestMessage timeReqMsg = new TimeSyncRequestMessage();
            netManager.SendMessage(timeReqMsg, LiteNetLib.SendOptions.ReliableOrdered);

            //Record when the time sync request was sent.
            timeSyncSentAt = Time.LocalTime;
        }
        #endregion
    }
}
