using UnityEngine;
using System.Collections;

public class GoblinDeath : State<GoblinBehavior> {
	
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
	
	public override void OnEnable(GoblinBehavior owner, StateMachine<GoblinBehavior> newStateMachine)
	{
		// Enable this state and grab components
		owner.CurrentEState = GoblinBehavior.EState.GoblinDeath;
		base.OnEnable (owner, newStateMachine);
		anim = owner.GetComponent<Animator> ();
		agent = owner.GetComponent<NavMeshAgent>();
		//anim.SetFloat ("Speed", 1.0f);
		anim.SetBool ("Dead", true);
		agent.Stop ();
	}
}