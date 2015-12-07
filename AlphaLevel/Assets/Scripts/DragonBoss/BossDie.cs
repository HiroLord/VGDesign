/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System.Collections;

public class BossDie : State<BossAgent> {
	private Transform head;
	private Transform leftHand;
	private Transform rightHand;
	private Transform tail;
	GameObject[] parts;

	
	//this is a dead state
	public override void CheckForNewState()
	{
		return;
	}
	// Update is called once per frame
	public override void Update () {
		return;	
	}
	
	public override void OnEnable(BossAgent owner, StateMachine<BossAgent> newStateMachine)
	{
		owner.CurrentEState = BossAgent.EState.Die;
		parts = new GameObject[4];
		// Enable this state and grab components
		base.OnEnable (owner, newStateMachine);
		owner.CurrentHealth = 0;
		parts [0] = owner.head;
		parts[1] = owner.leftHand;
		parts[2] = owner.rightHand;
		parts[3] = owner.tail;
		parts[1].GetComponentInChildren<Collider> ().isTrigger = false;
		parts[2].GetComponentInChildren<Collider> ().isTrigger = false;
		foreach (GameObject prop in parts) {
			Rigidbody rigid = prop.GetComponent<Rigidbody>();
			rigid.constraints = RigidbodyConstraints.None;
			rigid.useGravity = (true);
		}
	}
}