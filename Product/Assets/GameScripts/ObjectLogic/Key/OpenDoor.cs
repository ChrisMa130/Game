﻿using UnityEngine;

namespace MG
{
    public class OpenDoor : TimeUnit
    {
        public float MoveSpeed = 0.1f;
        public float KeepOpenTime = 2f;
		public bool Up = true;
        private float KeepTime = -1f;


        class UserData : TimeUnitUserData
        {
            public Dir CurrentDir;
            public float KeepTime = -1f;
        }

        void Start()
        {
            Init(false);
        }

        void Update()
		{

			// 上下移动。并到达位置后，开始计时。计时结束后往反方向移动。
			if (TimeController.Instance.IsOpTime ())
				return;

			if (Up) {
				if (KeepTime > 0f) {
					Rigid.AddForce (new Vector2 (0f, 2000f * MoveSpeed));
					KeepTime -= Time.deltaTime;
				}
			}
			else {
				// 移动部分
				if (KeepTime > 0f) {
                    KeepTime -= Time.deltaTime;
                } else {
                    Rigid.AddForce(new Vector2(0f, 2000f * MoveSpeed));
                }
			}

		}

        public void OpenTheDoor()
        {
			KeepTime = KeepOpenTime;
        }

        protected override TimeUnitUserData GetUserData()
        {
            UserData data = new UserData
            {
				KeepTime = KeepTime
            };
					
            return data;
        }

        protected override void SetUserData(TimeUnitUserData data)
        {
            UserData d = data as UserData;
            if (d == null)
                return;
            KeepTime = d.KeepTime;

        }
    }
}
