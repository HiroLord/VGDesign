using UnityEngine;
using System.Collections;

public class AudioZone : MonoBehaviour {

	AudioSource soundEffect;


	// Use this for initialization
	void Start () 
	{
		soundEffect = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnTriggerEnter(Collider col)
	{
		if(col.tag == "Player")
		{
			soundEffect.Play ();
			soundEffect.loop = true;
		}	
	}

	void OnTriggerExit(Collider col)
	{
		if(col.tag == "Player")
		{
			soundEffect.Stop ();
		}	
	}
}
