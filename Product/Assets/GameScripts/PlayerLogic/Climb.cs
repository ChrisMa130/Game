using UnityEngine;
using System.Collections;

public class Climb : MonoBehaviour {

	public Sprite climb1;
	public Sprite climb2;

	public float interval = .1f;
	private float currentInter;
	// Use this for initialization
	void Awake () {
		currentInter = interval;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.W) || Input.GetKey(KeyCode.S)) {
			if (currentInter > 0f)
				currentInter -= Time.deltaTime;
			else {
				if (GetComponent<SpriteRenderer> ().sprite == climb1)
					GetComponent<SpriteRenderer> ().sprite = climb2;
				else
					GetComponent<SpriteRenderer> ().sprite = climb1;
				currentInter = interval;
			}
		}
	}
}
