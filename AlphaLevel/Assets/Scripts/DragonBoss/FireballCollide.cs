using UnityEngine;
using System.Collections;

public class FireballCollide : MonoBehaviour {

	// Use this for initialization
	void OnCollisionEnter(Collision c) {
		if (c.gameObject.tag == "Ground") {
			Destroy (gameObject);
		}

		if (c.gameObject.tag == "Player") {
			Entity player = c.gameObject.GetComponent<Entity>();
			player.TakeDamage (50, gameObject.transform.position);
			Destroy (gameObject);
		}
	}
}
