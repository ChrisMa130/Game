using UnityEngine;

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
            //Anim.SetFloat("Speed", Mathf.Abs(moveParam));
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
            //Anim.SetBool("Dead", true);
            Rigidbody.isKinematic = true;
        }

        public void Jump()
        {
            //Anim.SetBool("Ground", false);
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
    }
}

