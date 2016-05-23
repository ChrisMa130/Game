using UnityEngine;
using System.Collections;

namespace MG
{
    public class PlayerState : MonoBehaviour
    {
        public Player Owner { get; private set; }
        private BaseState[] States;
        public StateType CurrentState { get; private set; }
        public StateType NextState { get; private set; }

        public void Init(Player player)
        {
            Owner = player;

            States = new BaseState[]
            {
                new StandState(Owner),
                new RunState(Owner), 
                new JumpState(Owner), 
                new ClimbState(Owner), 
                new DeadState(Owner), 
            };

            CurrentState    = StateType.Stand;
            NextState       = StateType.Invalid;
        }

        public void Activate(float deltaTime)
        {
            if (NextState != StateType.Invalid)
            {
                ChangeState();
            }

            if (CurrentState != StateType.Invalid)
            {
                States[(int)CurrentState].Activate(deltaTime);
            }
        }

        private void ChangeState()
        {
            if (Owner == null)
                return;

            if (CurrentState != StateType.Invalid)
            {
                States[(int)CurrentState].Exit();
            }

            CurrentState = NextState;
            NextState = StateType.Invalid;

            var state = States[(int)CurrentState];
            state.Enter();
        }

        public bool CanChangeState(StateType type)
        {
            var state = States[(int)CurrentState];
            return state.CanChange(type);
        }

        public bool Stand()
        {
            if (!CanChangeState(StateType.Stand))
                return false;

            NextState = StateType.Stand;

            return true;
        }

        public bool Run()
        {
            if (!CanChangeState(StateType.Run))
                return false;

            NextState = StateType.Run;

            return true;
        }

        public bool Jump()
        {
            if (!CanChangeState(StateType.Jump))
                return false;

            NextState = StateType.Jump;

            return true;
        }

        public bool Climb()
        {
            if (!CanChangeState(StateType.Climb))
                return false;

            NextState = StateType.Climb;

            return true;
        }

        public bool Dead()
        {
            if (!CanChangeState(StateType.Dead))
                return false;

            NextState = StateType.Dead;

            return true;
        }

        public void ApplyInput(GameInput input)
        {
            var state = States[(int)CurrentState];
            state.ApplyInput(input);
        }
    }
}
