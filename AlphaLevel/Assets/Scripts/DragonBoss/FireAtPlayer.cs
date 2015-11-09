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
	}
	// Update is called once per frame
	public override void Update () {
		Vector3 relativePos =  head.position - ownerObject.player.transform.position;
		Quaternion rotation = Quaternion.LookRotation(relativePos);
		head.rotation = Quaternion.Lerp(head.rotation, rotation, Time.deltaTime);

		Vector3 fireVector = ownerObject.player.transform.position - head.position;

		if (fireCooldown <= 0) {
			fireball.createFireball(origin.position, fireVector);
			fireCooldown = 3.5f;
		}
		fireCooldown -= 0.1f;
		/*Debug.Log ("timer");
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
		handTimer += 0.1f;*/
		
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