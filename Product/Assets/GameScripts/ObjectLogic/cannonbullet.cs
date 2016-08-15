using UnityEngine;
using System.Collections;

namespace MG
{
    public class cannonbullet : TimeUnit
    {
        private int DestoryCount;

        void Start()
        {
            Init();
            DestoryCount = GameDefine.CannonBulletDestoryCount;
        }

        void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.tag == "Building")
            {
                // 扣除一次  
                DestoryCount--;
            }
            else if (col.gameObject.tag == "ForbiddenZone" && col.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                DestoryCount--;
            }

            var player = col.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.Dead();
                DestoryCount = 0;
            }

            var npc = col.gameObject.GetComponent<Npc>();
            if (npc != null)
            {
                npc.Dead();
                DestoryCount = 0;
            }

            if (col.gameObject.tag == "CannonBullet")
                DestoryCount = 0;

            if (DestoryCount <= 0)
                GameObject.DestroyObject(gameObject);
        }
    }
}


