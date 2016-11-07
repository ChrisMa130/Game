using System;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;

// 角色逻辑
namespace MG
{
    public class Player : TimeUnit
    {
        private GameInput InputMgr;
        private Represent MyRepresent;
        private Vector3 RevivePoint;

        public PlayerState MyState;
        private Transform GroundCheck;
        private Transform HandObject;

        public bool Grounded { get; private set; }
        public bool OnTheLine { get; set; }
        public bool OnTheClimbAera { get; set; }
        public GameObject LadderObj { get; set; }

        private Dir CurrentDir;
		private GameObject climb;

        public Vector3 Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        class UserData : TimeUnitUserData
        {
            // 基础信息
            public bool Grounded;
            public bool OnTheLine;
            public bool OnTheClimbAera;
            public bool IsDead;
            public int DiariesCount;
			public float time;
            public Dir dir;

            // 角色状态
            public PlayerStateUserData StateUD;
        }

        // 猪脚一些属性
        public bool IsDead { get; private set; }
        public int DiariesCount { get; private set; }

        // 一些回调
        public Action<int> OnPickUpAction;

        void Awake()
        {
            // 出生后一些创建代码
            MyRepresent = gameObject.AddComponent<Represent>();
            MyState     = gameObject.AddMissingComponent<PlayerState>();

            DiariesCount = 0;

            MyState.Init(this);
            IsDead = false;
            OnTheClimbAera = false;
        }

        void Start()
        {
            Stand();
            GroundCheck = transform.Find("GroundCheck");
            HandObject = transform.Find("FrontHand");
			climb = transform.FindChild ("Climb").gameObject;

            SetRevivePoint(transform.position);

            Init(false);
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

        public void PickupDiarie(int id)
        {
            DiariesCount++;
            if (OnPickUpAction != null)
                OnPickUpAction(id);
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
            MyState.Dead();
            IsDead = true;
			gameObject.layer = 19;
            //EnableCollider(false);
            //Rigid.isKinematic = true;
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

        protected override TimeUnitUserData GetUserData()
        {
            UserData data = new UserData();

            data.Grounded = Grounded;
            data.IsDead = IsDead;
            data.OnTheLine = OnTheLine;
            data.OnTheClimbAera = OnTheClimbAera;
            data.dir = CurrentDir;
            data.DiariesCount = DiariesCount;

			if (climb.activeSelf)
				data.time = climb.GetComponent<Animator> ().GetFloat ("Time");

            data.StateUD = MyState.GetUserData();

            return data;
        }

        protected override void SetUserData(TimeUnitUserData data)
        {
            UserData d = data as UserData;
            if (d == null)
                return;

            if (!d.IsDead && IsDead)
            {
                //Rigid.isKinematic = false;
                //EnableCollider(true);
				gameObject.layer = 8;
                MyRepresent.Revive();
            }

            Grounded = d.Grounded;
            IsDead = d.IsDead;
            OnTheLine = d.OnTheLine;
            OnTheClimbAera = d.OnTheClimbAera;
            DiariesCount = d.DiariesCount;

			if (climb.activeSelf)
				climb.GetComponent<Animator> ().SetFloat ("Time", d.time);
			if (OnTheClimbAera == false) {
				transform.FindChild ("Climb").gameObject.SetActive (false);
				GetComponent<MeshRenderer> ().enabled = true;
			} else if (OnTheClimbAera == true && MyState.CurrentPlayerState == PlayerStateType.Climb) {
				transform.FindChild ("Climb").gameObject.SetActive (true);
				GetComponent<MeshRenderer> ().enabled = false;

			}
            TurnRound(d.dir);

            if (d.StateUD != null)
                MyState.SetUserData(d.StateUD);

            MyRepresent.Reset();
        }

        public void SetRevivePoint(Vector3 pos)
        {
            RevivePoint = pos;
			if (GameData.Instance != null)
            	GameData.Instance.SavePos(pos);
        }

        public void Revive()
        {
            GameMgr.Instance.LineMgr.CanDraw = true;
            IsDead = false;
            transform.position = RevivePoint;
			gameObject.layer = 8;
            Stand();
			var transitionFX = Camera.main.GetComponent<ProCamera2DTransitionsFX>();
			GameMgr.Instance.GetComponent<UIManager>().OpenUI(0);
			transitionFX.TransitionEnter();
        }

        public Transform GetHandObject()
        {
            return HandObject;
        }

		public Dir GetDir() {
			return CurrentDir;
		}

        public void EnableCollider(bool enable)
        {
            var c2d = gameObject.GetComponent<BoxCollider2D>();
            var c2c = gameObject.GetComponent<CircleCollider2D>();

            if (c2d != null)
                c2d.enabled = enable;

            if (c2c != null)
                c2c.enabled = enable;
        }
    }
}

