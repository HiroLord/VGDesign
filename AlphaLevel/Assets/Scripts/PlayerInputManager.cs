﻿/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System.Collections;

public class PlayerInputManager : MonoBehaviour {
	
	public bool isPlayer;

	public int playerID;
	
	//I feel like this should be in a seperate networking manager tbh
	private float h;
	private float v;
	private float oldH = 0;
	private float oldV = 0;
	private float oldRotation = 0f;
	private bool needsUpdate = false;
	private bool turnUpdate = true;
	private float turnTime = 0;
	
	private Movement move;
	private Animator anim;
	private bool shooting = false;
	private bool oldShooting = false;
	private Vector3 movement;
	
	private bool reviveBtn = false;
	
	int floorMask;
	float camRayLength = 100f;
	
	// Use this for initialization
	void Start () {
		move = gameObject.GetComponent<Movement>();
		anim = move.anim;
		movement = move.getMove ();
		floorMask = LayerMask.GetMask ("Floor");
		DontDestroyOnLoad (gameObject);
	}
	
	public bool ReviveBtn() {
		return reviveBtn;
	}

	public void setIsPlayer(bool isPlay) {
		this.isPlayer = isPlay;
	}
	
	void Update () {
		if (ControlInputWrapper.GetButton (ControlInputWrapper.Buttons.A))
			Debug.Log ("A");
		if (isPlayer && !move.GetDead()) {
			h = Input.GetAxisRaw ("Horizontal");
			v = Input.GetAxisRaw ("Vertical");
			float xaxis = ControlInputWrapper.GetAxis(ControlInputWrapper.Axis.LeftStickX);
			float yaxis = ControlInputWrapper.GetAxis(ControlInputWrapper.Axis.LeftStickY);
			if (Mathf.Abs(xaxis) > 0.5f) {
				h = Mathf.Max (Mathf.Min(xaxis, 1f), -1f);
			} else { 
				h = 0;
			}
			if (Mathf.Abs (yaxis) > 0.5f) {
				v = -Mathf.Max (Mathf.Min(yaxis, 1f), -1f);
			} else {
				v = 0;
			}
			if (h != oldH || v != oldV) {
				oldH = h;
				oldV = v;
				needsUpdate = true;
			}
			
			if (Input.GetKey ("space") || ControlInputWrapper.GetButton(ControlInputWrapper.Buttons.RightBumper)) {
				shooting = true;
			} else {
				shooting = false;
			}
			
			if (oldShooting != shooting) {
				needsUpdate = true;
				oldShooting = shooting;
			}
			
			//move into player health script
			/*
			if (Input.GetKey ("k")) {
				move.SetRagDoll (true);
			} else if (Input.GetKey ("l")){
				move.SetRagDoll (false);
			}
			*/
			
			if (Input.GetKey ("r")) {
				reviveBtn = true;
			} else {
				reviveBtn = false;
			}
			
			if (shooting) {
				h = 0;
				v = 0;
			}
		}

		GetComponentInChildren<Shooting> ().setShooting (shooting);
		
		//this should be somewhere else
		/*CapsuleCollider cc = GetComponent<CapsuleCollider> ();
		if (shooting) {
			h = 0;
			v = 0;
			cc.height = ccHeight * 0.8f;
		} else {
			cc.height = ccHeight;
		}*/
		
		move.Move (h, v);
		Turning ();
		movement = move.getMove ();
		
		Vector2 spdir = DetermineDir (h, v);
		anim.SetFloat ("Speed", spdir.x);
		anim.SetFloat ("Direction", spdir.y, .25f, Time.deltaTime);
		anim.SetBool ("Shooting", shooting);
	}
	
	public void Turning() {
		// Isometric turning
		if (isPlayer && !move.GetDead()) {
			Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
			
			RaycastHit floorHit;

			if (Input.GetJoystickNames().Length == 0) {
				if (Physics.Raycast (camRay, out floorHit, camRayLength, floorMask)) {
					Vector3 playerToMouse = floorHit.point - transform.position;
					playerToMouse.y = 0f;
					
					transform.rotation = Quaternion.LookRotation (playerToMouse);
				}
			}
			// Rotation

			float xaxis = ControlInputWrapper.GetAxis(ControlInputWrapper.Axis.RightStickX);
			float yaxis = ControlInputWrapper.GetAxis(ControlInputWrapper.Axis.RightStickY);
			if (Mathf.Abs(xaxis) > 0.1f || Mathf.Abs (yaxis) > 0.1f) {
				Vector3 aiming = new Vector3(xaxis, 0, yaxis);
				transform.rotation = Quaternion.LookRotation (aiming);
			}

			turnTime += Time.deltaTime;
			if (transform.rotation.y != oldRotation && turnTime > 10 * Time.deltaTime) {
				oldRotation = transform.rotation.y;
				turnUpdate = true;
			}
		} 
		// Third person turning
		/* else {
			float turnSpeed = Time.deltaTime * 100;
			if (Input.GetKey ("j")) {
				transform.Rotate (0, -turnSpeed, 0);
			} else if (Input.GetKey ("l")) {
				transform.Rotate (0, turnSpeed, 0);
			}
		} */
	}
	
	public float getH() {
		return h;
	}
	
	public float getV() {
		return v;
	}
	
	public float getRotation() {
		return transform.eulerAngles.y;
	}
	
	public bool getShooting() {
		return shooting;
	}
	
	public bool HasTurned() {
		if (turnUpdate) {
			turnUpdate = false;
			turnTime = 0f;
			return true;
		}
		return false;
	}
	
	public bool NeedsUpdate() {
		if (needsUpdate) {
			needsUpdate = false;
			return true;
		}
		return false;
	}
	
	public void setInputs(float hh, float vv, int shoot) {
		h = hh;
		v = vv;
		if (shoot > 0) {
			shooting = true;
		} else {
			shooting = false;
		}
	}
	
	public void setRotation(float rot) {
		move.setTargetTurn(Quaternion.AngleAxis (rot, new Vector3 (0, 1, 0)));
	}
	
	//Determines what to feed the player animator input based 2D rotation math
	Vector2 DetermineDir(float h, float v) {
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
		return new Vector2(sp,dir);
	}
}