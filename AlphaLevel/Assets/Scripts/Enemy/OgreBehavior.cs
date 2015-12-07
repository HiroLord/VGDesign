/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(CapsuleCollider), typeof(Rigidbody))]
public class OgreBehavior : EnemyNetwork 
{
	public string enemyType;
	public GameObject ammoCrate;
	private Animator anim;
	private NavMeshAgent agent;
	private StateMachine<OgreBehavior> stateMachine;

	public float attackDist = 1.2f;
	public bool playerFound;

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

		stateMachine = new StateMachine<OgreBehavior> (new Guard (), this);
	}

	public override void SetFromEState(int st) {
		if (st == (int)EState.Guard) {
			stateMachine.CurrentState = new Guard ();
		} else if (st == (int)EState.OgreAttack) {
			stateMachine.CurrentState = new OgreAttack ();
		}else if (st == (int)EState.OgreDeath) {
			stateMachine.CurrentState = new OgreDeath ();
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
		int drop = Random.Range (0, 3);
		if(drop == 0)
		{	
			Vector3 pos = this.transform.position;
			pos.y += 0.5f;
			Instantiate(ammoCrate, pos, this.transform.rotation);
		}
		Destroy (gameObject);

	}
}