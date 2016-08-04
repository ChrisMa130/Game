using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MG
{
    public class TimeController : SingletonMonoBehaviour<TimeController>
    {
        public int TimebackDuration = 90;
        public int Accuracy = 2;
        public float ObjectDestroyFlag = 1;
        public float TimeBackSpeed = 1;
        public List<TimeUnit> TheUnits;
        public bool TimebackStart = true;
        public bool DestroyCreatedObj;

        private float TimeAccuracy;
        private int RecordPoint;
        private float LastRecordPoint;

        public enum AccuracyType        // 记录精度
        {
            High,
            Mid,
            Low
        }

        public AccuracyType RecordAccuracy = AccuracyType.Mid;

        void Awake()
        {
            if (!object.ReferenceEquals(Instance, null))
            {
                GameObject.Destroy(Instance);
            }
            Instance = this;

            ObjectDestroyFlag = 1;
            DestroyCreatedObj = true;

            TheUnits = new List<TimeUnit>();

            switch (RecordAccuracy)
            {
                case AccuracyType.High:
                    TimeAccuracy = 0.1f;
                    Accuracy = 10;
                    break;
                case AccuracyType.Mid:
                    TimeAccuracy = 0.5f;
                    Accuracy = 2;
                    break;
                case AccuracyType.Low:
                    TimeAccuracy = 1f;
                    Accuracy = 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            RecordPoint = (TimebackDuration * Accuracy) - 1;
            LastRecordPoint = RecordPoint;
        }

        void Start()
        {
            StartCoroutine(DecRecordPoint());
        }

        public void StartTimeback()
        {
            for (int i = 0; i < TheUnits.Count; i++)
            {
                if (TheUnits[i] != null)
                {
                    TheUnits[i].StartRewind();
                }
            }

            StartCoroutine(SetCheck());
            TimebackStart = true;
        }

        public void StopTimeback()
        {
            for (int i = 0; i < TheUnits.Count; i++)
            {
                if (TheUnits[i] != null)
                    TheUnits[i].RestoreAllState(RecordPoint);
            }

            LastRecordPoint = (TimebackDuration * Accuracy) - 1;
            StopCoroutine(SetCheck());
            RecordPoint = (TimebackDuration * Accuracy) - 1;

            TimebackStart = false;
        }

        void Update()
        {
            if (DestroyCreatedObj)
            {
                if (ObjectDestroyFlag >= 0)
                    ObjectDestroyFlag -= Time.deltaTime;
            }

            // 暂停开关

//            if (Input.GetKeyDown(KeyCode.Y))
//                StopRecord();
//            if (Input.GetKeyDown(KeyCode.U))
//                StartRecord();
        }

        public void AddUnit(TimeUnit unit)
        {
            TheUnits.Add(unit);
        }


        public void FreezeAll()
        {
            for (int i = 0; i < TheUnits.Count; i++)
            {
                var unit = TheUnits[i];
                if (unit != null)
                {
                    unit.SetAllState();
                    unit.Freeze();
                }
            }

            TimebackStart = true;
        }

        public void UnfreezeAll()
        {
            for (int i = 0; i < TheUnits.Count; i++)
            {
                var unit = TheUnits[i];
                if (unit != null)
                {
                    unit.RestoreAllState(RecordPoint);
                }
            }

            TimebackStart = false;
        }

        void FixedUpdate()
        {
            if (TimebackStart)
                return;

            var time = (Time.time - LastRecordPoint)*(Accuracy*TimeBackSpeed);
            for (int i = 0; i < TheUnits.Count; i++)
            {
                var unit = TheUnits[i];
                if (unit != null)
                {
                    unit.Timeback(RecordPoint, time);
                }
            }
        }

        IEnumerator SetCheck()
        {
            for (int i = 0; i < TimebackDuration * Accuracy; i++)
            {
                yield return new WaitForSeconds(Accuracy / TimeBackSpeed);
                if (RecordPoint > 1)
                {
                    for (int j = 0; j < TheUnits.Count; j++)
                    {
                        var unit = TheUnits[i];
                        if (unit != null)
                        {
                            unit.SetCheck(RecordPoint);
                        }
                    }

                    RecordPoint--;
                }
                if (RecordPoint <= 1 || RecordPoint < LastRecordPoint + 2)
                {
                    for (int j = 0; j < TheUnits.Count; j++)
                    {
                        if (TheUnits[j] == null) continue;
                            TheUnits[j].StopAllAnim();
                    }
                }
            }

        }

        IEnumerator DecRecordPoint()
        {
            while (true)
            {
                if (!TimebackStart)
                {
                    LastRecordPoint--;

                    yield return new WaitForSeconds(Accuracy);
                }
                else
                {
                    yield return new WaitForSeconds(0.0001f);
                }
            }
        }
    }
}

