using UnityEngine;
using System.Collections;

public class Logo : MonoBehaviour {
	public float lifeTime = 5f;
	public float fadeTime = 5f;
	private float fadeSpeed;

	void Start() {
		adjustAlpha (0f);
		fadeSpeed = 1.0f / fadeTime;
	}
	// Update is called once per frame
	void Update () {
		float myAlpha = transform.GetComponent<SpriteRenderer> ().material.color.a;
		if ((myAlpha < 0.99f) && (lifeTime > 0f)) {
			FadeSequence (fadeSpeed);
		} else if ((myAlpha >= 0.99f) && (lifeTime > 0f)) {
			lifeTime -= Time.deltaTime;
		} else
			FadeSequence (-fadeSpeed);
	}

	private void FadeSequence (float fadeSpeed) {
		float myAlpha = transform.GetComponent<SpriteRenderer> ().material.color.a;
		float newAlpha = myAlpha + Time.deltaTime * fadeSpeed;
		adjustAlpha (newAlpha);
	}

	private void adjustAlpha(float newAlpha) {
		Color newColor = transform.GetComponent<SpriteRenderer> ().material.color;
		newColor.a = newAlpha;
		transform.GetComponent<SpriteRenderer> ().material.SetColor ("_Color", newColor);
	}
}
