//using Lidgren.Network;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Net;
//using UnityEngine;
//using Voxelated.Network;
//using Voxelated.Network.Lobby;

//namespace Voxelated.Network.Client {
//    /// <summary>
//    /// Script to allow the net client manager be attached to the
//    /// player game object.
//    /// </summary>
//    public class NetBehaviour : MonoBehaviour {
//        #region Properties
//        /// <summary>
//        /// The net client manager of this instance.
//        /// </summary>
//        public NetClientManager ClientManager { get; private set; }
//        #endregion

//        #region Mono Events
//        private void Awake() {
//            ClientManager = new NetClientManager();
//            ClientManager.Connect(NetUtility.Resolve("127.0.0.1", 14242), "Bert");
//        }

//        private void FixedUpdate() {
//            //Check for any recieved messages.
//            ClientManager.CheckForMessages();
//        }

//        private void OnApplicationQuit() {
//            ClientManager.Disconnect();
//        }

//        #endregion
//    }
//}