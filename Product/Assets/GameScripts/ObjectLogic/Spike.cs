using UnityEngine;

namespace MG
{
    public class Spike : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D other)
        {
            var player = other.GetComponent<Player>();
            if (player)
            {
                player.Dead();
            }
        }
    }
}

