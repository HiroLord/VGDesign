using UnityEngine;
using System.Collections;

public class TrollController : MonoBehaviour {
	Animator anim;
	Rigidbody rigid;
	Vector3 move;
	float speed = 0f;
	float attackTimer = 0f;
	Vector3 local;
	int floorMask;
	bool block = false;
	void Awake() {
		floorMask = LayerMask.GetMask ("Floor");
		anim = GetComponent<Animator> ();
		rigid = GetComponent<Rigidbody> ();
		local = transform.position;
	}

	void FixedUpdate() {
		bool b = Input.GetKey (KeyCode.Space);
		bool attack = Input.GetKeyDown (KeyCode.A);
		Debug.Log (b);
		bool moving = Input.GetMouseButton (0);

		if ((b) && (!block)) {
			anim.SetBool ("Block", true);
			anim.SetTrigger ("BlockStart");
			block = true;
		} else if (!b && block) {
			anim.SetBool ("Block", false);
			anim.ResetTrigger ("BlockStart");
			block = false;
		}

		if (attack) {
			anim.SetTrigger ("Attack");
		}

		if (!b) {
			//Jump (j);
			if (moving) {
				Move ();
			}
			Turning ();
			Animating (moving);
		}
	}

	void Turning() {
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit floorHit;
		
		if (Physics.Raycast (camRay, out floorHit, 100f, floorMask)) {
			Vector3 playerToMouse = floorHit.point - transform.position;
			playerToMouse.y = 0f;
			Quaternion newRotation = Quaternion.LookRotation (playerToMouse);
			rigid.MoveRotation (newRotation);
		}
		
	}

	void Move() {
		//move.Set (h, 0f, v);
		move = transform.forward * speed * Time.deltaTime;
		rigid.MovePosition (transform.position + move);
	}

	void Animating(bool walking) {
		if (walking && (speed < 3f)) {
			speed += 0.01f;
		} else if ((!walking) && (speed > 0f)) {
			speed -= 0.1f;
		}
		anim.SetFloat ("Speed", speed);
		anim.SetBool ("IsWalking", walking);
	}
}