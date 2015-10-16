using UnityEngine;
using System.Collections;

public class SwampBog : MonoBehaviour {

    public Terrain terrain;
    public AudioClip mudFootsteps;
    public AudioClip waterFootsteps;

    private AudioSource footsteps;

    // Use this for initialization
    void Awake () {
        footsteps = terrain.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position.y >= 0.6) footsteps.clip = mudFootsteps;
        else footsteps.clip = waterFootsteps;
	}
}
