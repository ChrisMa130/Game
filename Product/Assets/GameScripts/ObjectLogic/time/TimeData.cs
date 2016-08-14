using UnityEngine;

namespace MG
{
    public enum UnitState
    {
        Create,
        Running,
        Destory,
        Deaded,
    }

    public class TimeData
    {
        // 位置信息
        public Vector3 Position;
        public Vector3 Direction;

        // 创建和销毁
        public UnitState State;

        // TODO 物理信息
        // TODO 动画信息

    }
}
