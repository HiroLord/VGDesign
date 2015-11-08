using UnityEngine;
using System.Collections;

public class Showboating : State<BossAgent> {
	private Transform head;
	private Transform leftHand;
	private Transform rightHand;
	private Transform tail;

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
	
	float timer;
	float handTimer;
	// Use this for initialization
	void Start () {
		timer = 0f;
		handTimer = 0f;
	}
	
	//temporarily does nothing
	public override void CheckForNewState()
	{
		/*// If the agent is close to the player then transition into an attack state
	   if(Vector3.Distance(ownerObject.player.transform.position, ownerObject.transform.position) < 10f)
		{
			if(ownerObject.currentEnergy > 500 && ownerObject.currentHealth > 50)
			{
				anim.SetFloat ("Speed", 0.0f);
				ownerStateMachine.CurrentState = new AttackPlayer();
			}
		}*/
	}
	// Update is called once per frame
	public override void Update () {
		//Debug.Log ("timer");
		//head.Rotate(Vector3.up * 50 * Time.deltaTime);
		//leftHand.Translate (Vector3.forward * Time.deltaTime);

		//head animation
		if (timer < 7.5f) {
			head.Translate (Vector3.up * Time.deltaTime);
			head.Rotate (Vector3.left * 10 * Time.deltaTime);
		} else if (timer < 11.5f) {
			head.Translate (Vector3.down * Time.deltaTime);
			head.Rotate (Vector3.up * Time.deltaTime * 100);

		} else if (timer < 15.5) {
			head.Translate (Vector3.right * Time.deltaTime);
			head.Rotate (Vector3.down * Time.deltaTime * 150);
		} else if (timer < 19.5) {
			head.Translate (Vector3.down * Time.deltaTime);
			head.Rotate (Vector3.right * 2 * Time.deltaTime);
		} else if(timer < 28) {
			//reset for loop
			head.rotation = Quaternion.Lerp(head.rotation, startRot, Time.deltaTime);
			head.position = Vector3.Lerp(head.position, startPos, Time.deltaTime);
		} else {
			timer = 0f;
		}

		//hand animation
		if (handTimer < 5f) {
			leftHand.Translate (Vector3.up * 2 * Time.deltaTime);
			rightHand.Translate (Vector3.down * 2 * Time.deltaTime);
		} else if (handTimer < 10f) {
			leftHand.Translate (Vector3.down * 2 * Time.deltaTime);
			rightHand.Translate (Vector3.up * 2 * Time.deltaTime);
		} else if(handTimer < 15) {
			//reset for loop
			leftHand.position = Vector3.Lerp(leftHand.position, leftPos, Time.deltaTime);
			rightHand.position = Vector3.Lerp(rightHand.position, rightPos, Time.deltaTime);
		} else {
			handTimer = 0f;
		}

		timer += 0.1f;
		handTimer += 0.1f;
	
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
	}
}
