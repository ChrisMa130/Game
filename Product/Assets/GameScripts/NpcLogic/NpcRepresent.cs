﻿using UnityEngine;
using Spine;

namespace MG
{
    public class NpcRepresent : MonoBehaviour
    {
        private Animator Anim;
        private Rigidbody2D Rigidbody;
        private Npc Owner;

        void Awake()
        {
            Anim = GetComponent<Animator>();
            Rigidbody = GetComponent<Rigidbody2D>();
            Owner = GetComponent<Npc>();
        }

        public void Run(float moveParam)
        {
			Reset ();
            Anim.SetFloat("Speed", Mathf.Abs(moveParam));
        }

        public void Stand()
        {
            //Anim.SetBool("Ground", true);
            // Anim.SetFloat("vSpeed", Rigidbody.velocity.y);
            //Anim.SetFloat("Speed", 0);
        }

        public void Climb(float moveParam)
        {
            //Anim.SetFloat("Speed", Mathf.Abs(moveParam));
        }

        public void Dead()
        {
			Reset ();
            Anim.SetBool("Dead", true);
            //Rigidbody.isKinematic = true;
        }

        public void Revive()
        {
			Reset ();
			ChangeAttachment ();
            Anim.SetBool("Dead", false);
            //Rigidbody.isKinematic = false;
			Rigidbody.gravityScale = 1;
        }

        public void Jump()
        {
            //Anim.SetBool("Ground", false);
        }

        public void TurnRound(Dir dir)
        {
            Vector3 theScale = Owner.transform.localScale;

            if (dir == Dir.Left)
                theScale.x = Mathf.Abs(theScale.x);
            else
                theScale.x = -Mathf.Abs(theScale.x);

            Owner.transform.localScale = theScale;
        }

        public void IsKinematic(bool isKinematic)
        {
            Rigidbody.isKinematic = isKinematic;
        }

		public void Reset()
		{
			var spineRenderer = Anim.gameObject.GetComponent<Spine.Unity.SkeletonRenderer>();
			if (spineRenderer == null)
				return;
			spineRenderer.skeleton.SetBonesToSetupPose();
		}

		private void ChangeAttachment() {
			Skeleton skeleton = Anim.gameObject.GetComponent<Spine.Unity.SkeletonRenderer>().skeleton;
			skeleton.SetAttachment ("yan3", null);
			skeleton.SetAttachment ("yan2", null);
			skeleton.SetAttachment ("yan", "yan");
			skeleton.SetAttachment ("yan1", "yan1");
		}
    }
}

