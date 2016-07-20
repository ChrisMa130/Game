using UnityEditor;
using UnityEngine;

namespace MG
{
    public class ClimbState : BaseState
    {
        private float moveParam;
        private float OldGravityScale;
        private Collider2D c2d;

        public ClimbState(Player player) : base(player)
        {
        }

        public override void Enter()
        {
            OldGravityScale = Rigidbody.gravityScale;
            Rigidbody.gravityScale = 0;
            Move(0);

            c2d = Owner.LadderObj.GetComponent<Collider2D>();
//            var sss = c2d.bounds.size.x/2;
//            if (Owner.LadderObj.transform.position.x > Owner.Position.x)
//                Owner.transform.position = new Vector3(Owner.Position.x + sss, Owner.Position.y);
//            else
//                Owner.transform.position = new Vector3(Owner.Position.x - sss, Owner.Position.y);
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

        public override bool CanChange(PlayerStateType nextPlayerState)
        {
            if (nextPlayerState == PlayerStateType.Stand || nextPlayerState == PlayerStateType.Jump)
                return true;

            return false;
        }

        public override void ApplyInput(GameInput input)
        {
            float ladderY = Owner.LadderObj.transform.position.y;
            bool outladder = Owner.Position.y > (ladderY + c2d.bounds.size.y);

            if (input.Up && !outladder)
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
            if (GameDefine.FloatIsZero(param))
            {
                Rigidbody.velocity = new Vector2(0, 0);
            }
            else
            {
                Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, param * GameDefine.PlayerMaxSpeed);
            }
        }
    }


}

