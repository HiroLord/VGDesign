/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System.Collections;

public class HandSpinDeath : State<BossAgent> {
	
	private Transform head;
	private Transform leftHand;
	private Transform rightHand;
	private Transform tail;
	private Transform origin;
	private Transform center;
	private Quaternion startRot;
	private Vector3 startPos;
	
	private Quaternion leftRot;
	private Vector3 leftPos;
	
	private Quaternion rightRot;
	private Vector3 rightPos;

	
	private BossHealth health;
	
	float timer;
	float handTimer;
	float fireCooldown;
	Vector3 sRight;
	Vector3 sLeft;
	// Use this for initialization
	void Start () {
		timer = 0f;
		handTimer = 0f;
		//move these circling calculations into their own function
		float cx = center.position.x;
		float cz = center.position.z;
		float radius = 11f;
		float newx = Mathf.Sin(0.2f * 3) * radius;
		float newz = Mathf.Cos(0.2f * 3) * radius;
		sRight = new Vector3(cx-newx,2.0f,cz-newz);
		sLeft = new Vector3(cx+newx,2.0f,cz + newz);
	}
	
	//temporarily does nothing
	public override void CheckForNewState()
	{
		if (health.health <= 0) {
			ownerStateMachine.CurrentState = new Die ();
		}

		if (handTimer > 20f) {
			ownerStateMachine.CurrentState = new Showboating ();
		}
	}
	// Update is called once per frame
	public override void Update () {
		Vector3 relativePos =  head.position - ownerObject.players[0].transform.position;
		Quaternion rotation = Quaternion.LookRotation(relativePos);
		head.rotation = Quaternion.Lerp(head.rotation, rotation, Time.deltaTime);
		
		Vector3 fireVector = ownerObject.players[0].transform.position - head.position;

		if (timer < 2f) {
			leftHand.Translate (Vector3.up * 5 * Time.deltaTime);
			rightHand.Translate (Vector3.up * 5 * Time.deltaTime);
		} else if (timer < 2.5f) {
			leftHand.Translate (Vector3.left * 5 * Time.deltaTime);
			rightHand.Translate (Vector3.right * 5 * Time.deltaTime);
		} else if (timer < 3f) {
			leftHand.Translate (Vector3.down * 16 * Time.deltaTime);
			rightHand.Translate (Vector3.down * 16 * Time.deltaTime);
		} else if(handTimer < 0.25f) {
			//set for circling
			//I can't get this to work properly
			//leftHand.position = Vector3.Lerp(leftHand.position, sLeft, Time.deltaTime * 0.2f);
			//rightHand.position = Vector3.Lerp(rightHand.position, sRight, Time.deltaTime);
		} else if(handTimer < 15f) {
			//move in a cicle
			float cx = center.position.x;
			float cz = center.position.z;
			float radius = 11f;
			float newx = Mathf.Sin(handTimer*3) * radius;
			float newz = Mathf.Cos(handTimer*3) * radius;
			Vector3 right = new Vector3(cx-newx,2.0f,cz-newz);
			Vector3 left = new Vector3(cx+newx,2.0f,cz + newz);
			leftHand.position = left;
			rightHand.position = right;
		} else if(handTimer < 20f) {
			//reset for loop
			leftHand.position = Vector3.Lerp(leftHand.position, leftPos, Time.deltaTime);
			rightHand.position = Vector3.Lerp(rightHand.position, rightPos, Time.deltaTime);
		}

		if (timer < 3f) {
			timer += Time.deltaTime;
		} else {
			handTimer += Time.deltaTime;
		}
		
	}
	
	public override void OnEnable(BossAgent owner, StateMachine<BossAgent> newStateMachine)
	{
		// Enable this state and grab components
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
		center = owner.center.transform;
	}
}