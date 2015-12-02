/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System.Collections;

public class WeaponPickup : MonoBehaviour {

	void OnTriggerEnter(Component other){
		Shooting s = other.GetComponentInChildren<Shooting>();
		if (s != null) {
			string name = this.gameObject.name;
			name = name.Substring(0,name.IndexOf("Pickup"));
			s.changeWeapon (WeaponLibrary.weapons [name]);
			print (s.weapon.name);
		}
	}
}
