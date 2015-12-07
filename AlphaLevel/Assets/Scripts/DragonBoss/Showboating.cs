/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System.Collections;

public class Showboating : State<BossAgent> {
	private Transform head;
	private Transform leftHand;
	private Transform rightHand;
	private Transform tail;
	private Transform center;

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

	private BossHealth health;
	
	float timer;
	float handTimer;
	float tailTimer;
	// Use this for initialization
	void Start () {
		timer = 0f;
		handTimer = 0f;
		tailTimer = 0f;
	}
	
	//temporarily does nothing
	public override void CheckForNewState()
	{
		//ownerStateMachine.CurrentState = new FireAtPlayer();
		// If the agent is close to the player then transition into an attack state
	   //if(Vector3.Distance(ownerObject.player.transform.position, ownerObject.transform.position) < 35f)
		if ((timer >= 8.9f) && (handTimer >= 3.8f))
		{
			ownerStateMachine.CurrentState = new FireAtPlayer();
		}

		if (health.health <= 0) {
			ownerStateMachine.CurrentState = new BossDie ();
		}
	}
	// Update is called once per frame
	public override void Update () {
		//Debug.Log ("timer");
		//head.Rotate(Vector3.up * 50 * Time.deltaTime);
		//leftHand.Translate (Vector3.forward * Time.deltaTime);
		Debug.Log (handTimer);
		//head animation
		if (timer < 2.5f) {
			head.Translate (Vector3.up * Time.deltaTime);
			head.Rotate (Vector3.left * 10 * Time.deltaTime);
		} else if (timer < 3.1f) {
			head.Translate (Vector3.down * Time.deltaTime);
			head.Rotate (Vector3.up * Time.deltaTime * 100);
		} else if (timer < 3.8f) {
			head.Translate (Vector3.right * Time.deltaTime);
			head.Rotate (Vector3.down * Time.deltaTime * 150);
		} else if (timer < 5.6f) {
			head.Translate (Vector3.down * Time.deltaTime);
			head.Rotate (Vector3.right * 2 * Time.deltaTime);
			head.Rotate (Vector3.up * 2 * Time.deltaTime);
		} else if(timer < 9f) {
			//reset for loop
			head.rotation = Quaternion.Lerp(head.rotation, startRot, Time.deltaTime);
			head.position = Vector3.Lerp(head.position, startPos, Time.deltaTime);
		}

		//hand animation
		if (handTimer < 2f) {
			leftHand.Translate (Vector3.up * 2 * Time.deltaTime);
			rightHand.Translate (Vector3.down * 2 * Time.deltaTime);
		} else if (handTimer < 4f) {
			leftHand.Translate (Vector3.down * 2 * Time.deltaTime);
			rightHand.Translate (Vector3.up * 2 * Time.deltaTime);
		} else if(handTimer < 4.14f) {
			//reset for loop
			leftHand.position = Vector3.Lerp(leftHand.position, leftPos, Time.deltaTime);
			rightHand.position = Vector3.Lerp(rightHand.position, rightPos, Time.deltaTime);
		} else {
			handTimer = 0f;
		}

		//tail animation
		float cx = center.position.x - 5f;
		float cz = center.position.z - 5f;
		float radius = 5f;
		float newx = Mathf.Sin(tailTimer) * radius;
		float newz = Mathf.Cos(tailTimer) * radius;
		float newy = Mathf.Cos (tailTimer)*2.5f;
		Vector3 pos = new Vector3(cx+newx,newy,cz + newz);
		tail.position = pos;

		
		timer += Time.deltaTime;
		handTimer += Time.deltaTime;
		tailTimer += Time.deltaTime;
	
	}

	public override void OnEnable(BossAgent owner, StateMachine<BossAgent> newStateMachine)
	{
		owner.CurrentEState = BossAgent.EState.Showboating;
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
		health = owner.health;
		center = owner.center.transform;
	}
}
