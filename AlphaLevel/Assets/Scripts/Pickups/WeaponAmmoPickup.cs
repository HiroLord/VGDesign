using UnityEngine;
using System.Collections;

public class WeaponAmmoPickup : Pickup {

	public override void OnTriggerEnter(Component other){
		Shooting s = other.GetComponentInChildren<Shooting> ();
		bool full = true;
		if (s != null) {
			full = s.addClip();
		}
		if (full) {
			destroyObject ();
		}
	}
}
