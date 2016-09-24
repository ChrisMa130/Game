using UnityEngine;
using System.Collections;

namespace MG
{
    public class NpcStateUserData
    {
        public NpcStateType CurrentState;
        public NpcStateType NextState;
    }

    public class NpcState : MonoBehaviour
    {
        public Npc Owner { get; private set; }
        private NpcBaseState[] States;
        public NpcStateType CurrentState { get; private set; }
        public NpcStateType NextState { get; private set; }

        public void Init(Npc npc)
        {
            Owner = npc;

            States = new NpcBaseState[]
            {
                new NpcStandState(Owner),
                new NpcWalkState(Owner), 
                new NpcJumpState(Owner), 
                new NpcDeadState(Owner) 
            };

            CurrentState = NpcStateType.Stand;
            NextState = NpcStateType.Invalid;
        }

        public void Activate(float deltaTime)
        {
            if (NextState != NpcStateType.Invalid)
            {
                ChangeState();
            }

            if (CurrentState != NpcStateType.Invalid)
            {
                States[(int)CurrentState].Activate(deltaTime);
            }
        }

        private void ChangeState()
        {
            if (Owner == null)
                return;

            if (CurrentState != NpcStateType.Invalid)
            {
                States[(int)CurrentState].Exit();
            }

            CurrentState = NextState;
            NextState = NpcStateType.Invalid;

            var state = States[(int)CurrentState];
            state.Enter();
        }

        public bool CanChangeState(NpcStateType type)
        {
            var state = States[(int)CurrentState];
            return state.CanChange(type);
        }

        public bool Stand()
        {
            if (!CanChangeState(NpcStateType.Stand))
                return false;

            NextState = NpcStateType.Stand;

            return true;
        }

        public bool Walk()
        {
            if (!CanChangeState(NpcStateType.Walk))
                return false;

            NextState = NpcStateType.Walk;

            return true;
        }

        public bool Jump()
        {
            if (!CanChangeState(NpcStateType.Jump))
                return false;

            NextState = NpcStateType.Jump;

            return true;
        }

        public bool Dead()
        {
            if (!CanChangeState(NpcStateType.Dead))
                return false;

            NextState = NpcStateType.Dead;

            return true;
        }

        public void ChangeDir(Dir dir)
        {
            States[(int)CurrentState].ChangeDir(dir);
        }

        public NpcStateUserData GetUserData()
        {
            NpcStateUserData data = new NpcStateUserData();
            data.CurrentState = CurrentState;
            data.NextState = NextState;

            return null;
        }

        public void SetUserData(NpcStateUserData data)
        {
            if (data == null)
                return;

            CurrentState = data.CurrentState;
            NextState = data.NextState;
        }
    }
}


