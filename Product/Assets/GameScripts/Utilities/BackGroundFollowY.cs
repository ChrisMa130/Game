using UnityEngine;
using System.Collections;

public class BackGroundFollowY : MonoBehaviour {

	private float dealtaY;

	private float delay = 0f;
	void Start() {
		dealtaY = transform.position.y - Camera.main.transform.position.y;
	}

	// Update is called once per frame
	void Update () {
		if (delay > 0f)
			delay -= Time.deltaTime;
		else {
			Vector3 newPos = new Vector3 (transform.position.x, Camera.main.transform.position.y + dealtaY, transform.position.z);
			transform.position = newPos;
		}
	}
}
