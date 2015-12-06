using UnityEngine;
using System.Collections;

public class WeaponPowerUpItem : WeaponItem {

	int powerUp = 2;
	public WeaponPowerUpItem() : base("WeaponPowerUp"){
	}
	public override void doItemEffect (Weapon weapon){
		weapon.setDamage(weapon.damage * powerUp);
//		shooting.weapon.setDamage (shooting.weapon.damage + powerUp);
	}
	public override void removeItemEffect(Weapon weapon){
		weapon.setDamage (weapon.damage / powerUp);
	}
}
