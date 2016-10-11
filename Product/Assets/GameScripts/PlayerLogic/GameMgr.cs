using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;

// 游戏主体逻辑

namespace MG
{
    public class GameMgr : SingletonMonoBehaviour<GameMgr>
    {
        public GameObject PlayerObject;
        public Player PlayerLogic;
        public GameInput InputMgr;
        public LineMgr LineMgr;
        public UIManager UiManager;

        public bool IsPause { get; private set; }
        public int ExitCount = 0;
        private bool RecordingTime;

        public GameSwitch WorldSwith;

        void Start()
        {
			if (PlayerObject == null) {
				PlayerObject = GameObject.Find ("Faylisa");
				if (PlayerObject == null)
					PlayerObject = GameObject.Find ("BlueHat");
			}
            RecordingTime = true;
            IsPause = false;
            InputMgr = gameObject.AddComponent<GameInput>();
            PlayerLogic = PlayerObject.AddMissingComponent<Player>();
            LineMgr = gameObject.AddMissingComponent<LineMgr>();
            UiManager = gameObject.AddMissingComponent<UIManager>();
            UiManager.OpenUI(0);

            // 查找开关
            var s = GameObject.Find("GameSwitch");
            if (s != null)
            {
                WorldSwith = s.GetComponent<GameSwitch>();
            }

            if (WorldSwith == null)
            {
                WorldSwith = new GameSwitch();
                WorldSwith.ForbidDrawLine = false;
                WorldSwith.ForbidTimeOperation = false;
            }

			if (Camera.main.GetComponent<ProCamera2D> ().CameraTargets.Count == 0)
				Camera.main.GetComponent<ProCamera2D> ().AddCameraTarget (PlayerObject.transform);
			Camera.main.GetComponent<ProCamera2DTransitionsFX> ().TransitionEnter ();
        }

        void Update()
        {
            // TODO 场景加载完毕后才能做这件事情
            float timeDelta = Time.deltaTime;

            InputMgr.Activate();

            UpdateInput();

            if (!WorldSwith.ForbidTimeOperation && TimeController.Instance != null)
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
            if (!WorldSwith.ForbidTimeOperation && InputMgr.PauseTime)
            {
                IsPause = !IsPause;
                PauseGame(IsPause, true);
            }

            if (InputMgr.EscButton)
            {
                if (UiManager.IsUIOpening(1))
                {
                    UiManager.OpenUI(0);
                    PauseGame(false, false);
                }
                else
                {
                    UiManager.OpenUI(1);
                    PauseGame(true, false);
                }
            }

            if (!IsPause)
            {
                PlayerLogic.ApplyInput(InputMgr);
            }

            if (!WorldSwith.ForbidDrawLine)
                LineMgr.ApplyInput(InputMgr);
        }

        void ProcessTime()
        {
            TimeController ctrl = TimeController.Instance;
            var state = TimeController.Instance.CurrentState;

            if (InputMgr.TimebackDown)
            {
                if (!ctrl.IsRewindEnd())
                {
                    RecordingTime = false;
                    PauseGame(false, false);
                    ctrl.RewindTime();
                }
                else
                {
                    PauseGame(true, false);
                }
            }
            else if (InputMgr.TimebackUp)
            {
                if (!ctrl.IsRewindEnd())
                {
                    RecordingTime = false;
                    PauseGame(true, false);
                    ctrl.Freeze();
                }
                else
                {
                    PauseGame(true, false);
                }
            }
            else if (InputMgr.TimeForwardDown && state != TimeControllState.Recording)
            {
                if (!ctrl.IsForwardEnd())
                {
                    RecordingTime = false;
                    PauseGame(false, false);
                    ctrl.ForwardTime();
                }
                else
                {
                    PauseGame(true, false);
                }
            }
            else if (InputMgr.TimeForwardup && state != TimeControllState.Recording)
            {
                if (!ctrl.IsForwardEnd())
                {
                    RecordingTime = false;
                    PauseGame(true, false);
                    ctrl.Freeze();
                }
                else
                {
                    PauseGame(true, false);
                }
            }
        }

        public void PauseGame(bool pause, bool DoRecord)
        {
            Time.timeScale = pause ? 0 : 1;
            IsPause = pause;

            if (DoRecord)
            {
                RecordingTime = true;
            }
        }
    }
}

