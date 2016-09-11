using UnityEngine;
using System.Collections;

namespace MG
{
    public class cannon : TimeUnit
    {
        public Rigidbody2D Bullet;

		public float XAxis;
		public float YAxis;

        private float NextCreateBulletTime;
		public float myRate;

		public int myCount = 0;
		public float myBulletSpeed;

        class UserData : TimeUnitUserData
        {
            public float XAxis;
            public float YAxis;
            public float NextCreateBulletTime;
        }

        void Start()
        {
			NextCreateBulletTime = GameDefine.CannonBulletCreateInterval;
			if (myRate > 0f)
				NextCreateBulletTime = myRate;
			else
				myRate = GameDefine.CannonBulletCreateInterval;
			if (myBulletSpeed == 0f)
				myBulletSpeed = GameDefine.CannonBulletSpeed;
			
            Init(false);
        }

        void Update()
        {
			if (TimeController.Instance.IsOpTime())
				return;

			NextCreateBulletTime -= Time.deltaTime;
			if (Bullet == null || NextCreateBulletTime > 0)
				return;

			NextCreateBulletTime = myRate;

			Rigidbody2D bulletInstance = Instantiate(Bullet, transform.position, transform.rotation) as Rigidbody2D;
			bulletInstance.velocity = new Vector3(myBulletSpeed * XAxis, myBulletSpeed * YAxis);
			if (myCount != 0)
				bulletInstance.gameObject.GetComponent<cannonbullet> ().myCount = myCount;
        }

        protected override TimeUnitUserData GetUserData()
        {
            UserData data = new UserData();

            data.XAxis = XAxis;
            data.YAxis = YAxis;
            data.NextCreateBulletTime = NextCreateBulletTime;

            return data;
        }

        protected override void SetUserData(TimeUnitUserData data)
        {
            UserData d = data as UserData;
            if (d == null)
                return;

            XAxis = d.XAxis;
            YAxis = d.YAxis;
            NextCreateBulletTime = d.NextCreateBulletTime;
        }
    }
}

