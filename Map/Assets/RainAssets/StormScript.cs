using UnityEngine;
using System.Collections;

public class StormScript : MonoBehaviour {
	public Light dirLight;
	public AudioSource rainSound;
	public AudioSource doorSound;
	public AudioSource splashSound;
	AudioSource[] sounds;
	public bool playerInside;
	float timer;
	// Use this for initialization
	void Start () {
		timer = 0;
		playerInside = false;

	}
	void Awake(){
		sounds = GetComponents<AudioSource> ();
		rainSound = sounds [0];
		doorSound = sounds [1];
		splashSound = sounds [2];
	}
	// Update is called once per frame
	void Update () {
		timer += 1;
		if (timer > 520) {
			timer = 0;
		} else if (timer > 500) {
			dirLight.intensity = 90f;
			print ("lightning");
		} else {
			dirLight.intensity = 0.3f;
		}

		//audio
		if(playerInside){
			rainSound.volume = .4f;
			rainSound.Play ();
		}
		else{
			rainSound.volume = .9f;
			rainSound.Play();
		}
	}

}
