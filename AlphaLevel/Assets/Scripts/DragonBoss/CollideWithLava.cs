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
