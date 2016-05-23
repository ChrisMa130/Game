using UnityEngine;
using System.Collections;

namespace MG
{
    public class StandState : BaseState
    {
        public StandState(Player player) : base(player)
        {
        }

        public override void Enter()
        {
            base.Enter();

            Animator.Stand();
        }

        public override void Activate(float deltaTime)
        {
            // 判断是否在地面
            // 如果不在地面。那么转成jump状态??
            Rigidbody.velocity = new Vector2(0, Rigidbody.velocity.y);
        }

        public override void Exit()
        {
        }

        public override bool CanChange(StateType nextState)
        {
            if (Owner.IsDead)
                return false;

            return true;
        }

        public override void ApplyInput(GameInput input)
        {
            if (Owner == null || Owner.IsDead)
                return;

            if (input.Left || input.Right)
            {
                Owner.Run();
                return;
            }

            if (input.Up && Owner.OnTheClimbAera)
            {
                Owner.Climb();
                return;
            }

            if (input.Jump)
            {
                Owner.Jump();
                return;
            }
        }
    }
}