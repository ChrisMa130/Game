using UnityEngine;
using System.Collections;

public class LineManagement : MonoBehaviour 
{
		private LineRenderer line; // Reference to LineRenderer
	    private Vector3 mousePos;    
	    private Vector3 startPos;    // Start position of line
	    private Vector3 endPos;    // End position of line
		public float lengthLimit = 0.7f;
		public GameObject m_char;
		private bool validLine;
		private LineRenderer lastLine;

	    void Update () 
	    {
			if (!(m_char.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("BlueHat_Death"))) {
				// On mouse down new line will be created 
				if (Input.GetMouseButtonDown (0)) {
					if (line == null)
						createLine ();
					mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
					mousePos.z = 0;
					line.SetPosition (0, mousePos);
							
					startPos = mousePos;
				} else if (Input.GetMouseButtonUp (0)) {
					if (line) {
						if (lastLine != null && validLine)
							Destroy (lastLine.gameObject);
						mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
						mousePos.z = 0;
						float lineLength = Vector3.Distance (startPos, mousePos);
						if ((Vector3.SqrMagnitude (startPos - mousePos) > 0.01)) {
							line.SetPosition (1, mousePos);
							endPos = mousePos;
							addColliderToLine ();
						}
						if (lineLength > lengthLimit)
							Destroy (line.gameObject);
						if (validLine)
							lastLine = line;
						line = null;
					}
				} else if (Input.GetMouseButton (0)) {
					if (line) {
						mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
						mousePos.z = 0;
						line.SetPosition (1, mousePos);
						float lineLength = Vector3.Distance (startPos, mousePos);
						if (lineLength > lengthLimit) {
							line.SetColors (Color.red, Color.red);
							validLine = false;
						} else {
							line.SetColors (Color.white, Color.white);
							validLine = true;
						}
					}
				}
			}
		}
	    // Following method creates line runtime using Line Renderer component
	    private void createLine()
	    {
		        line = new GameObject("Line").AddComponent<LineRenderer>();
				line.material = new Material(Shader.Find("Particles/Additive"));
		        line.SetVertexCount(2);
		        line.SetWidth(0.02f,0.02f);
				line.SetColors(Color.white, Color.white);
		        line.useWorldSpace = true; 
		    }
	    // Following method adds collider to created line
	    private void addColliderToLine()
	    {
				BoxCollider2D col = line.gameObject.AddComponent<BoxCollider2D> ();
				/*
		        float lineLength = Vector3.Distance (startPos, endPos); // length of line
		        col.size = new Vector3 (lineLength, 0.1f, 1f); // size of collider is set where X is length of line, Y is width of line, Z will be set as per requirement
		        */
				float lineLength = Vector3.Distance (startPos, endPos); // length of line
				col.size = new Vector2(lineLength, 0.02f);
		        Vector3 midPoint = (startPos + endPos)/2;
		        col.transform.localPosition = midPoint; // setting position of collider object

		        // Following lines calculate the angle between startPos and endPos
		        float angle = (Mathf.Abs (startPos.y - endPos.y) / Mathf.Abs (startPos.x - endPos.x));
		        if((startPos.y<endPos.y && startPos.x>endPos.x) || (endPos.y<startPos.y && endPos.x>startPos.x))
			        {
			            angle*=-1;
			        }
		        angle = Mathf.Rad2Deg * Mathf.Atan (angle);
		        col.transform.Rotate (0, 0, angle);
				col.offset = new Vector2 (0, 0);
		    }
}