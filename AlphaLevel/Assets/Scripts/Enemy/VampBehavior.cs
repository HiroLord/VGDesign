/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(CapsuleCollider), typeof(Rigidbody))]
public class VampBehavior : EnemyNetwork 
{
	public string enemyType;
	public GameObject ammoCrate;
	private Animator anim;
	private NavMeshAgent agent;
	private StateMachine<VampBehavior> stateMachine;
	
	public float attackDist = 1.2f;
	public bool playerFound;
	
	public enum EState : int {VampGuard=0, VampAttack=1, VampDeath=2};
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
		base.Start ();
		agent = GetComponent<NavMeshAgent> ();
		anim = GetComponent<Animator> ();
		
		CurrTargetID = 0;
		
		stateMachine = new StateMachine<VampBehavior> (new VampGuard (), this);
		currentHealth = startingHealth;
		currentEnergy = startingEnergy;
	}
	
	public override void SetFromEState(int st) {
		if (st == (int)EState.VampGuard) {
			stateMachine.CurrentState = new VampGuard ();
		} else if (st == (int)EState.VampAttack) {
			stateMachine.CurrentState = new VampAttack ();
		}else if (st == (int)EState.VampDeath) {
			stateMachine.CurrentState = new VampDeath ();
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
		int drop = Random.Range (0, 5);
		if(drop == 0)
		{	
			Vector3 pos = this.transform.position;
			pos.y += 0.5f;
			Instantiate(ammoCrate, pos, this.transform.rotation);
		}
		Destroy (gameObject);
	}
}
