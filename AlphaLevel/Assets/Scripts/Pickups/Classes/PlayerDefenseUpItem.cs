using UnityEngine;
using System.Collections;

public class PlayerDefenseUpItem : PlayerItem
{
	int defenseUp = 2;
	public PlayerDefenseUpItem() : base("PlayerDefenseUp"){
	}
	public override void doItemEffect(Player player){
		player.setDefense (player.getDefense () + defenseUp);
	}
	public override void removeItemEffect(Player player){
		player.setDefense (player.getDefense () - defenseUp);
	}
}

