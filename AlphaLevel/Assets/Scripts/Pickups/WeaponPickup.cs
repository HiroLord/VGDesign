/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System.Collections;

public class WeaponPickup : MonoBehaviour {
	public GameObject effect;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Component other){
		Shooting s = other.GetComponentInChildren<Shooting>();
		print (effect);
		GameObject shot = GameObject.Instantiate (effect, gameObject.transform.position,Quaternion.LookRotation(Vector3.up)) as GameObject;
		shot.transform.parent = GameObject.Find ("TempObjects").transform;
		if (s != null) {
			string name = this.gameObject.name;
			name = name.Substring(0,name.IndexOf("Pickup"));
			s.changeWeapon (WeaponLibrary.weapons [name]);
			s.weapon.effect = this.effect;
			print (s.weapon.name);
		}
	}
}
