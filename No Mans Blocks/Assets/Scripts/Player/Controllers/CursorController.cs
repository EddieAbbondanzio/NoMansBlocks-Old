using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxelated.Utilities;

/// <summary>
/// Cursor Controller
/// 
/// Handles locking cursor to screen and making it invisible.
/// </summary>
public class CursorController : MonoBehaviour {

	void Update () {
		if(Input.GetKeyDown(KeyCode.Mouse0) && Cursor.visible){
			LockCursor ();
		}

		if (Input.GetKeyDown (KeyCode.Escape) && !Cursor.visible) {
			UnlockCursor ();
		}
	}

	//Locks and hides cursor
	void LockCursor(){
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	//Unlocks and makes cursor visible
	void UnlockCursor(){
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	void OnEnable(){
        InputManager.OnConsoleFocus += InputManager_OnConsoleFocus;
        InputManager.OnConsoleRelease += InputManager_OnConsoleRelease;

	}

    private void InputManager_OnConsoleRelease(object sender, System.EventArgs e) {
        LockCursor();
    }

    private void InputManager_OnConsoleFocus(object sender, System.EventArgs e) {
        UnlockCursor();
    }

    void OnDisable(){
        InputManager.OnConsoleFocus -= InputManager_OnConsoleFocus;
        InputManager.OnConsoleRelease -= InputManager_OnConsoleRelease;
    }
}
