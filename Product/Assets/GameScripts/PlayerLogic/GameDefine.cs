using UnityEngine;
using System.Collections;

// 一些游戏定义丢这里

namespace MG
{
    public static class GameDefine
    {
        public static readonly int GameFps = 18;    // 一个像素游戏，18帧/秒不过分吧。。
        public static readonly int CollectCount = 3;

        public static T AddMissingComponent<T>(this GameObject go) where T : Component
        {
            T comp = go.GetComponent<T>();

            if (comp == null)
            {
                comp = go.AddComponent<T>();
            }
            return comp;
        }
    }
}

