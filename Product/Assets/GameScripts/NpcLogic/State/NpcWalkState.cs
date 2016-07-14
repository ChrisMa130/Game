using UnityEngine;

namespace MG
{
    public class NpcWalkState : NpcBaseState
    {
        private float moveParam;

        public NpcWalkState(Npc npc) : base(npc)
        {
            moveParam = GameDefine.NpcMoveSpeed;
        }

        public override void Enter()
        {
            base.Enter();
            ChangeDir(Owner.CurrentDir);
        }

        public override void Activate(float deltaTime)
        {
            base.Activate(deltaTime);

            Move();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override bool CanChange(NpcStateType State)
        {
            return base.CanChange(State);
        }

        private void Move()
        {
            Animator.Run(moveParam);
            Rigidbody.velocity = new Vector2(moveParam, Rigidbody.velocity.y);
        }

        public override void ChangeDir(Dir dir)
        {
            if (dir == Dir.Left)
                moveParam = -Mathf.Abs(moveParam);
            else
                moveParam = Mathf.Abs(moveParam);
        }
    }
}


