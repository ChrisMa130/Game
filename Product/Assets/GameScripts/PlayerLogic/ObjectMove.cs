using UnityEngine;
using System.Collections;

// 对移动的封装
namespace MG
{
    // 直接移植就行了。这是参考官方例子的代码。这部分不需要太纠结。
    public class ObjectMove : MonoBehaviour
    {
        private         Transform   GroundCheck;
        private         Animator    Anim;
        private         Rigidbody2D Rigidbody;
        private         bool        Grounded;
        private const   float       GroundedRadius = 0.2f;
        private         bool        FacingRight;

        [SerializeField]
        private LayerMask WhatIsGround;
        [SerializeField]
        private float MaxSpeed = 1.2f;                    // The fastest the player can travel in the x axis.
        [SerializeField]
        private float JumpForce = 150f;                  // Amount of force added when the player jumps.
        [SerializeField]
        private bool AirControl = true;

        private void Awake()
        {
            AirControl  = true;
            FacingRight = true;
            GroundCheck = transform.Find("GroundCheck");
            Anim        = GetComponent<Animator>();
            Rigidbody   = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            Grounded = false;

            var colliders = Physics2D.OverlapCircleAll(GroundCheck.position, GroundedRadius, WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                    Grounded = true;
            }

            Anim.SetBool("Ground", Grounded);
            Anim.SetFloat("vSpeed", Rigidbody.velocity.y);
        }

        private void Flip()
        {
            FacingRight = !FacingRight;

            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        private void Move(float moveParam)
        {
            if (!Grounded && !AirControl)
                return;

            Anim.SetFloat("Speed", Mathf.Abs(moveParam));

            Rigidbody.velocity = new Vector2(moveParam * MaxSpeed, Rigidbody.velocity.y);
           
            if (moveParam > 0 && !FacingRight)
            {
                Flip();
            }
            else if (moveParam < 0 && FacingRight)
            {
                Flip();
            }
        }

        private void internalJump()
        {
            if (!Grounded || !Anim.GetBool("Ground"))
                return;

            Grounded = false;
            Anim.SetBool("Ground", false);
            Rigidbody.AddForce(new Vector2(0f, JumpForce));
        }

        // 下面是对移动接口的封装。
        public void MoveRight()
        {
            float moveParam = Input.GetAxis("Horizontal");
            Move(moveParam);
        }

        public void MoveLeft()
        {
            float moveParam = Input.GetAxis("Horizontal");
            Move(moveParam);
        }

        public void MoveUp()
        {
        }

        public void MoveDown()
        {        
        }

        public void Jump()
        {
            internalJump();
        }
    }

}


