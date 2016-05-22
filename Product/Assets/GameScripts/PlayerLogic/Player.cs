using UnityEngine;
using System.Collections;

// 角色逻辑
namespace MG
{
    public class Player : MonoBehaviour
    {
        private GameInput InputMgr;
        private ObjectMove MoveHelper;
        private Collect CollectItem;
        private Represent MyRepresent;

        void Awake()
        {
            // 出生后一些创建代码
            InputMgr    = gameObject.AddComponent<GameInput>();
            MoveHelper  = gameObject.AddComponent<ObjectMove>();
            MyRepresent = gameObject.AddComponent<Represent>();

            CollectItem = new Collect();
            MoveHelper.SetPlayer(this);
        }

        void Update()
        {
            if (InputMgr.Left)
                MoveHelper.MoveLeft();
            else if (InputMgr.Right)
                MoveHelper.MoveRight();
            else
                MoveHelper.MoveStop();

            if (InputMgr.Up)
                MoveHelper.MoveUp();
            else if (InputMgr.Down)
                MoveHelper.MoveDown();
            else
                MoveHelper.ClimbStop();
           
            if (InputMgr.Jump)
                MoveHelper.Jump();
        }

        public virtual void Activate(float deltaTime)
        {
            // 这里处理游戏逻辑。
        }

        public void AddCollectItem(int key, int value)
        {
            CollectItem.AddCollectItem(key, value);
        }

        public void Walk(float moveParam)
        {
            MyRepresent.Walk(moveParam);
        }

        public void Stand()
        {
            MyRepresent.Stand();
        }

        public void Climb(float moveParam)
        {
            MyRepresent.Climb(moveParam);
        }

        public void Dead()
        {
            MyRepresent.Dead();
        }

        public void Jump()
        {
            MyRepresent.Jump();
        }
    }
}

