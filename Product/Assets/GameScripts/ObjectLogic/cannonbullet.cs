using UnityEngine;

namespace MG
{
    public class cannonbullet : TimeUnit
    {
        private int DestoryCount;

        class UserData : TimeUnitUserData
        {
            public int DestoryCount;
        }

        void Start()
        {
            DestoryCount = GameDefine.CannonBulletDestoryCount;

            Init(true);
        }

        protected override TimeUnitUserData GetUserData()
        {
            var data = new UserData();
            data.DestoryCount = DestoryCount;

            return data;
        }

        protected override void SetUserData(TimeUnitUserData data)
        {
            UserData d = data as UserData;
            if (d == null)
                return;

            DestoryCount = d.DestoryCount;
        }

        void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.tag == "Building")
            {
                // 扣除一次  
                DestoryCount--;
            }
            else if (col.gameObject.tag == "ForbiddenZone" && col.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                DestoryCount--;
            }

            var player = col.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.Dead();
                DestoryCount = 0;
            }

            var npc = col.gameObject.GetComponent<Npc>();
            if (npc != null)
            {
                npc.Dead();
                DestoryCount = 0;
            }

            if (col.gameObject.tag == "CannonBullet")
                DestoryCount = 0;

            if (DestoryCount <= 0)
            {
                var unit = gameObject.GetComponent<TimeUnit>();
                if (unit != null)
                {
                    unit.DestoryMe();
                }
                else
                {
                    Debug.Log("销毁");
                    GameObject.DestroyObject(gameObject);
                }
            }
                
        }
    }
}


