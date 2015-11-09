using UnityEngine;
using System.Collections;

public class FireballCollide : MonoBehaviour {

	// Use this for initialization
	void OnCollisionEnter(Collision c) {
		if (c.gameObject.tag == "Ground") {
			Destroy (gameObject);
		}
	}
}
