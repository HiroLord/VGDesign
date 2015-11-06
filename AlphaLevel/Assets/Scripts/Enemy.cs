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

	private Animator anim;
	private NavMeshAgent agent;
	private StateMachine<Enemy> stateMachine;


	public int startingHealth = 100;
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
		stateMachine = new StateMachine<Enemy> (new MoveAlongPath (), this);
		currentHealth = startingHealth;
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
		
		// enemyAudio.Play();
		
		// For funsies
		//		hitPoint.Scale (new Vector3 (100f, 100f, 100f));
		//		GetComponent <Rigidbody> ().AddForce (hitPoint);
		
		currentHealth -= amount;
		
		// hitParticles.transform.position = hitPoint;
		// hitParticles.Play();
		
		if(currentHealth <= 0)
		{
			//Death();
		}
	}

	void Death ()
	{
		isDead = true;
		Destroy (gameObject);
		//		capsuleCollider.isTrigger = true;
		//		
		//		anim.SetTrigger ("Dead");
		//		
		//		enemyAudio.clip = deathClip;
		//		enemyAudio.Play ();
	}
}
