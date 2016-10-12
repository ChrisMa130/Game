using UnityEngine;
using System.Collections;

public class Climb : MonoBehaviour {

	public Sprite climb1;
	public Sprite climb2;

	public float interval = .3f;
	private float currentInter;
	// Use this for initialization
	void Awake () {
		currentInter = interval;
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (GetCurrentInterval());
		if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.S)) {
			if (currentInter > 0f)
				currentInter -= Time.deltaTime;
		}
		if(currentInter < 0f) {
				if (GetComponent<SpriteRenderer> ().sprite == climb1)
					GetComponent<SpriteRenderer> ().sprite = climb2;
				else
					GetComponent<SpriteRenderer> ().sprite = climb1;
				currentInter = interval;
		}
	}

	public float GetCurrentInterval() {
		return currentInter;
	}

	public void SetCurrentInterval(float inter) {
		currentInter = inter;
	}
}
