﻿using UnityEngine;
using System.Collections;

namespace MG
{
    public class OpenKey : TimeUnit
    {
        public bool InitPos = true;

        void Start()
        {
            Init(InitPos);
        }

        void Update()
        {
        }

        // 未进入时间操作时有效。未被拿起时有效。
//        void OnCollisionEnter2D(Collision2D obj)
//        {
//            if (transform.parent == null)
//            {
//                // 碰谁，谁拿
//                var player = obj.gameObject.GetComponent<Player>();
//                var npc = obj.gameObject.GetComponent<Npc>();
//                if (player != null || npc != null)
//                {
//                    var point = player ? player.GetHandObject() : null;
//                    if (point == null)
//                        point = npc ? npc.GetHandObject() : null;
//
//                    SetParent(point);
//                    c2d.isTrigger = true;
//                }
//                //transform.position = Vector3.zero;
//            }
//
//            var door = obj.gameObject.GetComponent<KeyHold>();
//            if (door != null)
//            {
//                TeachDoor(door);
//            }
//        }

        void TeachDoor(KeyHold door)
        {
            door.OpenDoor();
            Dead();
        }

        // 拿起后的逻辑
        void OnTriggerEnter2D(Collider2D obj)
        {
            if (obj.tag.Equals("ForbiddenZone") && obj.gameObject.layer == 10)
                Dead();
            if (TimeController.Instance.CurrentState == TimeControllState.Forward || TimeController.Instance.CurrentState == TimeControllState.Rewinding)
                return;
            if (transform.parent == null)
            {
                // 碰谁，谁拿
                var player = obj.gameObject.GetComponent<Player>();
                var npc = obj.gameObject.GetComponent<Npc>();
                if (player != null || npc != null)
                {
                    var point = player ? player.GetHandObject() : null;
					if (point != null && (player.GetDir() == Dir.Left))
						transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
					if (point == null) {
						point = npc ? npc.GetHandObject () : null;
						if (npc.GetDir() == Dir.Left)
							transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
					}
                    SetParent(point);
                }
                //transform.position = Vector3.zero;
            }

            var door = obj.gameObject.GetComponent<KeyHold>();
            if (door != null)
            {
                TeachDoor(door);
            }
        }

        public void Dead()
        {
            DestoryMe();
        }

        public void SetParent(Transform obj)
        {
            transform.parent = obj;
            if (obj != null)
                transform.localPosition = Vector3.zero;
        }

        protected override TimeUnitUserData GetUserData()
        {
            return new TimeUnitUserData();
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

