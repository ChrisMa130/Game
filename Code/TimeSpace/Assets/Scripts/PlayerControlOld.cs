using UnityEngine;
using System.Collections;

public class PlayerControlOld : MonoBehaviour {

    [SerializeField]
    private float m_MaxSpeed = 1.2f;
    private Rigidbody2D m_Rigidbody2D;
    private Animator anim;
    private JumpPhysics2D jumpPhy;
    //private bool m_Jump;//是否跳跃
    // Use this for initialization
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    void Start () {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        jumpPhy = GetComponent<JumpPhysics2D>();

        //jumpPhy.is
    }
	
	// Update is called once per frame
	//void () {
 //       //if (!m_Jump && !isDying())
 //       //{
 //       //    // Read the jump input in Update so button presses aren't missed.
 //       //    m_Jump = Input.GetButtonDown("Jump");
 //       //}
 //   }
    public void FixedUpdate()
    {
        if (!isDying())//没有死亡
        {
            //控制左右移动
            float h = Input.GetAxis("Horizontal");
            m_Rigidbody2D.velocity = new Vector2(h * m_MaxSpeed, m_Rigidbody2D.velocity.y);
            //m_Jump = false;
            anim.SetFloat("Speed", Mathf.Abs(h));
            // If the input is moving the player right and the player is facing left...
            if (h > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (h < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
        else {
            m_Rigidbody2D.velocity = new Vector2(0, 0);
        }

        


        anim.SetBool("Ground", jumpPhy.IsGrounded);

        // Set the vertical animation
        anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
    }
    bool isDying()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("BlueHat_Death");
    }
    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
