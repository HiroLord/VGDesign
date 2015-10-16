using UnityEngine;
using System.Collections;

public class HouseTrigger : MonoBehaviour {
	public AudioSource doorSound;
	public AudioSource splashSound;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Component other){
		doorSound.Play ();
		print ("play");
	}
}
