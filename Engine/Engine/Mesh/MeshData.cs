using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Voxelated.Engine.Mesh {
    /// <summary>
    /// Container class to hold triangles and vertices. This is threadsafe. Use it for storing mesh info for rendering.
    /// Collision mesh will be the same as rendering.
    /// </summary>
    public class MeshData {
        #region Properties
        /// <summary>
        /// Internal list of the mesh vertices. 
        /// </summary>
        private List<Vector3> vertices;

        /// <summary>
        /// Internal list of the triangles of the mesh
        /// </summary>
        private List<int> triangles;

        /// <summary>
        /// Internal list of the vertex colors.
        /// </summary>
        private List<Color32> colors;

        /// <summary>
        /// Where the sprite blocks should be placed
        /// if any.
        /// </summary>
        private List<Vector3> spritePositions;

        /// <summary>
        /// The id of the sprite. This is used with the
        /// corresponding sprite
        /// location to figure out where to place the sprite block.
        /// </summary>
        private List<byte> spriteData;

        /// <summary>
        /// The semaphore object
        /// </summary>
        private readonly object lockObj;

        /// <summary>
        /// The key to use to find it's meshfilter.
        /// </summary>
        private string renderKey;
   
        /// <summary>
        /// The name of the mesh
        /// </summary>
        public string RenderKey {
            get {
                lock (lockObj) {
                    return renderKey;
                }
            }
        }

        /// <summary>
        /// The vertices of the render / collision mesh.
        /// </summary>
        public Vector3[] Vertices {
            get {
                lock (lockObj) {
                    return vertices.ToArray();
                }
            }
        }

        /// <summary>
        /// The triangles of the render / collision mesh.
        /// </summary>
        public int[] Triangles {
            get {
                lock (lockObj) {
                    return triangles.ToArray();
                }
            }
        }

        /// <summary>
        /// The vertex colors of the render mesh.
        /// </summary>
        public Color32[] Colors {
            get {
                lock (lockObj) {
                    return colors.ToArray();
                }
            }
        }

        /// <summary>
        /// The position of the sprite entities to spawn. Use these in
        /// pair with their related sprite data.
        /// </summary>
        public Vector3[] SpritePositions {
            get {
                lock (lockObj) {
                    return spritePositions.ToArray();
                }
            }
        }

        /// <summary>
        /// The meta data of each sprite. Use these with each's 
        /// corresponding position.
        /// </summary>
        public byte[] SpriteData {
            get {
                lock (lockObj) {
                    return spriteData.ToArray();
                }
            }
        }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new mesh data
        /// </summary>
        public MeshData() {
            vertices = new List<Vector3>();
            triangles = new List<int>();
            colors = new List<Color32>();
            lockObj = new object();

            spritePositions = new List<Vector3>();
            spriteData = new List<byte>();
        }

        /// <summary>
        /// Create a new mesh data with a render key.
        /// </summary>
        public MeshData(string renderKey) {
            this.renderKey = renderKey;

            vertices = new List<Vector3>();
            triangles = new List<int>();
            colors = new List<Color32>();
            lockObj = new object();

            spritePositions = new List<Vector3>();
            spriteData = new List<byte>();
        }
        #endregion

        #region Publics
        /// <summary>
        /// Add a colored face to the mesh. Goes in order bottomleft, topleft, topright, bottomright, 
        /// </summary>
        public void AddColoredFace(Vector3[] vertices, Color32 color, bool backwards = false) {
            lock (lockObj) {
                //Add the four face vertices + colors
                for (int i = 0; i < 4; i++) {
                    this.vertices.Add(vertices[backwards ? 3 - i : i]);
                    this.colors.Add(color);
                }

                //First triangle
                triangles.Add(this.vertices.Count - 4);
                triangles.Add(this.vertices.Count - 3);
                triangles.Add(this.vertices.Count - 2);

                //Second triangle
                triangles.Add(this.vertices.Count - 4);
                triangles.Add(this.vertices.Count - 2);
                triangles.Add(this.vertices.Count - 1);
            }
        }

        /// <summary>
        /// Add a face that is facing one of the six directions. This is
        /// used by any voxel mesh.
        /// </summary>
        public void AddColoredFace(Vector3 pos, Vect2Int size, Color32 color, Direction direction) {
            Vector3[] faceVertices = GetFaceVertices(pos, size, direction);
            AddColoredFace(faceVertices, color);
        }

        /// <summary>
        /// Sprites can only be placed on 
        /// </summary>
        public void AddSprite(Vector3 pos, byte data) {
            lock (lockObj) {
                spritePositions.Add(pos);
                spriteData.Add(data);
            }
        }
        #endregion

        #region Helpers
        /// <summary>
        /// Genereate the face vertices for the block(s).
        /// </summary>
        private Vector3[] GetFaceVertices(Vector3 pos, Vect2Int size, Direction direction) {
            Vector3[] faceVertices = new Vector3[4];

            switch (direction) {
                case Direction.north:
                    faceVertices[0] = new Vector3(pos.x + size.X + 0.5f, pos.y - 0.5f, pos.z + 0.5f);
                    faceVertices[1] = new Vector3(pos.x + size.X + 0.5f, pos.y + size.Y + 0.5f, pos.z + 0.5f);
                    faceVertices[2] = new Vector3(pos.x - 0.5f, pos.y + size.Y + 0.5f, pos.z + 0.5f);
                    faceVertices[3] = new Vector3(pos.x - 0.5f, pos.y - 0.5f, pos.z + 0.5f);
                    break;
                case Direction.south:
                    faceVertices[0] = new Vector3(pos.x - 0.5f, pos.y - 0.5f, pos.z - 0.5f);
                    faceVertices[1] = new Vector3(pos.x - 0.5f, pos.y + size.Y + 0.5f, pos.z - 0.5f);
                    faceVertices[2] = new Vector3(pos.x + size.X + 0.5f, pos.y + size.Y + 0.5f, pos.z - 0.5f);
                    faceVertices[3] = new Vector3(pos.x + size.X + 0.5f, pos.y - 0.5f, pos.z - 0.5f);
                    break;
                case Direction.east:
                    faceVertices[0] = new Vector3(pos.x + 0.5f, pos.y - 0.5f, pos.z - 0.5f);
                    faceVertices[1] = new Vector3(pos.x + 0.5f, pos.y + size.Y + 0.5f, pos.z - 0.5f);
                    faceVertices[2] = new Vector3(pos.x + 0.5f, pos.y + size.Y + 0.5f, pos.z + size.X + 0.5f);
                    faceVertices[3] = new Vector3(pos.x + 0.5f, pos.y - 0.5f, pos.z + size.X + 0.5f);
                    break;
                case Direction.west:
                    faceVertices[0] = new Vector3(pos.x - 0.5f, pos.y - 0.5f, pos.z + size.X + 0.5f);
                    faceVertices[1] = new Vector3(pos.x - 0.5f, pos.y + size.Y + 0.5f, pos.z + size.X + 0.5f);
                    faceVertices[2] = new Vector3(pos.x - 0.5f, pos.y + size.Y + 0.5f, pos.z - 0.5f);
                    faceVertices[3] = new Vector3(pos.x - 0.5f, pos.y - 0.5f, pos.z - 0.5f);
                    break;
                case Direction.up:
                    faceVertices[0] = new Vector3(pos.x - 0.5f, pos.y + 0.5f, pos.z + size.Y + 0.5f);
                    faceVertices[1] = new Vector3(pos.x + size.X + 0.5f, pos.y + 0.5f, pos.z + size.Y + 0.5f);
                    faceVertices[2] = new Vector3(pos.x + size.X + 0.5f, pos.y + 0.5f, pos.z - 0.5f);
                    faceVertices[3] = new Vector3(pos.x - 0.5f, pos.y + 0.5f, pos.z - 0.5f);
                    break;
                case Direction.down:
                    faceVertices[0] = new Vector3(pos.x - 0.5f, pos.y - 0.5f, pos.z - 0.5f);
                    faceVertices[1] = new Vector3(pos.x + size.X + 0.5f, pos.y - 0.5f, pos.z - 0.5f);
                    faceVertices[2] = new Vector3(pos.x + size.X + 0.5f, pos.y - 0.5f, pos.z + size.Y + 0.5f);
                    faceVertices[3] = new Vector3(pos.x - 0.5f, pos.y - 0.5f, pos.z + size.Y + 0.5f);
                    break;
            }

            return faceVertices;
        }
        #endregion
    }
}