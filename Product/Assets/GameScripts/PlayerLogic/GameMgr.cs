using UnityEngine;

// 游戏主体逻辑

namespace MG
{
    public class GameMgr : SingletonMonoBehaviour<GameMgr>
    {
        public GameObject PlayerObject;
        public Player PlayerLogic;
        public GameInput InputMgr;
        public LineMgr LineMgr;
        public bool IsPause { get; private set; }
        private bool RecordingTime;

        void Start()
        {
            RecordingTime = true;
            IsPause = false;
            InputMgr = gameObject.AddComponent<GameInput>();
            PlayerLogic = PlayerObject.AddMissingComponent<Player>();
            LineMgr = gameObject.AddMissingComponent<LineMgr>();
        }

        void Update()
        {
            // TODO 场景加载完毕后才能做这件事情
            float timeDelta = Time.deltaTime;

            InputMgr.Activate();

            UpdateInput();

            if (TimeController.Instance != null)
            {
                ProcessTime();

                if (RecordingTime && !IsPause)
                {
                    TimeController.Instance.RecordTime();
                }
            }

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

            LineMgr.ApplyInput(InputMgr);
        }

        void ProcessTime()
        {
            TimeController ctrl = TimeController.Instance;

            if (InputMgr.TimebackDown)
            {
                RecordingTime = false;
                PauseGame(false);
                ctrl.RewindTime();
            }
            else if (InputMgr.TimebackUp)
            {
                RecordingTime = false;
                PauseGame(true);
                ctrl.Freeze();
            }
            else if (InputMgr.TimeForwardDown)
            {
                RecordingTime = false;
                PauseGame(false);
                ctrl.ForwardTime();
            }
            else if (InputMgr.TimeForwardup)
            {
                RecordingTime = false;
                PauseGame(true);
                ctrl.Freeze();
            }
        }

        public void PauseGame(bool pause)
        {
            // Debug.Log("PauseGame " + pause);
            Time.timeScale = pause ? 0 : 1;
            IsPause = pause;

            if (TimeController.Instance.CurrentState == TimeControllState.Freeze)
            {
                RecordingTime = true;
            }
                
        }
    }
}

