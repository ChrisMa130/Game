using UnityEngine;
using System.Collections;

namespace MG
{
    public class NpcSimpleAi : MonoBehaviour
    {
        private Npc NpcObject;
        private Vector2 NpcC2DSize;
        public bool FailDown = true;
        private GameObject LastCollObj;

        void Start()
        {
            NpcObject = gameObject.GetComponent<Npc>();

            var c2d = NpcObject.GetComponent<BoxCollider2D>();
            NpcC2DSize = c2d.bounds.size;
        }

        void Update()
        {
            NpcObject.Activate(Time.deltaTime);

            if (NpcObject.CurrentStateType != NpcStateType.Walk)
                NpcObject.Walk();

            if (CheckWall())
            {
                Debug.Log("没找到墙");
                NpcObject.TurnRound();
                return;
            }


            if (!FailDown && NpcObject.Grounded && CheckFallDown())
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
                if (!NpcObject.Grounded)
                    NpcObject.Jump();
                return;
            }

            // 测试逻辑，还是先污染，在治理。
            bool samePlane = LastCollObj != null && LastCollObj.GetInstanceID() == obj.gameObject.GetInstanceID();

            LastCollObj = obj.gameObject;

            var layer = obj.gameObject.layer;
            // 以下这个判断需要改变实现方法。太烂了。~~~
            if (!samePlane && obj.transform.tag.Equals("ForbiddenZone") && layer == LayerMask.NameToLayer("Ground"))
            {
                var c2d = obj.gameObject.GetComponent<BoxCollider2D>();
                var myHeight = NpcObject.GroundCheckPosition.y + GameDefine.StairsSlopeHeight;
                var targetHeight = obj.transform.position.y + 0.1f;
                float myBut = NpcObject.GroundCheckPosition.y;
                if (myBut < targetHeight && myHeight > targetHeight)
                {
                    var h = targetHeight + (NpcObject.Position.y - NpcObject.GroundCheckPosition.y) + 0.1f;
                    NpcObject.transform.position = new Vector3(NpcObject.Position.x, h, NpcObject.Position.z);
                }
                else if (GameMgr.Instance.LineMgr.LineAngle != 45 && myBut < targetHeight)
                {
                    NpcObject.TurnRound();
                }
                else if (NpcObject.GroundCheckPosition.y < obj.contacts[0].point.y)
                { 
                    NpcObject.TurnRound();
                }
            }
        }

        bool CheckWall()
        {
            var pos = new Vector3(NpcObject.GroundCheckPosition.x, NpcObject.GroundCheckPosition.y, NpcObject.Position.z);
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
            float xlen = NpcC2DSize.x/2;
            if (NpcObject.CurrentDir == Dir.Left)
                xlen = -Mathf.Abs(xlen);
            else
                xlen = Mathf.Abs(xlen);

            Vector3 startPos = new Vector3(NpcObject.GroundCheckPosition.x + xlen, NpcObject.GroundCheckPosition.y, NpcObject.Position.z);
            RaycastHit2D hit = Physics2D.Raycast(startPos, Vector2.down, 0.1f);
            if (hit.collider == null)
                return true;

            return false;
        }
    }
}