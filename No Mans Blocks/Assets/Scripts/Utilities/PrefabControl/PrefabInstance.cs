using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoMansBlocks.Prefab {
    /// <summary>
    /// Tracks what prefab pool the prefab belongs
    /// to and allows for it to be returned back to it's pool
    /// if desired.
    /// </summary>
    public class PrefabInstance : MonoBehaviour {
        #region Properties
        /// <summary>
        /// The type of prefab it is.
        /// </summary>
        public PrefabType Type;

        /// <summary>
        /// The pool it belongs to.
        /// </summary>
        public PrefabPool Pool;
        #endregion

        #region Publics
        /// <summary>
        /// Returns the prefab back to it's pool owner.
        /// </summary>
        public void ReturnToPool() {
            Pool?.ReturnInstance(this.gameObject);
        }
        #endregion
    }
}