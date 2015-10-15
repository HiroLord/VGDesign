using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (0, 1f, 1f);
	}

	void OnTriggerEnter (Collider other) {
		//PlayerScript pscript = other.GetComponent<PlayerScript>();
		if(other.tag == "Player")
		{
			Shooting comp = other.GetComponentInChildren<Shooting> ();
			comp.currentAmmo = comp.startingAmmo;
		}
		Destroy (this.gameObject);
	}
}
