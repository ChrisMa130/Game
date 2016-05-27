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
        public          bool        CanClimb;
        private         bool        FacingRight;
        private         float       OrgGravityScale;
        private         bool        OnTheLadder;

        [SerializeField]
        private LayerMask WhatIsGround;
        [SerializeField]
        private float MaxSpeed = 2.0f;                    // The fastest the player can travel in the x axis.
        [SerializeField]
        private float JumpForce = 200f;                  // Amount of force added when the player jumps.
        private bool AirControl = true;
        private float DisableJumpTime;  // 关于这个变量的作用，在起跳后一段时间不在接受跳跃指令，因为通过obj来检测是否在地面的，跳跃速度在前几帧无法跳出检测范围，所以在较小的情况下，可能会出现2个跳跃叠加在一起的情况。

        private void Awake()
        {
            AirControl  = true;
            FacingRight = true;
            CanClimb    = false;
            OnTheLadder = false;
            GroundCheck = transform.Find("GroundCheck");
            Anim        = GetComponent<Animator>();
            Rigidbody   = GetComponent<Rigidbody2D>();

            OrgGravityScale = Rigidbody.gravityScale;
            DisableJumpTime = 0f;

            WhatIsGround.value = -1;
        }

        private void FixedUpdate()
        {
            Grounded = Physics2D.Linecast(transform.position, GroundCheck.position, 1 << LayerMask.NameToLayer("Ground"));  
            Anim.SetBool("Ground", Grounded);
            Anim.SetFloat("vSpeed", Rigidbody.velocity.y);

            DisableJumpTime = Mathf.Max(DisableJumpTime -= Time.deltaTime, 0.0f);

            if (CanClimb && OnTheLadder)
            {
                Rigidbody.gravityScale = 0;
            }
            else
            {
                if (!CanClimb)
                {
                    Rigidbody.gravityScale = OrgGravityScale;
                    OnTheLadder = false;
                }
            }
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

        private void Climb(float moveParam)
        {
            if (!OnTheLadder)
                return;

            Anim.SetFloat("Speed", Mathf.Abs(moveParam));
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, moveParam * MaxSpeed);
        }

        private void internalJump()
        {
            if (!Grounded || DisableJumpTime > 0.0f)
                return;

            Grounded = false;
            DisableJumpTime = 0.2f;
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
            if (!CanClimb)
                return;

            float moveParam = Input.GetAxis("Vertical");
            OnTheLadder = true;
            Climb(moveParam);
        }

        public void MoveDown()
        {
            if (!CanClimb)
                return;

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


