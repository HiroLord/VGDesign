using UnityEngine;
using System.Collections;

public class splashtrigger : MonoBehaviour {
	public AudioSource splashSound;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerExit(){
		splashSound.Play ();
	}
}
