using UnityEngine;
using System.Collections;
using MG;

public class Crystal : TimeUnit {

	// Use this for initialization
	void Start () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            var player = other.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.PickUpCrystal();
                DestoryMe();
            }
        }
    }
}
