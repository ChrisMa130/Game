using UnityEngine;

namespace MG
{
    public class PlayerStateUserData
    {
        public PlayerStateType CurrentState;
        public PlayerStateType NextState;

        public PlayerStateRunTime RunTime;
    }

    public class PlayerStateRunTime
    { }

    public class PlayerState : MonoBehaviour
    {
        public Player Owner { get; private set; }
        private BaseState[] States;
        public PlayerStateType CurrentPlayerState { get; private set; }
        public PlayerStateType NextPlayerState { get; private set; }

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

            CurrentPlayerState    = PlayerStateType.Stand;
            NextPlayerState       = PlayerStateType.Invalid;
        }

        public void Activate(float deltaTime)
        {
            if (NextPlayerState != PlayerStateType.Invalid)
            {
                ChangeState();
            }

            if (CurrentPlayerState != PlayerStateType.Invalid)
            {
                States[(int)CurrentPlayerState].Activate(deltaTime);
            }
        }

        private void ChangeState()
        {
            if (Owner == null)
                return;

            if (CurrentPlayerState != PlayerStateType.Invalid)
            {
                States[(int)CurrentPlayerState].Exit();
            }

            CurrentPlayerState = NextPlayerState;
            NextPlayerState = PlayerStateType.Invalid;

            var state = States[(int)CurrentPlayerState];
            state.Enter();
        }

        public bool CanChangeState(PlayerStateType type)
        {
            var state = States[(int)CurrentPlayerState];
            return state.CanChange(type);
        }

        public bool Stand()
        {
            if (!CanChangeState(PlayerStateType.Stand))
                return false;

            NextPlayerState = PlayerStateType.Stand;

            return true;
        }

        public bool Run()
        {
            if (!CanChangeState(PlayerStateType.Run))
                return false;

            NextPlayerState = PlayerStateType.Run;

            return true;
        }

        public bool Jump()
        {
            if (!CanChangeState(PlayerStateType.Jump))
                return false;

            NextPlayerState = PlayerStateType.Jump;

            return true;
        }

        public bool Climb()
        {
            if (!CanChangeState(PlayerStateType.Climb))
                return false;

            NextPlayerState = PlayerStateType.Climb;

            return true;
        }

        public bool Dead()
        {
            if (!CanChangeState(PlayerStateType.Dead))
                return false;

            NextPlayerState = PlayerStateType.Dead;

            return true;
        }

        public void ApplyInput(GameInput input)
        {
            var state = States[(int)CurrentPlayerState];
            state.ApplyInput(input);
        }

        public PlayerStateUserData GetUserData()
        {
            PlayerStateUserData data = new PlayerStateUserData();
            data.CurrentState = CurrentPlayerState;
            data.NextState = NextPlayerState;
            data.RunTime = States[(int) CurrentPlayerState].GetUserData();

            return data;
        }

        public void SetUserData(PlayerStateUserData data)
        {
            if (data == null)
                return;

            CurrentPlayerState = data.CurrentState;
            NextPlayerState = data.NextState;
            if (data.RunTime != null)
                States[(int)CurrentPlayerState].SetUserData(data.RunTime);
        }
    }
}
