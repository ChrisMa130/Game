using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MG
{
    public class MovePlatform : PlatformBase
    {
        public Dir MoveDir;

        private Rigidbody2D Rigidbody;
        private float Speed;

        private bool BeMove;
        public List<GameObject> Partner;
        public bool PlayerStay;

        class UserData
        {
            public float Speed;
            public bool BeMove;
        }

        private Dictionary<int, UserData> UserDataTable;
        // private Stack<UserData> UserDataTable; 

        void Start()
        {
            UserDataTable = new Dictionary<int, UserData>();

            Rigidbody = gameObject.GetComponent<Rigidbody2D>();
            Speed = Mathf.Abs(GameDefine.MovePlatformSpeed);
            BeMove = true;
            PlayerStay = false;

            Init();
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

            if (TimeController.Instance.IsOpTime())
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

            for (int i = 0; i < Partner.Count; i++)
            {
                var o = Partner[i];
                if (o != null)
                {
                    o.transform.Translate(pos);
                }
            }

            if (PlayerStay && GameMgr.Instance.PlayerLogic.MyState.CurrentPlayerState == PlayerStateType.Stand)
            {
                GameMgr.Instance.PlayerObject.transform.Translate(pos);
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

            if (obj.gameObject.tag == "Player")
                PlayerStay = true;
        }

        void OnCollisionExit2D(Collision2D obj)
        {
            if (obj.gameObject.tag == "Player")
                PlayerStay = false;
        }

        protected override void SaveUserData(int frame)
        {
            base.SaveUserData(frame);
            UserData data;

            if (UserDataTable.Count == 0)
            {
                data = new UserData();

                data.BeMove = BeMove;
                data.Speed = Speed;

                UserDataTable.Add(0, data);
                return;
            }

            UserDataTable.TryGetValue(frame, out data);
            if (data != null)
                return;

            data = new UserData
            {
                BeMove = BeMove,
                Speed = Speed
            };

            UserDataTable.Add(frame, data);
        }

        protected override void LoadUserData(int frame)
        {
            base.LoadUserData(frame);

            UserData data = null;
            UserDataTable.TryGetValue(frame, out data);
            if (data == null)
                return;

            BeMove = data.BeMove;
            Speed = data.Speed;
        }

        protected override void ClearUserData()
        {
            base.ClearUserData();
            UserDataTable.Clear();
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


