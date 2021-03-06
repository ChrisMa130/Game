﻿using UnityEngine;
using System.Collections;

namespace MG
{
    public class RunState : BaseState
    {
        private float moveParam;
        
        public RunState(Player player) : base(player)
        {
        }

        public override void Enter()
        {
            // Move();
        }

        public override void Activate(float deltaTime)
        {
            // 如果移动速度为0了，那么就变成stand
            base.Activate(deltaTime);
            //moveParam = Input.GetAxis("Horizontal");
			float temp = Input.GetAxis("Horizontal");
			if (temp > 0)
				moveParam = 1;
			else
				moveParam = -1;
        }

        public override void Exit() { }

        public override bool CanChange(PlayerStateType nextPlayerState)
        {
            if (nextPlayerState == PlayerStateType.Dead)
                return true;

            if (nextPlayerState == PlayerStateType.Stand || nextPlayerState == PlayerStateType.Jump || nextPlayerState == PlayerStateType.Climb)
                return true;

            return false;
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

            if (input.Jump)
            {
                Owner.Jump();
                return;
            }

            if (input.Up && Owner.OnTheClimbAera)
            {
                Owner.Climb();
                return;
            }

            if (input.Left || input.Right)
                return;

            if ((!input.HasInput || input.Up) && Owner.Grounded)
            {
                Owner.Stand();
            }
        }

        private void Move()
        {
            Animator.Run(moveParam);
            Rigidbody.velocity = new Vector2(moveParam * GameDefine.PlayerMaxSpeed, Rigidbody.velocity.y);
        }
    }
}
