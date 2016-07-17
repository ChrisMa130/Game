using UnityEngine;
using System.Collections;

namespace MG
{
    public class NpcPortal : MonoBehaviour
    {
        public int ReviveCount;             // 复活数量
        public GameObject[] NpcObjects;        // NPC的预设对象
        public float ReviveTime;            // 复活时间间隔
        public Dir NpcDir;
        public bool FallDown;

        private float NextReviveTime = 0;

        void Start()
        {
            NpcObjects = new GameObject[ReviveCount];
            NextReviveTime = ReviveTime;
        }

        void Update()
        {
            float deltaTime = Time.deltaTime;
            NextReviveTime -= deltaTime;

            if (NextReviveTime > 0)
                return;

            NextReviveTime = ReviveTime;

            for (int i = 0; i < ReviveCount; i++)
            {
                if (NpcObjects[i] == null)
                {
                    NpcObjects[i] = ReviveNpc();
                    break;
                }
            }
        }

        GameObject ReviveNpc()
        {
            var o = Resources.Load("prefabs/Enemy") as GameObject;
            var enemy = Instantiate(o);
            enemy.transform.position = gameObject.transform.position;
            var npc = enemy.gameObject.GetComponent<Npc>();
            npc.CurrentDir = NpcDir;

            var ai = enemy.gameObject.GetComponent<NpcSimpleAi>();
            ai.FailDown = FallDown;
            return enemy;
        }

        public void DestoryAllNpc()
        {
            for (int i = 0; i < ReviveCount; i++)
            {
                if (NpcObjects[i] != null)
                {
                    Destroy(NpcObjects[i]);
                    NpcObjects[i] = null;
                }
            }
        }
    }
}


