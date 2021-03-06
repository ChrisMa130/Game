﻿using UnityEngine;
using System.Collections;

namespace MG
{
    public class NpcStandState : NpcBaseState
    {
        public NpcStandState(Npc npc) : base(npc)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Activate(float deltaTime)
        {
            base.Activate(deltaTime);
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

