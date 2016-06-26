using UnityEngine;
using System.Collections;

namespace MG
{
    public class NpcSimpleAi : MonoBehaviour
    {
        private Npc NpcObject;
        private Vector3 LastMovePosition;
        private float NextCheckPosTime;

        void Start()
        {
            NpcObject = gameObject.GetComponent<Npc>();
            LastMovePosition = Vector3.zero;
            NextCheckPosTime = 0.5f;
        }

        // Update is called once per frame
        void Update()
        {
            // 控制NPC方向。
            // 判断NPC是否撞墙了
            // 判断是否在跳跃状态
            // 是否踩死了一个玩家
            NpcObject.Activate(Time.deltaTime);
            NextCheckPosTime -= Time.deltaTime;
            if (NextCheckPosTime < 0)
            {
                if (NpcObject.CurrentStateType == NpcStateType.Walk && LastMovePosition != Vector3.zero)
                {
                    if (NpcObject.Position == LastMovePosition)
                    {
                        // 需要转身了
                        NpcObject.TurnRound();
                    }
                }
                else
                {
                    NpcObject.Walk();
                }

                NextCheckPosTime = 0.5f;
            }

            LastMovePosition = NpcObject.Position;
        }
    }

}

