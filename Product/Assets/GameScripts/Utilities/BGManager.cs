using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using MG;

public class BGManager : MonoBehaviour {
	private AudioSource currentMusic;
	private AudioSource lastMusic;
	private AudioSource[] sources;

	public float FadeTime;
	public float MaxVolume = 1.0f;
	private float fadeSpeed;

    private float playTime = 0f;
    private bool firstTime = true;
	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad(this.gameObject);
		sources = GetComponents<AudioSource> ();
		fadeSpeed = 1.0f / FadeTime;
	}

    // Update is called once per frame
    void Update() {
        ProcessMusicTime();

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

    private void ProcessMusicTime()
    {
        if (currentMusic != null)
        {
            if (TimeController.Instance.CurrentState == TimeControllState.Rewinding)
            {
                //currentMusic.timeSamples = currentMusic.clip.samples - 1;
                playerMusic();
                currentMusic.pitch = -1;
            }
            else if (TimeController.Instance.CurrentState == TimeControllState.Freeze && firstTime)
            {
                playTime = currentMusic.time;
                firstTime = false;
                currentMusic.Stop();
            }
            else if (TimeController.Instance.CurrentState == TimeControllState.Forward)
            {
                playerMusic();
                currentMusic.pitch = 2;
            }
            else
            {
                if (TimeController.Instance.CurrentState != TimeControllState.Freeze)
                {
                    playerMusic();
                    currentMusic.pitch = 1;
                }
            }
        }
    }

    private void playerMusic()
    {
        if (!currentMusic.isPlaying) {
            currentMusic.Play();
            currentMusic.time = playTime;
            firstTime = true;
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
