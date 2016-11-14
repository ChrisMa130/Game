using UnityEngine;
using System.Collections.Generic;
using Com.LuisPedroFonseca.ProCamera2D;
using UnityEngine.SceneManagement;

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

        public string LevelName;
        public string LevelAsName;

        private List<pickup> CollectObjects = new List<pickup>();

        public GameConfig Config;

        void Start()
        {
            if (PlayerObject == null)
            {
                PlayerObject = GameObject.Find("Faylisa");
                if (PlayerObject == null)
                    PlayerObject = GameObject.Find("BlueHat");
            }
            RecordingTime = true;
            IsPause = false;
            InputMgr = gameObject.AddComponent<GameInput>();
            PlayerLogic = PlayerObject.AddMissingComponent<Player>();
            LineMgr = gameObject.AddMissingComponent<LineMgr>();
            UiManager = gameObject.AddMissingComponent<UIManager>();
            UiManager.OpenUI(0);

            Config = new GameConfig();
            Config.Load();

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

            if (Camera.main.GetComponent<ProCamera2D>().CameraTargets.Count == 0)
                Camera.main.GetComponent<ProCamera2D>().AddCameraTarget(PlayerObject.transform);
            Camera.main.GetComponent<ProCamera2DTransitionsFX>().TransitionEnter();

            GameObject bgm = GameObject.Find("BGManager");
            if (bgm == null)
            {
                Instantiate(Resources.Load("prefabs/Utilities/BGManager"));
            }
            
            // 首先从gamedata中得到当前level的保存信息
            // 如果没有保存内容，那么就创建一个
            if (GameData.Instance != null)
            {
                SaveBlack data = GameData.Instance.GetLevelData(LevelName);
                if (data == null)
                {
                    GameData.Instance.AddNewLevel(LevelName, LevelName, PlayerLogic.Position);
                }
                else
                {
                    GameData.Instance.ChangeCurrentLevel(LevelName);

                    // 重置玩家坐标
                    PlayerLogic.SetRevivePoint(data.BornPos);
                    PlayerLogic.Position = data.BornPos;
                }
            }

            CreateLevelItem();

            PlayerLogic.OnPickUpAction += GameData.Instance.SaveCollectPickup;
        }

        public void OnDestroy()
        {
            PlayerLogic.OnPickUpAction = null;
        }

        void Update()
        {
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

        public void AddCollectInstance(pickup obj)
        {
            CollectObjects.Add(obj);
        }

        public void LastCheckPoint()
        {
            if (GameData.Instance == null)
                return;

            LoadLevel(LevelName);
        }

        public void LoadLevel(string name)
        {
            if (GameData.Instance == null)
                return;

            var data = GameData.Instance.GetLevelData(LevelName);
            if (data == null)
            {
                Debug.Log("未发现当前level的保存信息。bug");
                return;
            }

            var transitionFX = Camera.main.GetComponent<ProCamera2DTransitionsFX>();

            transitionFX.OnTransitionExitEnded = () =>
            {
                transitionFX.OnTransitionExitEnded = null;
                PlayerLogic.Revive();
                //SceneManager.LoadSceneAsync(LevelName);
            };

            transitionFX.TransitionExit();
            UiManager.CloseCurrentUI();
            PauseGame(false, false);
        }

        public void CreateLevelItem()
        {
            // 创建日志的道具
            string fileName = "1";
            // 根据类型，创建不同的对象
            SettingReader.Load(fileName, (l, i) =>
            {
                string path = l.GetString(i, "Prefab");
                float x = l.GetFloat(i, "PosX");
                float y = l.GetFloat(i, "PosY");
                string name = l.GetString(i, "Name");

                var o = Resources.Load(path) as GameObject;
                var item = Instantiate(o) as GameObject;
                item.name = name;
                item.transform.position = new Vector3(x, y, 0);
                item.transform.rotation = Quaternion.identity;
            });
        }
    }
}

