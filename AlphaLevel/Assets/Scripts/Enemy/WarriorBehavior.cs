/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System.Collections;

public class WarriorBehavior : Entity 
{
	public Transform currTarget;
	public string enemyType;
	
	private Animator anim;
	private NavMeshAgent agent;
	private StateMachine<WarriorBehavior> stateMachine;

	public int currPoint;
	public int pointsLen;
	public Transform[] points;
	
	public float attackDist = 1.0f;
	public bool playerFound;
	
	// Use this for initialization
	void Start () 
	{
		agent = GetComponent<NavMeshAgent> ();
		anim = GetComponent<Animator> ();
		stateMachine = new StateMachine<WarriorBehavior> (new WarriorPatrol (), this);
		currentHealth = startingHealth;
		currentEnergy = startingEnergy;
		currPoint = 0;
		currTarget = points [currPoint];
		pointsLen = points.Length;
	}
	
	// Update is called once per frame
	void Update () 
	{
		stateMachine.Update ();
	}
	
	void OnTriggerEnter(Collider col)
	{
		if(col.tag == "Player")
		{
			playerFound = true;
			//player = col.transform;
			currTarget = col.transform;
		}
	}
	
	public void Death ()
	{
		Destroy (gameObject);
	}
}
