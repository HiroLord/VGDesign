using UnityEngine;
using System.Collections;

public abstract class WeaponItem : Item
{
	public WeaponItem(string name) : base(name){
	}
	public abstract void doItemEffect (Weapon weapon);
	public abstract void removeItemEffect (Weapon weapon);
}

