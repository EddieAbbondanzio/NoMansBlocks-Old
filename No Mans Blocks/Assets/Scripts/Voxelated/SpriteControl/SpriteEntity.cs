using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a sprite block. It contains
/// the name of it's owner, location
/// in the world, and the sprite id / type
/// </summary>
public class SpriteEntity {
    #region Properties
    /// <summary>
    /// The render key of the container that owns this
    /// sprite block.
    /// </summary>
    public string RenderKey { get; private set; }

    /// <summary>
    /// Where the sprite is located in the world.
    /// </summary>
    public Vector3 Position { get; private set; }

    /// <summary>
    /// The meta data of the sprite block.
    /// </summary>
    public byte SpriteData { get; private set; }

    /// <summary>
    /// The gameobject being used to represent 
    /// the sprite block.
    /// </summary>
    public GameObject GameObject { get; private set; }
    #endregion

    #region Constructor(s)
    /// <summary>
    /// Create a new sprite entity. The entity will modify
    /// the gameobject as it sees fit.
    /// </summary>
    public SpriteEntity(string renderKey, Vector3 pos, GameObject obj) {
        RenderKey = renderKey;
        Position = pos;
        GameObject = obj;
    }
    #endregion
}
