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

            if (InputMgr.Up)
                MoveHelper.MoveUp();
            else if (InputMgr.Down)
                MoveHelper.MoveDown();
            else
                MoveHelper.ClimbStop();
           
            if (InputMgr.Jump)
                MoveHelper.Jump();


//            string m;
//            if (InputMgr.Left)
//                m = "左";
//            else if (InputMgr.Right)
//                m = "右";
//            else
//                m = "null";
//
//            Debug.Log(string.Format("向{0}移动，{1}跳跃", m, InputMgr.Jump ? "有" : "没有"));
        }

        public virtual void Activate(float deltaTime)
        {
            // 这里处理游戏逻辑。
        }
    }
}

