/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System.Collections;

public class WeaponPickup : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Component other){
		Shooting s = other.GetComponent<Shooting>();
		if (s != null) {
			string name = this.gameObject.name;
			name = name.Substring(0,name.IndexOf("Pickup"));
			s.changeWeapon (WeaponLibrary.weapons [name]);
		}
		print (s.weapon.name);
	}
}
