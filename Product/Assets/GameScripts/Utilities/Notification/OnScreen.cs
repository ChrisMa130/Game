using UnityEngine;
using System.Collections;

public class OnScreen : MonoBehaviour {

	public bool Error;
	public float LifeTime;
//	public float FadeTime;
//	public float Min;
//	public float Max;
	public float TransitionTime;

    public bool Fade;

	private float currentTime;

//	private float FadeSpeed;
//	private bool fadeout;
	// Use this for initialization
	void Start () {
		transform.parent = GameObject.FindWithTag ("MainCamera").transform;
		transform.localPosition = new Vector3 (0f, 2.05f, 10f);
		GetComponent<MeshRenderer>().sortingLayerName = "Foreground";
		currentTime = TransitionTime;
//		FadeSpeed = (1.0f / FadeTime);
	}
	
	// Update is called once per frame
	void Update () {
		LifeTime -= Time.deltaTime;
		if (Error)
			ColorControl ();
        if (Fade)
		if (LifeTime < 0)
			Destroy (gameObject);
	}

	private void ColorControl() {
		currentTime -= Time.deltaTime;
		if (currentTime < 0f) {
			if (GetComponent<TextMesh> ().color.g > 0f)
				GetComponent<TextMesh> ().color = Color.red;
			else
				GetComponent<TextMesh> ().color = Color.white;
			currentTime = TransitionTime;
		}
//		Color color = GetComponent<TextMesh> ().color;
//		if(color.b > Max)
//		float blue = color.b + Time.deltaTime * FadeSpeed;
//		float green = color.g + Time.deltaTime * FadeSpeed;
//		Color newColor = new Color32(color.r, blue, green);
//		GetComponent<TextMesh> ().color = newColor;
	}
}
