using UnityEngine;
using System.Collections;

namespace MG
{
    public class JumpState : BaseState
    {
        private bool IsJumped;
        private float DisableTime;
        public JumpState(Player player) : base(player)
        {
            IsJumped = false;
        }

        public override void Enter()
        {
            DisableTime = 0.2f;
            Animator.Jump();
            Rigidbody.AddForce(new Vector2(0f, GameDefine.JumpForce));
        }

        public override void Activate(float deltaTime)
        {
            DisableTime -= deltaTime;
            if (DisableTime > 0)
                return;

            if (Owner.Grounded)
            {
                Owner.Stand();
            }
        }

        public override void Exit() { }

        public override bool CanChange(StateType nextState)
        {
            if (Owner.IsDead)
                return false;

            return true; 
        }

        public override void ApplyInput(GameInput input)
        {
            if (input.Left)
            {
                Owner.TurnRound(Dir.Left);
                Move();
            } 
            else if (input.Right)
            {
                Owner.TurnRound(Dir.Right);
                Move();
            }

            if (input.Up && Owner.OnTheClimbAera)
                Owner.Climb();
        }

        private void Move()
        {
            var moveParam = Input.GetAxis("Horizontal");
            Rigidbody.velocity = new Vector2(moveParam * GameDefine.PlayerMaxSpeed, Rigidbody.velocity.y);
        }
    }

}


