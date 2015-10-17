using UnityEngine;
using System.Collections;

public class BlockHitScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	void OnCollisionEnter(Collision c) {
		if (c.collider.tag == "Block") {
			GetComponent<AudioSource> ().Play();
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
