using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Movement : MonoBehaviour {

	private Animator anim;

	private Vector3 movement;
	private Vector3 acceleration;
	private float accAmnt = 0.01f;
	private Rigidbody playerRigidbody;
	private float speed;
	int floorMask;
	int oceanCheck;
	float camRayLength = 100f;
	float gravity = 0f;
	private bool isGrounded;
	private bool isDead;
	private Vector3 spawn;
	public Image deadImage;
	private Vector3 direction;
	//private Vector3 angle = new Vector3(0f,0f,0f);
	private bool shooting;
	private bool f, b, l, r;

	public bool inThirdPerson = true;

	private AudioSource footstepSound;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		playerRigidbody = GetComponent<Rigidbody> ();
		floorMask = LayerMask.GetMask ("Floor");
		oceanCheck = LayerMask.GetMask ("OceanCheck");
		direction = Vector3.zero;
		speed = 2.5f;
		shooting = false;
		isGrounded = true;
		spawn = transform.position;
	}

	void FootStep() {
		if (footstepSound && movement.magnitude >= 0.6f) {
			footstepSound.Play ();
		}
	}

	void Move(float h, float v) {
		acceleration.Set (h, movement.y, v);
		acceleration = acceleration.normalized;
		/*
		if (movement.magnitude > 1) {
			movement = movement.normalized;
		}
		*/
		if (inThirdPerson) {
			movement = transform.rotation * movement;
		}
		float slide = 10f;
		gravity -= Time.deltaTime / 3.5f;
		RaycastHit hitInfo = new RaycastHit();
		Vector3 pos = playerRigidbody.position;
		pos.y += 1f;
		if (Physics.Raycast(new Ray(pos, Vector3.down), out hitInfo, 1.1f)) {
			if (hitInfo.collider.CompareTag("Ice")) {
				slide = 1f;
			}
			footstepSound = hitInfo.transform.gameObject.GetComponents<AudioSource> ()[0];
			gravity = 0;
		}

		movement = Vector3.Lerp(movement, acceleration, Time.deltaTime * slide);
		movement.y = gravity / Time.deltaTime;
		
		playerRigidbody.MovePosition (transform.position + (movement * speed * Time.deltaTime));
		Vector3 vl = playerRigidbody.velocity;
		float newSpeed = 1f - (vl.magnitude);
		if (newSpeed < 0.1f) {
			newSpeed = 0.1f;
		}
		anim.speed = newSpeed;
	}

	void Turning() {
		if (!inThirdPerson) {
			Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		
			RaycastHit floorHit;
		
			if (Physics.Raycast (camRay, out floorHit, camRayLength, floorMask)) {
				Vector3 playerToMouse = floorHit.point - transform.position;
				playerToMouse.y = 0f;
			
				//Quaternion newRotation = Quaternion.LookRotation (playerToMouse);
				//playerRigidbody.MoveRotation (newRotation);

				transform.rotation = Quaternion.LookRotation (playerToMouse);
			}
		} else {
			float turnSpeed = Time.deltaTime * 100;
			if (Input.GetKey ("j")) {
				transform.Rotate (0, -turnSpeed, 0);
			} else if (Input.GetKey ("l")) {
				transform.Rotate (0, turnSpeed, 0);
			}
		}
	}
	//respawns, won't be needed in final version
	void Respawn() {
		transform.position = spawn;
	}

	void OverWater() {
		Ray ray = new Ray (transform.position, new Vector3 (0, -1, 0));
		RaycastHit oceanHit;

		if (Physics.Raycast (ray, out oceanHit, 0.1f, oceanCheck)) {
			if (oceanHit.collider.CompareTag ("Kill")) {
				Debug.Log (oceanHit.collider.CompareTag ("Kill"));
				isDead = true;
				Respawn ();
				deadImage.color = Color.red;
			}
		}
	}

	// Update is called once per frame
	void Update () {
		OverWater ();
		if (isDead) {
			deadImage.color = Color.red;
		} else {
			deadImage.color = Color.Lerp (deadImage.color, Color.clear, Time.deltaTime * 10f);
		}
		isDead = false;

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
			float movAngle = Vector3.Angle (movement, new Vector3 (0, movement.y, 1));
			//float movAngle = Vector3.Angle (new Vector3(h, 0, v), new Vector3 (0, 0, 1));

			if (movement.x < -0.1f) {
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
