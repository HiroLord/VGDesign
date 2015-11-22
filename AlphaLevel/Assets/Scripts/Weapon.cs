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
	public GameObject effect;
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
		this.effect = GameObject.Find (effect);
		tempObjects = GameObject.Find ("TempObjects").transform;
		hasEffect = true;
		shootEffect (new Vector3 (0, 0, 0), Quaternion.LookRotation (Vector3.up));
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
		Debug.Log ("shoot effect" + effect);
		GameObject shot = GameObject.Instantiate (effect, location,direction) as GameObject;
		shot.transform.parent = tempObjects;
	}
}
