using UnityEngine;
using System.Collections;
using Voxelated;

/// <summary>
/// Selector
/// 
/// This is the transparent cube that forms around the blocks that the player is interacting with. It requires a cubeMesh for
/// rendering and is very basic
/// </summary>
public class Selector : MonoBehaviour {

	//What is displayed on screen.
	CubeMesh cubeMesh;

	//Position memory
	Vector3 currPos;
	Vector3 prevPos;
    
	//Rotation memory
	Quaternion currRot;
	Quaternion prevRot;

	//Lerp Control
	Vector3 lerpPosStart;
	Vector3 lerpPosEnd;
	Quaternion lerpRotStart;
	Quaternion lerpRotEnd;
	float lerpTimer;
	float lerpSpeed;
	bool isLerping;

	//Dimensions of tool size.
	Vector3 size;

	//Easy access to Size and Position.
	public Vector3 Position    { get { return currPos; } set { currPos = value; } }
	public Quaternion Rotation { get { return currRot; } set { currRot = value; } }

    /// <summary>
    /// Set the size of the visible selector, and update its mesh.
    /// </summary>
    public Vector3 Size {
        get {
            return size;
        }

        set {
            size = value;
            cubeMesh.UpdateSize(size);
        }
    }

	// Use this for initialization
	void Awake () {
		cubeMesh = GetComponent<CubeMesh> ();

		lerpTimer = 0.0f;
		lerpSpeed = 12.0f;
		isLerping = false;
	}
	
	// Update is called once per frame
	void Update () {
		////If new position is null. Destroy this object
		//if(currPos.Equals(Vector3Int.Null))
		//	Destroy(this.gameObject);

		////If new position is different. Lerp to it.
		//if (!prevPos.Equals (currPos)) {
		//	BeginLerping ();
		//	prevPos = currPos;
		//	prevRot = currRot;
		//}

		//if (isLerping)
		//	UpdateLerping ();
	}

    /// <summary>
    /// Begins the lerping of the gameobject to it's new target.
    /// </summary>
    void BeginLerping(){
		lerpPosStart = prevPos;
		Vector3 newPos = currPos;
		lerpPosEnd = newPos;

		lerpRotStart = prevRot;
		Quaternion newRot = currRot;
		lerpRotEnd = newRot;

		//Fix odd # dimension
		if ((int)size.x % 2 == 0)
			lerpPosEnd [0] -= 0.5f;
		if ((int)size.y % 2 == 0)
			lerpPosEnd [1] -= 0.5f;
		if ((int)size.z % 2 == 0)
			lerpPosEnd [2] -= 0.5f;

		lerpTimer = 0.0f;
		isLerping = true;
	}

	//Update lerping process

	/// <summary>
	/// Continue the lerping movement from prevPosition to new Position
	/// </summary>
	void UpdateLerping(){
		lerpTimer += UnityEngine.Time.deltaTime;

		if (lerpTimer > 1.0f) {
			lerpTimer = 1.0f;
			isLerping = false;
		}
			
		this.transform.position = Vector3.Lerp (lerpPosStart, lerpPosEnd, lerpTimer * lerpSpeed);
		this.transform.rotation = Quaternion.Lerp (lerpRotStart, lerpRotEnd, lerpTimer * lerpSpeed);
	}


}
