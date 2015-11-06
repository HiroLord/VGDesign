using UnityEngine;
using System.Collections;

public class MoveAlongPath : State<Enemy>
{
	NavMeshAgent agent;
	Animator anim;


	public override void CheckForNewState()
	{
		// If the agent is close to the player then transition into an attack state
		if(Vector3.Distance(ownerObject.player.transform.position, ownerObject.transform.position) < 10f)
		{
			if(ownerObject.currentEnergy > 500 && ownerObject.currentHealth > 50)
			{
				anim.SetFloat ("Speed", 0.0f);
				ownerStateMachine.CurrentState = new AttackPlayer();
			}
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

	public override void OnEnable(Enemy owner, StateMachine<Enemy> newStateMachine)
	{
		// Enable this state and grab components
		base.OnEnable (owner, newStateMachine);
		anim = owner.GetComponent<Animator> ();
		agent = owner.GetComponent<NavMeshAgent>();
	}

}
