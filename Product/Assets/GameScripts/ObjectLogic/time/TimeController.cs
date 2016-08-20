using System;
using UnityEngine;
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

        private int LastFrame; // 当前记录的最高帧
        private TimeControllState LastState;

        void Start()
        {
            CurrentFrame = 0;
            CurrentState = TimeControllState.Recording;
            LastState = TimeControllState.Recording;
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
                    DoRecordTime(false);
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

        void DoRecordTime(bool force)
        {
            RecordTimeInterval -= Time.deltaTime;

//            if (!force)
//            {
//                if (RecordTimeInterval > 0)
//                    return;
//
//                RecordTimeInterval = GameDefine.RecordInterval;
//            }

            // 这里不需要补帧，因为当再次执行到这里的时候，时间堆栈已经被清空了
            LastState = TimeControllState.Recording;

            TraversalUnit(o => { o.Record(CurrentFrame); });
            CurrentFrame++;
            LastFrame = CurrentFrame;
        }

        void DoForwardTime()
        {
            // 前进不可以超过最高帧
            if (LastFrame < CurrentFrame)
                return;

            if (LastState == TimeControllState.Rewinding)
            {
                TraversalUnit(o => { o.Forward(CurrentFrame); });
                CurrentFrame++;
            }   
            else
            {
                // CurrentFrame++;
                TraversalUnit(o => { o.Forward(CurrentFrame); });
                CurrentFrame++;
            }

            LastState = TimeControllState.Forward;
        }

        void DoRewindTime()
        {
            if (CurrentFrame < 0)
                return;

            if (LastState == TimeControllState.Forward)
            {
                TraversalUnit(o => { o.Rewind(CurrentFrame); });
                CurrentFrame--;
            }
            else
            {
                CurrentFrame--;
                TraversalUnit(o => { o.Rewind(CurrentFrame); });
            }

            LastState = TimeControllState.Rewinding;
        }

        void DoStopTimeAction()
        {
            // nothing to do.... :)
        }

        void DoReStore()
        {
            CurrentFrame = 0;
            LastFrame = 0;
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
            if (CurrentState == TimeControllState.Recording)
            {
                DoRecordTime(true);
            }

            CurrentState = TimeControllState.Rewinding;
        }

        public void ForwardTime()
        {
            if (CurrentState == TimeControllState.Recording)
                return;

            CurrentState = TimeControllState.Forward;
        }

        public void RecordTime()
        {
            if (CurrentState != TimeControllState.Recording)
                DoReStore();

            CurrentState = TimeControllState.Recording;
        }

        public void Freeze()
        {
            CurrentState = TimeControllState.Freeze;
        }

        public bool IsOpTime()
        {
            if (GameMgr.Instance.IsPause)
                return true;

            if (TimeController.Instance.CurrentState != TimeControllState.Recording)
                return true;

            return false;
        }
    }
}

