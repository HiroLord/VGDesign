using UnityEngine;
using System.Collections;

public abstract class Pickup : MonoBehaviour
{

	public abstract void OnTriggerEnter (Component other);

	public void destroyObject(){
		GameObject.Destroy (this.gameObject);
	}

}

