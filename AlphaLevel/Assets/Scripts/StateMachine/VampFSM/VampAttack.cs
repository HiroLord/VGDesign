/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System.Collections;

public class VampAttack : State<VampBehavior> 
{
	NavMeshAgent agent;
	Animator anim;
	float speed = 1.0f;
	bool attacking = false;
	float damping = 2.0f;
	Vector3 prevLoc;
	bool hitPlayer = false;
	ParticleSystem[] parts;
	Light lgt;
	
	// the Ogre will not flee until he runs out of energy, the troll will not flee if his health is low
	public override void CheckForNewState()
	{
		if(ownerObject.isDead && ownerObject.original)
		{
			anim.SetFloat ("Speed", 0.0f);
			ownerStateMachine.CurrentState = new VampDeath();
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
		
		if(agent.remainingDistance <= ownerObject.attackDist-1 && !attacking && agent.remainingDistance != 0)
		{
			parts[0].Clear ();
			parts[0].Play ();
			lgt.enabled = true;
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
			lgt.enabled = false;
		}
		else if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !anim.IsInTransition(0))
		{
			attacking = false;
			agent.Resume ();
			speed = 1.0f;
			anim.SetFloat ("Speed", speed);
			agent.SetDestination (finalDest);
			hitPlayer = false;
			lgt.enabled = false;
			
		}
		
		if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.7f && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.4f &&
		   !anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsTag ("Attack"))
		{
			Debug.Log("Success");
			float dist = Vector3.Distance (ownerObject.transform.position, ownerObject.currTarget.position);
			if(dist < ownerObject.attackDist + 0.3f  && !hitPlayer)
			{
				Player ent = ownerObject.currTarget.GetComponent<Player>();
				ent.TakeDamage (20, ownerObject.transform.position);
				ownerObject.GiveHealth (20);
				hitPlayer = true;
				parts[1].Clear ();
				Vector3 set = ent.transform.position;
				set.y += 1;
				parts[1].transform.position = set;
				parts[1].transform.rotation = ent.transform.rotation;
				parts[1].Play();
			}
			parts[0].Stop ();
			lgt.enabled = false;
		}
		anim.SetFloat ("Speed", speed);
	}
	
	public override void OnEnable(VampBehavior owner, StateMachine<VampBehavior> newStateMachine)
	{
		// Get components and set target to player
		owner.CurrentEState = VampBehavior.EState.VampAttack;
		base.OnEnable (owner, newStateMachine);
		anim = owner.GetComponent<Animator> ();
		agent = owner.GetComponent<NavMeshAgent>();
		anim.SetFloat ("Speed", 1.0f);
		parts = owner.GetComponentsInChildren<ParticleSystem> ();
		lgt = owner.GetComponentInChildren <Light> ();
		lgt.enabled = false;
		agent.SetDestination (owner.currTarget.position);
		//part = owner.GetComponentInChildren<ParticleSystem> ();
//		AudioSource groan = ownerObject.GetComponent<AudioSource> ();
//		if(groan != null)
//			groan.PlayOneShot (groan.clip, 1.0f);
		//owner.currTarget = owner.player;
	}
}
