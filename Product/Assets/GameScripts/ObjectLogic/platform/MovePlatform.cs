using UnityEngine;
using System.Collections;

namespace MG
{
    public class MovePlatform : PlatformBase
    {
        public Dir MoveDir;

        private Rigidbody2D Rigidbody;
        private float Speed;

        void Start()
        {
            Rigidbody = gameObject.GetComponent<Rigidbody2D>();

            UpdateSpeed();
        }

        public override void TurnOn(GameObject obj)
        {
            UpdateDir();
        }

        public override void TurnOff(GameObject obj)
        {
            UpdateDir();
        }

        private void UpdateSpeed()
        {
            Speed = Mathf.Abs(GameDefine.MovePlatformSpeed);
            if (MoveDir == Dir.Left)
                Speed = -Speed;
        }

        private void UpdateDir()
        {
            MoveDir = MoveDir == Dir.Left ? Dir.Right : Dir.Left;
        }

        void Update()
        {
            UpdateSpeed();
            Rigidbody.velocity = new Vector2(Speed, Rigidbody.velocity.y);
        }
    }
}


