using UnityEngine;
using System.Collections;

public class WarriorPatrol : State<WarriorBehavior> {

	NavMeshAgent agent;
	Animator anim;
	
	public override void CheckForNewState()
	{
		if(ownerObject.playerFound)
		{
			ownerStateMachine.CurrentState = new WarriorAttack();
		}
		
		if(ownerObject.isDead)
		{
			anim.SetFloat ("Speed", 0.0f);
			ownerStateMachine.CurrentState = new WarriorDeath();
		}
	}
	
	public override void Update()
	{
		// If the agent is close to his waypoint then move on to the next one
		if(agent.remainingDistance < 2)
		{
			ownerObject.GiveHealth(10);
			ownerObject.currPoint += 1;
			if(ownerObject.currPoint >= ownerObject.pointsLen)
				ownerObject.currPoint = 0;
			ownerObject.currTarget = ownerObject.points[ownerObject.currPoint];
		}
		ownerObject.GiveEnergy (1);
		agent.SetDestination (ownerObject.currTarget.position);
		anim.SetFloat ("Speed", 1.0f);
	}
	
	public override void OnEnable(WarriorBehavior owner, StateMachine<WarriorBehavior> newStateMachine)
	{
		// Enable this state and grab components
		base.OnEnable (owner, newStateMachine);
		anim = owner.GetComponent<Animator> ();
		agent = owner.GetComponent<NavMeshAgent>();
		anim.SetFloat ("Speed", 1.0f);
	}
}
