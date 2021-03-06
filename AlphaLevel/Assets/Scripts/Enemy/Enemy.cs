﻿/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(CapsuleCollider), typeof(Rigidbody))]
public class Enemy : MonoBehaviour 
{
	public Transform currTarget;
	public int currPoint;
	public Transform[] points;
	public int pointsLen;
	public Transform player;
	public int currentHealth;
	public int currentEnergy;
	public string enemyType;

	private Animator anim;
	private NavMeshAgent agent;
	private StateMachine<Enemy> stateMachine;


	public int startingHealth = 100;
	public int startingEnergy = 1000;
	public float attackDist = 1.0f;
	bool isDead;

	// Use this for initialization
	void Start () 
	{
		agent = GetComponent<NavMeshAgent> ();
		anim = GetComponent<Animator> ();
		//points = new Transform[3];
		currPoint = 0;
		currTarget = points [currPoint];
		pointsLen = points.Length;
		stateMachine = new StateMachine<Enemy> (new Patrol (), this);
		currentHealth = startingHealth;
		currentEnergy = startingEnergy;
	}
	
	// Update is called once per frame
	void Update () 
	{
		stateMachine.Update ();
	}

	public void TakeDamage(int amount, Vector3 hitPoint)
	{
		if (isDead)
			return;
		currentHealth -= amount;
		
		if(currentHealth <= 0)
		{
			//Death();
			currentHealth = 0;
		}
	}

	public void TakeEnergy(int amount)
	{
		currentEnergy -= amount;
		if (currentEnergy < 0)
			currentEnergy = 0;
	}

	public void GiveHealth(int amount)
	{
		currentHealth += amount;
		if (currentHealth > startingHealth)
			currentHealth = startingHealth;
	}

	public void GiveEnergy(int amount)
	{
		currentEnergy += amount;
		if (currentEnergy > startingEnergy)
			currentEnergy = startingEnergy;
	}

	void Death ()
	{
		isDead = true;
		Destroy (gameObject);
	}
}
