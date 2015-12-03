using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ItemLibrary:MonoBehaviour{
	public static Dictionary<string,WeaponItem> weaponItems;
	public static Dictionary<string,PlayerItem> playerItems;
	// Use this for initialization
	void Awake () {
		weaponItems = new Dictionary<string, WeaponItem> ();
		playerItems = new Dictionary<string, PlayerItem> ();

		weaponItems.Add ("WeaponPowerUp", new WeaponPowerUpItem ());

		playerItems.Add ("PlayerDefenseUp", new PlayerDefenseUpItem ());
	}


}


