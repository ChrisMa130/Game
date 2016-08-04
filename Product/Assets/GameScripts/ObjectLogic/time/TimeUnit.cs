using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MG
{
    public class TimeUnit : MonoBehaviour
    {
        public TimeData[] RtData;
        public TimeController Controller;

        private float Accuracy;

        private List<string> AnimNames;
        private float[] AnimSpeed;
        private Rigidbody MyRigidbody;
        private Animation MyAnimation;

        void Start()
        {
            MyRigidbody = gameObject.GetComponent<Rigidbody>();
            MyAnimation = gameObject.GetComponent<Animation>();

            Controller = TimeController.Instance;
            Controller.AddUnit(this);

            Init();

            StartCoroutine(SaveState());
        }

        public void Init()
        {
            AnimNames = new List<string>();

            int RtDataLen = 90 * 2;
            RtData = new TimeData[RtDataLen];
            for (int i = 0; i < RtDataLen; i++)
            {
                RtData[i] = new TimeData();
            }

            if (Controller.ObjectDestroyFlag <= 0)
                RtData[RtDataLen - 1].Instantiated = true;

            switch (Controller.RecordAccuracy)
            {
                case TimeController.AccuracyType.High:
                    Accuracy = 0.1f;
                    break;
                case TimeController.AccuracyType.Mid:
                    Accuracy = 0.5f;
                    break;
                case TimeController.AccuracyType.Low:
                    Accuracy = 1f;
                    break;
            }

            if (MyAnimation != null)
            {
                foreach (AnimationState state in MyAnimation)
                {
                    AnimNames.Add(state.name);
                }

                AnimSpeed = new float[AnimNames.Count];
                for (int i = 0; i < AnimSpeed.Length; i++)
                {
                    AnimSpeed[i] = MyAnimation[AnimNames[i]].speed;
                }
            }
        }

        public void SetActive()
        {
            for (int i = 0; i < RtData.Length; i++)
            {
                RtData[i].Position = gameObject.transform.position;
                RtData[i].Rotation = gameObject.transform.rotation;
                RtData[i].Scale = gameObject.transform.localScale;
            }

            if (MyRigidbody != null)
            {
                for (int i = 0; i < RtData.Length; i++)
                {
                    RtData[i].RigAngVelocity = MyRigidbody.angularVelocity;
                    RtData[i].RigVelocity = MyRigidbody.velocity;
                    RtData[i].RigMagnitue = MyRigidbody.velocity.magnitude;
                }
            }
        }

        public void StartRewind()
        {
            SetAllState();

            if (MyRigidbody != null)
                MyRigidbody.isKinematic = true;

            if (MyAnimation != null)
            {
                for (int i = 0; i < AnimSpeed.Length; i++)
                {
                    var name = AnimNames[i];
                    MyAnimation[name].speed = -AnimSpeed[i] * Controller.TimeBackSpeed;
                }
            }
        }

        public void SetAllState()
        {
            int slot = RtData.Length - 1;
            RtData[slot].Position = gameObject.transform.position;
            RtData[slot].Rotation = gameObject.transform.rotation;
            RtData[slot].Scale = gameObject.transform.localScale;

            if (MyRigidbody != null)
            {
                RtData[slot].RigAngVelocity = MyRigidbody.angularVelocity;
                RtData[slot].RigVelocity = MyRigidbody.velocity;
                RtData[slot].RigMagnitue = MyRigidbody.velocity.magnitude;
            }
        }

        public void Freeze()
        {
            if (MyRigidbody != null)
            {
                MyRigidbody.isKinematic = true;
            }

            if (MyAnimation != null)
            {
                foreach (AnimationState state in MyAnimation)
                {
                    state.speed = 0;
                }
            }
        }

        public void StopAllAnim()
        {
            string name;
            if (MyAnimation != null)
            {
                for (int i = 0; i < AnimSpeed.Length; i++)
                {
                    name = AnimNames[i];
                    MyAnimation[name].speed = 0;
                }
            }
        }

        public void RestoreAllState(int recordPoint)
        {
            if (MyRigidbody != null)
            {
                MyRigidbody.isKinematic = false;
                MyRigidbody.velocity = RtData[recordPoint].RigVelocity;
                MyRigidbody.angularVelocity = RtData[recordPoint].RigAngVelocity;
            }

            string name;
            if (MyAnimation != null)
            {
                for (int i = 0; i < AnimSpeed.Length; i++)
                {
                    name = AnimNames[i];
                    MyAnimation[name].speed = AnimSpeed[i];
                }
            }
        }

        public void Timeback(int point, float time)
        {
            // 差值计算，平滑回滚
            gameObject.transform.position = Vector3.Lerp(RtData[point].Position, RtData[point - 1].Position, time);
            gameObject.transform.rotation = Quaternion.Lerp(RtData[point].Rotation, RtData[point - 1].Rotation, time);
            gameObject.transform.localScale = Vector3.Lerp(RtData[point].Scale, RtData[point - 1].Scale, time);

            if (RtData[point].Instantiated)
                Destroy(gameObject);
        }

        public void SetCheck(int point)
        {
            var slot = RtData[point];

            if (MyAnimation != null && slot.AnimPoint != "")
            {
                if (slot.AnimationPlayType == TimeData.AnimPlayType.Play)
                {
                    MyAnimation[slot.AnimPoint].time = MyAnimation[slot.AnimPoint].length;
                    MyAnimation.Play(slot.AnimPoint);
                }
                else if (slot.AnimationPlayType == TimeData.AnimPlayType.CrossFade)
                {
                    MyAnimation[slot.AnimPoint].time = MyAnimation[slot.AnimPoint].length;
                    MyAnimation.CrossFade(slot.AnimPoint);
                }
            }
        }

        IEnumerator SaveState()
        {
            while (true)
            {
                if (Controller.TimebackStart)
                {
                    for (int i = 0; i < RtData.Length - 1; i++)
                    {
                        RtData[i].Position = RtData[i + 1].Position;
                        RtData[i].Rotation = RtData[i + 1].Rotation;
                        RtData[i].Scale = RtData[i + 1].Scale;

                        RtData[i].Instantiated = RtData[i + 1].Instantiated;
                        RtData[i + 1].Instantiated = false;

                    }

                    if (MyRigidbody != null)
                    {
                        for (int i = 0; i < RtData.Length - 1; i++)
                        {
                            RtData[i].RigVelocity = RtData[i + 1].RigVelocity;
                            RtData[i].RigMagnitue = RtData[i + 1].RigMagnitue;
                            RtData[i].RigAngVelocity = RtData[i + 1].RigAngVelocity;
                        }
                    }

                    if (MyAnimation != null)
                    {
                        for (int i = 0; i < RtData.Length - 1; i++)
                        {
                            RtData[i].AnimPoint = RtData[i + 1].AnimPoint;
                        }
                    }

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


