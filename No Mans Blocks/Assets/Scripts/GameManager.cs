using NoMansBlocks.Prefab;
using NoMansBlocks.Voxel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxelated;
using Voxelated.Engine;
using Voxelated.Network;
using Voxelated.Network.Messages;
using Voxelated.Utilities;

namespace NoMansBlocks {
    /// <summary>
    /// Master controller script for all key components
    /// of the game.
    /// </summary>
    [RequireComponent(typeof(MeshHandler))]
    public class GameManager : MonoBehaviour {
        #region Components
        /// <summary>
        /// The prefab controller. Manages object pools.
        /// </summary>
        public static PrefabController PrefabController { get; private set; }

        /// <summary>
        /// Finds and mantains a collection of rnder / collision
        /// meshes of the world. 
        /// </summary>
        public static MeshHandler MeshHandler { get; private set; }

        /// <summary>
        /// Responsible for finding and updating sprite gameobjects
        /// when needed.
        /// </summary>
        public static SpriteManager SpriteManager { get; private set; }

        /// <summary>
        /// Handles locating all of the gameobjects
        /// that voxelated uses.
        /// </summary>
        public static ObjectWrangler ObjectWrangler { get; private set; }
        #endregion

        private VoxelatedClient client;

        public bool sendMessage;

        public string text;

        private void Update() {
            if (sendMessage) {
                NetManager netManager = VoxelatedEngine.Engine.NetManager;

                LobbyChatMessage chatMsg = new LobbyChatMessage("BERT", text);
                text = "";

                netManager.SendMessage(chatMsg, Lidgren.Network.NetDeliveryMethod.ReliableOrdered, NetChannel.Chat);

                sendMessage = false;
            }
        }

        #region Mono Events
        private void Awake() {
            #if UNITY_EDITOR
                LoggerUtils.SetLogProfile(LogProfile.UnityDebug);
            #endif
        
            ObjectWrangler = new ObjectWrangler();
        }

        /// <summary>
        /// The voxel engine is started in Start. It can't start
        /// in awake since it relies on the MeshHandler to be ready
        /// </summary>
        private void Start() {
            ObjectWrangler.FindChunks();
            MeshHandler = GetComponent<MeshHandler>();
            client = new VoxelatedClient(MeshHandler);
            client.Start();
            client.Console.Parse("/connect 127.0.0.1:14242");
        }

        /// <summary>
        /// Called when Unity is closing down.
        /// </summary>
        private void OnDestroy() {
            NetClientManager clientManager = VoxelatedEngine.Engine.NetManager as NetClientManager;
            clientManager.Disconnect();
            client.Stop();
        }
        #endregion

        #region Setters
        /// <summary>
        /// Setter for the prefab controller component.
        /// If the component has already been set it returns false
        /// indicating the new prefab controller is a duplicate and should
        /// be destroyed.
        /// </summary>
        public static bool SetPrefabController(PrefabController prefabController) {
            if(PrefabController == null) {
                PrefabController = prefabController;
                return true;
            }
            else if(PrefabController == prefabController) {
                return true;
            }
            else {
                return false;
            }
        }

        /// <summary>
        /// Allows for the mesh handler component reference
        /// to be set. If it already is set it returns false
        /// to indicate the new one is a duplicate.
        /// </summary>
        public static bool SetMeshHandler(MeshHandler meshHandler) {
            if (MeshHandler == null) {
                MeshHandler = meshHandler;
                return true;
            }
            else if (MeshHandler == meshHandler) {
                return true;
            }
            else {
                return false;
            }
        }
        #endregion
    }
}