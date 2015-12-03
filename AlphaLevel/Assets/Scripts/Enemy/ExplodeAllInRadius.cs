using UnityEngine;
using System.Collections;

public class ExplodeAllInRadius : MonoBehaviour {
	ArrayList inRadius;

	void Start() {
		inRadius = new ArrayList ();
	}
	
	void OnTriggerEnter(Collider other) {
		GameObject obj = other.gameObject;
		if (obj.CompareTag ("Explode") || obj.CompareTag ("Player")) {
			Debug.Log ("Add");
			inRadius.Add (obj);
		}
	}
	void OnTriggerExit(Collider other) {
		GameObject obj = other.gameObject;
		Debug.Log (obj.tag);
		if (obj.CompareTag ("Explode") || obj.CompareTag ("Player")) {
			inRadius.Remove (obj);
		}
	}

	public void Boom(float time) {
		foreach (GameObject obj in inRadius) {
			Entity entity = obj.GetComponent<Entity>();
			if (obj.CompareTag("Explode")) {
				Destroy(obj,time);
			} else if (entity != null) {
				entity.TakeDamage (25, Vector3.forward);
			}
		}
	}
}
