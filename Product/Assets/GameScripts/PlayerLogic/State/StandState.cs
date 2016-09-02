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

        public override bool CanChange(PlayerStateType nextPlayerState)
        {
            if (Owner.IsDead)
                return false;

            return true;
        }

        public override void ApplyInput(GameInput input)
        {
            if (Owner == null || Owner.IsDead)
                return;

            if (!Owner.OnTheLine)
            {
                if (input.Left || input.Right)
                {
                    Owner.Run();
                    return;
                }
            }
            else
            {
                if (!Owner.Grounded && (input.Left || input.Right))
                {
                    Owner.Run();
                    return;
                }
            }

            if (input.Up && Owner.OnTheClimbAera)
            {
                // TODO 这里要从玩家坐标向玩家的朝向发射一个射线，如果发现与梯子距离小于设定值，
                // 那么按上则采取靠近梯子，当与梯子的距离足够近，并且，还是梯子中心的时候再转换成为爬梯子的状态
                                
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