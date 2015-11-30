/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System.Collections;

public class GeyserDamageScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Component other){
		if (other.name.Equals ("Player")) {
			Player player = other.GetComponent<Player>();
			if(player != null){
				print ("Geyser burn");
				player.currentHealth -= (int) (player.startingHealth * .10);
			}
		}
	}
}
