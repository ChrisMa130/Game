using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace MG
{
    public class outdoor : TimeUnit
    {
        public int[] NeedCollects = new int[GameDefine.CollectCount];

        public string NextLevelName;

        class UserData : TimeUnitUserData
        {
            public int[] NeedCollects = new int[GameDefine.CollectCount];
        }

        void Start()
        {
            Init(false);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            var player = other.GetComponent<Player>();
            if (player == null)
                return;

            bool finish = true;
            for (int i = 0; i < NeedCollects.Length; i++)
            {
                var count = NeedCollects[i];
                if (count > 0)
                {
                    var playerGet = player.GetCollectCount(i);
                    if (count > playerGet)
                    {
                        finish = false;
                        break;
                    }
                }
            }

            if (!finish)
            {
                // 没通关
                Debug.Log("没通关，谢谢");
                return;
            }

            // 胜利通关逻辑，什么切换下一关，显示胜利界面拉。balbala

            if (string.IsNullOrEmpty(NextLevelName))
            {
                Debug.Log("呵呵哒了，没下一关了。GGWP");
                return;
            }

            SceneManager.LoadSceneAsync(NextLevelName);
        }

        protected override TimeUnitUserData GetUserData()
        {
            UserData data = new UserData();

            NeedCollects.CopyTo(data.NeedCollects, 0);

            return data;
        }

        protected override void SetUserData(TimeUnitUserData data)
        {
            UserData d = data as UserData;
            if (d == null)
                return;

            d.NeedCollects.CopyTo(NeedCollects, 0);
        }
    }
}

