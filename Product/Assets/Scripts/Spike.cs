using UnityEngine;
using System.Collections;

public class Spike : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {
		other.GetComponent<Animator> ().SetBool ("Dead", true);
		other.GetComponent<Rigidbody2D> ().isKinematic = true;
	}
}
