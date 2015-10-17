using UnityEngine;
using System.Collections;

public class ParticleDamage : MonoBehaviour {
	//public Image deadImage;
	Rigidbody r;
	void Start() {
		r = GetComponent<Rigidbody> ();
	}
	
	void OnParticleCollision(GameObject other) {
		Debug.Log ("rawr");
		transform.position = new Vector3 (0, 0, 0);
	}

	void OnCollisionEnter(Collision collision) {
		foreach (ContactPoint contact in collision.contacts) {
			Debug.DrawRay(contact.point, contact.normal, Color.white);
		}
		if (collision.collider.CompareTag ("Kill")) {
			r.AddForce (new Vector3(0,1000,0), ForceMode.Impulse);
		}
	}
}
