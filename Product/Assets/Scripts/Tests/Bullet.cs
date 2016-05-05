using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody2D> ().AddForce (transform.right * -70.0f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
