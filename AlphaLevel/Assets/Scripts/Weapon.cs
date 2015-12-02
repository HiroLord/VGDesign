/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System.Collections;

public class Weapon{
	public string name;
	public int damage;
	public float fireRate;
	public float range;
	public int maxAmmo;
	public int currentAmmo;
	public string effect;
	public Transform tempObjects;
	public bool hasEffect;
	// Use this for initialization
	public Weapon(string name, int damage, float fireRate, float range, int maxAmmo, int currentAmmo, string effect){
		this.name = name;
		this.damage = damage;
		this.fireRate = fireRate;
		this.range = range;
		this.maxAmmo = maxAmmo;
		this.currentAmmo = currentAmmo;
		this.effect = effect;
		hasEffect = true;
	}

	public Weapon(string name, int damage, float fireRate, float range, int maxAmmo, int currentAmmo){
		this.name = name;
		this.damage = damage;
		this.fireRate = fireRate;
		this.range = range;
		this.maxAmmo = maxAmmo;
		this.currentAmmo = currentAmmo;
		hasEffect = false;
	}

	public Weapon(string name){
		this.name = name;
		hasEffect = false;
	}

	public void shootEffect(Vector3 location, Quaternion direction){
		GameObject shot = (GameObject)GameObject.Instantiate (Resources.Load (effect),location,direction);
		Debug.Log ("shooteffect" + shot);
		ParticleSystem ps = shot.GetComponent <ParticleSystem>();
		ps.Play ();
		shot.transform.parent = GameObject.Find ("TempObjects").transform;
	}

	public void setDamage(int damage){
		this.damage = damage;
	}
}
