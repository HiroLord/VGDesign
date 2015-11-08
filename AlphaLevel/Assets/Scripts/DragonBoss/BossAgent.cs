/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */


using UnityEngine;
using System.Collections;

public class BossAgent : MonoBehaviour {
	public GameObject head;
	public GameObject leftHand;
	public GameObject rightHand;
	public GameObject tail;



	StateMachine<BossAgent> sm;

	// Use this for initialization
	void Start () {
		sm = new StateMachine<BossAgent> (new Showboating (), this);
	}
	
	// Update is called once per frame
	void Update () {
		sm.Update ();
	
	}
}
