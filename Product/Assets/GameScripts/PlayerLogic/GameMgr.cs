using UnityEngine;

// 游戏主体逻辑

namespace MG
{
    public class GameMgr : SingletonMonoBehaviour<GameMgr>
    {
        public GameObject PlayerObject;
        private Player PlayerLogic;
        public GameInput InputMgr;
        public LineMgr LineMgr;
        private bool IsPause;

        void Start()
        {
            IsPause = false;
            InputMgr = gameObject.AddComponent<GameInput>();
            PlayerLogic = PlayerObject.AddMissingComponent<Player>();
            LineMgr = gameObject.AddMissingComponent<LineMgr>();
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
                IsPause = !IsPause;
                PauseGame(IsPause);
            }

            if (!IsPause)
            {
                PlayerLogic.ApplyInput(InputMgr);
            }

            if (InputMgr.TimebackDown && TimeController.Instance.TimebackStart)
            {
                TimeController.Instance.StopTimeback();
            }
            else if (InputMgr.TimebackDown && !TimeController.Instance.TimebackStart)
            {
                TimeController.Instance.StartTimeback();
            }

            LineMgr.ApplyInput(InputMgr);
        }

        public void PauseGame(bool pause)
        {
            Time.timeScale = pause ? 0 : 1;
        }
    }
}

