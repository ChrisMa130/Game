﻿using UnityEngine;
using System.Collections;

namespace MG
{
    public class NpcBaseState : MonoBehaviour
    {
        protected readonly Npc Owner;
        // protected readonly Represent Animator;
        // protected readonly Rigidbody2D Rigidbody;
        protected NpcBaseState(Npc npc)
        {
            Owner = npc;
            //Animator = player.gameObject.GetComponent<Represent>();
            // Rigidbody = player.gameObject.GetComponent<Rigidbody2D>();
        }

        public virtual void Enter() { }
        public virtual void Activate(float deltaTime) { }
        public virtual void Exit() { }
        public virtual bool CanChange(NpcStateType State) { return true; }
    }
}


