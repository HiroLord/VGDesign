using UnityEngine;
using System.Collections;

public class VampDeath : State<VampBehavior> {
	
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
	
	public override void OnEnable(VampBehavior owner, StateMachine<VampBehavior> newStateMachine)
	{
		// Enable this state and grab components
		owner.CurrentEState = VampBehavior.EState.VampDeath;
		base.OnEnable (owner, newStateMachine);
		anim = owner.GetComponent<Animator> ();
		agent = owner.GetComponent<NavMeshAgent>();
		//anim.SetFloat ("Speed", 1.0f);
		anim.SetBool ("Dead", true);
		agent.Stop ();
	}
}