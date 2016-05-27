using UnityEngine;
using System.Collections;

public class MoveTest : MonoBehaviour
{
    [SerializeField]
    private float m_MaxSpeed = 1.2f;
    private Rigidbody2D m_Rigidbody2D;
    // Use this for initialization
    void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");

        m_Rigidbody2D.velocity = new Vector2(h * m_MaxSpeed, m_Rigidbody2D.velocity.y);
    }


}
