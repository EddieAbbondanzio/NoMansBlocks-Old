using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles drawing sprite blocks. Also handles loading resources
/// for sprites. Sprites that don't change between mesh draws will be kept to
/// prevent unneeded use of resources.
/// </summary>
public class SpriteManager : MonoBehaviour {
    #region Members
    /// <summary>
    /// Each render key has a collection of sprites tied to it.
    /// This allows us to quickly retrieve all sprites for a mesh
    /// and modify them as needed.
    /// </summary>
    private Dictionary<string, List<SpriteEntity>> spriteEntities;
    #endregion

    #region Mono Events
    /// <summary>
    /// Initialize stuff on this object.
    /// </summary>
    private void Awake() {
        spriteEntities = new Dictionary<string, List<SpriteEntity>>();
    }

    private void Update() {
        
    }
    #endregion

    #region Publics
    /// <summary>
    /// Draws all the sprites for the inputted renderkey.
    /// </summary>
    public void DrawSpritesForMesh(string renderKey, Vector3[] spritePositions, byte[] spriteIds) {

    }

    /// <summary>
    /// Removes any sprites that have an owner id that
    /// matches renderKey.
    /// </summary>
    public void ClearSpritesForMesh(string renderKey) {
        //if the key has any sprites, get the list of objects for it.
        if (spriteEntities.ContainsKey(renderKey)) {
            List<SpriteEntity> spriteEntitites;
            this.spriteEntities.TryGetValue(renderKey, out spriteEntitites);

            //Return the gameobjects to the pool.
            if(spriteEntitites != null) {
                foreach(SpriteEntity entity in spriteEntitites) {

                }
            }

            //Lastly remove the reference
            this.spriteEntities.Remove(renderKey);
        }
    }
    #endregion
}
