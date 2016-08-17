using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MG
{
    public enum TimeControllState
    {
        Recording,  // 正在记录
        Rewinding,  // 正在倒退
        Forward,    // 正在前进
        Freeze,     // 啥不干
    }

    public class TimeController : SingletonMonoBehaviour<TimeController>
    {
        /// <summary>
        /// 所有时间单位存储在这里，无论是否已经销毁
        /// </summary>
        private readonly List<TimeUnit> Units = new List<TimeUnit>();

        private float RecordTimeInterval;
        private float CurrentTimeSpeed;

        public TimeControllState CurrentState { get; private set; }
        public float TimeSpeed;

        /// <summary>
        /// 时间关键字，用于同步每个对象每帧应该做的事情
        /// </summary>
        public int CurrentFrame { get; private set; }

        void Start()
        {
            CurrentFrame = 0;
            CurrentState = TimeControllState.Recording;
            TimeSpeed = 0.1f;
        }

        void Update()
        {
            DoTimeAction();
        }

        void DoTimeAction()
        {
            switch (CurrentState)
            {
                case TimeControllState.Recording:
                    DoRecordTime();
                    break;
                case TimeControllState.Forward:
                    DoForwardTime();
                    break;
                case TimeControllState.Freeze:
                    DoStopTimeAction();
                    break;
                case TimeControllState.Rewinding:
                    DoRewindTime();
                    break;

            }
        }

        void DoRecordTime()
        {
            RecordTimeInterval -= Time.deltaTime;

            if (RecordTimeInterval > 0)
                return;

            RecordTimeInterval = GameDefine.RecordInterval;

            TraversalUnit(o => { o.Record(CurrentFrame); });
            CurrentFrame++;
        }

        void DoForwardTime()
        {
            CurrentTimeSpeed -= Time.deltaTime;

            if (CurrentTimeSpeed > 0)
                return;

            CurrentTimeSpeed = TimeSpeed;
            TraversalUnit(o => { o.Forward(CurrentFrame); });
            CurrentFrame++;
        }

        void DoRewindTime()
        {
            CurrentTimeSpeed -= Time.deltaTime;

            if (CurrentTimeSpeed > 0)
                return;

            CurrentTimeSpeed = TimeSpeed;

            TraversalUnit(o => { o.Rewind(CurrentFrame); });
            CurrentFrame--;
        }

        void DoStopTimeAction()
        {
            // nothing to do.... :)
        }

        void DoReStore()
        {
            TraversalUnit(o => { o.Restore(); });
        }

        void TraversalUnit(Action<TimeUnit> OnHit)
        {
            if (OnHit == null)
                return;

            for (int i = 0; i < Units.Count; i++)
            {
                var u = Units[i];
                if (u != null)
                    OnHit(u);
            }
        }

        public void AddUnit(TimeUnit unit)
        {
            Units.Add(unit);
        }

        public void RewindTime()
        {
            CurrentState = TimeControllState.Rewinding;
        }

        public void ForwardTime()
        {
            CurrentState = TimeControllState.Forward;
        }

        public void RecordTime()
        {
            if (CurrentState != TimeControllState.Recording)
                DoReStore();

            CurrentState = TimeControllState.Recording;
        }
    }
}

