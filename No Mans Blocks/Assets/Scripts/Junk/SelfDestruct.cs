using UnityEngine;
using System.Collections;

public class SelfDestruct : MonoBehaviour {
	public float timeLimit;
	float timer;


	// Use this for initialization
	void Start () {
		timer = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;

		if (timer > timeLimit)
			Destroy (this.gameObject);
	}
}
