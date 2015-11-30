/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour {
	public GameObject fireball;

	public void createFireball(Vector3 position, Vector3 velocity) {
		GameObject ball = (GameObject) Instantiate(fireball, position, Quaternion.identity);
		ball.GetComponent<Rigidbody> ().AddForce (velocity, ForceMode.Impulse);
	}
}
