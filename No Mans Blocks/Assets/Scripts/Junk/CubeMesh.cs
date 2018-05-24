using UnityEngine;
using System.Collections;

/// <summary>
/// Cube mesh.
/// 
/// As the title states it creates a cube mesh. This was taken from:
/// http://wiki.unity3d.com/index.php/ProceduralPrimitives
/// This is needed for Selector.cs
/// </summary>

public class CubeMesh : MonoBehaviour {
	MeshFilter meshFilter;
	Mesh mesh;

	public float length;
	public float width;
	public float height;

	public bool updateMesh = true;

	// Use this for initialization
	void Awake () {
		FindReferences ();
	}
	
	// Update is called once per frame
	void Update () {
		if (updateMesh) {
			mesh.Clear();

			mesh.vertices = CreateVertices ();
			mesh.normals = CreateNormals ();
			mesh.uv = CreateUVs ();
			mesh.triangles = CreateTriangles ();

			mesh.RecalculateBounds();
			;

			updateMesh = false;
		}
	}

	//Set size of render mesh
	public void UpdateSize(float l, float w, float h){
		length = l;
		width = w;
		height = h;

		AddOffset ();
		Create ();
	}

	public void UpdateSize(Vector3 s){
		length = s.x;
		width = s.y;
		height = s.z;

		AddOffset ();
		Create ();
	}

	//Add offset to prevent clipping
	public void AddOffset(){
		length += 0.015625f;
		width += 0.015625f;
		height += 0.015625f;
	}

	//Create the mesh
	public void Create(){
		updateMesh = true;
	}

	//Find references for mesh
	void FindReferences() {
		meshFilter = GetComponent<MeshFilter>();
		mesh = meshFilter.mesh;
	}

	//Create mesh points
	Vector3[] CreateVertices(){
		#region Vertices
		Vector3 p0 = new Vector3( -length * .5f,	-width * .5f, height * .5f );
		Vector3 p1 = new Vector3( length * .5f, 	-width * .5f, height * .5f );
		Vector3 p2 = new Vector3( length * .5f, 	-width * .5f, -height * .5f );
		Vector3 p3 = new Vector3( -length * .5f,	-width * .5f, -height * .5f );	

		Vector3 p4 = new Vector3( -length * .5f,	width * .5f,  height * .5f );
		Vector3 p5 = new Vector3( length * .5f, 	width * .5f,  height * .5f );
		Vector3 p6 = new Vector3( length * .5f, 	width * .5f,  -height * .5f );
		Vector3 p7 = new Vector3( -length * .5f,	width * .5f,  -height * .5f );

		Vector3[] vertices = new Vector3[]
		{
			// Bottom
			p0, p1, p2, p3,

			// Left
			p7, p4, p0, p3,

			// Front
			p4, p5, p1, p0,

			// Back
			p6, p7, p3, p2,

			// Right
			p5, p6, p2, p1,

			// Top
			p7, p6, p5, p4
		};
		#endregion

		return vertices;
	}

	//Create normals of the mesh
	Vector3[] CreateNormals(){
		#region Normales
		Vector3 up 	= Vector3.up;
		Vector3 down 	= Vector3.down;
		Vector3 front 	= Vector3.forward;
		Vector3 back 	= Vector3.back;
		Vector3 left 	= Vector3.left;
		Vector3 right 	= Vector3.right;

		Vector3[] normales = new Vector3[]
		{
			// Bottom
			down, down, down, down,

			// Left
			left, left, left, left,

			// Front
			front, front, front, front,

			// Back
			back, back, back, back,

			// Right
			right, right, right, right,

			// Top
			up, up, up, up
		};
		#endregion	

		return normales;
	}

	//Create texture coords
	Vector2[] CreateUVs(){
		#region UVs
		Vector2 _00 = new Vector2( 0f, 0f );
		Vector2 _10 = new Vector2( 1f, 0f );
		Vector2 _01 = new Vector2( 0f, 1f );
		Vector2 _11 = new Vector2( 1f, 1f );

		Vector2[] uvs = new Vector2[]
		{
			// Bottom
			_11, _01, _00, _10,

			// Left
			_11, _01, _00, _10,

			// Front
			_11, _01, _00, _10,

			// Back
			_11, _01, _00, _10,

			// Right
			_11, _01, _00, _10,

			// Top
			_11, _01, _00, _10,
		};
		#endregion

		return uvs;
	}

	//Create mesh faces
	int[] CreateTriangles(){
		#region Triangles
		int[] triangles = new int[]
		{
			// Bottom
			3, 1, 0,
			3, 2, 1,			

			// Left
			3 + 4 * 1, 1 + 4 * 1, 0 + 4 * 1,
			3 + 4 * 1, 2 + 4 * 1, 1 + 4 * 1,

			// Front
			3 + 4 * 2, 1 + 4 * 2, 0 + 4 * 2,
			3 + 4 * 2, 2 + 4 * 2, 1 + 4 * 2,

			// Back
			3 + 4 * 3, 1 + 4 * 3, 0 + 4 * 3,
			3 + 4 * 3, 2 + 4 * 3, 1 + 4 * 3,

			// Right
			3 + 4 * 4, 1 + 4 * 4, 0 + 4 * 4,
			3 + 4 * 4, 2 + 4 * 4, 1 + 4 * 4,

			// Top
			3 + 4 * 5, 1 + 4 * 5, 0 + 4 * 5,
			3 + 4 * 5, 2 + 4 * 5, 1 + 4 * 5,

		};
		#endregion

		return triangles;
	}
}
