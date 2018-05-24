using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Script that takes a screentshot of game view and sends it to the screenshot folder
/// </summary>
public class ScreenShotScript : MonoBehaviour {
	private static string folderName = "Screenshots";
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.RightAlt))
			ScreenShot ();
	}

	void ScreenShot(){
		Debug.Log ("Say Cheese!");
		string fileName = System.DateTime.Now.ToString ();
		fileName = fileName.Replace ('/', '-');
		fileName = fileName.Replace (':', '-');

		Debug.Log (fileName);
		ScreenCapture.CaptureScreenshot(folderName + "/" + fileName + "capture.png");
	}
}
