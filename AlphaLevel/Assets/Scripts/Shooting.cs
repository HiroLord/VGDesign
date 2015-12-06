/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class Shooting : MonoBehaviour 
{
	public Weapon weapon;
	public List<WeaponItem> weaponItems;
	float timer;
	Ray shootRay;
	RaycastHit shootHit;
	int shootableMask;
	AudioSource[] soundEff;
	AudioSource gunAudio;
	AudioSource emptyClip;
	public Text disp;
	private bool shooting;
	private bool original = true;
	private int clipAmount;
	public RawImage[] clips;

	private float cooldown;

	private LineRenderer gunLine;
	public Material[] gunLineMaterials;
	public int gunLineMaterial;
	// ParticleSystem gunParticles;
	// LineRenderer gunLine;
	// Light gunLight;
	// float effectsDisplayTime = 0.2f;

	public void setOriginal(bool o) {
		original = o;
	}

	void Awake()
	{
		shootableMask = LayerMask.GetMask ("Shootable");
		soundEff = GetComponents<AudioSource> ();
		gunAudio = soundEff [0];
		emptyClip = soundEff [1];
		weapon = new Weapon ("Default", 10, .15f, 100f, 60, 60);
		weapon.currentAmmo = weapon.maxAmmo;
		weaponItems = new List<WeaponItem>();

		gunLine = GetComponent<LineRenderer> ();
		clipAmount = clips.Length;
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
		if (shooting) {
			if (timer >= weapon.fireRate) {
				if (Time.timeScale != 0 && weapon.currentAmmo > 0) {
					weapon.currentAmmo--;
					Shoot ();
				} else {
					timer = 0f;
					emptyClip.Play ();
				}
			}
		} else {
			timer = -3 * Time.deltaTime;
		}

		if (cooldown < Time.deltaTime) {
			gunLine.enabled = false;
		} else {
			cooldown -= Time.deltaTime;
		}

		if (original && disp != null)
		{
			disp.text = weapon.currentAmmo + "/" + weapon.maxAmmo;
		}
		//debug stuff
		if (Input.GetKeyUp ("r")) {
			if(clipAmount > 0 && clips.Length != 0)
			{
				weapon.currentAmmo = weapon.maxAmmo;
				clipAmount--;
				clips[clipAmount].enabled = false;
			}
		} else if (Input.GetKeyDown ("x")) {
			ItemLibrary.createRandomItem(this.gameObject.transform.position);
		}
//		if (Input.GetKeyUp ("m"))
//			addClip ();
//			if(timer >= timeBetweenBullets * effectsDisplayTime)
//			{
//				DisableEffects ();
//			}
	}

	public void setShooting (bool shoot){
		shooting = shoot;
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
		cooldown = 2 * Time.deltaTime;
//
//		gunLight.enabled = true;
//
//		gunParticles.Stop();
//		gunParticles.Play();
//
		gunLine.enabled = true;
		gunLine.SetPosition(0, transform.position);

		shootRay.origin = transform.position;
		shootRay.direction = transform.forward;

		if(original && Physics.Raycast (shootRay, out shootHit, weapon.range, shootableMask))
		{
			Entity enemyHealth = shootHit.collider.GetComponent<Entity>();
			if(enemyHealth != null)
			{
				Debug.Log ("Hit");
				enemyHealth.TakeDamage(weapon.damage, shootHit.point);
			}
			//should be somewhere else I'm sorry
			if (shootHit.collider.gameObject.tag == ("Dragon")) {
				shootHit.collider.gameObject.GetComponentInParent<BossHealth>().Damage(10f);
			}
			if(weapon.hasEffect){
				weapon.shootEffect(shootRay.origin,Quaternion.LookRotation(shootRay.direction));
			} else{
				gunLine.SetPosition (1, shootHit.point);
			}
		} else {	
			if(weapon.hasEffect){
				weapon.shootEffect(shootRay.origin,Quaternion.LookRotation(shootRay.direction));
			}else{
				gunLine.SetPosition(1, shootRay.origin + shootRay.direction * 10);
			}

		}
	}

	public void changeWeapon(Weapon weapon){
		this.weapon = weapon;
	}

	public void addWeaponItem(WeaponItem item){
		weaponItems.Add (item);
		if (item is WeaponPowerUpItem) {
			gunLine.material = gunLineMaterials[1];
		}
		item.doItemEffect (weapon);
		print (weapon.damage);
	}

	public void addClip()
	{
		int cLen = clips.Length;
		if (clipAmount == cLen && clips.Length == 0)
			return;
		clips [clipAmount++].enabled = true;
	}

}
