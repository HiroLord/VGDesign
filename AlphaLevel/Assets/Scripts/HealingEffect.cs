/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HealingEffect : MonoBehaviour {

	ParticleSystem particles;
	List<Player> guys = new List<Player>();
	bool healing;
	// Use this for initialization
	void Start () 
	{
		particles = GetComponent<ParticleSystem> ();
		particles.Stop ();
		healing = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (healing) {
			foreach(Player guy in guys) {
				guy.GiveHealth (1);
			}
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player")
		{
			if (guys.Count == 0) {
				particles.Play ();
			}
			guys.Add(col.GetComponent<Player>());
			healing = true;
		}
	}

	void OnTriggerExit(Collider col)
	{
		if (col.tag == "Player")
		{
			guys.Remove(col.GetComponent<Player>());
			if (guys.Count == 0) {
				particles.Stop ();
			}
			healing = false;
		}

	}
}
