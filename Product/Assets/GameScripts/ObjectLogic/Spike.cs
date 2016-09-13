using UnityEngine;

namespace MG
{
    public class Spike : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D other)
        {
            if (TimeController.Instance.IsOpTime())
                return;

            var player = other.GetComponent<Player>();
            if (player != null)
                player.Dead();

			//temp
			var npc = other.GetComponent<Npc>();
			if (npc != null)
				npc.Dead();

            var key = other.GetComponent<OpenKey>();
            if (key != null)
                key.Dead();
        }
    }
}

