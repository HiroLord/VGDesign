/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour {
	Vector3 dest;

	public static void createFireball(GameObject fireball, Vector3 position, Vector3 destination) {
		GameObject ball = (GameObject) Instantiate(fireball, position, Quaternion.identity);
		Fireball ballScript = ball.AddComponent<Fireball> ();
		ballScript.setDestination (destination);
	}

	void Update() {
		if (dest != null) {
			gameObject.transform.position = Vector3.Lerp (gameObject.transform.position,dest,Time.deltaTime);
		}
	}


	public void setDestination(Vector3 dest) {
		this.dest = dest;
	}

}
