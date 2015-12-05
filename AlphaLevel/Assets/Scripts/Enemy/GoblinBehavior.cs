/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(CapsuleCollider), typeof(Rigidbody))]
public class GoblinBehavior : EnemyNetwork 
{
	public string enemyType;
	
	private Animator anim;
	private NavMeshAgent agent;
	private StateMachine<GoblinBehavior> stateMachine;
	
	public float attackDist = 1.2f;
	public bool playerFound;
	
	public enum EState : int {GoblinAttack=0, GoblinDeath=1};
	private EState currentEState;
	public EState CurrentEState{
		get {
			return currentEState;
		}
		set {
			Debug.Log("State change!");
			currentEState = value;
			changedState = true;
		}
	}
	private bool changedState = false;
	
	public override bool getChangedState() {
		if (changedState) {
			changedState = false;
			return true;
		}
		return false;
	}
	
	public override int getEState() {
		return (int)currentEState;
	}
	
	// Use this for initialization
	void Start () 
	{
		agent = GetComponent<NavMeshAgent> ();
		anim = GetComponent<Animator> ();
		
		CurrTargetID = 0;
		
		stateMachine = new StateMachine<GoblinBehavior> (new GoblinAttack (), this);
		currentHealth = startingHealth;
		currentEnergy = startingEnergy;
	}
	
	public override void SetFromEState(int st) {
		if (st == (int)EState.GoblinAttack) {
			stateMachine.CurrentState = new GoblinAttack ();
		} else if (st == (int)EState.GoblinDeath) {
			stateMachine.CurrentState = new GoblinDeath ();
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		base.Update ();
		stateMachine.Update ();
	}
	
	
	public void Death ()
	{
		Destroy (gameObject);
	}
}