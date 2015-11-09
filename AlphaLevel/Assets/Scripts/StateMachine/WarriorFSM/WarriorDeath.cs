using UnityEngine;
using System.Collections;

public class WarriorDeath : State<WarriorBehavior> {

	NavMeshAgent agent;
	Animator anim;
	float timeWait = 0.0f;
	
	
	public override void CheckForNewState()
	{
		
	}
	
	public override void Update()
	{
		if(timeWait > 5.0f)
		{
			ownerObject.Death();
		}
		timeWait += Time.deltaTime;
	}
	
	public override void OnEnable(WarriorBehavior owner, StateMachine<WarriorBehavior> newStateMachine)
	{
		// Enable this state and grab components
		base.OnEnable (owner, newStateMachine);
		anim = owner.GetComponent<Animator> ();
		agent = owner.GetComponent<NavMeshAgent>();
		//anim.SetFloat ("Speed", 1.0f);
		anim.SetBool ("Dead", true);
		agent.Stop ();
	}
}
