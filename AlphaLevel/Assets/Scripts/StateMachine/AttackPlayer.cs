using UnityEngine;
using System.Collections;

public class AttackPlayer : State<Enemy> 
{
	NavMeshAgent agent;
	Animator anim;
	float speed = 1.0f;
	bool attacking = false;
	bool keepCheck = false;
	static int attackState = Animator.StringToHash ("Base Layer.Attack");
	float time = 0.0f;
	float damping = 2.0f;
	
	
	public override void CheckForNewState()
	{
		// If the agent has low health, then flee from the player
		if(ownerObject.currentHealth <= 20)
		{
			anim.SetFloat ("Speed", 0.0f);
			ownerStateMachine.CurrentState = new FleeFromPlayer();
			agent.Resume ();
		}

		if(ownerObject.currentEnergy <= 200)
		{
			anim.SetFloat ("Speed", 0.0f);
			ownerStateMachine.CurrentState = new MoveAlongPath();
			agent.Resume ();
		}
	}
	
	public override void Update()
	{
		// Rotate the agent towards the player he is attacking
		Quaternion rot = Quaternion.LookRotation(ownerObject.currTarget.position - ownerObject.transform.position);
		ownerObject.transform.rotation = Quaternion.Slerp(ownerObject.transform.rotation, rot, Time.deltaTime * damping);

		// If they agent is attacking then increment time
		if (attacking)
		{
			time += Time.deltaTime;
			if(time > 1.5f)
			{
				attacking = false;
				time = 0.0f;
				agent.Resume ();
				speed = 0.5f;
			}
		}


		if(agent.remainingDistance < 3f && agent.remainingDistance > 1f && !attacking)
		{
			speed -= 0.05f;
			if(speed < 0.0f)
				speed = 0.0f;
			agent.SetDestination (ownerObject.currTarget.position);
		}
		else if(agent.remainingDistance > 3f && !attacking)
		{
			speed = 1.0f;
			agent.SetDestination (ownerObject.currTarget.position);
		}
		else if(agent.remainingDistance <= 1f && (time == 0.0f || time > 1.0f) && !attacking)
		{
			//time = 0.0f;
			speed = 0.0f;
			anim.SetTrigger ("Attack");
			time += Time.deltaTime;
			attacking = true;
			agent.Stop ();
			ownerObject.TakeEnergy(100);
		}
		else if(attacking)
		{
		}
		else
		{
			speed += 0.11f;
			if(speed > 1.0f)
				speed = 1.0f;
			agent.Resume ();
		}
		anim.SetFloat ("Speed", speed);
	}
	
	public override void OnEnable(Enemy owner, StateMachine<Enemy> newStateMachine)
	{
		// Get components and set target to player
		base.OnEnable (owner, newStateMachine);
		anim = owner.GetComponent<Animator> ();
		agent = owner.GetComponent<NavMeshAgent>();
		owner.currTarget = owner.player;
	}
}
