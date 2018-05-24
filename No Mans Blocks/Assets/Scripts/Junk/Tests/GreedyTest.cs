//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Voxelated.Terrain;
//using Voxelated;
//using Voxelated.Engine.Mesh;
//using Voxelated.Utilities;

//public class GreedyTest : MonoBehaviour {

//    Vect3Int Size = new Vect3Int(4, 4, 4);
//    BlockContainer blocks;

//	// Use this for initialization
//	void Start () {
//        blocks = new BlockContainer(4, 4, 4);

//        for(int x = 0; x < 4; x++) {
//            for(int y = 0; y < 4; y++) {
//                blocks[x, y, 0] = Block.Black;
//            }
//        }

//        for (int x = 0; x < 4; x++) {
//            for (int y = 0; y < 4; y++) {
//                blocks[x, y, 1] = new Block(new Color16(29, 29, 29));
//            }
//        }

//        for (int x = 0; x < 4; x++) {
//            for (int y = 0; y < 4; y++) {
//                blocks[x, y, 2] = new Block(new Color16(20, 20, 20));
//            }
//        }

//        blocks[1, 2, 3] = Block.Black;
//        blocks[0, 1, 3] = Block.White;

//        blocks[1, 1, 0] = Block.Air;
//        blocks[1, 2, 0] = Block.Air;
//        blocks[2, 2, 0] = Block.Air;

//        MeshData mesh = GenerateMesh(); // GreedyMesh(MeshType.Render);
//        ApplyMesh(mesh);
//	}

//    public MeshData GreedyMeshIt(MeshType type) {
//        MeshData mesh = new MeshData();

//        //We need to build quads in each of the 6 sides
//        for (int d = 0; d < 6; d++) {
//            int a = d % 3;
//            int b = (d + 1) % 3;
//            int c = (d + 2) % 3;
//            bool[,] merged;
//            bool back = d < 3;

//            //Work positions
//            Vect2Int q = new Vect2Int();
//            Vect3Int x = new Vect3Int();
//            Vect3Int y = new Vect3Int();
//            Vect3Int dc, db, o;

//            //Face vertices
//            Vector3[] verts;

//            //Slice on the A axis.
//            for (x[a] = 0; x[a] < Size[a]; x[a]++) {
//                merged = new bool[Size[b], Size[c]];

//                //Build the slice
//                for (x[b] = 0; x[b] < Size[b]; x[b]++) {
//                    for (x[c] = 0; x[c] < Size[c]; x[c]++) {
//                        //Check if already merged
//                        if (merged[x[b], x[c]] || GetBlock(x).IsAir || !IsBlockVisible(x, a, back)) {
//                            continue;
//                        }

//                        //Figure out the width
//                        y = new Vect3Int();
//                        for (y[c] = x[c]; y[c] < Size[c] && !merged[y[b], y[c]] && GetBlock(x) == GetBlock(y) && !GetBlock(y).IsAir && IsBlockVisible(y, a, back); y[c]++) { }

//                        //Log width, and reset work pos
//                        q[0] = y[c] - x[c];
//                        y[c] = x[c];

//                        //Figure out the height
//                        for (y[b] = x[b] + 1; y[b] < Size[b] && !merged[y[b], y[c]] && GetBlock(x) == GetBlock(y) && !GetBlock(y).IsAir && IsBlockVisible(y, a, back); y[b]++) {
//                            for (y[c] = x[c]; y[c] - x[c] < q[0] && !merged[y[b], y[c]] && GetBlock(x) == GetBlock(y) && !GetBlock(y).IsAir && IsBlockVisible(y, a, back); y[c]++) { }

//                            //if we didn't reach w, then we don't have another row.
//                            if (y[c] - x[c] < q[0]) {
//                                break;
//                            }
//                            else {
//                                y[c] = x[c];
//                            }
//                        }

//                        //Log the height
//                        q[1] = y[b] - x[b];

//                        //Figure out the vertices
//                        dc = new Vect3Int();
//                        dc[c] = q[0];

//                        db = new Vect3Int();
//                        db[b] = q[1];

//                        //We need to add a slight offset when working with front faces.
//                        o = new Vect3Int(x);
//                        o[a] += back ? 0 : 1;

//                        //Draw the face to the mesh
//                        verts = new Vector3[] {  o, o + db, o + dc + db, o + dc  };
//                        mesh.AddColoredFace(verts, GetBlock(x).Color, back);
                      
//                        //Mark it merged
//                        for (int m = 0; m < q[0]; m++) {
//                            for (int n = 0; n < q[1]; n++) {
//                                merged[x[b] + n, x[c] + m] = true;
//                            }
//                        }
//                    }
//                }
//            }
//        }

//        return mesh;
//    }

//    /// <summary>
//    /// Generates a greedy mesh. This needs work but
//    /// it should become the final version.
//    /// </summary>
//    private MeshData GenerateMesh() {
//        MeshData mesh = new MeshData();

//        //This tracks if we merged the blocks in the slice.
//        bool[,] merged;

//        //Work vars
//        Vect3Int a, b, q, m, n, o;
//        Vector3[] verts;
//        int i, j;

//        //This is just beautiful. It counts 0-2 then 0-2 again. The second run through back is true.
//        bool back = false;
//        for (int d = 0, t = 0; d < 3 && t <= 1; t += d == 2 ? 1 : 0, d += d == 2 ? -2 : 1, back = t > 0) {
//            i = (d + 1) % 3;
//            j = (d + 2) % 3;

//            a = new Vect3Int();
//            b = new Vect3Int();

//            //This is the axis we will slice on
//            for (a[d] = 0; a[d] < Size[d]; a[d]++) {
//                merged = new bool[Size[i], Size[j]];

//                //These are dem slices we're building
//                for(a[i] = 0; a[i] < Size[i]; a[i]++) {
//                    for(a[j] = 0; a[j] < Size[j]; a[j]++) {

//                        //If this block has already been merged, is air, or not visible skip it.
//                        if(merged[a[i], a[j]] || GetBlock(a).IsAir || !IsBlockVisible(a, d, back)) {
//                            continue;
//                        }

//                        //Reset the work var
//                        q = new Vect3Int();

//                        //Figure out the width, then save it
//                        for (b = a, b[j]++; b[j] < Size[j] && CompareStep(a, b, d, back) && !merged[b[i], b[j]]; b[j]++) { }
//                        q[j] = b[j] - a[j];

//                        //Figure out the height, then save it
//                        for (b = a, b[i]++; b[i] < Size[i] && CompareStep(a, b, d, back) && !merged[b[i], b[j]]; b[i]++) {
//                            for (b[j] = a[j]; b[j] < Size[j] && CompareStep(a, b, d, back) && !merged[b[i], b[j]]; b[j]++) { }

//                            //If we didn't reach the end then its not a good add.
//                            if (b[j] - a[j] < q[j]) {
//                                break;
//                            }
//                            else {
//                                b[j] = a[j];
//                            }
//                        }
//                        q[i] = b[i] - a[i];

//                        //Now we add the quad to the mesh
//                        m = new Vect3Int();
//                        m[i] = q[i];

//                        n = new Vect3Int();
//                        n[j] = q[j];

//                        //We need to add a slight offset when working with front faces.
//                        o = a;
//                        o[d] += back ? 0 : 1;

//                        //Draw the face to the mesh
//                        verts = new Vector3[] { o, o + m, o + m + n, o + n };
//                        mesh.AddColoredFace(verts, GetBlock(a).Color, back);

//                        //Mark it merged
//                        for (int f = 0; f < q[i]; f++) {
//                            for (int g = 0; g < q[j]; g++) {
//                                merged[a[i] + f, a[j] + g] = true;
//                            }
//                        }
//                    }
//                }
//            }
//        }

//        return mesh;
//    }

    
//    private Block GetBlock(Vect3Int pos) {
//        return blocks.GetBlock(pos);
//    }

//    /// <summary>
//    /// Checks if the block is is visible on it's desired face.
//    /// </summary>
//    private bool IsBlockVisible(Vect3Int pos, int a, bool back) {
//        pos[a] += back ? -1 : 1;
//        return GetBlock(pos).IsAir;
//    }

//    /// <summary>
//    /// Tests 2 blocks to see if they are a match based
//    /// off our criteria.
//    /// </summary>
//    private bool CompareStep(Vect3Int a, Vect3Int b, int d, bool back) {
//        Block blockA = GetBlock(a);
//        Block blockB = GetBlock(b);

//        return blockA == blockB && !blockB.IsAir && IsBlockVisible(b, d, back);
//    }

//    /// <summary>
//    /// It's like greedy mesh but grosser
//    /// </summary>
//    /// <returns></returns>
//    private MeshData GrossMesh() {
//        MeshData mesh = new MeshData();

//        bool[,] merged;

//        Vect3Int a, b, q, m, n, o;
//        Vector3[] verts;

//        //Flip flop lol. First we do East, then we do West
//        for (bool back = false, flipped = false; !flipped || back; back = !back, flipped = true) {
            
//            for (int x = 0; x < Size.x; x++) {
//                merged = new bool[Size.y, Size.z];

//                //Build each slice
//                for (int y = 0; y < Size.y; y++) {
//                    for(int z = 0; z < Size.z; z++) {
//                        //Start pos.
//                        a = new Vect3Int(x, y, z);
//                        q = new Vect3Int();

//                        //If already merged. Skip it.
//                        if (merged[y, z] || GetBlock(a).IsAir || !IsBlockVisible(a, 0, back)) {
//                            continue;
//                        }

//                        //Figure out the width
//                        for (b = a, b.y++; b.y < Size.y && CompareStep(a, b, 0, back) && !merged[b.y,b.z]; b.y++) { }

//                        //Log width
//                        q.y = b.y - a.y;

//                        //Figure out the height
//                        for (b = a, b.z++; b.z < Size.z && CompareStep(a, b, 0, back) && !merged[b.y, b.z]; b.z++) {
//                            for (b.y = a.y; b.y < Size.y && CompareStep(a, b, 0, back) && !merged[b.y, b.z]; b.y++) { }

//                            //If we didn't reach the end then its not a good add.
//                            if (b.y - a.y < q.y) {
//                                break;
//                            }
//                            else {
//                                b.y = a.y;
//                            }
//                        }

//                        //Log dat height
//                        q.z = b.z - a.z;

//                        //Add to mesh
//                        m = new Vect3Int();
//                        m.y = q.y;

//                        n = new Vect3Int();
//                        n.z = q.z;

//                        //We need to add a slight offset when working with front faces.
//                        o = a;
//                        o.x += back ? 0 : 1; 

//                        //Draw the face to the mesh
//                        verts = new Vector3[] { o, o + m, o + m + n, o + n };
//                        mesh.AddColoredFace(verts, GetBlock(a).Color, back);

//                        //Mark it merged
//                        for (int i = 0; i < q.y; i++) {
//                            for (int j = 0; j < q.z; j++) {
//                                merged[a.y + i, a.z + j] = true;
//                            }
//                        }
//                    }
//                }
//            }
//        }


//        //Flip flop lol.First we do Up, then we do Down
//        for (bool back = false, flipped = false; !flipped || back; back = !back, flipped = true) {

//            for (int y = 0; y < Size.y; y++) {
//                merged = new bool[Size.z, Size.x];

//                //Build each slice
//                for (int z = 0; z < Size.z; z++) {
//                    for (int x = 0; x < Size.x; x++) {
//                        //Start pos.
//                        a = new Vect3Int(x, y, z);
//                        q = new Vect3Int();

//                        //If already merged. Skip it.
//                        if (merged[z, x] || GetBlock(a).IsAir || !IsBlockVisible(a, 1, back)) {
//                            continue;
//                        }

//                        //Figure out the width
//                        for (b = a, b.z++; b.z < Size.z && CompareStep(a, b, 1, back) && !merged[b.z, b.x]; b.z++) { }

//                        //Log width
//                        q.z = b.z - a.z;

//                        //Figure out the height
//                        for (b = a, b.x++; b.x < Size.x && CompareStep(a, b, 1, back) && !merged[b.z, b.x]; b.x++) {
//                            for (b.z = a.z; b.z < Size.z && CompareStep(a, b, 1, back) && !merged[b.z, b.x]; b.z++) { }

//                            //If we didn't reach the end then its not a good add.
//                            if (b.z - a.z < q.z) {
//                                break;
//                            }
//                            else {
//                                b.z = a.z;
//                            }
//                        }

//                        //Log dat height
//                        q.x = b.x - a.x;

//                        //Add to mesh
//                        m = new Vect3Int();
//                        m.z = q.z;

//                        n = new Vect3Int();
//                        n.x = q.x;

//                        //We need to add a slight offset when working with front faces.
//                        o = a;
//                        o.y += back ? 0 : 1;

//                        //Draw the face to the mesh
//                        verts = new Vector3[] { o, o + m, o + m + n, o + n };
//                        mesh.AddColoredFace(verts, GetBlock(a).Color, back);

//                        //Mark it merged
//                        for (int i = 0; i < q.z; i++) {
//                            for (int j = 0; j < q.x; j++) {
//                                merged[a.z + i, a.x + j] = true;
//                            }
//                        }
//                    }
//                }
//            }
//        }


//        //Flip flop lol.First we do North, then we do South
//        for (bool back = false, flipped = false; !flipped || back; back = !back, flipped = true) {

//            for (int z = 0; z < Size.z; z++) {
//                merged = new bool[Size.x, Size.y];

//                //Build each slice
//                for (int x = 0; x < Size.x; x++) {
//                    for (int y = 0; y < Size.y; y++) {
//                        //Start pos.
//                        a = new Vect3Int(x, y, z);
//                        q = new Vect3Int();

//                        //If already merged. Skip it.
//                        if (merged[x, y] || GetBlock(a).IsAir || !IsBlockVisible(a, 2, back)) {
//                            continue;
//                        }

//                        //Figure out the width
//                        for (b = a, b.x++; b.x < Size.x && CompareStep(a, b, 2, back) && !merged[b.x, b.y]; b.x++) { }

//                        //Log width
//                        q.x = b.x - a.x;

//                        //Figure out the height
//                        for (b = a, b.y++; b.y < Size.y && CompareStep(a, b, 2, back) && !merged[b.x, b.y]; b.y++) {
//                            for (b.x = a.x; b.x < Size.x && CompareStep(a, b, 2, back) && !merged[b.x, b.y]; b.x++) { }

//                            //If we didn't reach the end then its not a good add.
//                            if (b.x - a.x < q.x) {
//                                break;
//                            }
//                            else {
//                                b.x = a.x;
//                            }
//                        }

//                        //Log dat height
//                        q.y = b.y - a.y;

//                        //Add to mesh
//                        m = new Vect3Int();
//                        m.x = q.x;

//                        n = new Vect3Int();
//                        n.y = q.y;

//                        //We need to add a slight offset when working with front faces.
//                        o = a;
//                        o.z += back ? 0 : 1;

//                        //Draw the face to the mesh
//                        verts = new Vector3[] { o, o + m, o + m + n, o + n };
//                        mesh.AddColoredFace(verts, GetBlock(a).Color, back);

//                        //Mark it merged
//                        for (int i = 0; i < q.x; i++) {
//                            for (int j = 0; j < q.y; j++) {
//                                merged[a.x + i, a.y + j] = true;
//                            }
//                        }
//                    }
//                }
//            }
//        }



//        return mesh;
//    }

   













//    /// <summary>
//    /// Renders the chunk and applies it's collider mesh
//    /// </summary>
//    private void ApplyMesh(MeshData meshData) {
//        MeshFilter meshFilter = GetComponent<MeshFilter>();
//        MeshCollider meshCollider = GetComponent<MeshCollider>();

//        if (meshFilter == null) {
//            LoggerUtils.LogError("No mesh filter found for: " + meshData.RenderKey);
//            return;
//        }

//        //CLear old mesh and set new vertices / triangles
//        meshFilter.mesh.Clear();
//        meshFilter.mesh.vertices = meshData.Vertices;
//        meshFilter.mesh.triangles = meshData.Triangles;

//        //Color mesh and calculate normals
//        meshFilter.mesh.colors32 = meshData.Colors;
//        meshFilter.mesh.RecalculateNormals();

//        Mesh mesh = new Mesh();
//        mesh.vertices = meshData.Vertices;
//        mesh.triangles = meshData.Triangles;
//        mesh.RecalculateNormals();

//        //If there's a mesh collider apply the collide mesh
//        if (meshCollider != null) {
//            meshCollider.sharedMesh = mesh;
//        }
//    }
//}
