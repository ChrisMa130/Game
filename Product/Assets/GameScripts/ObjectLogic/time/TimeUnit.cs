using UnityEngine;
using System.Collections.Generic;

namespace MG
{
    public class TimeUnitUserData {}

    // 记录每帧的各种数据，回退，暂停记录和前进时间。
    public class TimeUnit : MonoBehaviour
    {
        private Stack<TimeData> FrameTimeData;
        private Stack<TimeData> ForwardTimeData;
        private TimeData CreateData;
        private Rigidbody2D Rigid;
        private Animator Anim;

        private Dictionary<int, TimeUnitUserData> UserDataTable;

        protected void Init()
        {
            Rigid = gameObject.GetComponent<Rigidbody2D>();
            Anim = gameObject.GetComponent<Animator>();

            FrameTimeData = new Stack<TimeData>();
            ForwardTimeData = new Stack<TimeData>();
            UserDataTable = new Dictionary<int, TimeUnitUserData>();

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

            if (Anim != null)
            {
                Anim.Play(data.AnimHash, 0, data.AnimTime);
            }

            LoadUserData(frame);
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

            if (Rigid != null)
            {
                // Rigid.isKinematic = true;
                Rigid.velocity = Vector2.zero;
                Rigid.gravityScale = 0;
                Rigid.angularVelocity = 0;
            }

            if (Anim != null)
            {
                Anim.Play(data.AnimHash, 0, data.AnimTime);
            }

            LoadUserData(frame);
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

            ClearUserData();
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

            // 动画部分
            if (Anim != null)
            {
				if (Anim.gameObject.tag.Equals ("Player")) {                //TEMP!!!! ERASE WHEN ANIMATION IS FINISHED
					var state = Anim.GetCurrentAnimatorStateInfo (0);
					data.AnimHash = state.fullPathHash;
					data.AnimTime = state.normalizedTime;
				}
            }

            // 自主数据
            SaveUserData(frame);

            return data;
        }

        void SaveUserData(int frame)
        {
            TimeUnitUserData data;

            if (UserDataTable.Count == 0)
            {
                data = GetUserData();
                UserDataTable.Add(0, data);
                return;
            }

            UserDataTable.TryGetValue(frame, out data);
            if (data != null)
                return;

            data = GetUserData();
            UserDataTable.Add(frame, data);
        }

        void LoadUserData(int frame)
        {
            TimeUnitUserData data = null;
            UserDataTable.TryGetValue(frame, out data);
            if (data == null)
                return;

            SetUserData(data);
        }

        void ClearUserData()
        {
            UserDataTable.Clear();
        }

        protected virtual TimeUnitUserData GetUserData()
        {
            return null;
        }

        protected virtual void SetUserData(TimeUnitUserData data)
        {
        }
    }
}


