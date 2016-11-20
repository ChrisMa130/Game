using UnityEngine;
using System.Collections;
using MG;

public class Crystal : TimeUnit {

    private bool inside;
	// Use this for initialization
	void Start () {
	
	}

    void Update()
    {
        if(inside)
        {
            if (TimeController.Instance.CurrentState == TimeControllState.Rewinding || TimeController.Instance.CurrentState == TimeControllState.Forward || TimeController.Instance.CurrentState == TimeControllState.Freeze)
                return;
            var player = GameObject.Find("Faylisa").GetComponent<Player>();
            if (player != null)
            {
                player.PickUpCrystal();
                DestoryMe();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            inside = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            inside = false;
        }
    }
}
