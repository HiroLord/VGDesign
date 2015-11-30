/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System.Collections;

public class HealingEffect : MonoBehaviour {

	ParticleSystem particles;
	Player guy;
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
		if (healing)
			guy.GiveHealth (1);
	}

	void OnTriggerEnter(Collider col)
	{
		if(col.tag == "Player")
		{
			particles.Play ();
			guy = col.GetComponent<Player>();
			healing = true;
		}
	}

	void OnTriggerExit(Collider col)
	{
		if (col.tag == "Player")
		{
			particles.Stop ();
			guy = null;
			healing = false;
		}

	}
}
