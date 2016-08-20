using UnityEngine;

namespace MG
{
    public class NpcDeadState : NpcBaseState
    {
        public NpcDeadState(Npc npc) : base(npc)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("小兵delad");
            GameObject.Destroy(Owner.gameObject);  // TODO 这个死亡要调回NPC
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



