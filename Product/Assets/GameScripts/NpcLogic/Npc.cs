﻿using UnityEngine;
using System.Collections;

namespace MG
{
    public class Npc : TimeUnit
    {
        public NpcState State { get; private set; }
        public NpcStateType CurrentStateType
        {
            get { return State.CurrentState; }
        }
        private NpcRepresent Represent;
        private Transform GroundCheck;
        public bool Grounded { get; private set; }

        public Dir CurrentDir;

        public Vector3 Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        public Vector3 GroundCheckPosition { get { return GroundCheck.position; } }

        public bool IsDead { get; private set; }

        class UserData : TimeUnitUserData
        {
            // 基本数据
            public Dir dir;
            public bool IsDead;

            // 状态数据
            public NpcStateUserData StateData;
        }

        protected override TimeUnitUserData GetUserData()
        {
            UserData data = new UserData();
            data.dir = CurrentDir;
            data.IsDead = IsDead;
            data.StateData = State.GetUserData();

            return data;
        }

        protected override void SetUserData(TimeUnitUserData data)
        {
            UserData d = data as UserData;
            if (d == null)
                return;

            TurnRound(d.dir);
            IsDead = d.IsDead;

            State.SetUserData(d.StateData);
        }

        void Awake()
        {
            Represent = gameObject.AddComponent<NpcRepresent>();
            State = gameObject.AddMissingComponent<NpcState>();
            State.Init(this);

            IsDead = false;
        }

        void Start()
        {
            Init(true);

            Stand();
            GroundCheck = transform.Find("GroundCheck");
            // CurrentDir = Dir.Right;
            TurnRound(CurrentDir);
        }

        public void Activate(float deltaTime)
        {
            if (TimeController.Instance.IsOpTime())
                return;

            Grounded = Physics2D.Linecast(transform.position, GroundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
            if (!Grounded)
                Grounded = Physics2D.Linecast(transform.position, GroundCheck.position, 1 << LayerMask.NameToLayer("Wall"));

            if (State != null)
                State.Activate(deltaTime);
        }

        public void Walk()
        {
            State.Walk();
        }

        public void Stand()
        {
            State.Stand();
        }

        public void Dead()
        {
            IsDead = true;
            State.Dead();
        }

        public void Jump()
        {
            State.Jump();
        }

        public void TurnRound()
        {
            if (CurrentDir == Dir.Left)
                TurnRound(Dir.Right);
            else
                TurnRound(Dir.Left);
        }

        public void TurnRound(Dir dir)
        {
            Represent.TurnRound(dir);
            State.ChangeDir(dir);
            CurrentDir = dir;
        }

        public void IsKinematic(bool isKinematic)
        {
            Represent.IsKinematic(isKinematic);
        }
    }
}



