using UnityEngine;
using System.Collections;

// 对移动的封装
namespace MG
{
    // 直接移植就行了。这是参考官方例子的代码。这部分不需要太纠结。
    public class ObjectMove : MonoBehaviour
    {
        private         Transform   GroundCheck;
        private         Transform   ClimbObj;
        private         Animator    Anim;
        private         Rigidbody2D Rigidbody;
        private         bool        Grounded;
        public          bool        CanClimb;
        private const   float       ClimbRadius = 0.2f;
        private         bool        FacingRight;
        private         float       OrgGravityScale;

        [SerializeField]
        private LayerMask WhatIsGround;
        [SerializeField]
        private float MaxSpeed = 1.2f;                    // The fastest the player can travel in the x axis.
        [SerializeField]
        private float JumpForce = 200f;                  // Amount of force added when the player jumps.
        private bool AirControl = true;

        private void Awake()
        {
            AirControl  = true;
            FacingRight = true;
            CanClimb    = false;
            GroundCheck = transform.Find("GroundCheck");
            ClimbObj    = transform.Find("ClimbCheck");
            Anim        = GetComponent<Animator>();
            Rigidbody   = GetComponent<Rigidbody2D>();

            OrgGravityScale = Rigidbody.gravityScale;

            WhatIsGround.value = -1;
        }

        private void FixedUpdate()
        {
            Grounded = Physics2D.Linecast(transform.position, GroundCheck.position, 1 << LayerMask.NameToLayer("Ground"));  
            Anim.SetBool("Ground", Grounded);
            Anim.SetFloat("vSpeed", Rigidbody.velocity.y);

            if (CanClimb)
                Rigidbody.gravityScale = 0;
            else
                Rigidbody.gravityScale = OrgGravityScale;
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
            // Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, moveParam * MaxSpeed);

            if (moveParam > 0 && !FacingRight)
            {
                Flip();
            }
            else if (moveParam < 0 && FacingRight)
            {
                Flip();
            }
        }

        private void Climb(float moveParam)
        {
            // TODO 动画待接入
            if (!CanClimb)
                return;

            Anim.SetFloat("Speed", Mathf.Abs(moveParam));
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, moveParam * MaxSpeed);
        }

        private void internalJump()
        {
            if (!Grounded)
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

        public void MoveStop()
        {
            Move(0);
        }

        public void MoveUp()
        {
            float moveParam = Input.GetAxis("Vertical");
            Climb(moveParam);
        }

        public void MoveDown()
        {
            float moveParam = Input.GetAxis("Vertical");
            Climb(moveParam);
        }

        public void ClimbStop()
        {
            Climb(0);
        }

        public void Jump()
        {
            internalJump();
        }
    }

}


