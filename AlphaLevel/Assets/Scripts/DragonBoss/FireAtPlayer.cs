using UnityEngine;
using System.Collections;

public class FireAtPlayer : State<BossAgent> {
	private Fireball fireball;

	private Transform head;
	private Transform leftHand;
	private Transform rightHand;
	private Transform tail;
	private Transform origin;
	
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
	float fireCooldown;
	float fireTime = 2.0f;
	// Use this for initialization
	void Start () {
		timer = 0f;
		handTimer = 0f;
	}
	
	//temporarily does nothing
	public override void CheckForNewState()
	{
		if (health.health <= 0) {
			ownerStateMachine.CurrentState = new Die ();
		}

		if (timer > 14.5) {
			ownerStateMachine.CurrentState = new HandSpinDeath();
		}
	}
	// Update is called once per frame
	public override void Update () {
		Vector3 relativePos =  head.position - ownerObject.players[0].transform.position;
		Quaternion rotation = Quaternion.LookRotation(relativePos);
		head.rotation = Quaternion.Lerp(head.rotation, rotation, Time.deltaTime);

		Vector3 fireVector = ownerObject.players[0].transform.position - head.position;

		if (fireCooldown <= 0) {
			fireball.createFireball(origin.position, fireVector);
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
	
		handTimer += 0.1f;
		timer += 0.1f;
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
		fireball = owner.fire;
		origin = owner.origin.transform;
		health = owner.health;
	}
}