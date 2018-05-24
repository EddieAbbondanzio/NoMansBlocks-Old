using UnityEngine;
using System.Collections;
using Voxelated.Utilities;

//Rotation method taken from here: http://gamedev.stackexchange.com/questions/104693/how-to-use-input-getaxismouse-x-y-to-rotate-the-camera

/// <summary>
/// Camera controller.
/// 
/// Basic controller that allows camera to "fly". Useful for build mode in map editor.
/// </summary>

public class CameraController : MonoBehaviour {
	
	public float mSpeed;		//Movement speed
	public float lSpeed;		//Look speed

	//Speed boost when holding left control.
	float boost;
	bool boosting;
	public bool isActive = true;

	float yaw = 0.0f;			//X rotation
	float pitch = 0.0f;			//Y rotation

	void Start() {
		boosting = false;
		boost = mSpeed;
	}

	// Update is called once per frame
	void Update () {
		if (isActive) {
			//Speed
			if (Input.GetKeyDown (KeyCode.LeftControl))
				boosting = true;

			if (Input.GetKeyUp (KeyCode.LeftControl))
				boosting = false;

			if (boosting)
				boost = 3.0f;
			else
				boost = 1.0f;

			if(Input.GetKeyDown(KeyCode.Mouse0) & Cursor.visible){
				InputManager.LockCursor ();
			}

			//Forward
			if (Input.GetKey (KeyCode.W)) {
				transform.Translate (Vector3.forward * mSpeed * boost * Time.deltaTime);
			}

			//Backwards
			if (Input.GetKey (KeyCode.S)) {
				transform.Translate (Vector3.back * mSpeed * boost * Time.deltaTime);
			}

			//Left
			if (Input.GetKey (KeyCode.A)) {
				transform.Translate (Vector3.left * mSpeed * boost * Time.deltaTime);
			}

			//Right
			if (Input.GetKey (KeyCode.D)) {
				transform.Translate (Vector3.right * mSpeed * boost * Time.deltaTime);
			}

			//Up
			if (Input.GetKey (KeyCode.Space)) {
				transform.Translate (Vector3.up * mSpeed * boost * Time.deltaTime);
			}

			//Down
			if (Input.GetKey (KeyCode.LeftShift)) {
				transform.Translate (Vector3.down * mSpeed * boost * Time.deltaTime);
			}


			//Rotate camera up / down
			yaw += lSpeed * Input.GetAxis ("Mouse X");
			pitch -= lSpeed * Input.GetAxis ("Mouse Y");
			pitch = Mathf.Clamp (pitch, -85, 90);


			transform.eulerAngles = new Vector3 (pitch, yaw, 0.0f);
		

			//Allow user to show cursor again.
			if (Input.GetKeyDown (KeyCode.Escape)) {
				InputManager.UnlockCursor ();
			}
		}
		
	}
	//Prevents memory leaks

	void OnEnable(){
        InputManager.OnConsoleFocus += InputManager_OnConsoleFocus;
        InputManager.OnConsoleRelease += InputManager_OnConsoleRelease;
	}

    private void InputManager_OnConsoleRelease(object sender, System.EventArgs e) {
        Activate();
        InputManager.LockCursor();
    }

    private void InputManager_OnConsoleFocus(object sender, System.EventArgs e) {
        DeActivate();
    }

    void OnDisable(){
        InputManager.OnConsoleFocus += InputManager_OnConsoleFocus;
        InputManager.OnConsoleRelease += InputManager_OnConsoleRelease;
    }



	//Prevent player from moving
	public void Activate(){
		isActive = true;
	}

	//Allow moving to resume
	public void DeActivate(){
		isActive = false;
	}
}
