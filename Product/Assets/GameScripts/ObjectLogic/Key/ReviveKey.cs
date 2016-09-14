using UnityEngine;
using System.Collections;

namespace MG
{
    public class ReviveKey : TimeUnit
    {
        private float ReviveTime;
        private GameObject TheKey;
        private Vector3 KeyPos;
        private Quaternion KeyRot;

        class UserData : TimeUnitUserData
        {
            public float ReviveTime;
        }

        void Start()
        {
            ReviveTime = -1f;
            KeyPos = TheKey.transform.position;
            KeyRot = TheKey.transform.rotation;

            Init(false);
        }

        void Update()
        {
            if (TimeController.Instance.IsOpTime())
                return;

            if ((TheKey == null || TheKey.activeSelf == false) && ReviveTime < 0f)
            {
                ReviveTime = GameDefine.OpenKeyReviveTime;
            }

            if (ReviveTime > 0)
            {
                ReviveTime -= Time.deltaTime;
            }

            if (ReviveTime < 0)
            {
                var o = Resources.Load("Resources/prefabs/Key") as GameObject;
                TheKey = Instantiate(o, KeyPos, KeyRot) as GameObject;
            }
        }

        protected override TimeUnitUserData GetUserData()
        {
            UserData data = new UserData();
            data.ReviveTime = ReviveTime;

            return data;
        }

        protected override void SetUserData(TimeUnitUserData data)
        {
            UserData d = data as UserData;
            if (d == null)
                return;

            ReviveTime = d.ReviveTime;
        }
    }
}

