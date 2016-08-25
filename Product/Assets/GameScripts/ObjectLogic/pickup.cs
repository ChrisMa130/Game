using UnityEngine;

namespace MG
{
    public class pickup : TimeUnit
    {
        public int Key      = 0;
        public int Value    = 0;

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                var player = other.gameObject.GetComponent<Player>();
                if (player != null)
                {
                    player.AddCollectItem(Key, Value);
                    DestoryMe();
                    // GameObject.DestroyObject(gameObject);
                }
            }
        }
    }
}

