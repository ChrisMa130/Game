using UnityEngine;
using System.Collections;

namespace MG
{
    public class ladder : MonoBehaviour
    {
        void Start()
        {
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                var player = other.gameObject.GetComponent<Player>();
                if (player != null)
                {
                    player.OnTheClimbAera = true;
                    player.LadderObj = gameObject;
                }
                    
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                var player = other.gameObject.GetComponent<Player>();
                if (player != null)
                {
                    player.OnTheClimbAera = false;
                    player.LadderObj = null;
                }
                    
            }
        }
    }
}

