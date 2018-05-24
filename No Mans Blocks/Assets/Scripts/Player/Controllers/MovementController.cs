using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxelated.Utilities;

[RequireComponent (typeof (CharacterController))]
public class MovementController : MonoBehaviour {
	public float walkSpeed = 6.0f;
	public float runSpeed = 11.0f;

	public float jumpSpeed = 8.0f;
	public float gravity = 20.0f;

	// Units that player can fall before a falling damage function is run. To disable, type "infinity" in the inspector
	public float fallingDamageThreshold = 10.0f;

	// If checked, then the player can change direction while in the air
	public bool airControl = false;

	// Small amounts of this results in bumping when walking down slopes, but large amounts results in falling too fast
	public float antiBumpFactor = .75f;

	// Player must be grounded for at least this many physics frames before being able to jump again; set to 0 to allow bunny hopping
	public int antiBunnyHopFactor = 1;

	private Vector3 moveDirection = Vector3.zero;
	private bool grounded = false;
	private bool isActive = true;
	private CharacterController controller;
	private Transform myTransform;
	private float speed;
	private RaycastHit hit;
	private float fallStartLevel;
	private bool falling;
	private bool playerControl = false;
	private int jumpTimer;

	void Start() {
		controller = GetComponent<CharacterController>();
		myTransform = transform;
		speed = walkSpeed;
		jumpTimer = antiBunnyHopFactor;
	}

	void FixedUpdate() {
		if (isActive) {
			//Get input values, and determine if we need to limit due to diagonal.
			float inputX = Input.GetAxis ("Horizontal");
			float inputY = Input.GetAxis ("Vertical");
			float inputModifyFactor = (inputX != 0.0f && inputY != 0.0f) ? .7071f : 1.0f;

			if (grounded) {
				// If we were falling, and we fell a vertical distance greater than the threshold, run a falling damage routine
				if (falling) {
					falling = false;
					//Take fall damage
					if (myTransform.position.y < fallStartLevel - fallingDamageThreshold)
						FallingDamageAlert (fallStartLevel - myTransform.position.y);
				}

				moveDirection = new Vector3 (inputX * inputModifyFactor, -antiBumpFactor, inputY * inputModifyFactor);
				moveDirection = myTransform.TransformDirection (moveDirection) * speed;
				playerControl = true;

				// Jump! But only if the jump button has been released and player has been grounded for a given number of frames
				if (!Input.GetButton ("Jump"))
					jumpTimer++;
				else if (jumpTimer >= antiBunnyHopFactor) {
					moveDirection.y = jumpSpeed;
					jumpTimer = 0;
				}
			} else {
				// If we stepped over a cliff or something, set the height at which we started falling
				if (!falling) {
					falling = true;
					fallStartLevel = myTransform.position.y;
				}

				// If air control is allowed, check movement but don't touch the y component
				if (airControl && playerControl) {
					moveDirection.x = inputX * speed * inputModifyFactor;
					moveDirection.z = inputY * speed * inputModifyFactor;
					moveDirection = myTransform.TransformDirection (moveDirection);
				}
			}

			// Apply gravity
			moveDirection.y -= gravity * Time.deltaTime;

			// Move the controller, and set grounded true or false depending on whether we're standing on something
			grounded = (controller.Move (moveDirection * Time.deltaTime) & CollisionFlags.Below) != 0;
		}
	}



	// If falling damage occured, this is the place to do something about it. You can make the player
	// have hitpoints and remove some of them based on the distance fallen, add sound effects, etc.
	void FallingDamageAlert (float fallDistance) {
		print ("Ouch! Fell " + fallDistance + " units!");   
	}

	void OnEnable(){
        InputManager.OnConsoleFocus += InputManager_OnConsoleFocus;
        InputManager.OnConsoleRelease += InputManager_OnConsoleRelease;

	}

    private void InputManager_OnConsoleRelease(object sender, System.EventArgs e) {
        Activate();
    }

    private void InputManager_OnConsoleFocus(object sender, System.EventArgs e) {
        DeActivate();
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