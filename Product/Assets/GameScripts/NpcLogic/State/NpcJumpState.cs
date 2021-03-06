﻿using UnityEngine;
using System.Collections;

namespace MG
{
    public class NpcJumpState : NpcBaseState
    {
        public NpcJumpState(Npc npc) : base(npc)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Rigidbody.AddForce(new Vector2(0, GameDefine.NpcJumpForce));
        }

        public override void Activate(float deltaTime)
        {
            base.Activate(deltaTime);

            if (Owner.Grounded)
                Owner.Walk();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override bool CanChange(NpcStateType State)
        {
            return base.CanChange(State);
        }
    }
}

