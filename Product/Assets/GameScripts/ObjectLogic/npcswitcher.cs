using UnityEngine;
using System.Collections;

namespace MG
{
    public class npcswitcher : MonoBehaviour
    {
        public GameObject[] Portals;

        public void OnSwitch()
        {
            if (Portals == null)
            {
                return;
            }

            int len = Portals.Length;
            for (int i = 0; i < len; i++)
            {
                var portal = Portals[i].GetComponent<NpcPortal>();
				if (portal != null) {
					portal.DestoryAllNpc ();
				}
            }
        }

        void OnTriggerStay2D(Collider2D obj)
        {
            var player = obj.gameObject.GetComponent<Player>();
            if (player == null)
                return;

			if (GameMgr.Instance.InputMgr.UpUp) {
				OnSwitch ();
				GetComponent<SpriteRenderer> ().flipX = !GetComponent<SpriteRenderer> ().flipX;
			}
        }
    }
}

