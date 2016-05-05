using UnityEngine;
using System.Collections;

public class ForbiddenZone : MonoBehaviour {

	// Use this for initialization
	private GameManager manager;
	void Awake () {
		manager = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameManager> ();
	}
	
	void OnMouseOver() {
		manager.canDraw = false;
	}
	void OnMouseExit() {
		manager.canDraw = true;
	}
	void OnMouseDown() {

	}
}
