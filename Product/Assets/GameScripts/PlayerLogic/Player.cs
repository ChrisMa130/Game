using UnityEngine;
using System.Collections;

// 角色逻辑
namespace MG
{
    public class Player : MonoBehaviour
    {
        private GameInput InputMgr;

        void Start()
        {
            // 出生后一些创建代码
            InputMgr = gameObject.AddComponent<GameInput>();
        }

        void Update()
        {
            // 讲道理，这里应该只做表现逻辑，游戏逻辑丢这里的话，可能会产生时序的bug。
        }

        public virtual void Activate(float deltaTime)
        {
            // 这里处理游戏逻辑。    
        }
    }
}

