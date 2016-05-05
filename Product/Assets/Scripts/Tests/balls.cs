using UnityEngine;
using System.Collections;

public class balls : MonoBehaviour {
	public GameObject ball;

	void Start() {
		InvokeRepeating ("SpawnBall", 0f, 3f);
	}
	// Update is called once per frame
	void SpawnBall() {
		Instantiate (ball, new Vector3(2.25f, 0.83f, 0f), ball.transform.rotation);
	}
}
