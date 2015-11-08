/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(CapsuleCollider), typeof(Rigidbody))]
public class Enemy : Entity 
{
	public Transform currTarget;
	public int currPoint;
	public Transform[] points;
	public int pointsLen;
	public Transform player;
	public string enemyType;

	private Animator anim;
	private NavMeshAgent agent;
	private StateMachine<Enemy> stateMachine;

	public float attackDist = 1.0f;

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
	void Death ()
	{
		//isDead = true;
		Destroy (gameObject);
	}
}
