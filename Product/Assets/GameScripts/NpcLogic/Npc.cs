using UnityEngine;
using System.Collections;

namespace MG
{
    public class Npc : MonoBehaviour
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

        void Awake()
        {
            Represent = gameObject.AddComponent<NpcRepresent>();
            State = gameObject.AddMissingComponent<NpcState>();
            State.Init(this);

            IsDead = false;
        }

        void Start()
        {
            Stand();
            GroundCheck = transform.Find("GroundCheck");
            // CurrentDir = Dir.Right;
            TurnRound(CurrentDir);
        }

        public void Activate(float deltaTime)
        {
            Grounded = Physics2D.Linecast(transform.position, GroundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

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
    }
}



