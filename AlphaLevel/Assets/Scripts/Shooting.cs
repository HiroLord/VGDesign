/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Shooting : MonoBehaviour 
{
	public Weapon weapon;
	float timer;
	Ray shootRay;
	RaycastHit shootHit;
	int shootableMask;
	AudioSource[] soundEff;
	AudioSource gunAudio;
	AudioSource emptyClip;
	public Text disp;
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
		weapon = new Weapon ("Default", 10, .15f, 100f, 30, 30);
		weapon.currentAmmo = weapon.maxAmmo;

		//disp = GameObject.Find ("Text").GetComponent<GUIText> ();
		//print (disp.ToString());
		// gunLight = GetComponent<Light> ();
		// gunParticles = GetComponent<ParticleSystem> ();
		// gunLine = GetComponent<LineRenderer> ();
	}

	// Update is called once per frame
	void Update () 
	{
		timer += Time.deltaTime;
		if(/*Input.GetButton ("Fire1") && */Input.GetButton("Jump") && timer >= weapon.fireRate) {
	   		if (Time.timeScale != 0 && weapon.currentAmmo > 0)
			{
				weapon.currentAmmo--;
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

		if(disp != null)
		{
			disp.text = "Ammo: " + weapon.currentAmmo + "/" + weapon.maxAmmo;
		}

		if(Input.GetKey("r"))
		{
			weapon.currentAmmo = weapon.maxAmmo;
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
		if(Physics.Raycast (shootRay, out shootHit, weapon.range, shootableMask))
		{
			Enemy enemyHealth = shootHit.collider.GetComponent<Enemy>();
			if(enemyHealth != null)
			{
				enemyHealth.TakeDamage(weapon.damage, shootHit.point);
			}
		}
//		else
//		{
//			gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
//		}
	}

	public void changeWeapon(Weapon weapon){
		this.weapon = weapon;
	}

}
