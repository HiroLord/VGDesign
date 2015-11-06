using UnityEngine;
using System.Collections;

public class AttackPlayer : State<Enemy> 
{
	NavMeshAgent agent;
	Animator anim;
	float speed = 1.0f;
	bool attacking = false;
	float damping = 2.0f;
	Vector3 prevLoc;
	
	// the Ogre will not flee until he runs out of energy, the troll will not flee if his health is low
	public override void CheckForNewState()
	{
		// If the agent has low health, then flee from the player
		if(ownerObject.currentHealth <= 20 && ownerObject.enemyType != "Troll")
		{
			anim.SetFloat ("Speed", 0.0f);
			ownerStateMachine.CurrentState = new FleeFromPlayer();
			agent.Resume ();
		}

		if(ownerObject.currentEnergy <= 200 && (ownerObject.enemyType != "Ogre" || ownerObject.currentEnergy == 0))
		{
			anim.SetFloat ("Speed", 0.0f);
			ownerStateMachine.CurrentState = new Patrol();
			agent.Resume ();
		}
	}
	
	public override void Update()
	{
		Vector3 finalDest = ownerObject.currTarget.position;
		if(prevLoc == null)
		{
			prevLoc = finalDest;
		}
		else
		{
			Vector3 vec = ownerObject.currTarget.position - prevLoc;
			finalDest += vec;
			prevLoc = ownerObject.currTarget.position;
		}

		// Rotate the agent towards the player he is attacking
		Quaternion rot = Quaternion.LookRotation(ownerObject.currTarget.position - ownerObject.transform.position);
		ownerObject.transform.rotation = Quaternion.Slerp(ownerObject.transform.rotation, rot, Time.deltaTime * damping);

		if(agent.remainingDistance <= ownerObject.attackDist && !attacking)
		{
			speed = 0.0f;
			anim.SetFloat ("Speed", speed);
			anim.SetTrigger ("Attack");
			attacking = true;
			agent.Stop ();
			ownerObject.TakeEnergy(100);
		}
		else if(agent.remainingDistance < 5f && agent.remainingDistance > ownerObject.attackDist && !attacking)
		{
			speed -= 0.01f;
			if(speed < 0.0f)
				speed = 0.0f;
			agent.SetDestination (finalDest);
		}
		else if(agent.remainingDistance > 5f && !attacking)
		{
			speed = 1.0f;
			agent.SetDestination (finalDest);
		}
		else if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !anim.IsInTransition(0))
		{
			attacking = false;
			agent.Resume ();
			speed = 1.0f;
			anim.SetFloat ("Speed", speed);
			agent.SetDestination (finalDest);
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
