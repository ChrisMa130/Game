using UnityEngine;
using System.Collections;
using System.Linq;
namespace MG {
	public class Lever : TimeUnit {
		public PlatformBase[] Platforms;
		private Animator anim;
		private bool hasPlayer;
		private Collider2D obj;

		class UserData : TimeUnitUserData
		{
			public bool Left;
		}

		void Start () {
			anim = transform.FindChild("lever").GetComponent<Animator> ();
			hasPlayer = false;
			Init(false);
		}

		void Update() {
			if (Input.GetKeyUp ("w") && !anim.IsInTransition(0) && hasPlayer) {
				anim.SetBool ("Left", !anim.GetBool ("Left"));
				foreach (var p in Platforms.Where(p => p != null))
				{
					p.TurnOn(this.obj.gameObject);
				}
			}
		}

		void OnTriggerEnter2D (Collider2D obj) {
			var npc = obj.GetComponent<Npc> ();
			if (npc != null) {
				if (!anim.IsInTransition(0)) {
					anim.SetBool ("Left", !anim.GetBool ("Left"));
					foreach (var p in Platforms.Where(p => p != null))
					{
						p.TurnOn(obj.gameObject);
					}
				}
			}
			var player = obj.GetComponent<Player>();
			if (player != null) {
				this.obj = obj;
				hasPlayer = true;
			}
		}

		void OnTriggerExit2D (Collider2D obj) {
			var player = obj.GetComponent<Player>();
			if (player == null) {
				return;
			}
			this.obj = null;
			hasPlayer = false;
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
