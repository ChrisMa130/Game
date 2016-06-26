using UnityEngine;
using System.Collections;

namespace MG
{
    public class NpcSimpleAi : MonoBehaviour
    {
        private Npc NpcObject;

        void Start()
        {
            NpcObject = gameObject.GetComponent<Npc>();
        }

        void Update()
        {
            NpcObject.Activate(Time.deltaTime);

            var pos = NpcObject.Position;

            if (NpcObject.CurrentStateType != NpcStateType.Walk)
                NpcObject.Walk();
        }

        void OnCollisionEnter2D(Collision2D obj)
        {
            var player = obj.gameObject.GetComponent<Player>();
            if (player != null)
            {
                // player.Dead();
                Debug.Log("玩家狗带了");
                return;
            }

            var layer = obj.gameObject.layer;
            if (layer == LayerMask.NameToLayer("Wall"))
            {
                NpcObject.TurnRound();
            }
        }
    }
}