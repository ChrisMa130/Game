using UnityEngine;

namespace MG
{
    public enum UnitState
    {
        Create,
        Running,
        Deaded,
    }

    public class TimeData
    {
        public int Frame;

        // 位置信息
        public Vector3 Position;
        public Vector3 Direction;

        // 创建和销毁
        public UnitState State;

        // TODO 物理信息
        public Vector2 Velocity;
        public float gravityScale;
        public float angularVelocity;
        public RigidbodyConstraints2D constraints;

        // TODO 动画信息

    }
}
