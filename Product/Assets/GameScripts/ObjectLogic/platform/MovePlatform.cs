using System;
using UnityEngine;
using System.Collections;

namespace MG
{
    public class MovePlatform : PlatformBase
    {
        public Dir MoveDir;

        private Rigidbody2D Rigidbody;
        private float Speed;

        private bool BeMove;
        public GameObject[] Partner;

        void Start()
        {
            Rigidbody = gameObject.GetComponent<Rigidbody2D>();
            Speed = Mathf.Abs(GameDefine.MovePlatformSpeed);
            BeMove = true;
        }

        public override void TurnOn(GameObject obj)
        {
            UpdateDir();

            BeMove = true;
        }

        public override void TurnOff(GameObject obj)
        {
            UpdateDir();
            BeMove = true;
        }

        private void UpdateDir()
        {
            switch (MoveDir)
            {
                case Dir.Left:
                    MoveDir = Dir.Right;
                    break;
                case Dir.Right:
                    MoveDir = Dir.Left;
                    break;
                case Dir.Up:
                    MoveDir = Dir.Down;
                    break;
                case Dir.Down:
                    MoveDir = Dir.Up;
                    break;
            }
        }

        void Update()
        {
            if (!BeMove)
                return;

            switch (MoveDir)
            {
                case Dir.Left:
                    Move(Vector2.left * Speed);
                    break;
                case Dir.Right:
                    Move(Vector2.right * Speed);
                    break;
                case Dir.Up:
                    Move(Vector2.up * Speed);
                    break;
                case Dir.Down:
                    Move(Vector2.down * Speed);
                    break;
            }
        }

        void Move(Vector2 pos)
        {
            if (Partner == null)
                return;

            for (int i = 0; i < Partner.Length; i++)
            {
                var o = Partner[i];
                if (o)
                {
                    o.transform.Translate(pos);
                }
            }
        }

        void OnCollisionEnter2D(Collision2D obj)
        {
            if (obj.gameObject.tag == "Building")
            {
                BeMove = false;
            }
            else if (obj.gameObject.tag == "ForbiddenZone" && obj.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                BeMove = false;
            }
        }

//        void OnTriggerStay2D(Collider2D obj)
//        {
//            var platform = obj.gameObject.GetComponent<npcswitcher>();
//            if (platform != null)
//            {
//                Switcher = obj.gameObject;
//                return;
//            }
//
//            Switcher = null;
//        }
    }
}


