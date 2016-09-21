using UnityEngine;

// 角色逻辑
namespace MG
{
    public class Player : TimeUnit
    {
        private GameInput InputMgr;
        private Collect CollectItem;
        private Represent MyRepresent;
        private Vector3 RevivePoint;

        public PlayerState MyState;
        private Transform GroundCheck;
        public bool Grounded { get; private set; }
        public bool OnTheLine { get; set; }
        public bool OnTheClimbAera { get; set; }
        public GameObject LadderObj { get; set; }

        private Dir CurrentDir;

        public Vector3 Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        class UserData : TimeUnitUserData
        {
            // 物品收集的信息
            public int[] Items = new int[GameDefine.CollectCount];

            // 基础信息
            public bool Grounded;
            public bool OnTheLine;
            public bool OnTheClimbAera;
            public bool IsDead;

            public Dir dir;

            // 角色状态
            public PlayerStateUserData StateUD;
        }

        // 猪脚一些属性
        public bool IsDead { get; private set; }

        void Awake()
        {
            // 出生后一些创建代码
            MyRepresent = gameObject.AddComponent<Represent>();
            MyState     = gameObject.AddMissingComponent<PlayerState>();

            CollectItem = new Collect();

            MyState.Init(this);
            IsDead = false;
            OnTheClimbAera = false;
        }

        void Start()
        {
            Stand();
            GroundCheck = transform.Find("GroundCheck");

            SetRevivePoint(transform.position);

            Init(false);
        }

        void Update()
        {
        }

        public void Activate(float deltaTime)
        {
            if (TimeController.Instance.IsOpTime())
                return;

            Grounded = Physics2D.Linecast(transform.position, GroundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
            if (!Grounded)
                Grounded = Physics2D.Linecast(transform.position, GroundCheck.position, 1 << LayerMask.NameToLayer("Wall"));

            if (MyState != null)
                MyState.Activate(deltaTime);
        }

        public void ApplyInput(GameInput input)
        {
            MyState.ApplyInput(input);
        }

        public void AddCollectItem(int key, int value)
        {
            CollectItem.AddCollectItem(key, value);
        }

        public void Run()
        {
            MyState.Run();
        }

        public void Stand()
        {
            MyState.Stand();
        }

        public void Climb()
        {
            MyState.Climb();
        }

        public void Dead()
        {
            IsDead = true;
            MyState.Dead();
        }

        public void Jump()
        {
            MyState.Jump();
        }

        public void TurnRound(Dir dir)
        {
            CurrentDir = dir;
            MyRepresent.TurnRound(dir);
        }

        public int GetCollectCount(int key)
        {
            return CollectItem.GetCount(key);
        }

        protected override TimeUnitUserData GetUserData()
        {
            UserData data = new UserData();

            data.Items = new int[GameDefine.CollectCount];
            for (int i = 0; i < GameDefine.CollectCount; i++)
            {
                data.Items[i] = CollectItem.GetCount(i);
            }

            data.Grounded = Grounded;
            data.IsDead = IsDead;
            data.OnTheLine = OnTheLine;
            data.OnTheClimbAera = OnTheClimbAera;
            data.dir = CurrentDir;

            data.StateUD = MyState.GetUserData();

            return data;
        }

        protected override void SetUserData(TimeUnitUserData data)
        {
            UserData d = data as UserData;
            if (d == null)
                return;

            for (int i = 0; i < GameDefine.CollectCount; i++)
            {
                CollectItem.AddCollectItem(i, d.Items[i]);
            }

            Grounded = d.Grounded;
            IsDead = d.IsDead;
            OnTheLine = d.OnTheLine;
            OnTheClimbAera = d.OnTheClimbAera;
            TurnRound(d.dir);

            if (d.StateUD != null)
                MyState.SetUserData(d.StateUD);

            MyRepresent.Reset();
        }

        public void SetRevivePoint(Vector3 pos)
        {
            RevivePoint = pos;
        }

        public void Revive()
        {
            GameMgr.Instance.LineMgr.CanDraw = true;
            IsDead = false;
            transform.position = RevivePoint;
            Stand();
        }
    }
}

