//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Voxelated;
//using Voxelated.Terrain;

///// <summary>
///// Block Collider.
///// 
///// Takes a block array as input and generates a mesh of box colliders that will satisfy perfectly replicate the
///// block array.
///// </summary>
//public class BlockCollider : MonoBehaviour {
//	//Collision Mesh Related
//	private List<BoxCollider> boxColliders = new List<BoxCollider>();

//	// Use this for initialization
//	void Start () {
		
//	}

//	/// <summary>
//	/// Removes old box colliders from fragment. These need to be recalculated on spawn.
//	/// </summary>
//	public void ClearColliders(){
//		boxColliders.Clear ();

//		//Remove each collider from the fragment.
//		foreach (BoxCollider bColl in GetComponents<BoxCollider>())
//			Destroy (bColl);
//	}

//	#region Collision Mesh Generation

//	/// <summary>
//	/// Builds the collision mesh of the worldfragment using
//	/// a greedy approach. This was taken from:
//	/// http://www.ben-drury.co.uk/index.php/2016/12/19/generating-collision-mesh-voxel-chunk/
//	/// </summary>
//	public void BuildCollisionMesh(ref Block[, ,] blocks, Vector3Int fragSize){
//		bool[, ,] tested = new bool[fragSize.x, fragSize.y, fragSize.z];
//		Dictionary<Vector3Int, Vector3Int> boxes = new Dictionary<Vector3Int, Vector3Int> ();

//		for (int x = 0; x < fragSize.x; x++) {
//			for (int y = 0; y < fragSize.y; y++) {
//				for (int z = 0; z < fragSize.z; z++) {
//					//Block has not been checked. Do so now.
//					if (!tested [x, y, z]) {
//						tested [x, y, z] = true;

//						//If the block is not air.
//						lock (blocks.SyncRoot) {
//							if (!blocks [x, y, z].IsAir) {
//								Vector3Int boxStart = new Vector3Int (x, y, z);
//								Vector3Int boxSize = new Vector3Int (1, 1, 1);

//								bool canSpreadX = true;
//								bool canSpreadY = true;
//								bool canSpreadZ = true;

//								//Attempts to expand in all directions and stops in each direction when it can't anymore.
//								while (canSpreadX || canSpreadY || canSpreadZ) {
//									canSpreadX = TrySpreadX (ref blocks, fragSize, canSpreadX, ref tested, boxStart, ref boxSize);
//									canSpreadY = TrySpreadY (ref blocks, fragSize, canSpreadY, ref tested, boxStart, ref boxSize);
//									canSpreadZ = TrySpreadZ (ref blocks, fragSize, canSpreadZ, ref tested, boxStart, ref boxSize);
//								}
//								boxes.Add (boxStart, boxSize);
//							}
//						}
//					}
//				}
//			}
//		}
//		//Send the boxes to the collision mesh.
//		StartCoroutine(SetCollisionMesh (boxes));
//	}

//	/// <summary>
//	/// Determines the largest area that the current box collider can be spread out to in the X direction.
//	/// </summary>
//	private bool TrySpreadX(ref Block[, ,] blocks, Vector3Int fragSize, bool CanSpreadX, ref bool[, ,] tested, Vector3Int boxStart, ref Vector3Int boxSize){
//		int yLimit = boxStart.y + boxSize.y;
//		int zLimit = boxStart.z + boxSize.z;

//		for (int y = boxStart.y; y < yLimit; y++) {
//			for (int z = boxStart.z; z < zLimit; z++) {
//				int currX = boxStart.x + boxSize.x;

//				//If we are out of bounds, have already checked here, or on an air block it can't spread.
//				lock (blocks.SyncRoot) {
//					if (currX >= fragSize.x || tested [currX, y, z] || blocks [currX, y, z].IsAir)
//						CanSpreadX = false;
//				}
//			}
//		}

//		//The box can spread. Mark it tested and increase it's size!
//		if (CanSpreadX) {
//			for (int y = boxStart.y; y < yLimit; y++) {
//				for (int z = boxStart.z; z < zLimit; z++) {
//					int currX = boxStart.x + boxSize.x;
//					tested [currX, y, z] = true;
//				}
//			}
//			boxSize.x++;
//		}
//		return CanSpreadX;
//	}

//	/// <summary>
//	/// Determines the largest area that the current box collider can be spread out to in the Y direction.
//	/// </summary>
//	private bool TrySpreadY(ref Block[, ,] blocks, Vector3Int fragSize, bool CanSpreadY, ref bool[, ,] tested, Vector3Int boxStart, ref Vector3Int boxSize){
//		int xLimit = boxStart.x + boxSize.x;
//		int zLimit = boxStart.z + boxSize.z;

//		for (int x = boxStart.x; x < xLimit; x++) {
//			for (int z = boxStart.z; z < zLimit; z++) {
//				int currY = boxStart.y + boxSize.y;

//				//If we are out of bounds, have already checked here, or on an air block it can't spread.
//				lock (blocks.SyncRoot) {
//					if (currY >= fragSize.y || tested [x, currY, z] || blocks [x, currY, z].IsAir)
//						CanSpreadY = false;
//				}
//			}
//		}

//		//The box can spread. Mark it tested and increase it's size!
//		if (CanSpreadY) {
//			for (int x = boxStart.x; x < xLimit; x++) {
//				for (int z = boxStart.z; z < zLimit; z++) {
//					int currY = boxStart.y + boxSize.y;
//					tested [x, currY, z] = true;
//				}
//			}
//			boxSize.y++;
//		}
//		return CanSpreadY;
//	}

//	/// <summary>
//	/// Determines the largest area that the current box collider can be spread out to in the Z direction.
//	/// </summary>
//	private bool TrySpreadZ(ref Block[, ,] blocks, Vector3Int fragSize, bool CanSpreadZ, ref bool[, ,] tested, Vector3Int boxStart, ref Vector3Int boxSize){
//		int xLimit = boxStart.x + boxSize.x;
//		int yLimit = boxStart.y + boxSize.y;

//		for (int x = boxStart.x; x < xLimit; x++) {
//			for (int y = boxStart.y; y < yLimit; y++) {
//				int currZ = boxStart.z + boxSize.z;

//				//If we are out of bounds, have already checked here, or on an air block it can't spread.
//				lock (blocks.SyncRoot) {
//					if (currZ >= fragSize.z || tested [x, y, currZ] || blocks [x, y, currZ].IsAir)
//						CanSpreadZ = false;
//				}
//			}
//		}

//		//The box can spread. Mark it tested and increase it's size!
//		if (CanSpreadZ) {
//			for (int x = boxStart.x; x < xLimit; x++) {
//				for (int y = boxStart.y; y < yLimit; y++) {
//					int currZ = boxStart.z + boxSize.z;
//					tested [x, y, currZ] = true;
//				}
//			}
//			boxSize.z++;
//		}
//		return CanSpreadZ;
//	}

//	/// <summary>
//	/// Takes the greedy solution calcuated and creates the according box meshes on the gameobject.
//	/// </summary>
//	/// <param name="boxData">Box data.</param>
//	private IEnumerator SetCollisionMesh(Dictionary<Vector3Int, Vector3Int> boxData){
//		int colliderIndex = 0;
//		int exisitingColliderCount = boxColliders.Count;

//		foreach (KeyValuePair<Vector3Int, Vector3Int> box in boxData) {
//			//Position is the center of the box collider
//			Vector3 position = (Vector3)box.Key + ((Vector3)box.Value / 2.0f);

//			//Fix some wierd placement issue.
//			position.x -= 0.5f;
//			position.y -= 0.5f;
//			position.z -= 0.5f;

//			//If an old collider can be reused
//			if (colliderIndex < exisitingColliderCount) {
//				boxColliders [colliderIndex].center = position;
//				boxColliders [colliderIndex].size = (Vector3)box.Value;
//			}
//			//Else if there were more boxes on this mesh gen than on previous
//			else {
//				//Add new boxCollider to the container.
//				BoxCollider boxColl = gameObject.AddComponent<BoxCollider> ();

//				//Set it's size and position accordingly
//				boxColl.center = position;
//				boxColl.size = (Vector3)box.Value;

//				//Add to reference list.
//				boxColliders.Add (boxColl);
//			}
//			colliderIndex++;

//			if (colliderIndex == 40)
//				yield return null;
//		}

//		//Deletes all the unused boxes if this mesh gen has less.
//		if (colliderIndex < exisitingColliderCount) {
//			for (int i = exisitingColliderCount - 1; i >= colliderIndex; i--)
//				Destroy (boxColliders [i]);
//		}

//		//Ensure we didn't hit a negative
//		int diff = exisitingColliderCount - colliderIndex;
//		int range = (diff > 0) ? diff : 0;

//		boxColliders.RemoveRange (colliderIndex, range);
//	}

//	#endregion

//}
