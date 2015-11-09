/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System.Collections;

public class TrollMovement : MonoBehaviour {

	public float jumpHeight = 0.4f;
	public float smoothing = 5f;

	Vector3 movement = Vector3.zero;
	Animator anim;
	Rigidbody playerRigidbody;
	CapsuleCollider col;
	AnimatorStateInfo currentBaseState;
	
	static int jumpState = Animator.StringToHash ("Base Layer.Jumping");
	
	void Awake()
	{
		anim = GetComponent<Animator> ();
		playerRigidbody = GetComponent<Rigidbody> ();
		col = GetComponent<CapsuleCollider> ();
	}
	
	void FixedUpdate()
	{
		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw ("Vertical");

		currentBaseState = anim.GetCurrentAnimatorStateInfo (0);
		
		Move (h, v);
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.name == "Death") {
			playerRigidbody.MovePosition (new Vector3 (0f, 0.125f, -12f));
		}
	}
	
	void Move (float h, float v)
	{
		movement = Vector3.Lerp (movement, new Vector3 (h, 0f, v), smoothing * Time.deltaTime);

		anim.SetFloat ("Speed", movement.z);
		anim.SetFloat ("Direction", movement.x);

		Jump ();
	}

	void Jump ()
	{
		if (currentBaseState.fullPathHash == jumpState)
		{
			if (!anim.IsInTransition(0))
			{
				col.height = anim.GetFloat ("ColliderHeight");
				col.center = new Vector3 (0f, anim.GetFloat ("ColliderY"), 0f);
				
				anim.SetBool ("Jump", false);
			}

			Ray ray = new Ray (transform.position + Vector3.up, -Vector3.up);
			RaycastHit hitInfo = new RaycastHit ();

			if (Physics.Raycast (ray, out hitInfo)) {
				if (hitInfo.distance > 1.75f) {
					//anim.MatchTarget (hitInfo.point, Quaternion.identity, AvatarTarget.Root, new MatchTargetWeightMask (new Vector3 (0, 1, 0), 0), 0.35f, 0.5f);
				}
			}
		} else {			
			col.height = 2f;
			col.center = new Vector3 (0f, 1f, 0f);

			if (Input.GetButtonDown ("Jump"))
			{
				anim.SetBool ("Jump", true);
			}
		}
	}
}
