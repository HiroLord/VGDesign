/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour {
	Vector3 dest;
	Vector3 cache;
	AudioSource src;
	public static void createFireball(GameObject fireball, Vector3 position, Vector3 destination) {
		GameObject ball = (GameObject) Instantiate(fireball, position, Quaternion.identity);
		Fireball ballScript = ball.AddComponent<Fireball> ();
		Vector3 adjusted = new Vector3 (destination.x, destination.y - 0.75f, destination.z);
		ballScript.setDestination (adjusted);
		ballScript.src = ball.GetComponent<AudioSource> ();
	}

	void Update() {
		if (dest != null) {
			gameObject.transform.position = Vector3.Lerp (gameObject.transform.position, dest, Time.deltaTime * 5);
			if (gameObject.transform.position == dest) {
				explode ();
			}
		} else {
			explode ();
		}
	}


	public void setDestination(Vector3 dest) {
		this.dest = dest;
	}

	//I know this is bad practice to have it in two places. Will fix
	void explode() {
		src.Play ();
		var exp = GetComponent<ParticleSystem>();
		exp.Play();
		Destroy(gameObject, exp.duration);
	}

}
