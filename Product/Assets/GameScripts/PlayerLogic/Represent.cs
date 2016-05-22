using UnityEngine;

namespace MG
{
    public class Represent : MonoBehaviour
    {
        private Animator Anim;
        private Rigidbody2D Rigidbody;

        void Start()
        {
            Anim = GetComponent<Animator>();
            Rigidbody = GetComponent<Rigidbody2D>();
        }

        public void Walk(float moveParam)
        {
            Anim.SetFloat("Speed", Mathf.Abs(moveParam));
        }

        public void Stand()
        {
            Anim.SetBool("Ground", true);
            Anim.SetFloat("vSpeed", Rigidbody.velocity.y);
        }

        public void Climb(float moveParam)
        {
            Anim.SetFloat("Speed", Mathf.Abs(moveParam));
        }

        public void Dead()
        {
            Anim.SetBool("Dead", true);
            Rigidbody.isKinematic = true;
        }

        public void Jump()
        {
            Anim.SetBool("Ground", false);
        }
    }
}

