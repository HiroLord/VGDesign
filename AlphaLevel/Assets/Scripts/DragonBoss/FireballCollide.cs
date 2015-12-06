/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System.Collections;

public class FireballCollide : MonoBehaviour {

	void OnTriggerEnter(Collider c) {
		if (c.gameObject.tag == "Ground") {
			explode ();
		}
		
		if (c.gameObject.tag == "Player") {
			Entity player = c.gameObject.GetComponent<Entity>();
			player.TakeDamage (50, gameObject.transform.position);
			explode ();
		}
	}

	void OnCollisionEnter(Collision c) {
		if (c.gameObject.tag == "Ground") {
			explode ();
		}

		if (c.gameObject.tag == "Player") {
			Entity player = c.gameObject.GetComponent<Entity>();
			player.TakeDamage (50, gameObject.transform.position);
			explode ();
		}
	}

	void explode() {
		var exp = GetComponent<ParticleSystem>();
		exp.Play();
		Destroy(gameObject, exp.duration);
	}
}
