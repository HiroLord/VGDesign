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

	
	public static void createDefenseUpItem(Vector3 vector){
		GameObject item = GameObject.Instantiate(Resources.Load("PlayerDefenseUpItem", typeof(GameObject)),vector,Quaternion.LookRotation(Vector3.up)) as GameObject;
		item.name = "PlayerDefenseUpItem";
	}

	public static void createRandomItem(Vector3 vector){
		string name = (Random.Range (0, 100) > 50) ? "PlayerDefenseUpItem" : "WeaponPowerUpItem"; 
		GameObject item = GameObject.Instantiate(Resources.Load(name, typeof(GameObject)),vector,Quaternion.LookRotation(Vector3.up)) as GameObject;
	}

}


