using UnityEngine;

namespace MG
{
    public class TimeData
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Scale;
        public Vector3 RigVelocity;
        public Vector3 RigAngVelocity;
        public float RigMagnitue;

        public string AnimPoint;
        public enum AnimPlayType
        {
            Play,
            CrossFade
        }
        public AnimPlayType AnimationPlayType;
        public bool Instantiated; // 是否被实例化
    }
}
