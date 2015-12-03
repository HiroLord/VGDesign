using UnityEngine;
using System.Collections;

public abstract class PlayerItem : Item
{
	public PlayerItem(string name):base(name){
	}
	public abstract void doItemEffect (Player player);
	public abstract void removeItemEffect (Player player);
}

