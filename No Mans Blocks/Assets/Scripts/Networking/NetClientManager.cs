//using Lidgren.Network;
//using System.Net;
//using Voxelated.Network.Messages;
//using Voxelated.Network.Player;
//using System;
//using Voxelated.Events;
//using Voxelated.Utilities;
//using Voxelated.Network.Lobby;

//namespace Voxelated.Network.Client {
//    /// <summary>
//    /// A client instance in the game network.
//    /// </summary>
//    public class NetClientManager : NetManager, IDisposable {
//        #region Properties
//        /// <summary>
//        /// Clients are never host.
//        /// </summary>
//        public override bool IsHost {
//            get { return false; }
//        }
//        #endregion

//        #region Members
//        /// <summary>
//        /// The lidgren netclient instance.
//        /// </summary>
//        private NetClient netClient;

//        /// <summary>
//        /// The player of the client.
//        /// </summary>
//        private NetPlayer player;

//        /// <summary>
//        /// The unique id used to verify identity
//        /// with the server.
//        /// </summary>
//        private uint connectionKey;

//        /// <summary>
//        /// If the manager has been disposed of yet.
//        /// </summary>
//        private bool disposed;
//        #endregion

//        #region Constructors / Destructor
//        /// <summary>
//        /// Create a new net client manager that checks
//        /// for messages 60 times a second.
//        /// </summary>
//        public NetClientManager() : base() {
//            NetMessageHandler.OnInfoMessage += HandleInfoMessage;
//            NetMessageHandler.OnLobbySyncMessage += HandleLobbySyncMessage;

//            //Set the permissions level.
//            Permissions = Permissions.Player;
//            disposed = false;
//        }

//        ~NetClientManager() {
//            Dispose();
//        }
//        #endregion

//        #region Message Events
//        /// <summary>
//        /// Handle an info message from the server.
//        /// </summary>
//        private void HandleInfoMessage(object sender, NetMessageArgs e) {
//            InfoMessage infoMsg = e.Message as InfoMessage;
//            LoggerUtils.Log(infoMsg.Information);
//        }

//        /// <summary>
//        /// When the lobby sync message is recieved from the server.
//        /// </summary>
//        private void HandleLobbySyncMessage(object sender, NetMessageArgs e) {
//            LobbySyncMessage syncMsg = e.Message as LobbySyncMessage;

//            if (syncMsg == null) {
//                LoggerUtils.LogError("Invalid lobby sync message recieved");
//                return;
//            }

//            this.connectionKey = syncMsg.ConnectionKey;
//            this.player = syncMsg.LocalPlayer;
//            this.Lobby = syncMsg.Lobby;
//            this.Chat = new NetChat(this.player.Name);

//            LoggerUtils.Log("Lobby synced up. Local Player name: " + syncMsg.LocalPlayer.Name + ". Current lobby size: " + syncMsg.Lobby.CurrentSize);
//        }
//        #endregion

//        #region Publics
//        /// <summary>
//        /// Connect to a lobby.
//        /// </summary>
//        public void Connect(IPEndPoint serverAddress, string nickName) {
//            //Set up the configuration.
//            var config = new NetPeerConfiguration("Voxelated") {
//                //SimulatedMinimumLatency = 0.2f,
//                //SimulatedLoss = 0.1f
//            };

//            //Get the client going.
//            netClient = new NetClient(config);
//            netClient.Start();

//            NetOutgoingMessage hailMsg = CreateMessage();
//            hailMsg.Write(nickName);

//            netClient.Connect(serverAddress, hailMsg);
//        }

//        /// <summary>
//        /// Disconnect from the server.
//        /// </summary>
//        public void Disconnect() {
//            netClient.Disconnect("Bye!");
//            Lobby = null;
//        }

//        /// <summary>
//        /// Send a message to the lobby
//        /// </summary>
//        public override void SendMessage(NetMessage netMsg, NetDeliveryMethod method, NetChannel channel) {
//            NetOutgoingMessage outMsg = BuildMessage(netMsg);
//            netClient.SendMessage(outMsg, method, (int)channel);
//        }

//        /// <summary>
//        /// Set the connection key of the client.
//        /// </summary>
//        public void SetConnectionKey(uint connectionKey) {
//            this.connectionKey = connectionKey;
//        }

//        /// <summary>
//        /// Set the local player of the client.
//        /// </summary>
//        public void SetPlayer(NetPlayer player) {
//            this.player = player;
//        }

//        /// <summary>
//        /// Returns a copy of the player for this client.
//        /// </summary>
//        public NetPlayer GetPlayerInfo() {
//            return player.GetCopy();
//        }
//        #endregion

//        #region Protecteds
//        /// <summary>
//        /// Create a new outgoing message
//        /// </summary>
//        protected override NetOutgoingMessage CreateMessage() {
//            return netClient.CreateMessage();
//        }

//        /// <summary>
//        /// Read the message that was recieved.
//        /// </summary>
//        protected override NetIncomingMessage ReadMessage() {
//            return netClient.ReadMessage();
//        }

//        /// <summary>
//        /// Call this after reading a message.
//        /// </summary>
//        protected override void Recycle(NetIncomingMessage im) {
//            netClient.Recycle(im);
//        }
//        #endregion

//        #region Helpers
//        /// <summary>
//        /// Adds client unique id to the outgoing message.
//        /// Target of 0 is server, 1-31 are players, 255 is all.
//        /// </summary>
//        protected override NetOutgoingMessage BuildMessage(NetMessage netMsg) {
//            NetOutgoingMessage outMsg = CreateMessage();

//            //Build server message header
//            outMsg.Write(connectionKey);
//            outMsg.Write(player.Id);

//            //Add regular message
//            netMsg.Encode(outMsg);
//            return outMsg;
//        }

//        #endregion

//        /// <summary>
//        /// Remove handles from events
//        /// </summary>
//        public void Dispose() {
//            if (disposed) {
//                return;
//            }

//            NetMessageHandler.OnInfoMessage -= HandleInfoMessage;
//            NetMessageHandler.OnLobbySyncMessage -= HandleLobbySyncMessage;

//            disposed = true;
//        }
//    }
//}