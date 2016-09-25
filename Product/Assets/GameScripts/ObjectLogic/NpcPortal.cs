using UnityEngine;

namespace MG
{
    public class NpcPortal : TimeUnit
    {
        public int ReviveCount;             // 复活数量
        public GameObject[] NpcObjects;        // NPC的预设对象
        public float ReviveTime;            // 复活时间间隔
        public Dir NpcDir;
        public bool FallDown;

        private float NextReviveTime = 0;
        public bool BeginRevive;

        class UserData : TimeUnitUserData
        {
            public float NextReviveTime;
            public GameObject[] NpcObjects;
        }

        void Start()
        {
            NpcObjects = new GameObject[ReviveCount];
            NextReviveTime = ReviveTime;
            BeginRevive = true;

            Init(false);
        }

        void Update()
        {
            if (TimeController.Instance.IsOpTime())
                return;

            if (!BeginRevive)
            {
                if (!CheckRevive())
                    return;

                BeginRevive = true;
                NextReviveTime = ReviveTime;
            }

            float deltaTime = Time.deltaTime;
            NextReviveTime -= deltaTime;

            if (NextReviveTime > 0)
                return;

            for (int i = 0; i < ReviveCount; i++)
            {
                if (NpcObjects[i] == null)
                {
                    NpcObjects[i] = ReviveNpc();
                    break;
                }
            }

            BeginRevive = false;
        }

        bool CheckRevive()
        {
            for (int i = 0; i < ReviveCount; i++)
            {
                var npc = NpcObjects[i].GetComponent<Npc>();
                if (npc != null && npc.IsDead)
                {
                    NpcObjects[i] = null;
                    return true;
                }
            }

            return false;
        }

        GameObject ReviveNpc()
        {
            var o = Resources.Load("prefabs/MovingObjects/Enemy") as GameObject;
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

        protected override TimeUnitUserData GetUserData()
        {
            UserData data = new UserData();

            data.NextReviveTime = NextReviveTime;
            data.NpcObjects = new GameObject[ReviveCount];
            NpcObjects.CopyTo(data.NpcObjects, 0);

            return data;
        }

        protected override void SetUserData(TimeUnitUserData data)
        {
            UserData d = data as UserData;
            if (d == null)
                return;

            NextReviveTime = d.NextReviveTime;
            d.NpcObjects.CopyTo(NpcObjects, 0);
        }
    }
}


