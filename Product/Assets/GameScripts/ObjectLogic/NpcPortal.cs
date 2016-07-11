using UnityEngine;
using System.Collections;

namespace MG
{
    public class NpcPortal : MonoBehaviour
    {
        public int ReviveCount;             // 复活数量
//        public GameObject NpcObject;        // NPC的预设对象
//        public GameObject SwitchTigger;     // 对应的开关对象
        public float ReviveTime;            // 复活时间间隔
        public Dir NpcDir;
        public bool FallDown;

        private float NextReviveTime = 0;

        void Start()
        {
            NextReviveTime = ReviveTime;
        }

        void Update()
        {
            float deltaTime = Time.deltaTime;
            NextReviveTime -= deltaTime;

            if (NextReviveTime > 0)
                return;

            NextReviveTime = ReviveTime;
            ReviveNpc();
        }

        void ReviveNpc()
        {
            var o = Resources.Load("prefabs/Enemy") as GameObject;
            var enemy = Instantiate(o);
            enemy.transform.position = gameObject.transform.position;
            var npc = enemy.gameObject.GetComponent<Npc>();
            npc.CurrentDir = NpcDir;

            var ai = enemy.gameObject.GetComponent<NpcSimpleAi>();
            ai.FailDown = FallDown;
        }
    }
}


