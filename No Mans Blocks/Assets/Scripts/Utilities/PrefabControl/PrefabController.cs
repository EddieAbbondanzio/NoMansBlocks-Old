using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxelated;

namespace NoMansBlocks.Prefab {
    /// <summary>
    /// Manages object pools of various prefabs. Helps reduce in
    /// game lag of instantiaing / destroying gameobjects.
    /// </summary>
    public class PrefabController : MonoBehaviour {
        #region Properties
        /// <summary>
        /// The collection of prefab pools this
        /// controller handles.
        /// </summary>
        private Dictionary<PrefabType, PrefabPool> pools;
        #endregion

        #region Mono Events
        /// <summary>
        /// Set up this object.
        /// </summary>
        private void Awake() {
            pools = new Dictionary<PrefabType, PrefabPool>();

            PrefabPool[] prefabPools = GetComponentsInChildren<PrefabPool>();
            foreach (PrefabPool pool in prefabPools) {
                pools.Add(pool.Type, pool);
            }

            //Try to set the singleton reference. If it's already set destroy this
            bool isDupe = !GameManager.SetPrefabController(this);

            if (isDupe) {
                Destroy(this.gameObject);
            }
        }
        #endregion

        #region Publics
        /// <summary>
        /// Get an instance of the type from it's pool.
        /// </summary>
        public GameObject GetPooledInstance(PrefabType type) {
            PrefabPool pool;

            if(pools.TryGetValue(type, out pool)) {
                return pool.GetPooledInstance();
            }
            else {
                return null;
            }
        }
        
        /// <summary>
        /// Get an instance of the type from it's pool
        /// and set it's location.
        /// </summary>
        public GameObject GetPooledInstance(PrefabType type, Vector3 pos) {
            PrefabPool pool;

            if (pools.TryGetValue(type, out pool)) {
                return pool.GetPooledInstance();
            }
            else {
                return null;
            }
        }

        /// <summary>
        /// Get an instance of the type from it's
        /// pool and set if it's static, and it's position.
        /// </summary>
        public GameObject GetPooledInstance(PrefabType type, Vector3 pos, bool isStatic) {
            PrefabPool pool;

            if (pools.TryGetValue(type, out pool)) {
                return pool.GetPooledInstance(pos, isStatic);
            }
            else {
                return null;
            }
        }

        /// <summary>
        /// Return an instance back to it's pool.
        /// </summary>
        public void ReturnToPool(GameObject obj) {
            PrefabInstance instance = obj.GetComponent<PrefabInstance>();
            instance?.ReturnToPool();
        }
        #endregion
    }
}