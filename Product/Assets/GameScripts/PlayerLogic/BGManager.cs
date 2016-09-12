using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class BGManager : MonoBehaviour {
	private AudioSource currentMusic;
	private AudioSource lastMusic;
	private AudioSource[] sources;

	public float FadeTime;
	public float MaxVolume = 1.0f;
	private float fadeSpeed;
	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad(this.gameObject);
		sources = GetComponents<AudioSource> ();
		fadeSpeed = 1.0f / FadeTime;
	}

	// Update is called once per frame
	void Update () {
		if (currentMusic != null) {
			if (currentMusic.volume < MaxVolume)
				fadeSequence (currentMusic, fadeSpeed);
		}
		if (currentMusic != sources [GetLevel ()]) {
			lastMusic = currentMusic;
			currentMusic = sources [GetLevel ()];
			currentMusic.volume = 0f;
			currentMusic.Play ();
		}
		if (lastMusic != null) {
			if (lastMusic.volume > 0f)
				fadeSequence (lastMusic, -fadeSpeed);
			else
				lastMusic.Stop ();
		}
	}

	private void fadeSequence(AudioSource source,float fadeSpeed) {
		source.volume += fadeSpeed * Time.deltaTime;
	}

	private int GetLevel() {
		string sceneName = SceneManager.GetActiveScene ().name;
		return int.Parse(sceneName.Split ("-"[0])[0])-1;
	}
}
