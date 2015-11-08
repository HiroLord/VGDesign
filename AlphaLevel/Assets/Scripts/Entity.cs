using UnityEngine;
using System.Collections;

public abstract class Entity : MonoBehaviour {
	public int startingHealth = 100;
	public int startingEnergy = 1000;
	public bool isDead;
	public int currentHealth;
	public int currentEnergy;

	public virtual void TakeDamage(int amount, Vector3 hitPoint)
	{
		if (isDead)
			return;
		currentHealth -= amount;
		
		if(currentHealth <= 0)
		{
			currentHealth = 0;
			isDead = true;
		}
	}
	
	public virtual void TakeEnergy(int amount)
	{
		currentEnergy -= amount;
		if (currentEnergy < 0)
			currentEnergy = 0;
	}
	
	public virtual void GiveHealth(int amount)
	{
		currentHealth += amount;
		if (currentHealth > startingHealth)
			currentHealth = startingHealth;
	}
	
	public virtual void GiveEnergy(int amount)
	{
		currentEnergy += amount;
		if (currentEnergy > startingEnergy)
			currentEnergy = startingEnergy;
	}
	//public abstract void Death ();
}
