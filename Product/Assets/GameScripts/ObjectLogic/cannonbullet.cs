using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MG
{
    public class cannonbullet : TimeUnit
    {
        private int DestoryCount;

        class UserData
        {
            public int DestoryCount;
        }

        private Dictionary<int, UserData> UserDataTable;

        void Start()
        {
            UserDataTable = new Dictionary<int, UserData>();
            DestoryCount = GameDefine.CannonBulletDestoryCount;

            Init();
        }

        protected override void SaveUserData(int frame)
        {
            base.SaveUserData(frame);

            UserData data;

            if (UserDataTable.Count == 0)
            {
                data = new UserData();

                data.DestoryCount = DestoryCount;

                UserDataTable.Add(0, data);
                return;
            }

            UserDataTable.TryGetValue(frame, out data);
            if (data != null)
                return;

            data = new UserData
            {
                DestoryCount = DestoryCount,
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

            DestoryCount = data.DestoryCount;
        }

        protected override void ClearUserData()
        {
            base.ClearUserData();
            UserDataTable.Clear();
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


