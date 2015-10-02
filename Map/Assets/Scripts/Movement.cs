using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

	private Animator anim;

	private Vector3 movement;
	private Rigidbody playerRigidbody;
	private float speed;
	int floorMask;
	float camRayLength = 100f;
	float gravity = 0f;
	private bool isGrounded;

	private Vector3 direction;
	//private Vector3 angle = new Vector3(0f,0f,0f);
	private bool shooting;
	private bool f, b, l, r;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		playerRigidbody = GetComponent<Rigidbody> ();
		floorMask = LayerMask.GetMask ("Floor");

		direction = Vector3.zero;
		speed = 2.5f;
		shooting = false;
		isGrounded = true;
	}

	void Move(float h, float v) {
		movement.Set (h, gravity, v);
		movement = movement.normalized * speed * Time.deltaTime;

		gravity -= Time.deltaTime / 3.5f;
		RaycastHit hitInfo = new RaycastHit();
		Vector3 pos = playerRigidbody.position;
		pos.y += 1f;
		if (Physics.Raycast(new Ray(pos, Vector3.down), out hitInfo, 1.1f)) {
			gravity = 0;
		}
		movement.y = gravity;
		
		playerRigidbody.MovePosition (transform.position + movement);
		Vector3 vl = playerRigidbody.velocity;
		float newSpeed = 1f - (vl.magnitude);
		if (newSpeed < 0.1f) {
			newSpeed = 0.1f;
		}
		anim.speed = newSpeed;
	}

	void Turning() {
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		
		RaycastHit floorHit;
		
		if (Physics.Raycast (camRay, out floorHit, camRayLength, floorMask)) {
			Vector3 playerToMouse = floorHit.point - transform.position;
			playerToMouse.y = 0f;
			
			//Quaternion newRotation = Quaternion.LookRotation (playerToMouse);
			//playerRigidbody.MoveRotation (newRotation);

			transform.rotation = Quaternion.LookRotation(playerToMouse);
		}
	}
	
	// Update is called once per frame
	void Update () {

		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw ("Vertical");

		if (Input.GetKey ("space")) {
			shooting = true;
		} else {
			shooting = false;
		}

		if (shooting) {
			h = 0;
			v = 0;
		}
		Move (h, v);
		Turning ();

		float rot = transform.eulerAngles.y;

		float sp = 0;
		float dir = 0; 

		if ( h != 0 || v != 0) {
			float angle = rot;
			float movAngle = Vector3.Angle (new Vector3 (h, 0, v), new Vector3 (0, 0, 1));
			if (h < 0) {
				movAngle = 360f - movAngle;
			}

			if (angle - movAngle > 180) {
				angle -= 360f;
			} else if (movAngle - angle > 180) {
				angle += 360f;
			}

			sp =  Mathf.Abs( angle - movAngle ) <= 90f ? 1 : -1;


			if (movAngle < rot - 180f) {
				movAngle += 360;
			}
			angle = rot - movAngle;
			dir = 1;

			if (angle < 0f) {
				angle = -angle;
			} else {
				dir = -1;
			}

			if (angle > 90f) {
				angle = 180f - angle;
			}

			dir *= angle/90f;

		}

		anim.SetFloat ("Speed", sp);
		anim.SetFloat ("Direction", dir, .25f, Time.deltaTime);

		anim.SetBool ("Shooting", shooting);
	}
}
