/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/**
 * Movement class. Should only be responsible for moving. Make an input wrapper class. 
**/
public class Movement : MonoBehaviour {

	//networking
	public bool isPlayer = false;

	//animator
	public Animator anim;

	private Vector3 movement;
	private Vector3 acceleration;
	private float accAmnt = 0.01f;
	private Rigidbody playerRigidbody;
	private float speed;
	int floorMask;
	float camRayLength = 100f;
	float gravity = 0f;
	private bool isGrounded;
	private bool isDead;
	private Vector3 spawn;
	private Vector3 direction;
	private bool shooting;
	private bool f, b, l, r;
	private float ccHeight;

	public int tester = 42;

	//swaps back and forth between third person and perspective
	public bool inThirdPerson = true;

	public Vector3 getMove() {
		return movement;
	}

	private AudioSource footstepSound;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		playerRigidbody = GetComponent<Rigidbody> ();
		floorMask = LayerMask.GetMask ("Floor");
		direction = Vector3.zero;
		speed = 5f;
		shooting = false;
		isGrounded = true;
		spawn = transform.position;
		RagDoll (false);
		ccHeight = GetComponent<CapsuleCollider>().height;
	}

	public void SetRagDoll(bool rag) {
		RagDoll (rag);
	}

	void RagDoll(bool rag) {
		if (rag) {
			anim.enabled = false;
			isDead = true;
		} else {
			anim.enabled = true;
		}
		Rigidbody[] bodies = GetComponentsInChildren<Rigidbody> ();
		foreach (Rigidbody body in bodies) {
			if (body.name == "Player") { 
				//continue;
			}
			body.isKinematic = !rag;
		}
		GetComponent<Rigidbody> ().isKinematic = rag;
		foreach (Collider c in GetComponentsInChildren<Collider> ()) {
			c.enabled = rag;
		}
		GetComponent<CapsuleCollider>().enabled = !rag;
		GetComponent<Rigidbody> ().isKinematic = rag;
	}

	void FootStep() {
		if (footstepSound && movement.magnitude >= 0.6f) {
			footstepSound.Play ();
		}
	}

	public void Move(float h, float v) {
		acceleration.Set (h, movement.y, v);
		acceleration = acceleration.normalized;

		if (inThirdPerson) {
			acceleration = transform.rotation * acceleration;
		}
		float slide = 10f;
		gravity -= Time.deltaTime / 3.5f;
		RaycastHit hitInfo = new RaycastHit();
		Vector3 pos = playerRigidbody.position;
		pos.y += 1f;
		bool overSnow = false;

		//THIS SHOULD ALL BE A SEPERATE CLASS
	
		if (Physics.Raycast (new Ray (pos, Vector3.down), out hitInfo, 1.1f, 1 << 11)) {
			if (hitInfo.collider.CompareTag ("Ice")) {
				slide = 1f;
			} else if (hitInfo.collider.CompareTag("Snow")) {
				overSnow = true;
			}
			AudioSource[] sources = hitInfo.transform.gameObject.GetComponents<AudioSource> ();
			if (sources.Length > 0) {
				footstepSound = sources [0];
			}
			gravity = 0;
		}

		/*if (overSnow && movement.magnitude > 0.1) {
			ParticleSystem part = GetComponentInChildren<ParticleSystem> ();
			part.Play ();
		}*/

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

	public void Turning() {
		if (!inThirdPerson && isPlayer) {
			Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		
			RaycastHit floorHit;
		
			if (Physics.Raycast (camRay, out floorHit, camRayLength, floorMask)) {
				Vector3 playerToMouse = floorHit.point - transform.position;
				playerToMouse.y = 0f;

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

	private float h;
	private float v;
	private float oldH = 0;
	private float oldV = 0;
	private bool needsUpdate = false;

	public float getH() {
		return h;
	}

	public float getV() {
		return v;
	}

	public bool NeedsUpdate() {
		if (needsUpdate) {
			needsUpdate = false;
			return true;
		}
		return false;
	}

	public void setInputs(float hh, float vv) {
		h = hh;
		v = vv;
	}



	// Update is called once per frame
	//Moving this into PlayerInputManager
	/*void Update () {
		OverWater ();
		if (isDead) {
			//deadImage.color = Color.red;
			return;
		} else {
			//deadImage.color = Color.Lerp (deadImage.color, Color.clear, Time.deltaTime * 10f);
		}
		isDead = false;

		if (isPlayer) {
			h = Input.GetAxisRaw ("Horizontal");
			v = Input.GetAxisRaw ("Vertical");
			if (h != oldH || v != oldV) {
				oldH = h;
				oldV = v;
				needsUpdate = true;
			}
		}

		if (Input.GetKey ("space")) {
			shooting = true;
		} else {
			shooting = false;
		}

		if (Input.GetKey ("k")) {
			RagDoll (true);
		}

		if (shooting) {
			h = 0;
			v = 0;
		}
		CapsuleCollider cc = GetComponent<CapsuleCollider> ();
		if (shooting) {
			h = 0;
			v = 0;
			cc.height = ccHeight * 0.8f;
		} else {
			cc.height = ccHeight;
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
	}*/
}
