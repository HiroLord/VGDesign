/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System.Collections;

public class VampGuard : State<VampBehavior>
{
	NavMeshAgent agent;
	Animator anim;
	
	
	public override void CheckForNewState()
	{
		if(ownerObject.playerFound && ownerObject.original)
		{
			ownerStateMachine.CurrentState = new VampAttack();
		}
		
		if(ownerObject.isDead && ownerObject.original)
		{
			anim.SetFloat ("Speed", 0.0f);
			ownerStateMachine.CurrentState = new VampDeath();
		}
	}
	
	public override void Update()
	{

	}
	
	public override void OnEnable(VampBehavior owner, StateMachine<VampBehavior> newStateMachine)
	{
		// Enable this state and grab components
		owner.CurrentEState = VampBehavior.EState.VampGuard;
		base.OnEnable (owner, newStateMachine);
		anim = owner.GetComponent<Animator> ();
		agent = owner.GetComponent<NavMeshAgent>();
	}
}