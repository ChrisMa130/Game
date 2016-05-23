using UnityEngine;
using System.Collections;

// 一些游戏定义丢这里

namespace MG
{
    public enum StateType
    {
        Invalid = -1,

        Stand,
        Run,
        Jump,
        Climb,
        Dead,
    }

    public enum Dir
    {
        Left,
        Right,
    }

    public static class GameDefine
    {
        public static readonly int GameFps = 18;    // 一个像素游戏，18帧/秒不过分吧。。
        public static readonly int CollectCount = 3;
        public static readonly float Epsilon = 0.0000001f;      // 为什么要自定义呢，因为编译手机的话，在mono的Epsilon是0.这是一个bug。
        public static readonly float PlayerMaxSpeed = 1.2f;
        public static readonly float JumpForce = 200.0f;

        public static T AddMissingComponent<T>(this GameObject go) where T : Component
        {
            T comp = go.GetComponent<T>();

            if (comp == null)
            {
                comp = go.AddComponent<T>();
            }
            return comp;
        }

        public static bool FloatIsZero(float value)
        {
            if (value >= -Epsilon && value <= Epsilon)
                return true;

            return false;
        }
    }
}

