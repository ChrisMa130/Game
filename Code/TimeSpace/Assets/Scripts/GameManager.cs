using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public bool Paused;
	public AudioClip slow;
	public AudioClip normal;
	AudioSource audioSource;
	public float respawnX;
	public float respawnY;
	// Use this for initialization
	void Awake () {
		audioSource = GetComponent<AudioSource> ();
		audioSource.pitch = audioSource.pitch + 2f;
	}
	
	// Update is called once per frame
	void Update () {
		timePause ();
	}



	void timePause() {
		if (Input.GetKeyDown ("e")) {
			if (!Paused) {
				Time.timeScale = 0;
				audioSource.clip = slow;
				audioSource.Play ();
			} else {
				Time.timeScale = 1.0f;
				audioSource.clip = normal;
				audioSource.Play ();
			}
			Paused = !Paused;
		}
	}
}
