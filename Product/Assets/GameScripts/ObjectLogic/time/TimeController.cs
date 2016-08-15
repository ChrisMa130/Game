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

        void Start()
        {
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

            TraversalUnit(o => { o.Record(); });
        }

        void DoForwardTime()
        {
            CurrentTimeSpeed -= Time.deltaTime;

            if (CurrentTimeSpeed > 0)
                return;

            CurrentTimeSpeed = TimeSpeed;

            TraversalUnit(o => { o.Forward(); });
        }

        void DoRewindTime()
        {
            CurrentTimeSpeed -= Time.deltaTime;

            if (CurrentTimeSpeed > 0)
                return;

            CurrentTimeSpeed = TimeSpeed;

            TraversalUnit(o => { o.Rewind(); });
        }

        void DoStopTimeAction()
        {
            // nothing to do.... :)
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
            Debug.Log("Rewind");
            CurrentState = TimeControllState.Rewinding;
        }

        public void ForwardTime()
        {
            CurrentState = TimeControllState.Forward;
        }

        public void RecordTime()
        {
            CurrentState = TimeControllState.Recording;
        }
    }
}

