using UnityEngine;
using System.Collections;

namespace MG
{
    public class NpcSimpleAi : MonoBehaviour
    {
        private Npc NpcObject;
        private Vector2 NpcC2DSize;
        private Vector2 NpcC2DOffset;
        public bool FailDown = true;

        void Start()
        {
            NpcObject = gameObject.GetComponent<Npc>();

            var c2d = NpcObject.GetComponent<BoxCollider2D>();
            NpcC2DSize = c2d.bounds.size;
            NpcC2DOffset = c2d.offset;
        }

        void Update()
        {
            NpcObject.Activate(Time.deltaTime);

            if (NpcObject.CurrentStateType != NpcStateType.Walk)
                NpcObject.Walk();

            if (CheckWall())
            {
                NpcObject.TurnRound();
                return;
            }


            if (CheckFallDown())
            {
                NpcObject.TurnRound();
                return;
            }
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
                var myHeight = NpcObject.Position.y - NpcC2DSize.y / 2 + GameDefine.StairsSlopeHeight;
                var targetHeight = obj.transform.position.y + c2d.bounds.size.y / 2;
                float myBut = NpcObject.Position.y - NpcC2DSize.y / 2;
                if (myBut < targetHeight && myHeight > targetHeight)
                {
                    var h = targetHeight - myBut + 0.2f;
                    NpcObject.transform.position = new Vector3(NpcObject.Position.x, NpcObject.Position.y + h, NpcObject.Position.z);
                }
            }
        }

        bool CheckWall()
        {
            var pos = new Vector3(NpcObject.Position.x, NpcObject.Position.y - NpcC2DSize.y / 2, NpcObject.Position.z);
            float rayLen = NpcC2DSize.x / 2 + 0.01f;
            RaycastHit2D hit;
            if (NpcObject.CurrentDir == Dir.Left)
            {
                hit = Physics2D.Raycast(pos, Vector2.left, rayLen, 1 << LayerMask.NameToLayer("Wall"));
                Debug.DrawRay(pos, Vector3.left, Color.blue);
            }
            else
            {
                hit = Physics2D.Raycast(pos, Vector2.right, rayLen, 1 << LayerMask.NameToLayer("Wall"));
                Debug.DrawRay(pos, Vector3.right, Color.blue);
            }

            if (hit.collider != null && hit.transform.gameObject.layer == LayerMask.NameToLayer("Wall"))
                return true;

            return false;
        }

        bool CheckFallDown()
        {
            return false;
        }
    }
}