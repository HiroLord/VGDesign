using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour 
{
	public int damagePerShot = 20;
	public float timeBetweenBullets = 0.15f;
	public float range = 100f;


	float timer;
	Ray shootRay;
	RaycastHit shootHit;
	int shootableMask;
	// ParticleSystem gunParticles;
	// LineRenderer gunLine;
	// AudioSource gunAudio;
	// Light gunLight;
	// float effectsDisplayTime = 0.2f;

	void Awake()
	{
		shootableMask = LayerMask.GetMask ("Shootable");
		// gunParticles = GetComponent<ParticleSystem> ();
		// gunLine = GetComponent<LineRenderer> ();
		// gunAudio = GetComponent<AudioSource> ();
		// gunLight = GetComponent<Light> ();
	}

	// Update is called once per frame
	void Update () 
	{
//		if(Input.GetButton("Jump"))
//		{
//			print ("Aiming");
			if(Input.GetButton ("Fire1"))// && timer >= timeBetweenBullets && Time.timeScale != 0)
			{
				//print ("Shooting");
				Shoot();
			}
//			if(timer >= timeBetweenBullets * effectsDisplayTime)
//			{
//				DisableEffects ();
//			}

//		}
	}


//		public void DisableEffects ()
//		{
//			gunLine.enabled = false;
//			gunLight.enabled = false;
//		}

	void Shoot()
	{
		timer = 0f;
//		gunAudio.Play();
//
//		gunLight.enabled = true;
//
//		gunParticles.Stop();
//		gunParticles.Play();
//
//		gunLine.enabled = true;
//		gunLine.SetPosition(0, transform.position);

		shootRay.origin = transform.position;
		shootRay.direction = transform.forward;
		if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
		{
			print ("It's away!");
		}
		else
		{
			print ("Miss!");
		}
	}
}
