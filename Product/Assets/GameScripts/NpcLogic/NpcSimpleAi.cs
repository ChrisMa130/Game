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
            // Init();

            NpcObject = gameObject.GetComponent<Npc>();

            var c2d = NpcObject.GetComponent<BoxCollider2D>();
            NpcC2DSize = c2d.bounds.size;
        }

        void Update()
        {
            if (TimeController.Instance.IsOpTime())
                return;

            NpcObject.Activate(Time.deltaTime);

			if (NpcObject.CurrentStateType != NpcStateType.Dead)
                NpcObject.Walk();

            if (CheckWall())
            {
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
            if (TimeController.Instance.IsOpTime())
                return;

            var player = obj.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.Dead();
                Debug.Log("玩家狗带了");
                if (!NpcObject.Grounded)
                    NpcObject.Jump();
                return;
            }

            var npc = obj.gameObject.GetComponent<Npc>();
            if (npc != null)
            {
                if (!NpcObject.Grounded)
                    NpcObject.Jump();
                else if (NpcObject.Grounded && npc.Grounded)
                    NpcObject.TurnRound();
            }

            bool samePlane = LastCollObj != null && LastCollObj.GetInstanceID() == obj.gameObject.GetInstanceID();

            LastCollObj = obj.gameObject;

            var layer = obj.gameObject.layer;
			if (!samePlane && (obj.transform.tag.Equals("Building") || obj.transform.tag.Equals("ForbiddenZone")) && layer == LayerMask.NameToLayer("Ground"))
            {
                var myHeight = NpcObject.GroundCheckPosition.y + GameDefine.StairsSlopeHeight;
                
                float addonesHigh = 0;
                if (obj.transform.tag.Equals("Building"))
                    addonesHigh = GameDefine.StairsSlopeHeight + 0.1f;
                else
                    addonesHigh = 0.1f;

                var targetHeight = obj.transform.position.y + addonesHigh;
                float myBut = NpcObject.GroundCheckPosition.y;
                if (myBut < targetHeight && myHeight > targetHeight)
                {
                    var h = targetHeight + (NpcObject.Position.y - NpcObject.GroundCheckPosition.y) + 0.1f;
                    NpcObject.transform.position = new Vector3(NpcObject.Position.x, h, NpcObject.Position.z);
                }
                else if (GameMgr.Instance.LineMgr.LineAngle != 45 && myBut < targetHeight - addonesHigh)
                {
                    Debug.Log("直线线段上转身。");
                    NpcObject.TurnRound();
                }
                else if (GameMgr.Instance.LineMgr.LineAngle == 45 && NpcObject.GroundCheckPosition.y + 0.1f < obj.contacts[0].point.y)
                { 
                    Debug.Log("其他转身");
                    NpcObject.TurnRound();
                }
            }
        }

        bool CheckWall()
        {
            var pos = new Vector3(NpcObject.GroundCheckPosition.x, NpcObject.GroundCheckPosition.y + 0.03f, NpcObject.Position.z);
			float rayLen = NpcC2DSize.x / 2 + 0.01f;
            RaycastHit2D[] hit;
            if (NpcObject.CurrentDir == Dir.Left)
            {
                hit = Physics2D.RaycastAll(pos, Vector2.left, rayLen, 1 << LayerMask.NameToLayer("Wall"));
                Debug.DrawRay(pos, Vector3.left, Color.blue);
            }
            else
            {
                hit = Physics2D.RaycastAll(pos, Vector2.right, rayLen, 1 << LayerMask.NameToLayer("Wall"));
                Debug.DrawRay(pos, Vector3.right, Color.blue);
            }

			if (hit == null || hit.Length == 0) {
				return false;
			}

            for (int i = 0; i < hit.Length; i++)
            {
                var h = hit[i];
                if (h.collider != null && h.transform.gameObject.layer == LayerMask.NameToLayer("Wall"))
                    return true;
            }

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
            RaycastHit2D[] hits = Physics2D.RaycastAll(startPos, Vector2.down, 0.1f);
            if (hits == null || hits.Length == 0)
                return true;

            bool HasGround = false;
            for (int i = 0; i < hits.Length; i++)
            {
                var h = hits[i];
                if (h.collider == null)
                    continue;

                if (h.collider.gameObject.layer == LayerMask.NameToLayer("Ground") || h.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
                {
                    HasGround = true;
                    break;
                }
            }

            if (HasGround)
                return false;

            for (int i = 0; i < hits.Length; i++)
            {
                var h = hits[i];
                if (h.collider == null)
                    continue;

                if (h.collider.tag.Equals("ForbiddenZone"))
                {
                    startPos.x = startPos.x + GameDefine.ForbiddenLineAddWidth;
                    var downHit = Physics2D.Raycast(startPos, Vector2.down, 0.1f);
                    if (downHit.collider == null)
                        return true;

                    if (downHit.collider.tag.Equals("ForbiddenZone"))
                        return true;
                }

            }

            return false;
        }

//        void OnCollisionStay2D(Collision2D obj)
//        {
//            var npc = obj.gameObject.GetComponent<Npc>();
//            if (npc == null)
//                return;
//
//            NpcObject.TurnRound();
//        }
    }
}