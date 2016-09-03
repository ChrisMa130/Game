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
            Rigidbody.velocity = Vector2.zero;
        }

        public override void Activate(float deltaTime) { }

        public override void Exit() { }

        public override bool CanChange(PlayerStateType nextPlayerState)
        {
            if (nextPlayerState != PlayerStateType.Stand)
                return false;

            return true;
        }

        public override void ApplyInput(GameInput input)
        {
            var anim = Animator.GetAnimator();

            var state = anim.GetCurrentAnimatorStateInfo(0);
            if (state.IsName("die") && state.length - state.normalizedTime < 0.1f)
            {
                // 动画时间外
                GameMgr.Instance.LineMgr.CanDraw = false;
                if (input.Jump)
                {
                    Owner.Revive();
                }
            }
        }
    }
}
