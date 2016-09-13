using UnityEngine;
using System.Collections;

public class Fade : MonoBehaviour {


	public float maxAlpha = 0.8f;
	public float minAlpha = 0.2f;
	public float alpha;

	public float fadeTime = 1.5f; 

	private float fadeSpeed;
	private bool fadeout;
	// Use this for initialization
	void Awake() {
		alpha = maxAlpha;
		adjustAlpha ();
		fadeSpeed = (1.0f / fadeTime);
		fadeout = true;
	}
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		alpha = transform.GetComponent<SpriteRenderer> ().material.color.a;
		if (alpha > maxAlpha)
			fadeout = true;
		else if(alpha < minAlpha)
			fadeout = false;

		if (alpha > minAlpha && fadeout)
			FadeSequence (-fadeSpeed);
		else
			FadeSequence (fadeSpeed);
	}
			

	private void FadeSequence (float fadeSpeed) {
		alpha += Time.deltaTime * fadeSpeed;
		adjustAlpha ();
	}

	private void adjustAlpha() {
		Color newColor = transform.GetComponent<SpriteRenderer> ().material.color;
		newColor.a = alpha;
		transform.GetComponent<SpriteRenderer> ().material.SetColor ("_Color", newColor);
	}
}
