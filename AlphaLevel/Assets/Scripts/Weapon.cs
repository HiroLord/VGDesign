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
	// Use this for initialization
	public Weapon(string name, int damage, float fireRate, float range, int maxAmmo, int currentAmmo){
		this.name = name;
		this.damage = damage;
		this.fireRate = fireRate;
		this.range = range;
		this.maxAmmo = maxAmmo;
		this.currentAmmo = currentAmmo;
	}

	public Weapon(string name){
		this.name = name;
	}
}
