using UnityEngine;
using System.Collections;

namespace MG
{
    public class outdoor : MonoBehaviour
    {
        public int[] NeedCollects = new int[GameDefine.CollectCount];

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

        }
    }
}

