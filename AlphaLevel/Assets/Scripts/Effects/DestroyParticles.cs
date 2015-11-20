﻿using UnityEngine;
using System.Collections;

public class DestroyParticles : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!((ParticleSystem)GetComponent<ParticleSystem>()).IsAlive ()) {
			Destroy(gameObject);
		}
	}
}
