using UnityEngine;
using System.Collections;

namespace MG
{
    public class cannon : MonoBehaviour
    {
        public Rigidbody2D Bullet;

		public float XAxis;
		public float YAxis;

        private float NextCreateBulletTime;

        void Start()
        {
            NextCreateBulletTime = GameDefine.CannonBulletCreateInterval;
        }

        void Update()
        {
//            if (!TimeController.Instance.TimebackStart)
//                return;

            NextCreateBulletTime -= Time.deltaTime;
            if (Bullet == null || NextCreateBulletTime > 0)
                return;

            NextCreateBulletTime = GameDefine.CannonBulletCreateInterval;

            Rigidbody2D bulletInstance = Instantiate(Bullet, transform.position, transform.rotation) as Rigidbody2D;
            bulletInstance.velocity = new Vector3(GameDefine.CannonBulletSpeed * XAxis, GameDefine.CannonBulletSpeed * YAxis);
            
        }
    }
}

