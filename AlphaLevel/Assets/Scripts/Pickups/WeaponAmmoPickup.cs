using UnityEngine;
using System.Collections;

public class WeaponAmmoPickup : Pickup {
	public AudioSource src;

	public override void OnTriggerEnter(Component other){
		Shooting s = other.GetComponentInChildren<Shooting> ();
		bool full = true;
		if (s != null) {
			full = s.addClip();
		}
		if (full) {
			src.Play ();
			//gameObject.renderer.enabled = false;
			Destroy(gameObject, src.clip.length);
		}
	}
}
