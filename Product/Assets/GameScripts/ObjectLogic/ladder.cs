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
                var playerMove = other.gameObject.GetComponent<ObjectMove>();
                if (playerMove != null)
                    playerMove.CanClimb = true;
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                var playerMove = other.gameObject.GetComponent<ObjectMove>();
                if (playerMove != null)
                    playerMove.CanClimb = false;
            }
        }
    }
}

