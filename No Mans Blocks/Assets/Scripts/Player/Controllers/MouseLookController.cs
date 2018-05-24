using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxelated.Utilities;

/// <summary>
/// Mouse Look Controller.
/// 
/// Allows for rotation of player camera via mouse control.
/// Rotates player gameobject as well.
/// </summary>
public class MouseLookController : MonoBehaviour {
	public Vector2 clampInDegrees = new Vector2(360, 180);
	public Vector2 sensitivity = new Vector2(2, 2);
	public Vector2 smoothing = new Vector2(3, 3);

	private Vector2 targetDirection;
	private Vector2 targetCharacterDirection;
	private GameObject characterBody;

	[SerializeField]
	private bool invertYAxis;
	private bool isActive;
	private Vector2 mouseAbs;
	private Vector2 mouseSmooth;

	public bool InvertYAxs { get { return invertYAxis; } set { invertYAxis = value; } }

	void Awake () { 
		isActive = true;

		characterBody = transform.parent.gameObject;

		// Set target direction to the camera's initial orientation.
		targetDirection = transform.localRotation.eulerAngles;
		targetCharacterDirection = characterBody.transform.localRotation.eulerAngles;
	}

	// Update is called once per frame
	void Update () {
		if (isActive) {
			// Allow the script to clamp based on a desired target value.
			Quaternion targetOrientation = Quaternion.Euler(targetDirection);
			Quaternion targetCharacterOrientation = Quaternion.Euler(targetCharacterDirection);

			// Get raw mouse input for a cleaner reading on more sensitive mice.
			Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

			if (invertYAxis)
				mouseDelta.y *= -1;

			// Scale input against the sensitivity setting and multiply that against the smoothing value.
			mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity.x * smoothing.x, sensitivity.y * smoothing.y));

			// Interpolate mouse movement over time to apply smoothing delta.
			mouseSmooth.x = Mathf.Lerp(mouseSmooth.x, mouseDelta.x, 1f / smoothing.x);
			mouseSmooth.y = Mathf.Lerp(mouseSmooth.y, mouseDelta.y, 1f / smoothing.y);

			// Find the absolute mouse movement value from point zero.
			mouseAbs += mouseSmooth;

			// Clamp and apply the local x value first, so as not to be affected by world transforms.
			if (clampInDegrees.x < 360)
				mouseAbs.x = Mathf.Clamp(mouseAbs.x, -clampInDegrees.x * 0.5f, clampInDegrees.x * 0.5f);

			Quaternion xRotation = Quaternion.AngleAxis(-mouseAbs.y, targetOrientation * Vector3.right);
			transform.localRotation = xRotation;

			// Then clamp and apply the global y value.
			if (clampInDegrees.y < 360)
				mouseAbs.y = Mathf.Clamp(mouseAbs.y, -clampInDegrees.y * 0.5f, clampInDegrees.y * 0.5f);

			transform.localRotation *= targetOrientation;

			//Handle character body
			Quaternion yRotation = Quaternion.AngleAxis(mouseAbs.x, characterBody.transform.up);
			characterBody.transform.localRotation = yRotation;
			characterBody.transform.localRotation *= targetCharacterOrientation;
		}
	}
		
	void OnEnable(){
        InputManager.OnConsoleFocus += InputManager_OnConsoleFocus;
        InputManager.OnConsoleRelease += InputManager_OnConsoleRelease;

	}

    private void InputManager_OnConsoleRelease(object sender, System.EventArgs e) {
        DeActivate();
    }

    private void InputManager_OnConsoleFocus(object sender, System.EventArgs e) {
        Activate();
    }

    void OnDisable(){
        InputManager.OnConsoleFocus -= InputManager_OnConsoleFocus;
        InputManager.OnConsoleRelease -= InputManager_OnConsoleRelease;
    }

	/// <summary>
	/// Re enable update() of this script.
	/// </summary>
	public void Activate(){
		isActive = true;
	}

	/// <summary>
	/// Turns off update() of the script.
	/// </summary>
	public void DeActivate(){
		isActive = false;
	}
}
