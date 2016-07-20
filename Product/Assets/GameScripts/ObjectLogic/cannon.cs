using UnityEngine;
using System.Collections;

namespace MG
{
    public class cannon : MonoBehaviour
    {
        public Rigidbody2D Bullet;

        public int XAxis;
        public int YAxis;

        private float NextCreateBulletTime;

        void Start()
        {
            NextCreateBulletTime = GameDefine.CannonBulletCreateInterval;
        }

        void Update()
        {
            NextCreateBulletTime -= Time.deltaTime;
            if (Bullet == null || NextCreateBulletTime > 0)
                return;

            NextCreateBulletTime = GameDefine.CannonBulletCreateInterval;

            Rigidbody2D bulletInstance = Instantiate(Bullet, transform.position, transform.rotation) as Rigidbody2D;
            bulletInstance.velocity = new Vector3(GameDefine.CannonBulletSpeed * XAxis, GameDefine.CannonBulletSpeed * YAxis);
            
        }
    }
}

