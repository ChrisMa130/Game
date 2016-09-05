using UnityEngine;
using System.Collections;

namespace MG
{
    public class npcswitcher : MonoBehaviour
    {
        public GameObject[] Portals;
		public GameObject HintObject;

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
			DisplayHint (obj);
			if (GameMgr.Instance.InputMgr.UpUp) {
				OnSwitch ();
				GetComponent<SpriteRenderer> ().flipX = !GetComponent<SpriteRenderer> ().flipX;
			}
        }

		void OnTriggerExit2D(Collider2D obj){
			var player = obj.gameObject.GetComponent<Player>();
			if (player == null)
				return;
			DestroyHint (obj);
		}

		void DisplayHint (Collider2D obj) {
			if (obj.transform.childCount == 1) {
				GameObject hint = Instantiate (HintObject);
				hint.transform.parent = obj.transform;
				hint.transform.localPosition = new Vector3 (0, 4.8f, 0);
				if (obj.name.Equals("BlueHat"))
					hint.transform.localPosition = new Vector3 (0, 1.25f, 0);
			}
		}


		void DestroyHint(Collider2D obj) {
			Destroy (obj.transform.GetChild(1).gameObject);
		}
    }
}

