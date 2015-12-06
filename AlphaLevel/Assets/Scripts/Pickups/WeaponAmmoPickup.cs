using UnityEngine;
using System.Collections;

public class WeaponAmmoPickup : Pickup {

	public override void OnTriggerEnter(Component other){
		Shooting s = other.GetComponentInChildren<Shooting> ();
		if (s != null) {
			s.addClip();
		}
		destroyObject ();
	}
}
