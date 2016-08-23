﻿using UnityEditor;
using UnityEngine;

namespace MG
{
    public class ClimbState : BaseState
    {
        private float moveParam;
        private float OldGravityScale;
        private Collider2D c2d;

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
            ladder ld = Owner.LadderObj.GetComponent<ladder>();

            Owner.transform.position = new Vector3(ld.MidPosition.x, Owner.Position.y);
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

