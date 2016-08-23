using UnityEngine;
using System.Collections;
using System.Linq;

namespace MG
{
	public class PlatformSwitch_Stay : MonoBehaviour
	{
		public PlatformBase[] Platforms;
		private bool HasPlayer;

		void Start()
		{
			HasPlayer = false;
		}

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

			var player = obj.GetComponent<Player>();
			if (player != null)
			{
				if (HasPlayer)
					return;
				else
					HasPlayer = true;
			}

			foreach (var p in Platforms.Where(p => p != null))
			{
				p.TurnOn(obj.gameObject);
			}
		}

		void OnTriggerExit2D(Collider2D obj)
		{
			var player = obj.GetComponent<Player>();
			if (player != null)
				HasPlayer = false;
			
			foreach (var p in Platforms.Where(p => p != null))
			{
				p.TurnOn(obj.gameObject);
			}
		}
	}
}