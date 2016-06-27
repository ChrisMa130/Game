using UnityEngine;
using System.Collections;

namespace MG
{
    public class NpcSimpleAi : MonoBehaviour
    {
        private Npc NpcObject;
        private float CurrentNpcClimbHeight;
        private Vector2 NpcC2DSize;

        void Start()
        {
            NpcObject = gameObject.GetComponent<Npc>();

            var c2d = NpcObject.GetComponent<BoxCollider2D>();
            NpcC2DSize = c2d.bounds.size;
            CurrentNpcClimbHeight = c2d.size.y / 2 - GameDefine.StairsSlopeHeight;
        }

        void Update()
        {
            NpcObject.Activate(Time.deltaTime);

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
            if (obj.transform.tag.Equals("ForbiddenZone") && layer == LayerMask.NameToLayer("Ground"))
            {
                var c2d = obj.gameObject.GetComponent<BoxCollider2D>();
                var myHeight = NpcObject.Position.y - CurrentNpcClimbHeight;
                var targetHeight = obj.transform.position.y + c2d.bounds.size.y / 2;
                float myBut = NpcObject.Position.y - NpcC2DSize.y / 2;
                if (myBut < targetHeight && myHeight > targetHeight)
                {
                    var h = targetHeight - myBut + 0.2f;
                    NpcObject.transform.position = new Vector3(NpcObject.Position.x, NpcObject.Position.y + h, NpcObject.Position.z);
                }
            }

            if (layer == LayerMask.NameToLayer("Wall"))
            {
               NpcObject.TurnRound();
            }
        }
    }
}