using NoMansBlocks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxelated.Engine.Mesh;
using Voxelated.Engine.Mesh.Render;
using Voxelated.Utilities;

/// <summary>
/// Handles storing a reference to each mesh in the game. 
/// Allows for rendering of visual meshes and applying of
/// collision meshes.
/// </summary>
public class MeshHandler : MonoBehaviour, IMeshRenderer {
    #region Constants
    /// <summary>
    /// How long to wait between performing render jobs.
    /// </summary>
    public const float RenderInterval = 0.005f;

    /// <summary>
    /// How many meshes to render per job.
    /// </summary>
    public const int RenderCount = 4;
    #endregion

    #region Members
    /// <summary>
    /// The mesh filters of the world.
    /// </summary>
    private Dictionary<string, MeshFilter> meshFilters;

    /// <summary>
    /// The mesh colliders of the world.
    /// </summary>
    private Dictionary<string, MeshCollider> meshColliders;

    /// <summary>
    /// Meshes that have been optimized 
    /// and are waiting to be rendered to the 
    /// screen.
    /// </summary>
    private ThreadableQueue<MeshData> renderMeshQueue;

    /// <summary>
    /// Timer to track when to perform render jobs.
    /// </summary>
    private float renderTimer;

    public int MeshCount;
    #endregion

    #region Mono Events
    /// <summary>
    /// Initialize this object
    /// </summary>
    private void Awake () {
        renderMeshQueue = new ThreadableQueue<MeshData>();
        renderTimer = 0.0f;

        //Initialize the reference dictionaries.
        meshFilters = new Dictionary<string, MeshFilter>();
        meshColliders = new Dictionary<string, MeshCollider>();

        bool isDupe = !GameManager.SetMeshHandler(this);

        if (isDupe) {
            Destroy(this.gameObject);
        }
    }
	
    /// <summary>
    /// Update the timer and render if it's time.
    /// </summary>
	private void Update () {
        MeshCount = renderMeshQueue.Count;

        //If there are any meshes that need rendering. Do so
        if (renderMeshQueue.Count > 0 && renderTimer > RenderInterval) {
            renderTimer = 0.0f;
            int workCount = Mathf.Min(renderMeshQueue.Count, RenderCount);

            for (int i = 0; i < workCount; i++) {
                ApplyMesh(renderMeshQueue.Dequeue());
            }
        }

        renderTimer += Time.deltaTime;
    }
    #endregion

    #region Publics
    /// <summary>
    /// Add a mesh filter to the reference dictionary.
    /// </summary>
    public void AddMeshFilter(string key, MeshFilter filter) {
        meshFilters.Add(key, filter);
    }

    /// <summary>
    /// Add a mesh collider to the reference dictionary.
    /// </summary>
    public void AddMeshCollider(string key, MeshCollider collider) {
        meshColliders.Add(key, collider);
    }

    /// <summary>
    /// Add a mesh to the render queue for rendering.
    /// </summary>
    public void RenderMesh(MeshData mesh) {
        renderMeshQueue.Enqueue(mesh);
    }
    #endregion

    #region Helpers
    /// <summary>
    /// Renders the chunk and applies it's collider mesh
    /// </summary>
    private void ApplyMesh(MeshData meshData) {
        MeshFilter meshFilter = FindMeshFilter(meshData.RenderKey);
        MeshCollider meshCollider = FindMeshCollider(meshData.RenderKey);

        if (meshFilter == null) {
            LoggerUtils.LogError("No mesh filter found for: " + meshData.RenderKey);
            return;
        }

        //If there's any sprites. Send em to the sprite manager.
        Vector3[] spritePositions = meshData.SpritePositions;
        byte[] spriteData = meshData.SpriteData;

        //CLear old mesh and set new vertices / triangles
        meshFilter.mesh.Clear();
        meshFilter.mesh.vertices = meshData.Vertices;
        meshFilter.mesh.triangles = meshData.Triangles;

        //Color mesh and calculate normals
        meshFilter.mesh.colors32 = meshData.Colors;
        meshFilter.mesh.RecalculateNormals();

        Mesh mesh = new Mesh();
        mesh.vertices = meshData.Vertices;
        mesh.triangles = meshData.Triangles;
        mesh.RecalculateNormals();

        //If there's a mesh collider apply the collide mesh
        if (meshCollider != null) {
            meshCollider.sharedMesh = mesh;
        }
    }

    /// <summary>
    /// Attempts to find the mesh filter
    /// with the specific key. If not found
    /// null is returned.
    /// </summary>
    private MeshFilter FindMeshFilter(string key) {
        MeshFilter filter = null;

        meshFilters.TryGetValue(key, out filter);
        return filter;
    }

    /// <summary>
    /// Attempts to find the mesh collider with the specific key. If not
    /// found null is returned.
    /// </summary>
    private MeshCollider FindMeshCollider(string key) {
        MeshCollider collider = null;

        meshColliders.TryGetValue(key, out collider);
        return collider;
    }
    #endregion
}
