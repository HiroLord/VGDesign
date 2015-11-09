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
	
	//animator
	public Animator anim;
	
	private Vector3 movement;
	private Vector3 acceleration;
	private float accAmnt = 0.01f;
	private Rigidbody playerRigidbody;
	private float speed;
	float gravity = 0f;
	private bool isGrounded;
	private bool isDead;
	private Vector3 spawn;
	private Vector3 direction;
	private float targetTurn = 0f;
	//private bool shooting;
	//private bool f, b, l, r;
	private float ccHeight;
	
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
		
		direction = Vector3.zero;
		speed = 5f;
		//shooting = false;
		isGrounded = true;
		spawn = transform.position;
		RagDoll (false);
		ccHeight = GetComponent<CapsuleCollider>().height;
	}
	
	public void SetRagDoll(bool rag) {
		RagDoll (rag);
	}

	public void setTargetTurn(float target) {
		targetTurn = target;
	}
	
	public bool GetDead() {
		return isDead;
	}
	
	public void Kill() {
		setDead (true);
	}
	
	public void Revive() {
		setDead (false);
	}
	
	public void setDead(bool dead) {
		isDead = dead;
		RagDoll (isDead);
	}
	
	void RagDoll(bool rag) {
		if (rag) {
			anim.enabled = false;
			isDead = true;
		} else {
			anim.enabled = true;
			isDead = false;
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

		LerpRotation ();
		
		playerRigidbody.MovePosition (transform.position + (movement * speed * Time.deltaTime));
		Vector3 vl = playerRigidbody.velocity;
		float newSpeed = 1f - (vl.magnitude);
		if (newSpeed < 0.1f) {
			newSpeed = 0.1f;
		}
		anim.speed = newSpeed;
	}

	private void LerpRotation() {
		if (targetTurn != 0f) {

		}
	}
	
	//respawns, won't be needed in final version
	void Respawn() {
		transform.position = spawn;
	}
	
}