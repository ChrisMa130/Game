using UnityEngine;
using System.Collections;

// 角色逻辑
namespace MG
{
    public class Player : MonoBehaviour
    {
        private GameInput InputMgr;
        private Collect CollectItem;
        private Represent MyRepresent;
        public PlayerState MyState;
        private Transform GroundCheck;
        public bool Grounded { get; private set; }
        public bool OnTheLine { get; set; }
        public bool OnTheClimbAera { get; set; }
        public GameObject LadderObj { get; set; }

        public Vector3 Position
        {
            get { return transform.position; }
            set { transform.position = value; }
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
        }

        void Update()
        {
        }

        public void Activate(float deltaTime)
        {
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
            MyRepresent.TurnRound(dir);
        }

        public int GetCollectCount(int key)
        {
            return CollectItem.GetCount(key);
        }
    }
}

