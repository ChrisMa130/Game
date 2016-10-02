using UnityEngine;
using System.Collections;
using System.Linq;
namespace MG
{
    public class Lever : TimeUnit
    {
        public PlatformBase[] Platforms;
		public bool left = true;
		public GameObject HintObject;
        private Animator anim;
        private bool hasPlayer;
        private Collider2D obj;

        class UserData : TimeUnitUserData
        {
            public bool hasPlayer;
            public bool Left;
        }

        void Start()
        {
			if (HintObject == null) {
				HintObject = Resources.Load ("prefabs/Utilities/W") as GameObject;
			}
            anim = transform.FindChild("lever").GetComponent<Animator>();
			anim.SetBool ("Left", left);
            hasPlayer = false;
            Init(false);
        }

        void Update()
        {
            if (Input.GetKeyUp("w") && !anim.IsInTransition(0) && hasPlayer)
            {
                anim.SetBool("Left", !anim.GetBool("Left"));
                foreach (var p in Platforms.Where(p => p != null))
                {
                    p.TurnOn(this.obj.gameObject);
                }
            }
        }

        void OnTriggerEnter2D(Collider2D obj)
        {
            var npc = obj.GetComponent<Npc>();
            if (npc != null)
            {
                if (!anim.IsInTransition(0))
                {
                    anim.SetBool("Left", !anim.GetBool("Left"));
                    foreach (var p in Platforms.Where(p => p != null))
                    {
                        p.TurnOn(obj.gameObject);
                    }
                }
            }
            var player = obj.GetComponent<Player>();
            if (player != null)
            {
				DisplayHint (obj);
                this.obj = obj;
                hasPlayer = true;
            }
        }

        void OnTriggerExit2D(Collider2D obj)
        {
            var player = obj.GetComponent<Player>();
            if (player == null)
            {
                return;
            }
			DestroyHint (obj);
            this.obj = null;
            hasPlayer = false;
        }

        protected override TimeUnitUserData GetUserData()
        {
            var data = new UserData
            {
                hasPlayer = this.hasPlayer,
                Left = anim.GetBool("Left"),
            };

            return data;
        }

        protected override void SetUserData(TimeUnitUserData data)
        {
            UserData d = data as UserData;
            if (d == null)
                return;

            this.hasPlayer = d.hasPlayer;

            anim.SetBool("Left", d.Left);
        }

		void DisplayHint (Collider2D obj) {
			if (obj.transform.childCount == 2) {
				GameObject hint = Instantiate (HintObject);
				hint.transform.parent = obj.transform;
				hint.transform.localPosition = new Vector3 (0, 4.8f, 0);
				if (obj.name.Equals("BlueHat"))
					hint.transform.localPosition = new Vector3 (0, 1.25f, 0);
			}
		}

		void DestroyHint(Collider2D obj) {
			Destroy (obj.transform.GetChild(2).gameObject);
		}
        //		private void PlayAnimation() {
        //			anim.Play("TurnLeft", -1, 0f);
        //			if (Left) {
        //				anim ["TurnLeftLegacy"].speed = 0.7f;
        //				anim.Play ();
        //				Left = false;
        //			} else {
        //				anim ["TurnLeftLegacy"].speed = -0.7f;
        //				anim ["TurnLeftLegacy"].time = anim ["TurnLeftLegacy"].length;
        //				anim.Play ();
        //				Left = true;
        //			}
        //		}
    }
}
