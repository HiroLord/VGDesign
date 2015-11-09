using UnityEngine;
using System.Collections;

public class Player : Entity {

	// Use this for initialization
	void Start () 
	{
		currentEnergy = startingEnergy;
		currentHealth = startingHealth;
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
		currentHealth -= amount;
		
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
}
