using UnityEngine;

namespace MG
{
    public class ClimbState : BaseState
    {
        private float moveParam;
        private float OldGravityScale;
        private Collider2D c2d;
		private BoxCollider2D ownerB2d;
		private CircleCollider2D ownerC2d;

		private float ownerHeight;
        class ClimbUserData : PlayerStateRunTime
        {
            public float moveParam;
            public float OldGravityScale;
        }

        public ClimbState(Player player) : base(player)
        {
        }

        public override void Enter()
        {
            OldGravityScale = Rigidbody.gravityScale;
            Rigidbody.gravityScale = 0;
            Move(0);

            c2d = Owner.LadderObj.GetComponent<Collider2D>();
			ownerB2d = Owner.GetComponent<BoxCollider2D> ();
			ownerC2d = Owner.GetComponent<CircleCollider2D> ();

			ownerHeight = ownerB2d.bounds.size.y / 2 + ownerC2d.bounds.size.y / 2 + ownerC2d.bounds.size.y / 5;
            ladder ld = Owner.LadderObj.GetComponent<ladder>();
			FixPosition (c2d);


            Owner.transform.position = new Vector3(ld.MidPosition.x, Owner.Position.y);
        }

		private void FixPosition(Collider2D collider) {
			float ladderY = Owner.LadderObj.transform.position.y;
			bool outladder = (Owner.Position.y + ownerHeight) > (ladderY + c2d.bounds.size.y);

			if (outladder) {
				Owner.Position = new Vector3 (Owner.Position.x, ladderY + c2d.bounds.size.y - ownerHeight, Owner.Position.z);
			}
		}

        public override void Activate(float deltaTime)
        {
            base.Activate(deltaTime);
            moveParam = Input.GetAxis("Vertical");
            Animator.Climb(moveParam);

            if (Owner.LadderObj == null)
                Owner.Stand();
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
            if (Owner.LadderObj == null)
                return;

			float ladderY = Owner.LadderObj.transform.position.y;
			bool outladder = (Owner.Position.y + (ownerHeight)) > (ladderY + c2d.bounds.size.y);
			bool outBottom = (Owner.Position.y - (ownerHeight)) < (ladderY - c2d.bounds.size.y);

			if (input.Up && !outladder && !outBottom) {
				Move (moveParam);
			} else if (input.Down) {
				Move (moveParam);
				if (Owner.Grounded)
					Owner.Stand ();
			} else {
				Move (0);
			}

			if (input.Left)
				Owner.TurnRound (Dir.Left);
			else if (input.Right)
				Owner.TurnRound (Dir.Right);

			if (!Owner.OnTheClimbAera)
				Owner.Stand ();

			if (!input.Jump)
				return;

			Owner.Stand ();
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

        public override PlayerStateRunTime GetUserData()
        {
            ClimbUserData data = new ClimbUserData();
            data.moveParam = moveParam;
            data.OldGravityScale = OldGravityScale;

            return data;
        }

        public override void SetUserData(PlayerStateRunTime data)
        {
            ClimbUserData d = data as ClimbUserData;
            if (d == null)
                return;

            moveParam = d.moveParam;
            OldGravityScale = d.OldGravityScale;
        }
    }


}

