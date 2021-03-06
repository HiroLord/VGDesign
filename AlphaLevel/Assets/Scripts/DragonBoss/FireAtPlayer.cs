﻿/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System.Collections;

public class FireAtPlayer : State<BossAgent> {
	private Transform head;
	private Transform leftHand;
	private Transform rightHand;
	private Transform tail;
	private Transform origin;
	private GameObject fireball;
	
	private Quaternion startRot;
	private Vector3 startPos;
	
	private Quaternion leftRot;
	private Vector3 leftPos;
	
	private Quaternion rightRot;
	private Vector3 rightPos;
	
	private Vector3 headDir;
	private Vector3 leftDir;
	private Vector3 rightDir;
	private Vector3 tailDir;
	
	private GameObject target;
	private BossHealth health;

	float timer;
	float handTimer;
	float fireCooldown;
	float fireTime = 3.5f;
	// Use this for initialization
	void Start () {
		timer = 0f;
		handTimer = 0f;
	}
	
	//temporarily does nothing
	public override void CheckForNewState()
	{
		if (health.health <= 0) {
			ownerStateMachine.CurrentState = new BossDie ();
		}

		if (timer > 7f) {
			ownerStateMachine.CurrentState = new HandSpinDeath();
		}
	}
	// Update is called once per frame
	public override void Update () {
		Vector3 lookAt = ownerObject.currTarget.position;
		lookAt.y += 1f;

		Vector3 relativePos =  origin.position - lookAt;
		Quaternion rotation = Quaternion.LookRotation(relativePos);
		head.rotation = Quaternion.Lerp(head.rotation, rotation, Time.deltaTime);

		//Vector3 fireVector = targetTransform.position - origin.position;
		//Debug.DrawLine (origin.position, targetTransform.position);
		if (fireCooldown <= 0) {
			Fireball.createFireball(fireball, origin.position, lookAt);
			fireCooldown = fireTime;
		}
		fireCooldown -= 0.1f;
		
		//hand animation
		if (handTimer < 5f) {
			leftHand.Translate (Vector3.down * 3 * Time.deltaTime);
			rightHand.Translate (Vector3.down * 3 * Time.deltaTime);
		} else if(handTimer < 15) {
			//reset for loop
			leftHand.position = Vector3.Lerp(leftHand.position, leftPos, Time.deltaTime);
			rightHand.position = Vector3.Lerp(rightHand.position, rightPos, Time.deltaTime);
		} else {
			handTimer = 0f;
		}
	
		handTimer += Time.deltaTime;
		timer += Time.deltaTime;
	}

	//To rotate the player currently being targeted
	public void targetPlayer(GameObject player) {
		target = player;
		ownerObject.currTarget = target.transform.Find ("ActualTransform");
	}

	public override void OnEnable(BossAgent owner, StateMachine<BossAgent> newStateMachine)
	{
		// Enable this state and grab components
		owner.CurrentEState = BossAgent.EState.FireAtPlayer;
		base.OnEnable (owner, newStateMachine);
		head = owner.head.transform;
		leftHand = owner.leftHand.transform;
		rightHand = owner.rightHand.transform;
		tail = owner.tail.transform;
		startRot = head.rotation;
		startPos = head.position;
		leftPos = leftHand.position;
		rightPos = rightHand.position;
		origin = owner.origin.transform;
		health = owner.health;
		fireball = owner.fireball;
		//right now just attacks the first player in the array;
		targetPlayer (ownerObject.players [0]);
	}
	
}