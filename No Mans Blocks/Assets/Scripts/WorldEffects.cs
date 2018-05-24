using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using Voxelated.Utilities;
//using Voxelated.Terrain;
//using Voxelated;
//using Voxelated.Settings;

///// <summary>
///// World effects.
///// 
///// This will be the class to handle all effects such as rigid body blocks falling or hit particles.
///// There should only be one of these in the scene.
///// </summary>
public class WorldEffects : MonoBehaviour {
//	public GameObject worldFragment;
//	public GameObject blockHitParticles;
//	public GameObject blockDeathParticles;
//	public GameObject gridPrefab;
//	public ObjectPool objectPool;

//	//Used for finding blocks etc...
//	private World world;
//	private WorldRenderer worldRenderer;

//	//Blocks that are floating are thrown here.
//	private ThreadableQueue<List<Vector3Int>> floaters = new ThreadableQueue<List<Vector3Int>>();

//	public WorldRenderer WorldRenderer { get { return worldRenderer; } }

//	//Find needed references
//	void Awake(){
//		worldRenderer   = GetComponent<WorldRenderer> ();
//	}

//	void Update(){
//		if (floaters.Count > 0) {
//			List<Vector3Int> currFloaters = floaters.Dequeue ();
//			StartCoroutine(GenerateFragments (currFloaters));

//			//Delete blocks from world.
//		//	foreach (Vector3Int delPos in currFloaters)
//				//world.SetBlock (delPos.x, delPos.y,delPos.z, Block.Air);
//		}
//	}

//	//Subscribe to any desired events
//	void OnEnable() {
//		//EventUtils.OnWorldLoad += SpawnGrids;
//	}

//	//Desubscribe from any events
//	void OnDisable() {
//		//EventUtils.OnWorldLoad -= SpawnGrids;
//	}

//	/// <summary>
//	/// Creates the four walls that should surrond the map.
//	/// </summary>
//	public void SpawnGrids(){
//		Direction[] gridDirections = { Direction.north, Direction.east, Direction.south, Direction.west };

//		foreach (Direction direction in gridDirections) {
//			GameObject gridGO = (GameObject)Instantiate (gridPrefab, new Vector3 (0.0f, 0.0f, 0.0f), Quaternion.identity);
//			gridGO.transform.SetParent (transform.GetChild (3));
//			GridScript gridScript = gridGO.GetComponent<GridScript> ();
//			gridGO.name = "Grid-" + direction.ToString ();

//            //Set the position and location of the wall.
//            Vector3Int worldSize = WorldSettings.PlayableChunkSize;
//			gridScript.SetGrid (worldSize.x * Chunk.ChunkSize, (worldSize.y + 1) * Chunk.ChunkSize, worldSize.z * Chunk.ChunkSize, direction);
//		}
//	}

//	/// <summary>
//	/// Add blocks to floater list. This also prevents threading issues.
//	/// </summary>
//	public void AddFloaters(List<Vector3Int> newFloaters){
//		floaters.Enqueue (newFloaters);
//	}

//	#region Block Particles
//	/// <summary>
//	/// Spawns hit particles on the block at the point of impact. This
//	/// is called in TerrainEdit.DamageBlock().
//	/// </summary>
//	public void SpawnHitParticles(int x, int y, int z, int[] hitInfo){
//        Block blockHit = Block.Air;// world.GetBlock(x, y, z);

//        //If it was null or an air block return.
//        if (blockHit.IsAir)
//            return;

//        Vector3 hitNorm = TerrainEdit.GetFaceNormal(hitInfo[0]).ToVector3();
//        Vector2 hitPosOnFace = new Vector2((hitInfo[1] / 64.0f) - 0.5f, (hitInfo[2] / 64.0f) - 0.5f);
//        Vector3 spawnPos = new Vector3(x + (hitNorm.x / 2.0f), y + (hitNorm.y / 2.0f), z + (hitNorm.z / 2.0f));

//        //Add face offset depending on the normal.
//        if (hitNorm.x != 0) {
//            spawnPos.y += hitPosOnFace.x;
//            spawnPos.z += hitPosOnFace.y;
//        }
//        else if(hitNorm.y != 0) {
//            spawnPos.x += hitPosOnFace.x;
//            spawnPos.z += hitPosOnFace.y;
//        }
//        else {
//            spawnPos.x += hitPosOnFace.x;
//            spawnPos.y += hitPosOnFace.y;
//        }
        
//        //Instantiate new particles and get a reference to their particle system.
//        GameObject particlesGO = objectPool.GetItem("HitParticles");
//		particlesGO.transform.position = spawnPos;
//		particlesGO.transform.rotation = Quaternion.LookRotation (hitNorm);

//        //Set their color to match the block and set burst size.
//        ParticleSystem particles = particlesGO.GetComponent<ParticleSystem> ();
//		var main = particles.main;
//		main.startColor = (Color)blockHit.Color;
//		particles.emission.SetBursts(new ParticleSystem.Burst[]{ new ParticleSystem.Burst (0.0f, 3) });
//	}

//	/// <summary>
//	/// Spawn death particles at block. Sets the color to match the block and how many.
//	/// </summary>
//	public void SpawnDeathParticles(float x, float y, float z, Block block, Quaternion quaternion){
//		Vector3 particlePosition = new Vector3 (x,y,z);

//		//Instantiate new particles and set up their color / burst size
//		//GameObject particlesGO = (GameObject) Instantiate (blockDeathParticles, particlePosition, quaternion);
//		GameObject particlesGO = objectPool.GetItem("DestroyParticles");
//		particlesGO.transform.position = particlePosition;
//		particlesGO.transform.rotation = quaternion;

//		ParticleSystem particles = particlesGO.GetComponent<ParticleSystem> ();

//		//Set color to match block and emit 8 of them.
//		var main = particles.main;
//		main.startColor = (Color) block.Color;

//		particles.emission.SetBursts(new ParticleSystem.Burst[]{ new ParticleSystem.Burst (0.0f, 10) });
//	}
//	#endregion

//	#region Fragments
//	/// <summary>
//	/// Takes a list of blocks and generates one giant world fragment for it.
//	/// This handles the mass, and colliders too.
//	/// </summary>
//	/// <param name="blocks">Blocks.</param>
//	IEnumerator GenerateFragments(List<Vector3Int> blockPositions){
//		float timer = 0.0f;
//		float timeLimit = 1 / 60.0f;

//		Vector3Int spawnPos = new Vector3Int (0, 0, 0);

//		//Get the blocks we need from the world.
//		Block[, ,] blocks = GetBlocksFromWorld(blockPositions, ref spawnPos);

//		//Get how big each fragment should be.
//		Vector3Int blockDimensions = new Vector3Int (blocks.GetLength (0), blocks.GetLength (1), blocks.GetLength (2));

//		//Figure out how many fragments we need to make.
//		Vector3Int splitCounts = new Vector3Int (0, 0, 0);
//		splitCounts.x = (int) Mathf.Min (blockDimensions.x, 5);
//		splitCounts.y = (int) Mathf.Min (blockDimensions.y, 5);
//		splitCounts.z = (int) Mathf.Min (blockDimensions.z, 5);

//		//Determine their dimensions and create a gameobject array
//		Vector3Int[, ,] fragmentDimensions = GetFragmentDimensions (blockDimensions, splitCounts);
//		GameObject[, ,] fragmentGOs = new GameObject[splitCounts.x, splitCounts.y, splitCounts.z];

//		//Calcuate their spawn offsets.
//		Vector3Int[, ,] spawnOffsets = CalculateFragmentSpawnOffsets(fragmentDimensions);

//		//Iterate through each object to spawn.
//		for (int x = 0; x < spawnOffsets.GetLength (0); x++) {
//			for (int y = 0; y < spawnOffsets.GetLength (1); y++) {
//				for (int z = 0; z < spawnOffsets.GetLength (2); z++) {
//					//Generate spawn point. 
//					Vector3Int currSpawnOff = spawnOffsets[x,y,z];
//					Vector3Int currSpawnPos = new Vector3Int (0, 0, 0);
//					currSpawnPos.x += currSpawnOff.x + spawnPos.x;
//					currSpawnPos.y += currSpawnOff.y + spawnPos.y;
//					currSpawnPos.z += currSpawnOff.z + spawnPos.z;

//					//Find the blocks we want
//					Vector3Int currDims = fragmentDimensions[x,y,z];
//					Block[, ,] currBlocks = new Block[currDims.x, currDims.y, currDims.z];
//					bool isOnlyAir = true;

//					//Get the blocks for it and make sure we want to spawn it.
//					for (int bX = 0; bX < currDims.x; bX++) {
//						for (int bY = 0; bY < currDims.y; bY++) {
//							for (int bZ = 0; bZ < currDims.z; bZ++) {
//								currBlocks [bX, bY, bZ] = blocks [bX + currSpawnOff.x, bY + currSpawnOff.y, bZ + currSpawnOff.z];

//								//If we hit a non air block this fragment is worth instantiating.
//								if (!currBlocks [bX, bY, bZ].IsAir && isOnlyAir)
//									isOnlyAir = false;
//							}
//						}
//					}

//					//Ensure the fragment is not just air.
//					if (isOnlyAir)
//						continue;

//					//Get a fragment from the objectpool

//					//GameObject worldFragGO = Instantiate (worldFragment, currSpawnPos.ToVector3 (), Quaternion.identity) as GameObject;
//					GameObject worldFragGO = objectPool.GetItem("Fragment");
//					worldFragGO.transform.position = currSpawnPos.ToVector3 ();

//					//Instantiate object and get reference to world fragment script
//					fragmentGOs [x, y, z] = worldFragGO;
//					//WorldFragment currFrag = worldFragGO.GetComponent<WorldFragment> ();
//					//currFrag.objectPool = objectPool;
//					//currFrag.worldEffects = this;

//					//Give the fragment the blocks. Fragments love blocks
//					//currFrag.SetBlocks (currBlocks);
//				}
//			}

//			timer += Time.deltaTime;
//			if (timer > timeLimit) {
//				yield return null;
//				timer = 0.0f;
//			}
//		}
//	}

//	/// <summary>
//	/// Converts position list into a block array that we can then use in the PrefabRenderer.
//	/// </summary>
//	public Block[, ,] GetBlocksFromWorld(List<Vector3Int> positions, ref Vector3Int spawnPos){
//		//Determine how big of an array we need and instantiate it.
//		Vector3Int blockDims = DetermineDimensions (positions, ref spawnPos);
//		Block[, ,] blocks = new Block[ blockDims.x, blockDims.y, blockDims.z];

//		//For every position in the list get the proper block and store it.
//		foreach (Vector3Int pos in positions) {
//			Vector3Int localPos = new Vector3Int (pos.x - spawnPos.x, pos.y - spawnPos.y, pos.z - spawnPos.z);
//            blocks[localPos.x, localPos.y, localPos.z] = Block.Air; //world.GetBlock (pos.x, pos.y, pos.z);
//		}

//		return blocks;
//	}

//	/// <summary>
//	/// Finds the dimensions of the block list and where we need to instantiate the GO at.
//	/// </summary>
//	Vector3Int DetermineDimensions(List<Vector3Int> blocks, ref Vector3Int spawnPos){
//		Vector3Int min = new Vector3Int (blocks [0]);
//		Vector3Int max = new Vector3Int (blocks [0]);

//		foreach (Vector3Int pos in blocks) {
//			//Find minimum coordinates.
//			min.x = pos.x < min.x ? pos.x : min.x;
//			min.y = pos.y < min.y ? pos.y : min.y;
//			min.z = pos.z < min.z ? pos.z : min.z;

//			//Find maximum coordinates
//			max.x = pos.x > max.x ? pos.x : max.x;
//			max.y = pos.y > max.y ? pos.y : max.y;
//			max.z = pos.z > max.z ? pos.z : max.z;
//		}

//		//We need this to know where in the world we are.
//		spawnPos = new Vector3Int (min.x, min.y, min.z);
//		return new Vector3Int (max.x - min.x + 1, max.y - min.y + 1, max.z - min.z + 1);
//	}




//	/// <summary>
//	/// Determine the spawnpoint offset of each fragment to generate.
//	/// </summary>
//	/// <returns>The fragment spawns.</returns>
//	/// <param name="fragmentSizes">Fragment sizes.</param>
//	Vector3Int[, ,] CalculateFragmentSpawnOffsets(Vector3Int[, ,] fragmentSizes){
//		Vector3Int[, ,] spawnOffs = new Vector3Int[fragmentSizes.GetLength(0), fragmentSizes.GetLength(1), fragmentSizes.GetLength(2)];

//		for (int x = 0; x < spawnOffs.GetLength (0); x++) {
//			for (int y = 0; y < spawnOffs.GetLength (1); y++) {
//				for (int z = 0; z < spawnOffs.GetLength (2); z++) {
//					Vector3Int currPos = new Vector3Int (0, 0, 0);

//					//Add the previous spawnoffsets to the current one.
//					currPos.x = x > 0 ? spawnOffs[x - 1, y, z].x + fragmentSizes[x - 1, y, z].x : 0;
//					currPos.y = y > 0 ? spawnOffs[x, y - 1, z].y + fragmentSizes[x, y - 1, z].y : 0;
//					currPos.z = z > 0 ? spawnOffs[x, y, z - 1].z + fragmentSizes[x, y, z - 1].z : 0;

//					spawnOffs [x, y, z] = currPos;
//				}
//			}
//		}
//		return spawnOffs;
//	}

//	/// <summary>
//	/// Calculates how to evenly break up blocks into specific 
//	/// fragment sizes. Returns an array of dimensions for each
//	/// fragment.
//	/// </summary>
//	/// <returns>The fragment dimensions.</returns>
//	/// <param name="blockDims">Block dims.</param>
//	Vector3Int[, ,] GetFragmentDimensions(Vector3Int blockDims, Vector3Int splitCounts){
//		Vector3Int[, ,] fragDims = new Vector3Int[splitCounts.x, splitCounts.y, splitCounts.z];

//		//Find base size of each fragment dimension. And find how many remainder blocks exist in each direction.
//		Vector3Int fragSize  = new Vector3Int (blockDims.x / splitCounts.x, blockDims.y / splitCounts.y, blockDims.z / splitCounts.z);
//		Vector3Int remainder = new Vector3Int (blockDims.x % splitCounts.x, blockDims.y % splitCounts.y, blockDims.z % splitCounts.z);

//		for (int x = 0; x < fragDims.GetLength (0); x++) {
//			for (int y = 0; y < fragDims.GetLength (1); y++) {
//				for (int z = 0; z < fragDims.GetLength (2); z++) {
//					Vector3Int newDim = new Vector3Int (fragSize);
			
//						//Do we have any remainders we need to handle?
//					if (remainder.x != 0 && splitCounts.x >= 5 && shouldAddRemainder (remainder.x, x, splitCounts.x))
//						newDim.x++;
//					if (remainder.y != 0 && splitCounts.y >= 5 && shouldAddRemainder (remainder.y, y, splitCounts.y))
//						newDim.y++;
//					if (remainder.z != 0 && splitCounts.z >= 5 && shouldAddRemainder (remainder.z, z, splitCounts.z))
//						newDim.z++;

//					fragDims [x, y, z] = newDim;
//				}
//			}
//		}
//		return fragDims;
//	}

//	/// <summary>
//	/// Determines if a remainder value should be added to the current fragment
//	/// dimension.
//	/// </summary>
//	bool shouldAddRemainder(int remainder, int index, int splitCount){
//		if (index == 0 || index == 4) {
//			if(remainder == (splitCount - 1))
//				return true;
//		}

//		if (index == 1 || index == 3) {
//			if (remainder > 1)
//				return true;
//		}

//		if (index == 2) {
//			if (remainder % 2 == 1)
//				return true;
//		}

//		return false;
//	}

//	#endregion
}