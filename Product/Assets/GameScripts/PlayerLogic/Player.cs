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
            // 讲道理，这里应该只做表现逻辑，游戏逻辑丢这里的话，可能会产生时序的bug。
            // 临时测试，bug:停不下来，跳跃无效。
            if (InputMgr.Left)
                MoveHelper.MoveLeft();

            if (InputMgr.Right)
                MoveHelper.MoveRight();

            if (InputMgr.Jump)
                MoveHelper.Jump();
        }

        public virtual void Activate(float deltaTime)
        {
            // 这里处理游戏逻辑。
        }
    }
}

