﻿using UnityEngine;

namespace MG
{
    public class Represent : MonoBehaviour
    {
        private Animator Anim;
		private Animator Climb_Anim;
        private Rigidbody2D Rigidbody;
        private Player Owner;

		private float Climb_Interval;

        void Awake()
        {
            Anim = GetComponent<Animator>();
			Climb_Anim = transform.FindChild ("Climb").GetComponent<Animator> ();
            Rigidbody = GetComponent<Rigidbody2D>();
            Owner = GetComponent<Player>();
        }

        public void Run(float moveParam)
        {
			Reset();
			DisableMesh (false);
			Anim.SetBool ("Move", true);
			Anim.SetBool("Ground", true);
            Anim.SetFloat("Speed", Mathf.Abs(moveParam));
        }

        public void Stand()
        {
			Reset();
			DisableMesh (false);
			Anim.SetBool("Move", false);
            Anim.SetBool("Ground", true);
            Anim.SetBool("Dead", false);
            Anim.SetFloat("Speed", 0);
        }

        public void Climb(float moveParam)
        {
			Reset ();
			DisableMesh (true);
            Anim.SetFloat("Speed", Mathf.Abs(moveParam));
			if (Climb_Anim.GetFloat ("Time") < 0f)
				Climb_Anim.SetFloat ("Time", .3f);
			if (Input.GetKey (KeyCode.W) || Input.GetKey(KeyCode.S)) {
				Climb_Anim.SetFloat ("Time", Climb_Anim.GetFloat ("Time") - Time.deltaTime);
			}
        }

		private void DisableMesh (bool disable) {
			Anim.gameObject.GetChildByName("Climb").SetActive (disable);
			GetComponent<MeshRenderer> ().enabled = !disable;
		}

        public void Dead()
        {
			Reset ();
            Anim.SetBool("Dead", true);
        }

        public void Revive()
        {
            Anim.SetBool("Dead", false);
        }

        public void Jump()
        {
			Reset ();
            Anim.SetBool("Ground", false);
        }

        public void TurnRound(Dir dir)
        {
            Vector3 theScale = Owner.transform.localScale;

            if (dir == Dir.Left)
                theScale.x = -Mathf.Abs(theScale.x);
            else
                theScale.x = Mathf.Abs(theScale.x);

            Owner.transform.localScale = theScale;
        }

		public void Reset()
        {
			var spineRenderer = Anim.gameObject.GetComponent<Spine.Unity.SkeletonRenderer>();

			if (spineRenderer == null)
				return;

			spineRenderer.skeleton.SetBonesToSetupPose();
		}

        public Animator GetAnimator()
        {
            return Anim;
        }
    }
}

