using UnityEngine;
using System.Collections;

namespace MG
{
    public class DeadState : BaseState
    {
        public DeadState(Player player) : base(player)
        {
        }

        public override void Enter()
        {
            Animator.Dead();
        }

        public override void Activate(float deltaTime) { }

        public override void Exit() { }

        public override bool CanChange(StateType nextState)
        {
            if (nextState != StateType.Stand)
                return false;

            return true;
        }

        public override void ApplyInput(GameInput input)
        {
            if (input.Jump)
            {
                // TODO 返回检查点。
                Owner.Stand();
            }
                
        }


    }


}
