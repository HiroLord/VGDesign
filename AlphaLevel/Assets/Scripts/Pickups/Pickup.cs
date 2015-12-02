using UnityEngine;
using System.Collections;

public abstract class Pickup : MonoBehaviour
{

	public abstract void OnTriggerEnter (Component other);

}

