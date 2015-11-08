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
			//p.SetRagDoll(true);
		}
	}
}
