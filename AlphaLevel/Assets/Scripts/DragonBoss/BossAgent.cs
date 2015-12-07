/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */


using UnityEngine;
using System.Collections;

public class BossAgent : EnemyNetwork {
	public GameObject head;
	public GameObject leftHand;
	public GameObject rightHand;
	public GameObject tail;
	public GameObject[] players;
	public GameObject fireball;
	public GameObject origin;
	public BossHealth health;
	public GameObject center;

	StateMachine<BossAgent> stateMachine;

	public enum EState : int {Showboating=0, FireAtPlayer=1, Die=2, HandSpinDeath=3};
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
	void Start () {
		// Needs to be changed to account for all characters
		players = GameObject.FindGameObjectsWithTag("Player");
		CurrTargetID = 0;
		stateMachine = new StateMachine<BossAgent> (new Showboating (), this);
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();
		stateMachine.Update ();
	}

	public override void SetFromEState(int st) {
		if (st == (int)EState.Showboating) {
			stateMachine.CurrentState = new Showboating ();
		} else if (st == (int)EState.FireAtPlayer) {
			stateMachine.CurrentState = new FireAtPlayer ();
		} else if (st == (int)EState.HandSpinDeath) {
			stateMachine.CurrentState = new HandSpinDeath ();
		} else if (st == (int)EState.Die) {
			stateMachine.CurrentState = new BossDie ();
		}
	}
}
