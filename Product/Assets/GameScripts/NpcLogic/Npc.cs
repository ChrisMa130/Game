using UnityEngine;
using System.Collections;

namespace MG
{
    public class Npc : MonoBehaviour
    {
        public NpcState State { get; private set; }

        public Vector3 Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        public bool IsDead { get; private set; }

        void Awake()
        {
            State = gameObject.AddMissingComponent<NpcState>();
            State.Init(this);

            IsDead = false;
        }

        public void Activate(float deltaTime)
        {
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
    }
}



