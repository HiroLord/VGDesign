using UnityEngine;
using System.Collections;

public class TreasurePickup : MonoBehaviour {
	public PostDeath death;

	void OnTriggerEnter(Component other){
		death.treasureTaken ();
		Destroy (gameObject);
	}
}
