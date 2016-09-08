using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace MG
{
    public class outdoor : MonoBehaviour
    {
        public int[] NeedCollects = new int[GameDefine.CollectCount];

        public string NextLevelName;

        void Start()
        {
        }

        void OnTriggerStay2D(Collider2D other)
        {
            var player = other.GetComponent<Player>();
            if (player == null)
                return;

//            bool finish = true;
//            for (int i = 0; i < NeedCollects.Length; i++)
//            {
//                var count = NeedCollects[i];
//                if (count > 0)
//                {
//                    var playerGet = player.GetCollectCount(i);
//                    if (count > playerGet)
//                    {
//                        finish = false;
//                        break;
//                    }
//                }
//            }

//            if (!finish)
//            {
//                // 没通关
//                Debug.Log("没通关，谢谢");
//                return;
//            }
//
//            // 胜利通关逻辑，什么切换下一关，显示胜利界面拉。balbala
//
//            if (string.IsNullOrEmpty(NextLevelName))
//            {
//                Debug.Log("呵呵哒了，没下一关了。GGWP");
//                return;
//            }
			if (GameMgr.Instance.InputMgr.UpUp) {
				SceneManager.LoadSceneAsync (NextLevelName);
			}
        }
    }
}

