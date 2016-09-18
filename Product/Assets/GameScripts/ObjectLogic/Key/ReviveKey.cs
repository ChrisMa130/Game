using UnityEngine;
using System.Collections;

namespace MG
{
    public class ReviveKey : TimeUnit
    {
        private float ReviveTime;
        public GameObject TheKey;
        private Vector3 KeyPos;
        private Quaternion KeyRot;

        class UserData : TimeUnitUserData
        {
            public float ReviveTime;
        }

        void Start()
        {
            ReviveTime = GameDefine.OpenKeyReviveTime;
            KeyPos = TheKey.transform.position;
            KeyRot = TheKey.transform.rotation;

            Init(false);
        }

        void Update()
        {
            if (TimeController.Instance.IsOpTime())
                return;

            if (TheKey != null || TheKey.activeSelf)
                return;

            if (ReviveTime < 0f)
            {
                var o = Resources.Load("prefabs/Key") as GameObject;
                TheKey = Instantiate(o, KeyPos, KeyRot) as GameObject;
                ReviveTime = GameDefine.OpenKeyReviveTime;
                return;
            }

            ReviveTime -= Time.deltaTime;
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

