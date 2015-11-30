/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System.Collections;

public class CollideWithLava : MonoBehaviour {
		
	void OnCollisionEnter(Collision c) {
		Debug.Log ("rawr");
		if (c.gameObject.tag == "Player") {
			Debug.Log ("Rawr");
			c.gameObject.GetComponent<Movement>().Kill ();
		}
	}
}
