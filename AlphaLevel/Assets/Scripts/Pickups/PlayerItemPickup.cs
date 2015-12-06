using UnityEngine;
using System.Collections;

public class PlayerItemPickup : Pickup {

	Player player;
	public override void OnTriggerEnter(Component other){
		try{
			player = other.GetComponentInChildren<Player>();
		}catch(UnityException e){
		}
		string name = this.gameObject.name;
		name = name.Substring(0,name.IndexOf("Item"));
		player.addPlayerItem (ItemLibrary.playerItems [name]);
		destroyObject ();
	}
	
}
