using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Shooting : MonoBehaviour 
{
	public int damagePerShot = 20;
	public float timeBetweenBullets = 0.15f;
	public float range = 100f;
	public int startingAmmo = 30;
	public int currentAmmo;
	public Text disp;
	float timer;
	Ray shootRay;
	RaycastHit shootHit;
	int shootableMask;
	AudioSource[] soundEff;
	AudioSource gunAudio;
	AudioSource emptyClip;

	// ParticleSystem gunParticles;
	// LineRenderer gunLine;
	// Light gunLight;
	// float effectsDisplayTime = 0.2f;

	void Awake()
	{
		shootableMask = LayerMask.GetMask ("Shootable");
		soundEff = GetComponents<AudioSource> ();
		gunAudio = soundEff [0];
		emptyClip = soundEff [1];
		currentAmmo = startingAmmo;

		//disp = GameObject.Find ("Text").GetComponent<GUIText> ();
		// gunLight = GetComponent<Light> ();
		// gunParticles = GetComponent<ParticleSystem> ();
		// gunLine = GetComponent<LineRenderer> ();
	}

	// Update is called once per frame
	void Update () 
	{
		timer += Time.deltaTime;
		if(/*Input.GetButton ("Fire1") && */Input.GetButton("Jump") && timer >= timeBetweenBullets) {
	   		if (Time.timeScale != 0 && currentAmmo > 0)
			{
				currentAmmo--;
				Shoot();
			}
			else
			{
				// For some reason this clip is not playing
				timer = 0f;
				emptyClip.Play();
				print ("Empty!");
			}
		}
		disp.text = "Ammo: " + currentAmmo + "/" + startingAmmo;
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
		gunAudio.Play();
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
			EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();
			if(enemyHealth != null)
			{
				enemyHealth.TakeDamage(damagePerShot, shootHit.point);
			}
		}
//		else
//		{
//			gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
//		}
	}

}
