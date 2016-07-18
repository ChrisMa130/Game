using UnityEngine;
using System.Collections;
using System.Linq;

namespace MG
{
    public class TrapPlatform : MonoBehaviour
    {
        public PlatformBase[] Platforms;

        bool CheckObj(GameObject obj)
        {
            if (Platforms == null || Platforms.Length == 0)
                return false;

            var player = obj.GetComponent<Player>();
            var npc = obj.GetComponent<Npc>();

            return npc != null || player != null;
        }

        void OnTriggerEnter2D(Collider2D obj)
        {
            if (!CheckObj(obj.gameObject))
                return;

            Flip();
            foreach (var p in Platforms.Where(p => p != null))
            {
                p.TurnOn(obj.gameObject);
            }
        }

        private void Flip()
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

}


