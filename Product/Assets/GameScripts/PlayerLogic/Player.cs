using UnityEngine;
using System.Collections;

// 角色逻辑
namespace MG
{
    public class Player : MonoBehaviour
    {
        private GameInput InputMgr;
        private ObjectMove MoveHelper;

        void Start()
        {
            // 出生后一些创建代码
            InputMgr    = gameObject.AddComponent<GameInput>();
            MoveHelper  = gameObject.AddComponent<ObjectMove>();
        }

        void Update()
        {
            if (InputMgr.Left)
                MoveHelper.MoveLeft();
            else if (InputMgr.Right)
                MoveHelper.MoveRight();
            else
                MoveHelper.MoveStop();

            if (InputMgr.Jump)
                MoveHelper.Jump();
        }

        public virtual void Activate(float deltaTime)
        {
            // 这里处理游戏逻辑。
        }
    }
}

