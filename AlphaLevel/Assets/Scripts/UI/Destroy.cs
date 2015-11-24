using UnityEngine;
using System.Collections;

public class Destroy : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Delete() {
		Debug.Log ("DELETE");
		Destroy(gameObject);
	}
}
