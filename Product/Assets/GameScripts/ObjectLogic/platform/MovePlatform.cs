using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MG
{
    public class MovePlatform : PlatformBase
    {
        public Dir MoveDir;
		public bool Parent;
        //private Rigidbody2D Rigidbody;
        private float Speed;

        private bool BeMove;
        public List<GameObject> Partner;
        public bool PlayerStay;

		public float MySpeed;

        private GameObject Line;
        bool HasLine;

        class UserData : TimeUnitUserData
        {
            public float Speed;
            public bool BeMove;
        }

        void Start()
        {
            //Rigidbody = gameObject.GetComponent<Rigidbody2D>();
            Speed = Mathf.Abs(GameDefine.MovePlatformSpeed);
			if (MySpeed > 0.0f)
				Speed = Mathf.Abs (MySpeed);
            BeMove = true;
            HasLine = false;
            PlayerStay = false;
            Init(false);
        }

        public override void TurnOn(GameObject obj)
        {
            UpdateDir();
            if (!HasLine && Line == null)
                BeMove = true;
            
        }

        public override void TurnOff(GameObject obj)
        {
            UpdateDir();
            if (!HasLine && Line == null)
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
            {
                if (HasLine && Line == null)
                {
                    BeMove = true;
                    HasLine = false;
                }
                else
                    return;
            }

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

            //if (PlayerStay && GameMgr.Instance.PlayerLogic.MyState.CurrentPlayerState == PlayerStateType.Stand)
            //{
            //    GameMgr.Instance.PlayerObject.transform.Translate(pos);
            //}
        }

        void OnCollisionEnter2D(Collision2D obj)
        {
            if (obj.gameObject.tag == "Building")
            {
                BeMove = false;
            }
			else if (obj.gameObject.tag == "ForbiddenZone" && (obj.gameObject.layer == LayerMask.NameToLayer("Ground") || obj.gameObject.layer == LayerMask.NameToLayer("Default")))
            {
                Line = obj.gameObject;
                HasLine = true;
				BeMove = false;
            }

            //if (obj.gameObject.tag == "Player")
            //    PlayerStay = true;
			if (obj.gameObject.tag == "ForbiddenZone" && obj.gameObject.layer == LayerMask.NameToLayer("Player") && Parent)
				obj.gameObject.transform.parent = transform;
//			if (obj.gameObject.tag == "NPC") {
//				obj.gameObject.transform.parent = transform;
//				obj.gameObject.transform.GetComponent<Rigidbody2D> ().velocity -= new Vector2 (10, 0);
//			}
        }

        void OnCollisionExit2D(Collision2D obj)
        {
            if (obj.gameObject.tag == "Building")
            {
                BeMove = true;
            }
            else if (obj.gameObject.tag == "ForbiddenZone" && obj.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                BeMove = true;
            }

            //if (obj.gameObject.tag == "Player")
            //    PlayerStay = false;
			if (obj.gameObject.tag == "ForbiddenZone" && obj.gameObject.layer == LayerMask.NameToLayer("Default"))
				obj.gameObject.transform.GetComponent<Rigidbody2D> ().velocity += new Vector2 (10, 0);
			if (obj.gameObject.tag == "ForbiddenZone")
				obj.transform.parent = null;
        }

        protected override TimeUnitUserData GetUserData()
        {
            var data = new UserData
            {
                BeMove = BeMove,
                Speed = Speed
            };

			Rigid.constraints = RigidbodyConstraints2D.FreezeAll;
            return data;
        }

        protected override void SetUserData(TimeUnitUserData data)
        {
            UserData d = data as UserData;
            if (d == null)
                return;

            BeMove = d.BeMove;
            Speed = d.Speed;

			Rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
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


