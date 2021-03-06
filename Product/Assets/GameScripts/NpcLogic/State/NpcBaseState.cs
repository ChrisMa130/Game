﻿using UnityEngine;
using System.Collections;

namespace MG
{
    public class NpcBaseState
    {
        protected Npc Owner;
        protected NpcRepresent Animator;
        protected Rigidbody2D Rigidbody;
        protected NpcBaseState(Npc npc)
        {
            Owner = npc;
            Animator = Owner.gameObject.GetComponent<NpcRepresent>();
            Rigidbody = Owner.gameObject.GetComponent<Rigidbody2D>();
        }

        public virtual void Enter() { }
        public virtual void Activate(float deltaTime) { }
        public virtual void Exit() { }
        public virtual bool CanChange(NpcStateType State) { return true; }
        public virtual void ChangeDir(Dir dir) { }
    }
}


