﻿using UnityEngine;
using System.Collections;

public class WarriorAttack : State<WarriorBehavior> 
{

	NavMeshAgent agent;
	Animator anim;
	float speed = 1.0f;
	bool attacking = false;
	float damping = 2.0f;
	Vector3 prevLoc;
	bool hitPlayer = false;
	
	// the Ogre will not flee until he runs out of energy, the troll will not flee if his health is low
	public override void CheckForNewState()
	{
		if(ownerObject.isDead)
		{
			anim.SetFloat ("Speed", 0.0f);
			ownerStateMachine.CurrentState = new WarriorDeath();
		}
	}
	
	public override void Update()
	{
		// Here the AI will try to predict the player's trajectory
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
		
		if(agent.remainingDistance <= ownerObject.attackDist && !attacking && agent.remainingDistance != 0)
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
			hitPlayer = false;
			
		}
		
		if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.7f && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.4f &&
		   !anim.IsInTransition(0))
		{
			float dist = Vector3.Distance (ownerObject.transform.position, ownerObject.currTarget.position);
			if(dist < ownerObject.attackDist + 0.3f  && !hitPlayer)
			{
				Player ent = ownerObject.currTarget.GetComponent<Player>();
				ent.TakeDamage (20, ownerObject.transform.position);
				hitPlayer = true;
			}
		}
		anim.SetFloat ("Speed", speed);
	}
	
	public override void OnEnable(WarriorBehavior owner, StateMachine<WarriorBehavior> newStateMachine)
	{
		// Get components and set target to player
		base.OnEnable (owner, newStateMachine);
		anim = owner.GetComponent<Animator> ();
		agent = owner.GetComponent<NavMeshAgent>();
		anim.SetFloat ("Speed", 1.0f);
		AudioSource yell = ownerObject.GetComponent<AudioSource> ();
		if (yell != null)
			yell.PlayOneShot (yell.clip, 1.0f);
		//owner.currTarget = owner.player;
	}
}
