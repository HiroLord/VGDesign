/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(CapsuleCollider), typeof(Rigidbody))]
public class OgreBehavior : Entity 
{
	public Transform currTarget;
	public string enemyType;
	
	private Animator anim;
	private NavMeshAgent agent;
	private StateMachine<OgreBehavior> stateMachine;

	public float attackDist = 1.2f;
	public bool playerFound;
	
	// Use this for initialization
	void Start () 
	{
		agent = GetComponent<NavMeshAgent> ();
		anim = GetComponent<Animator> ();
		stateMachine = new StateMachine<OgreBehavior> (new Guard (), this);
		currentHealth = startingHealth;
		currentEnergy = startingEnergy;
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
