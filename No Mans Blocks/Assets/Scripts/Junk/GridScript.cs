using UnityEngine;
using System.Collections;
using Voxelated;
using Voxelated.Terrain;

public class GridScript : MonoBehaviour {
    private Renderer mrenderer;

	void Awake(){
		mrenderer = GetComponent<Renderer> ();
	}

	/// <summary>
	/// Sets the position of the wall. Takes an input of one of the four
	/// cardinal directions. The rest of the work is performed by the method.
	/// </summary>
	public void SetGrid(int worldLength, int worldHeight, int worldWidth, Direction direction){
		//Prevents ugly vertex issues.
		float offset = 0.001f;

		//This is how much of the map we keep out of bounds.
		int borderBlockSize = WorldSettings.BorderBlockSize;

		switch (direction) {
		case (Direction.north):
			SetSize (worldLength, worldHeight);
			Vector3 northPos = new Vector3 (worldLength / 2.0f - 0.5f + borderBlockSize, worldHeight / 2.0f - 0.5f, worldWidth + borderBlockSize - 0.5f - offset);
			transform.position = northPos;
			transform.Rotate (90.0f, 180.0f, 0.0f);
			break;

		case (Direction.east):
			SetSize (worldWidth, worldHeight);
			Vector3 eastPos = new Vector3 (worldLength + borderBlockSize - 0.5f - offset, worldHeight / 2.0f - 0.5f, worldWidth / 2 - 0.5f + borderBlockSize);
			transform.position = eastPos;
			transform.Rotate (90.0f, -90.0f, 0.0f);
			break;

		case (Direction.south):
			SetSize (worldLength, worldHeight);
			Vector3 southPos = new Vector3 (worldLength / 2.0f - 0.5f + borderBlockSize, worldHeight / 2.0f - 0.5f, borderBlockSize - 0.5f + offset);
			transform.position = southPos;
			transform.Rotate (90.0f, 0.0f, 0.0f);
			break;

		case (Direction.west):
			SetSize (worldWidth, worldHeight);
			Vector3 westPos = new Vector3 (borderBlockSize -0.5f + offset, worldHeight / 2.0f - 0.5f, worldWidth / 2.0f - 0.5f + borderBlockSize);
			transform.position = westPos;
			transform.Rotate (90.0f, 0.0f, 270.0f);
			break;
		}
	}

	/// <summary>
	/// Sets the size of the wall. Inputs should be in block values.
	/// </summary>
	void SetSize(int length, int height){
		int borderBlockSize = WorldSettings.BorderBlockSize;
		float l = length / 10f;
		float h = height / 10f;

		//Set the scale of the wall to match world size.
		Vector3 newScale = new Vector3 (l, 1.0f, h);
		transform.localScale = newScale;

		//Set shader to match block size of grid
		mrenderer.material.mainTextureScale = new Vector2(1.0f / (512.0f / (length - 2 * borderBlockSize)), 1.0f);
	}
}
