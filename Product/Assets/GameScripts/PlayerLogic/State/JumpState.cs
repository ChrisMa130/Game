using UnityEngine;

namespace MG
{
    public class JumpState : BaseState
    {
        private bool IsJumped;
        private float DisableTime;

        class JumpUserData : PlayerStateRunTime
        {
            public bool IsJumped;
            public float DisableTime;
        }

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

			if (Owner.Grounded && Rigidbody.velocity == Vector2.zero) {
				Owner.Stand ();
			} else if (Owner.Grounded && Rigidbody.velocity != Vector2.zero) {
				Owner.Run ();
			}
        }

        public override void Exit() { }

        public override bool CanChange(PlayerStateType nextPlayerState)
        {
            if (Owner.IsDead)
                return false;

            return true; 
        }

        public override void ApplyInput(GameInput input)
        {
            if (Owner.OnTheLine)
                return;

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

            // TODO 当玩家是X到达梯子中心点的时候，在切换状态
            if ((input.Up || input.Down) && Owner.OnTheClimbAera)
                Owner.Climb();
        }

        private void Move()
        {
            var moveParam = Input.GetAxis("Horizontal");
            Rigidbody.velocity = new Vector2(moveParam * GameDefine.PlayerMaxSpeed, Rigidbody.velocity.y);
        }

        public override PlayerStateRunTime GetUserData()
        {
            JumpUserData data = new JumpUserData();
            data.DisableTime = DisableTime;
            data.IsJumped = IsJumped;

            return data;
        }

        public override void SetUserData(PlayerStateRunTime data)
        {
            JumpUserData d = data as JumpUserData;
            if (d == null)
                return;

            d.IsJumped = IsJumped;
            d.DisableTime = DisableTime;
        }
    }

}


