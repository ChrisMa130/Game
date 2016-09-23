using UnityEngine;
using System.Collections;
using System.Linq;
namespace MG {
	public class Trolley : TimeUnit {
		public bool Left = true;
		private Animation anim;

		class UserData : TimeUnitUserData
		{
			public bool Left;
		}

		void Start () {
			anim = GetComponent<Animation> ();

			Init(false);
		}

		void OnTriggerStay2D (Collider2D obj) {
			var player = obj.GetComponent<Player>();
			if (player = null) {
				return;
			}
			if (Input.GetKeyUp ("w") && !anim.isPlaying) {
				PlayAnimation ();
			}
		}

		void OnTriggerEnter2D (Collider2D obj) {
			var npc = obj.GetComponent<Npc> ();
			if (npc = null) {
				return;
			}

			if (!anim.isPlaying)
				PlayAnimation ();
		}

		private void PlayAnimation() {
			if (Left) {
				anim ["TurnLeft"].speed = 0.7f;
				anim.Play ();
				Left = false;
			} else {
				anim ["TurnLeft"].speed = -0.7f;
				anim ["TurnLeft"].time = anim ["TurnLeft"].length;
				anim.Play ();
				Left = true;
			}
		}

		protected override TimeUnitUserData GetUserData()
		{
			UserData data = new UserData();

			data.Left = Left;

			return data;
		}

		protected override void SetUserData(TimeUnitUserData data)
		{
			UserData d = data as UserData;
			if (d == null)
				return;

			Left = d.Left;
		}
	}
}
