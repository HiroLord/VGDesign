using UnityEngine;
using System.Collections;

public class FleeFromPlayer : State<Enemy>
{
	NavMeshAgent agent;
	Animator anim;
	
	
	public override void CheckForNewState()
	{
		if(agent.remainingDistance < 5f)
		{
			anim.SetFloat ("Speed", 0.0f);
			ownerStateMachine.CurrentState = new MoveAlongPath();
		}
	}
	
	public override void Update()
	{
		agent.SetDestination (ownerObject.currTarget.position);
		anim.SetFloat ("Speed", 1.0f);
	}
	
	public override void OnEnable(Enemy owner, StateMachine<Enemy> newStateMachine)
	{
		base.OnEnable (owner, newStateMachine);
		anim = owner.GetComponent<Animator> ();
		agent = owner.GetComponent<NavMeshAgent>();

		float minDist = Vector3.Distance (ownerObject.transform.position, ownerObject.points [0].position);
		int index = 0;
		for(int i = 1; i < ownerObject.pointsLen; i++)
		{
			float curr = Vector3.Distance (ownerObject.transform.position, ownerObject.points[i].position);
			if(curr < minDist)
			{
				minDist = curr;
				index = i;
			}
		}

		ownerObject.currPoint = index;
		ownerObject.currTarget = ownerObject.points [index];
	}
}
