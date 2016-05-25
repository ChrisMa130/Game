using UnityEngine;
using System.Collections;

namespace MG
{
    public class ClimbState : BaseState
    {
        private float moveParam;
        private float OldGravityScale;

        public ClimbState(Player player) : base(player)
        {
        }

        public override void Enter()
        {
            OldGravityScale = Rigidbody.gravityScale;
            Rigidbody.gravityScale = 0;
        }

        public override void Activate(float deltaTime)
        {
            base.Activate(deltaTime);
            moveParam = Input.GetAxis("Vertical");
            Animator.Climb(moveParam);
        }

        public override void Exit()
        {
            Rigidbody.gravityScale = OldGravityScale;
        }

        public override bool CanChange(StateType nextState)
        {
            if (nextState == StateType.Stand || nextState == StateType.Jump)
                return true;

            return false;
        }

        public override void ApplyInput(GameInput input)
        {
            if (input.Up)
            {
                Move(moveParam);
            }
            else if (input.Down)
            {
                Move(moveParam);
                if (Owner.Grounded)
                    Owner.Stand();
            }
            else
            {
                Move(0);
            }

            if (input.Left)
                Owner.TurnRound(Dir.Left);
            else if (input.Right)
                Owner.TurnRound(Dir.Right);

            if (!Owner.OnTheClimbAera)
                Owner.Stand();

            if (!input.Jump)
                return;

            Owner.Stand();
        }

        private void Move(float param)
        {
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, param * GameDefine.PlayerMaxSpeed);
        }
    }


}

