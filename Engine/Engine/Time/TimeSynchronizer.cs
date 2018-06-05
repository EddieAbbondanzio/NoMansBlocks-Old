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
        /// Increment counter, and check to see if another time sync needs to be sent.
        /// </summary>
        /// <param name="deltaTime">The time that's passed since
        /// the last frame.</param>
        public void Update(float deltaTime) {
            timeSinceLastSync += deltaTime;

            if(timeSinceLastSync > TimeBetweenSyncs && !netManager.IsServer) {
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
