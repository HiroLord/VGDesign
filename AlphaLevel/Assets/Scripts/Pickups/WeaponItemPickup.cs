using UnityEngine;
using System.Collections;

public class WeaponItemPickup : Pickup {
	Shooting shooting;
	Item item;
	public override void OnTriggerEnter(Component other){
		try{
			shooting = other.GetComponentInChildren<Shooting>();
		} catch(UnityException e){
		}
		string name = this.gameObject.name;
		name = name.Substring(0,name.IndexOf("Item"));
		shooting.addWeaponItem (ItemLibrary.weaponItems [name]);
		destroyObject ();
	}
}
