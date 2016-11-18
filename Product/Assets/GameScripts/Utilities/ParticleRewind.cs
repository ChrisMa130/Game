using UnityEngine;
using System.Collections;
using MG;
public class ParticleRewind : MonoBehaviour {

    private ParticleSystem particle;

	// Use this for initialization
	void Start () {
        particle = GetComponent<ParticleSystem>();
	}

    // Update is called once per frame
    void Update()
    {
        var vel = particle.velocityOverLifetime;
        var force = particle.forceOverLifetime;
        if (TimeController.Instance.CurrentState == TimeControllState.Rewinding)
        {
            float playbackTime = particle.time;
            particle.Clear();
            particle.Simulate(playbackTime - Time.deltaTime);
            particle.Play();
        }
        else
        {
            vel.x = new ParticleSystem.MinMaxCurve(-0.6f, -1f);
            force.y = new ParticleSystem.MinMaxCurve(-0.1f, -0.1f);
        }
    }
}
