using UnityEngine;
using System.Collections.Generic;

namespace MG
{
    public class MovePlatform : PlatformBase
    {
        public Dir MoveDir;
        public bool Parent;
        private float Speed;

        public List<GameObject> Partner;
        public bool PlayerStay;

        public float MySpeed;

        private Vector3 LeftCheckPoint;
        private Vector3 RightCheckPoint;
		private Vector3 UpCheckPoint;
		private Vector3 DownCheckPoint;
        BoxCollider2D b2c;

        class UserData : TimeUnitUserData
        {
            public float Speed;
            public bool BeMove;
        }

        void Start()
        {
            b2c = GetComponent<BoxCollider2D>();

            Speed = Mathf.Abs(GameDefine.MovePlatformSpeed);
            if (MySpeed > 0.0f)
                Speed = Mathf.Abs(MySpeed);

            PlayerStay = false;
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

        void Update()
        {
            if (TimeController.Instance.IsOpTime())
                return;

            if (CanMove())
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
            if (obj.gameObject.tag == "ForbiddenZone" && obj.gameObject.layer == LayerMask.NameToLayer("Player") && Parent)
                obj.gameObject.transform.parent = transform;
        }

        void OnCollisionExit2D(Collision2D obj)
        {
            if (obj.gameObject.tag == "ForbiddenZone" && obj.gameObject.layer == LayerMask.NameToLayer("Default"))
                obj.gameObject.transform.GetComponent<Rigidbody2D>().velocity += new Vector2(10, 0);
            if (obj.gameObject.tag == "ForbiddenZone")
                obj.transform.parent = null;
        }

        protected override TimeUnitUserData GetUserData()
        {
            var data = new UserData
            {
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

            Speed = d.Speed;

            Rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        bool CanMove()
        {
            RaycastHit2D[] hit;

            LeftCheckPoint = transform.position;
            RightCheckPoint = transform.position;
			UpCheckPoint = transform.position;
			DownCheckPoint = transform.position;

            LeftCheckPoint.x = LeftCheckPoint.x - b2c.size.x / 2 - 0.02f;
            RightCheckPoint.x = RightCheckPoint.x + b2c.size.x / 2 + 0.02f;
			UpCheckPoint.y = UpCheckPoint.y + b2c.size.y / 2 + 0.02f;
			DownCheckPoint.y = DownCheckPoint.y - b2c.size.y / 2 - 0.02f;

			Vector2 offsetH = new Vector2 (0.03f, b2c.size.y - 0.01f);
			Vector2 offsetV = new Vector2 (b2c.size.x - 0.01f, 0.03f);

			if (MoveDir == Dir.Left || MoveDir == Dir.Right) {
				if (MoveDir == Dir.Left) {
					hit = Physics2D.BoxCastAll (LeftCheckPoint, offsetH, 0f, Vector2.left);
					Debug.DrawRay (LeftCheckPoint, Vector3.left, Color.blue, offsetH.x);
				} else {
					hit = Physics2D.BoxCastAll (RightCheckPoint, offsetH, 0f, Vector2.right);
					Debug.DrawRay (RightCheckPoint, Vector3.right, Color.blue, offsetH.x);
				}
			} else {
				if (MoveDir == Dir.Up) {
					hit = Physics2D.BoxCastAll (UpCheckPoint, offsetV, 0f,Vector2.up);
					Debug.DrawRay (UpCheckPoint, Vector3.up, Color.blue, offsetV.y);
				} else {
					hit = Physics2D.BoxCastAll (DownCheckPoint, offsetV, 0f, Vector2.down);
					Debug.DrawRay (DownCheckPoint, Vector3.down, Color.blue, offsetV.y);
				}
			}

            int len = hit.Length;
            if (len == 0)
                return false;

            for (int i = 0; i < len; i++)
            {
                RaycastHit2D h = hit[i];
                if (h.collider == null)
                    break;
                
                GameObject o = h.collider.gameObject;

                if (o.tag.Equals("Building") || o.tag.Equals("ForbiddenZone") && (o.layer == LayerMask.NameToLayer("Ground") || o.layer == LayerMask.NameToLayer("Default")))
                {
                    return true;
                }
            }
            return false;
        }
    }
}


