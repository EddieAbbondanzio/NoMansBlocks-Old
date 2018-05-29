using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Network.Messages;

namespace Voxelated.Network.Time {
    /// <summary>
    /// Synchronized time across all clients
    /// and the server. Used to track lobby states.
    /// </summary>
    public class NetTime {
        #region Constants
        /// <summary>
        /// How many seconds to wait between syncing up time.
        /// </summary>
        private const float TimeBetweenSyncs = 30.0f;
        #endregion

        #region Properties
        /// <summary>
        /// The connected to server's time.
        /// </summary>
        public double ServerTime { get; private set; }

        /// <summary>
        /// The local net time. Only used for retrieving 
        /// server time.
        /// </summary>
        public double LocalTime { get; private set; }
        #endregion

        #region Members
        /// <summary>
        /// How long it's been since the last time sync.
        /// </summary>
        private float timeSinceLastSync;

        /// <summary>
        /// If another time sync request has been sent.
        /// </summary>
        private bool requestSent;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new NetTime instance.
        /// </summary>
        public NetTime() {
            ServerTime = 0;
            LocalTime = 0;

            timeSinceLastSync = 0.0f;
            requestSent = false;

            NetMessageListener.OnTimeMessage += OnTimeMessage;
        }

        /// <summary>
        /// Free up resources on destroy
        /// </summary>
        ~NetTime() {
            NetMessageListener.OnTimeMessage -= OnTimeMessage;
        }
        #endregion

        #region Message Handlers
        /// <summary>
        /// Process time messages recieved.
        /// </summary>
        /// <param name="sender">NetMessageListener</param>
        /// <param name="e">The time net message received.</param>
        private void OnTimeMessage(object sender, NetMessageArgs e) {
            if(e.Message.Type == NetMessageType.TimeSync) {
                TimeSyncMessage syncMsg = e.Message as TimeSyncMessage;

                if(syncMsg != null) {


                    requestSent = false;
                }
            }

        }
        #endregion

        #region Publics
        /// <summary>
        /// Update the netTime.
        /// </summary>
        /// <param name="deltaTime">How much time
        /// has passed since the last update.</param>
        public void Update(float deltaTime) {
            LocalTime += deltaTime;
            timeSinceLastSync += deltaTime;

            //Only clients need to send sync requests
            if(!requestSent && timeSinceLastSync > TimeBetweenSyncs && !VoxelatedEngine.Engine.NetManager.IsServer) {
                requestSent = true;

                //Send out the time request.
                TimeSyncRequestMessage timeRequest = new TimeSyncRequestMessage();
                VoxelatedEngine.Engine.NetManager.SendMessage(timeRequest, LiteNetLib.SendOptions.ReliableOrdered);
            }
        }

        /// <summary>
        /// Get the server's net time.
        /// </summary>
        /// <returns>The Server's net time, 0 if not
        /// connected to a server.</returns>
        public DateTime GetServerTime() {
            return DateTime.Now;
        }

        /// <summary>
        /// Get the local net peer's time.
        /// </summary>
        /// <returns>The current time of the local
        /// net peer.</returns>
        public DateTime GetLocalTime() {
            return DateTime.Now;
        }
        #endregion
    }
}
