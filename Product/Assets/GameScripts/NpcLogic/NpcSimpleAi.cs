using UnityEngine;
using System.Collections;

namespace MG
{
    public class NpcSimpleAi : MonoBehaviour
    {
        private Npc NpcObject;
        private float CurrentNpcClimbHeight;

        void Start()
        {
            NpcObject = gameObject.GetComponent<Npc>();

            var c2d = NpcObject.GetComponent<BoxCollider2D>();
            CurrentNpcClimbHeight = c2d.bounds.size.y/2 - GameDefine.StairsSlopeHeight;
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
            var myHeight = NpcObject.Position.y - CurrentNpcClimbHeight;

            var layer = obj.gameObject.layer;
            if (obj.transform.tag.Equals("ForbiddenZone") && layer == LayerMask.NameToLayer("Ground"))
            {
                var c2d = obj.gameObject.GetComponent<BoxCollider2D>();
                var targetHeight = obj.transform.position.y + c2d.bounds.size.y / 2;
                if (myHeight > targetHeight)
                {
                    NpcObject.transform.position = new Vector3(NpcObject.Position.x, NpcObject.Position.y + GameDefine.StairsSlopeHeight, NpcObject.Position.z);
                }
            }

            if (layer == LayerMask.NameToLayer("Wall"))
            {
               NpcObject.TurnRound();
            }
        }
    }
}