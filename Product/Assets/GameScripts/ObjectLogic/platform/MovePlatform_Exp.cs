using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MG
{
    public class MovePlatform_Exp : PlatformBase
    {
        public Dir MoveDir;
		public bool Parent;
        private float Speed;

        public List<GameObject> Partner;

		public float MySpeed;

		private GameObject clone;

        class UserData : TimeUnitUserData
        {
			public Dir MoveDir;
            public float Speed;
        }

        void Start()
        {
            Speed = Mathf.Abs(GameDefine.MovePlatformSpeed);
			if (MySpeed > 0.0f)
				Speed = Mathf.Abs (MySpeed);
			Partner = new List<GameObject> ();
			Partner.Add (gameObject);
			if (Parent) {
				clone = new GameObject ();
				clone.transform.position = transform.position;
			}
            Init(false);
        }

        public override void TurnOn(GameObject obj)
        {
            UpdateDir();
        }

        public override void TurnOff(GameObject obj)
        {
            UpdateDir();
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

        void FixedUpdate()
        {
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
					GetComponent<Rigidbody2D> ().velocity = pos;
					if (clone != null) {
						clone.transform.position = transform.position;
					}
                }
            }
        }

        void OnCollisionEnter2D(Collision2D obj)
        {
			if (obj.gameObject.tag == "ForbiddenZone" && obj.gameObject.layer == LayerMask.NameToLayer ("Player") && Parent) {
				obj.gameObject.transform.parent = clone.transform;

			}
        }

        void OnCollisionExit2D(Collision2D obj)
        {
			if (obj.gameObject.tag == "ForbiddenZone" && obj.gameObject.layer == LayerMask.NameToLayer("Default"))
				obj.gameObject.transform.GetComponent<Rigidbody2D> ().velocity += new Vector2 (10, 0);
			if (obj.gameObject.tag == "ForbiddenZone")
				obj.transform.parent = null;
        }

        protected override TimeUnitUserData GetUserData()
        {
            var data = new UserData
            {
                Speed = Speed
            };
					
            return data;
        }

        protected override void SetUserData(TimeUnitUserData data)
        {
            UserData d = data as UserData;
            if (d == null)
                return;
            Speed = d.Speed;
        }
    }
}


