using UnityEngine;

namespace MG
{
    public class OpenDoor : TimeUnit
    {
        public Dir CurrentDir = Dir.Up;
        public float MoveSpeed = 0.1f;
        public float KeepOpenTime = 2f;

        private float MoveHighPos;
        private float MoveLowPos;

        private bool CanMove;

        private float KeepTime = -1f;

        private bool HasLine;
        private GameObject Line;

        class UserData : TimeUnitUserData
        {
            public Dir CurrentDir;
            public bool CanMove;
            public float KeepTime = -1f;
        }

        void Start()
        {
            MoveHighPos = transform.position.y + GameDefine.OpenDoorMoveHigh;
            MoveLowPos = transform.position.y;
			CanMove = true;
            HasLine = false;
            
            Init(false);
        }

        void Update()
        {
            if (!CanMove)
            {
                if (HasLine && Line == null)
                {
                    CanMove = true;
                    HasLine = false;
                }
                else
                    return;
            }

            // 上下移动。并到达位置后，开始计时。计时结束后往反方向移动。
            if (TimeController.Instance.IsOpTime())
                return;

            // 移动部分
            if (KeepTime < 0 && CurrentDir == Dir.Up && transform.position.y < MoveHighPos)
            {
				transform.Translate(Vector3.up * MoveSpeed);
                return;
            }

            if (CurrentDir == Dir.Down && transform.position.y > MoveLowPos)
            {
                transform.Translate(Vector3.down * MoveSpeed);
            }
            else if (KeepTime < 0)
            {
                KeepTime = KeepOpenTime;
            }
            else
            {
                KeepTime -= Time.deltaTime;
                if (KeepTime < 0)
                {
                    CurrentDir = Dir.Up;
                }
            }
        }

        public void OpenTheDoor()
        {
            CurrentDir = Dir.Down;
        }

        void OnCollisionEnter2D(Collision2D obj)
        {
            if (obj.gameObject.tag == "Building")
            {
                CanMove = false;
            }
            else if (obj.gameObject.tag == "ForbiddenZone" && obj.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                Line = obj.gameObject;
                HasLine = true;
                CanMove = false;
            }
        }

        void OnCollisionExit2D(Collision2D obj)
        {
            if (obj.gameObject.tag == "Building")
            {
                CanMove = true;
            }
            else if (obj.gameObject.tag == "ForbiddenZone" && obj.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                CanMove = true;
            }
        }

        protected override TimeUnitUserData GetUserData()
        {
            UserData data = new UserData
            {
                CurrentDir = CurrentDir,
                CanMove = CanMove,
                KeepTime = KeepTime
            };

            Rigid.constraints = RigidbodyConstraints2D.FreezeAll;
            return data;
        }

        protected override void SetUserData(TimeUnitUserData data)
        {
            UserData d = data as UserData;
            if (d == null)
                return;

            CurrentDir = d.CurrentDir;
            CanMove = d.CanMove;
            KeepTime = d.KeepTime;

            Rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
}
