using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxelated;

namespace NoMansBlocks.Prefab {
    /// <summary>
    /// Prefab Pool retains a collection of gameobjects
    /// of a specific prefab type. It handles giving out 
    /// instances to use instead of creating new ones during
    /// run time.
    /// </summary>
    public class PrefabPool : MonoBehaviour {
        #region Properties
        /// <summary>
        /// The type of prefab the pool contains.
        /// </summary>
        public PrefabType Type;

        /// <summary>
        /// The target size of the pool. 
        /// </summary>
        public int Size;

        /// <summary>
        /// The prefab of the pool.
        /// </summary>
        public GameObject Prefab;
        #endregion

        #region Members
        /// <summary>
        /// The current index of the next available
        /// instance.
        /// </summary>
        private int currentInstance;

        /// <summary>
        /// If instances are being added to the pool.
        /// </summary>
        private bool addingInstances;

        /// <summary>
        /// If instances are being removed from the pool.
        /// </summary>
        private bool removingInstances;
        #endregion

        #region MonoEvents
        /// <summary>
        /// Sets the pool up.
        /// </summary>
        private void Awake() {
            addingInstances = false;
            removingInstances = false;

            currentInstance = 0;
        }

        /// <summary>
        /// When the pool is oversized and has inactive objects remove some.
        /// </summary>
        private void Update() {
            if (!addingInstances && !removingInstances) { 
                if (transform.childCount != Size && currentInstance < Size) {
                    UpdatePoolSize();
                }
            }
        }

        /// <summary>
        /// Called when an editor field is modified.
        /// </summary>
        private void OnValidate() {
            //Ensure pool is greater than or equal to 1.
            if(Size < 1) {
                Size = 1;
            }

            UpdatePoolSize();
        }
        #endregion

        #region Publics
        /// <summary>
        /// Returns an instance from the pool. If the
        /// pool is of fixed type, it may return null.
        /// </summary>
        public GameObject GetPooledInstance() {
            return GetInstance();
        }

        /// <summary>
        /// Returns an instance from the pool at the
        /// desired location. If the pool is fixed,
        /// and it may return null.
        /// </summary>
        public GameObject GetPooledInstance(Vector3 pos) {
            GameObject instance = GetInstance();
            instance.transform.position = pos;

            return instance;
        }

        /// <summary>
        /// Returns an instance from the pool at the desired location.
        /// If the pool is fixed, it may return null. The prefab is also
        /// set to static.
        /// </summary>
        public GameObject GetPooledInstance(Vector3 pos, bool isStatic) {
            GameObject instance = GetInstance();
            instance.transform.position = pos;
            instance.isStatic = isStatic;

            return instance;
        }

        /// <summary>
        /// Return an instance back to the pool.
        /// </summary>
        public void ReturnInstance(GameObject obj) {
            if(obj == null) {
                return;
            }

            PrefabInstance instance = obj.GetComponent<PrefabInstance>();
            if(instance?.Type == Type && instance?.Pool == this) {
                obj.SetActive(false);
                currentInstance--;

                obj.isStatic = false;
                obj.transform.SetSiblingIndex(currentInstance);
                obj.transform.position = Vector3.zero;
            }
        }
        #endregion

        #region Helpers
        /// <summary>
        /// Get an instance from the pool. If the pool has none
        /// to give out, create one.
        /// </summary>
        /// <returns></returns>
        private GameObject GetInstance() {
            if(currentInstance == transform.childCount) {
                AddInstance();
            }

            GameObject instance = transform.GetChild(currentInstance).gameObject;
            instance.SetActive(true);

            PrefabInstance prefabInstance = instance.GetComponent<PrefabInstance>();
            if(prefabInstance != null) {
                prefabInstance.Pool = this;
            }

            currentInstance++;
            return instance;
        }

        /// <summary>
        /// Add another instance to the pool.
        /// </summary>
        private void AddInstance() {
            GameObject newObj = GameObject.Instantiate(Prefab, this.transform);
            newObj.SetActive(false);
        }

        /// <summary>
        /// Remove an instance, ensure it's inactive first.
        /// </summary>
        private void RemoveInstance() {
            GameObject lastInstance = transform.GetChild(currentInstance).gameObject;

            if (!lastInstance.activeSelf) {
                DestroyImmediate(lastInstance);
            }
        }

        /// <summary>
        /// Gets the pool size back to it's correct value.
        /// </summary>
        private void UpdatePoolSize() {
            //Add some if we need it.
            if (transform.childCount < Size) {
                StartCoroutine(AddInstances(Size - transform.childCount));
            }
            //Remove some theres too many
            else if (transform.childCount > Size) {
                StartCoroutine(RemoveInstances(transform.childCount - Size));
            }
        }

        /// <summary>
        /// Adds several new instance to the pool.
        /// </summary>
        private IEnumerator AddInstances(int count) {
            addingInstances = true;

            yield return null;

            for (int i = 0; i < count; i++) {
                AddInstance();

                if (i != 0 && i % 16 == 0) {
                    yield return null;
                }
            }

            addingInstances = false;
        }


        /// <summary>
        /// Removes more than 1 instance in the pool.
        /// </summary>
        private IEnumerator RemoveInstances(int count) {
            removingInstances = true;

            yield return null;

            for(int i = 0; i < count; i++) {
                RemoveInstance();

                if(i != 0 && i % 16 == 0) {
                    yield return null; 
                }
            }

            removingInstances = false;
        }
        #endregion
    }
}