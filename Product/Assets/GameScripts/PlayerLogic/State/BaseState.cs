using UnityEngine;

namespace MG
{
    public abstract class BaseState
    {
        protected readonly Player Owner;
        protected readonly Represent Animator;
        protected readonly Rigidbody2D Rigidbody;
        protected BaseState(Player player)
        {
            Owner = player;
            Animator = player.gameObject.GetComponent<Represent>();
            Rigidbody = player.gameObject.GetComponent<Rigidbody2D>();
        }

        public virtual void Enter() { }
        public virtual void Activate(float deltaTime) { }
        public virtual void Exit() { }
        public virtual bool CanChange(PlayerStateType nextPlayerState) { return true; }
        public virtual void ApplyInput(GameInput input) { }
    }
}

