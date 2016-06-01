using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

// 游戏主体逻辑

namespace MG
{
    public class GameMgr : MonoBehaviour
    {
        public GameObject PlayerObject;
        private Player PlayerLogic;
        private GameInput InputMgr;
        private bool IsPause;

        void Start()
        {
            IsPause = false;
            InputMgr = gameObject.AddComponent<GameInput>();
            PlayerLogic = PlayerObject.AddMissingComponent<Player>();
            
        }

        void Update()
        {
            float timeDelta = Time.deltaTime;

            InputMgr.Activate();

            UpdateInput();

            PlayerLogic.Activate(timeDelta);  // 暂时不拆，因为可能回滚时间要active的。
        }

        void UpdateInput()
        {
            if (InputMgr.PauseTime)
            {
                Debug.Log("暂停");
                IsPause = !IsPause;
                PauseGame(IsPause);
            }

            if (InputMgr.Jump)
            {
                Debug.Log("跳跃");
            }

            if (InputMgr.Down)
            {
                Debug.Log("下");
            }

            if (!IsPause)
            {
                PlayerLogic.ApplyInput(InputMgr);
            }
        }

        public void PauseGame(bool pause)
        {
            Time.timeScale = pause ? 0 : 1;
        }
    }
}

