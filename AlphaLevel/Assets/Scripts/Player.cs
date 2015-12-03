/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : Entity {
	List<PlayerItem> playerItems;
	int defense;
	// Use this for initialization
	void Start () 
	{
		playerItems = new List<PlayerItem> ();
		currentEnergy = startingEnergy;
		currentHealth = startingHealth;
		defense = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(isDead)
		{
			Movement p = GetComponent<Movement>();
			p.Kill();
		}
	}


	public virtual void TakeDamage(int amount, Vector3 hitPoint)
	{
		if (isDead)
			return;
		currentHealth -= (amount - defense);
		
		if(currentHealth <= 0)
		{
			currentHealth = 0;
			isDead = true;
		}
		else
		{
			AudioSource grunt = GetComponent<AudioSource>();
			grunt.PlayOneShot (grunt.clip, 1.0f);
		}
	}

	public virtual void GiveHealth(int amount)
	{
		currentHealth += amount;
		if (currentHealth > startingHealth)
			currentHealth = startingHealth;
	}

	public void addPlayerItem(PlayerItem item){
		playerItems.Add (item);
		item.doItemEffect (this);
		print (defense);
	}

	public void setDefense(int defense){
		this.defense = defense;
	}

	public int getDefense(){
		return defense;
	}

}
