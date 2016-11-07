using UnityEngine;
using System.Collections;

namespace MG
{
    public class Diaries : TimeUnit
    {
        public int Id; // 日志ID

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                var player = other.gameObject.GetComponent<Player>();
                if (player != null)
                {
                    player.PickupDiarie(Id);
                    DestoryMe();
                }
            }
        }
    }

}
