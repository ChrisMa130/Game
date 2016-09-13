using UnityEngine;
using System.Collections;

namespace MG
{
    public class OpenKey : TimeUnit
    {
        private BoxCollider2D c2d;

        void Start()
        {
            c2d = GetComponent<BoxCollider2D>();

            Init(true);
        }

        void Update()
        {
        }

        // 未进入时间操作时有效。未被拿起时有效。
        void OnCollisionEnter2D(Collision2D obj)
        {
            if (transform.parent == null)
            {
                // 碰谁，谁拿
                var player = obj.gameObject.GetComponent<Player>();
                var npc = obj.gameObject.GetComponent<Npc>();
                if (player != null || npc != null)
                {
                    SetParent(obj.gameObject);
                }

                // 一旦被拾取。那么就进入tragger模式。
                c2d.isTrigger = true;
                //transform.position = Vector3.zero;
            }

            // TODO 碰到门了
            var door = obj.gameObject.GetComponent<OpenDoor>();
            if (door != null)
            {
                TeachDoor(door);
            }
        }

        void TeachDoor(OpenDoor door)
        {
            // blblblblb
            Dead();
        }

        // 拿起后的逻辑
        void OnTriggerStay2D(Collider2D other)
        {
            var door = other.gameObject.GetComponent<OpenDoor>();
            if (door != null)
            {
                TeachDoor(door);
            }
        }

        public void Dead()
        {
            DestoryMe();
        }

        public void SetParent(GameObject obj)
        {
            if (obj == null)
            {
                c2d.isTrigger = false;
                //transform.position = transform.parent.position;
            }

            transform.parent = obj.transform.parent;
        }

        protected override TimeUnitUserData GetUserData()
        {
            return base.GetUserData();
        }

        protected override void SetUserData(TimeUnitUserData data)
        {
            // 时间操作时。脱离宿主
            if (transform.parent != null)
                SetParent(null);

            base.SetUserData(data);
        }
    }
}

