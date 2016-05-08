using UnityEngine;
using System.Collections;

public class Player_Control : MonoBehaviour {

	private Player_Movement m_Character;
	private bool m_Jump;
	private Animator anim; 
	private GameManager controller_Script;
	// Use this for initialization
	void Awake() {
		m_Character = GetComponent<Player_Movement>();
		anim = GetComponent<Animator>();
		controller_Script = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameManager> ();
	}
	
	private void Update()
	{
		if (!m_Jump && !isDying())
		{
			// Read the jump input in Update so button presses aren't missed.
			m_Jump = Input.GetButtonDown("Jump");
		}
	}

	void FixedUpdate() {
		if (!isDying ()) {
			float h = Input.GetAxis ("Horizontal");
			m_Character.Move (h, m_Jump);
			m_Jump = false;
		} else {
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
		}
	}

	bool isDying() {
		return anim.GetCurrentAnimatorStateInfo (0).IsName ("BlueHat_Death");
	}
	void Respawn() {
		float respawnX = controller_Script.respawnX;
		float respawnY = controller_Script.respawnY;
		transform.position = new Vector3 (respawnX, respawnY, 0);
		anim.SetBool ("Dead", false);
		GetComponent<Rigidbody2D> ().isKinematic = false;
        print(" Respawn");
	}
}
