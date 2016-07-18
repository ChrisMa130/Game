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
            if (MoveDir == Dir.Left || MoveDir == Dir.Down)
                Speed = -Speed;
        }

        private void UpdateDir()
        {
            MoveDir = MoveDir == Dir.Left ? Dir.Right : MoveDir == Dir.Up ? Dir.Up : Dir.Down;
        }

        void Update()
        {
            UpdateSpeed();
            if (MoveDir == Dir.Left || MoveDir == Dir.Right)
                Rigidbody.velocity = new Vector2(Speed, Rigidbody.velocity.y);
            else
                Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, Speed);
        }
    }
}


