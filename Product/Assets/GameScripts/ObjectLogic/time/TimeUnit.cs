using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.Assertions;

namespace MG
{
    // 记录每帧的各种数据，回退，暂停记录和前进时间。
    public class TimeUnit : MonoBehaviour
    {
        private Stack<TimeData> FrameTimeData;
        private Stack<TimeData> ForwardTimeData;
        private TimeData CreateData;
        private Rigidbody2D Rigid;

        protected void Init()
        {
            Rigid = gameObject.GetComponent<Rigidbody2D>();

            FrameTimeData = new Stack<TimeData>();
            ForwardTimeData = new Stack<TimeData>();

            CreateData = Snapshot(null, TimeController.Instance.CurrentFrame, true);
            FrameTimeData.Push(CreateData);

            TimeController.Instance.AddUnit(this);
        }

        // 记录当前帧
        public void Record(int frame)
        {
            if (FrameTimeData.Count == 0)
            {
                var d = Snapshot(null, frame, false);
                FrameTimeData.Push(d);
                return;
            }

            var prevShot = FrameTimeData.Peek();
            if (prevShot.Frame == frame)
                return;

            var oneShot = Snapshot(prevShot, frame, false);
            FrameTimeData.Push(oneShot);
        }

        // 时间倒退
        public void Rewind(int frame)
        {
            if (FrameTimeData.Count == 0)
                return;

            var prev = FrameTimeData.Peek();
            if (frame != prev.Frame)
                return;

            var data = FrameTimeData.Pop();
            ForwardTimeData.Push(data);

            // 销毁和创建处理
            switch (data.State)
            {
                case UnitState.Deaded:
                    gameObject.SetActive(false);
                    return;
                case UnitState.Running:
                    gameObject.SetActive(true);
                    break;
                case UnitState.Create:
                    gameObject.SetActive(false);
                    Debug.Log("gameObject.SetActive(false);");
                    break;
            }

            transform.position = transform.position - data.Direction;

            // 设置物理属性
            if (Rigid != null)
            {
                // Rigid.isKinematic = true;
                Rigid.velocity = Vector2.zero;
                Rigid.gravityScale = 0;
                Rigid.angularVelocity = 0;
            }
        }

        // 前进
        public void Forward(int frame)
        {
            if (ForwardTimeData.Count == 0)
            {
                return;
            }

            // 轨迹设置
            var prev = ForwardTimeData.Peek();
            if (prev.Frame != frame)
                return;

            var data = ForwardTimeData.Pop();
            FrameTimeData.Push(data);

            switch (data.State)
            {
                case UnitState.Deaded:
                    gameObject.SetActive(false);
                    return;
                case UnitState.Running:
                    break;
                case UnitState.Create:
                    gameObject.SetActive(true);
                    break;
            }

            transform.position = transform.position + data.Direction;

            // TODO 物理属性
            if (Rigid != null)
            {
                // Rigid.isKinematic = true;
                Rigid.velocity = Vector2.zero;
                Rigid.gravityScale = 0;
                Rigid.angularVelocity = 0;
            }
        }

        public void Freeze()
        {
        }

        public void Restore()
        {
            if (Rigid != null && CreateData != null)
            {
                TimeData data = FrameTimeData.Count > 0 ? FrameTimeData.Peek() : CreateData;
                Rigid.velocity = data.Velocity;
                Rigid.gravityScale = data.gravityScale;
                Rigid.angularVelocity = data.angularVelocity;
            }
        }

        // TODO
        public void DestoryMe()
        {
            gameObject.SetActive(false);
        }

        public bool IsDead()
        {
            return gameObject.activeInHierarchy;
        }

        private TimeData Snapshot(TimeData PrevData, int frame, bool bCreate)
        {
            TimeData data = new TimeData();
            data.Frame = frame;

            if (PrevData != null && !gameObject.activeSelf)
            {
                data.State = UnitState.Deaded;
                return data;
            }

            data.State = bCreate ? UnitState.Create : UnitState.Running;

            // 记录移动方向和距离
            var pos = transform.position;
            var prevPos = PrevData != null ? PrevData.Position : transform.position;
            var dist = Vector3.Distance(pos, prevPos);//B - A
            var dir = (pos - prevPos).normalized;

            data.Direction = dist*dir;
            data.Position = pos;

            // 物理部分
            if (Rigid != null)
            {
                data.Velocity = Rigid.velocity;
                data.angularVelocity = Rigid.angularVelocity;
                data.gravityScale = Rigid.gravityScale;
            }

            // 自主数据
            SaveUserData(frame);

            return data;
        }

        protected virtual void SaveUserData(int frame)
        {
            
        }

        protected virtual void LoadUserData(int frame)
        {
            
        }

        protected virtual void ClearUserData()
        {

        }
        //        public TimeData[] RtData;
        //        public TimeController Controller;
        //
        //        private float Accuracy;
        //
        //        private List<string> AnimNames;
        //        private float[] AnimSpeed;
        //        private Rigidbody MyRigidbody;
        //        private Animation MyAnimation;
        //
        //        void Start()
        //        {
        //            MyRigidbody = gameObject.GetComponent<Rigidbody>();
        //            MyAnimation = gameObject.GetComponent<Animation>();
        //
        //            Controller = TimeController.Instance;
        //            Controller.AddUnit(this);
        //
        //            Init();
        //
        //            StartCoroutine(SaveState());
        //        }
        //
        //        public void Init()
        //        {
        //            AnimNames = new List<string>();
        //
        //            int RtDataLen = Controller.TimebackDuration * Controller.Accuracy;
        //            RtData = new TimeData[RtDataLen];
        //            for (int i = 0; i < RtDataLen; i++)
        //            {
        //                RtData[i] = new TimeData();
        //            }
        //
        //            if (Controller.ObjectDestroyFlag <= 0)
        //                RtData[RtDataLen - 1].Instantiated = true;
        //
        //            switch (Controller.RecordAccuracy)
        //            {
        //                case TimeController.AccuracyType.High:
        //                    Accuracy = 0.1f;
        //                    break;
        //                case TimeController.AccuracyType.Mid:
        //                    Accuracy = 0.5f;
        //                    break;
        //                case TimeController.AccuracyType.Low:
        //                    Accuracy = 1f;
        //                    break;
        //            }
        //
        //            if (MyAnimation != null)
        //            {
        //                foreach (AnimationState state in MyAnimation)
        //                {
        //                    AnimNames.Add(state.name);
        //                }
        //
        //                AnimSpeed = new float[AnimNames.Count];
        //                for (int i = 0; i < AnimSpeed.Length; i++)
        //                {
        //                    AnimSpeed[i] = MyAnimation[AnimNames[i]].speed;
        //                }
        //            }
        //
        //            SetActive();
        //        }
        //
        //        public void SetActive()
        //        {
        //            for (int i = 0; i < RtData.Length; i++)
        //            {
        //                RtData[i].Position = gameObject.transform.position;
        //                RtData[i].Rotation = gameObject.transform.rotation;
        //                RtData[i].Scale = gameObject.transform.localScale;
        //            }
        //
        //            if (MyRigidbody != null)
        //            {
        //                for (int i = 0; i < RtData.Length; i++)
        //                {
        //                    RtData[i].RigAngVelocity = MyRigidbody.angularVelocity;
        //                    RtData[i].RigVelocity = MyRigidbody.velocity;
        //                    RtData[i].RigMagnitue = MyRigidbody.velocity.magnitude;
        //                }
        //            }
        //        }
        //
        //        public void StartRewind()
        //        {
        //            SetAllState();
        //
        //            if (MyRigidbody != null)
        //                MyRigidbody.isKinematic = true;
        //
        //            if (MyAnimation != null)
        //            {
        //                for (int i = 0; i < AnimSpeed.Length; i++)
        //                {
        //                    var name = AnimNames[i];
        //                    MyAnimation[name].speed = -AnimSpeed[i] * Controller.TimeBackSpeed;
        //                }
        //            }
        //        }
        //
        //        public void SetAllState()
        //        {
        //            int slot = RtData.Length - 1;
        //            RtData[slot].Position = gameObject.transform.position;
        //            RtData[slot].Rotation = gameObject.transform.rotation;
        //            RtData[slot].Scale = gameObject.transform.localScale;
        //
        //            if (MyRigidbody != null)
        //            {
        //                RtData[slot].RigAngVelocity = MyRigidbody.angularVelocity;
        //                RtData[slot].RigVelocity = MyRigidbody.velocity;
        //                RtData[slot].RigMagnitue = MyRigidbody.velocity.magnitude;
        //            }
        //        }
        //
        //        public void Freeze()
        //        {
        //            if (MyRigidbody != null)
        //            {
        //                MyRigidbody.isKinematic = true;
        //            }
        //
        //            if (MyAnimation != null)
        //            {
        //                foreach (AnimationState state in MyAnimation)
        //                {
        //                    state.speed = 0;
        //                }
        //            }
        //        }
        //
        //        public void StopAllAnim()
        //        {
        //            string name;
        //            if (MyAnimation != null)
        //            {
        //                for (int i = 0; i < AnimSpeed.Length; i++)
        //                {
        //                    name = AnimNames[i];
        //                    MyAnimation[name].speed = 0;
        //                }
        //            }
        //        }
        //
        //        public void RestoreAllState(int recordPoint)
        //        {
        //            if (MyRigidbody != null)
        //            {
        //                MyRigidbody.isKinematic = false;
        //                MyRigidbody.velocity = RtData[recordPoint].RigVelocity;
        //                MyRigidbody.angularVelocity = RtData[recordPoint].RigAngVelocity;
        //            }
        //
        //            string name;
        //            if (MyAnimation != null)
        //            {
        //                for (int i = 0; i < AnimSpeed.Length; i++)
        //                {
        //                    name = AnimNames[i];
        //                    MyAnimation[name].speed = AnimSpeed[i];
        //                }
        //            }
        //        }
        //
        //        public void Timeback(int point, float time)
        //        {
        //            // 差值计算，平滑回滚
        //            gameObject.transform.position = Vector3.Lerp(RtData[point].Position, RtData[point - 1].Position, time);
        //            gameObject.transform.rotation = Quaternion.Lerp(RtData[point].Rotation, RtData[point - 1].Rotation, time);
        //            gameObject.transform.localScale = Vector3.Lerp(RtData[point].Scale, RtData[point - 1].Scale, time);
        //
        //            if (RtData[point].Instantiated)
        //                Destroy(gameObject);
        //        }
        //
        //        public void SetCheck(int point)
        //        {
        //            var slot = RtData[point];
        //
        //            if (MyAnimation != null && slot.AnimPoint != "")
        //            {
        //                if (slot.AnimationPlayType == TimeData.AnimPlayType.Play)
        //                {
        //                    MyAnimation[slot.AnimPoint].time = MyAnimation[slot.AnimPoint].length;
        //                    MyAnimation.Play(slot.AnimPoint);
        //                }
        //                else if (slot.AnimationPlayType == TimeData.AnimPlayType.CrossFade)
        //                {
        //                    MyAnimation[slot.AnimPoint].time = MyAnimation[slot.AnimPoint].length;
        //                    MyAnimation.CrossFade(slot.AnimPoint);
        //                }
        //            }
        //        }
        //
        //        IEnumerator SaveState()
        //        {
        //            while (true)
        //            {
        //                if (Controller.TimebackStart)
        //                {
        //                    for (int i = 0; i < RtData.Length - 1; i++)
        //                    {
        //                        RtData[i].Position = RtData[i + 1].Position;
        //                        RtData[i].Rotation = RtData[i + 1].Rotation;
        //                        RtData[i].Scale = RtData[i + 1].Scale;
        //
        //                        RtData[i].Instantiated = RtData[i + 1].Instantiated;
        //                        RtData[i + 1].Instantiated = false;
        //
        //                    }
        //
        //                    if (MyRigidbody != null)
        //                    {
        //                        for (int i = 0; i < RtData.Length - 1; i++)
        //                        {
        //                            RtData[i].RigVelocity = RtData[i + 1].RigVelocity;
        //                            RtData[i].RigMagnitue = RtData[i + 1].RigMagnitue;
        //                            RtData[i].RigAngVelocity = RtData[i + 1].RigAngVelocity;
        //                        }
        //                    }
        //
        //                    if (MyAnimation != null)
        //                    {
        //                        for (int i = 0; i < RtData.Length - 1; i++)
        //                        {
        //                            RtData[i].AnimPoint = RtData[i + 1].AnimPoint;
        //                        }
        //                    }
        //                     
        //                    SetAllState();
        //
        //                    yield return new WaitForSeconds(Accuracy);
        //                }
        //                else
        //                {
        //                    yield return new WaitForSeconds(0.0001f);
        //                }
        //            }
        //        }
    }
}


