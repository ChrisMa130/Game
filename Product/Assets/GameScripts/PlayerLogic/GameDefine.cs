﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 一些游戏定义丢这里

namespace MG
{
    public enum PlayerStateType
    {
        Invalid = -1,

        Stand,
        Run,
        Jump,
        Climb,
        Dead,
    }

    public enum NpcStateType
    {
        Invalid = -1,

        Stand,
        Walk,
        Jump,
        Dead
    }

    public enum Dir
    {
        Left,
        Right,
        Up,
        Down
    }

    public static class GameDefine
    {
        public static readonly int GameFps = 18;    // 一个像素游戏，18帧/秒不过分吧。。
        public static readonly int CollectCount = 1;
        public static readonly float Epsilon = 0.0000001f;      // 为什么要自定义呢，因为编译手机的话，在mono的Epsilon是0.这是一个bug。
        public static readonly float PlayerMaxSpeed = 3.7f;
        public static readonly float JumpForce = 73000f;

        public static readonly float LineMaxLimit = 2.5f;
        public static readonly float LineMinLimit = 0.5f;
        public static readonly float LineSize = 0.27f;
        public static readonly float DisableLineLiveTime = 0.001f;
        public static readonly float ForbiddenLineAddLen = 0.6f;
        public static readonly float ForbiddenLineAddWidth = 0.6f;
		public static readonly float StairsSlopeHeight = 0.1f;

        public static readonly float NpcMoveSpeed = 1.3f;
        public static readonly float NpcJumpForce = 2000.0f;

        public static readonly float MovePlatformSpeed = 0.03f;

        // 大炮相关
        public static readonly float CannonBulletSpeed = 4.0f;
        public static readonly float CannonBulletCreateInterval = 2.0f;
        public static readonly int CannonBulletDestoryCount = 1;

        // 时间控制相关
        public static readonly float RecordInterval = 0.02f;

        // 门的移动范围
        public static readonly float OpenDoorMoveHigh = 1f; // 从初始化的位置沿着Y轴向上移动的距离
        public static readonly float OpenDoorMoveLow = 1f;  // 从初始化的位置沿着Y轴向下移动的距离

        // 钥匙重生时间
        public static readonly float OpenKeyReviveTime = 7f;

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

        public static T GetChildByName<T>(this GameObject go, string name) where T : Component
        {
            var child = GetChildByName(go, name);
            if (child == null)
                return default(T);

            return child.gameObject.GetComponent<T>();
        }
        public static GameObject GetChildByName(this GameObject go, string name)
        {
            return GetChildByName(go, name, true);
        }

        public static GameObject GetChildByName(GameObject Parent, string ChildName, bool IsDepth)
        {
            Transform Child = null;
            GameObject CurChild = null;
            Transform TempChild = null;

            Child = Parent.transform.FindChild(ChildName);
            if (Child != null)
            {
                CurChild = Child.gameObject;
                return CurChild;
            }
            if (IsDepth == true)
            {
                int ChildNum = Parent.transform.childCount;
                for (int i = 0; i < ChildNum; i++)
                {
                    Child = Parent.transform.GetChild(i);
                    TempChild = Child.FindChild(ChildName);
                    if (TempChild != null)
                    {
                        CurChild = TempChild.gameObject;
                        break;
                    }
                    else
                    {
                        CurChild = GetChildByName(Child.gameObject, ChildName);
                        if (CurChild != null)
                        {
                            break;
                        }
                    }
                }
            }

            return CurChild;
        }

        public static TValue GetValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue defaultValue = default(TValue))
        {
            TValue v = defaultValue;
            if (!dict.TryGetValue(key, out v))
                return defaultValue;

            return v;
        }
        public static void Forecah<TKey, TValue>(this Dictionary<TKey, TValue> dict, System.Action<TKey, TValue> EnumeratorFunc)
        {
            if (dict == null || EnumeratorFunc == null)
                throw new System.ArgumentNullException();

            var i = dict.GetEnumerator();
            while (i.MoveNext())
            {
                EnumeratorFunc(i.Current.Key, i.Current.Value);
            }
        }
    }
}

