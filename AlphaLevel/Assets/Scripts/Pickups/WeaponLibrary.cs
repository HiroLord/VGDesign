/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class WeaponLibrary:MonoBehaviour{
	public static Dictionary<string,Weapon> weapons;
	// Use this for initialization
	void Awake () {
		weapons = new Dictionary<string, Weapon> ();
		weapons.Add ("DefaultWeapon", new Weapon ("DefaultWeapon", 10, .15f, 100f, 30, 30));
		weapons.Add ("Pistol", new Weapon ("Pistol", 17, .5f, 40f, 12, 12));
		weapons.Add ("Shotgun", new Weapon ("Shotgun", 50, .5f, 20f, 20, 20));
	}

}
