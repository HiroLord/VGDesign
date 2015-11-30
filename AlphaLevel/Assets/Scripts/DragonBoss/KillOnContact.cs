/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System.Collections;

public class KillOnContact : MonoBehaviour {

	// This one does't?
	void OnCollisionEnter(Collision c) {
		if (c.gameObject.tag == "Player") {
			Entity player = c.gameObject.GetComponent<Entity>();
			player.TakeDamage (100, gameObject.transform.position);
		}
	}

	//this one works
	void OnTriggerEnter(Collider c) {
		if (c.gameObject.tag == "Player") {
			Entity player = c.gameObject.GetComponent<Entity>();
			player.TakeDamage (100, gameObject.transform.position);
		}
	}
}
