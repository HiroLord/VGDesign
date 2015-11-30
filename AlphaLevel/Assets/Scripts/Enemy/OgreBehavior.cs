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
	public int currTargetID;
	public string enemyType;
	
	private Animator anim;
	private NavMeshAgent agent;
	private StateMachine<OgreBehavior> stateMachine;

	public float attackDist = 1.2f;
	public bool playerFound;

	public bool original = true;

	public enum EState : int {Guard=0, OgreAttack=1, OgreDeath=2};
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

	public bool getChangedState() {
		if (changedState) {
			changedState = false;
			return true;
		}
		return false;
	}

	public int getEState() {
		return (int)currentEState;
	}

	// Use this for initialization
	void Start () 
	{
		agent = GetComponent<NavMeshAgent> ();
		anim = GetComponent<Animator> ();

		currTargetID = 0;

		stateMachine = new StateMachine<OgreBehavior> (new Guard (), this);
		currentHealth = startingHealth;
		currentEnergy = startingEnergy;
	}

	public void SetFromEState(int st) {
		if (st == (int)EState.Guard) {
			stateMachine.CurrentState = new Guard ();
		} else if (st == (int)EState.OgreAttack) {
			stateMachine.CurrentState = new OgreAttack ();
		} else if (st == (int)EState.OgreDeath) {
			stateMachine.CurrentState = new OgreDeath ();
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		stateMachine.Update ();
	}

	
	public void Death ()
	{
		Destroy (gameObject);
	}
}
