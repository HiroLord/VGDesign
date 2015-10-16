using UnityEngine;
using System.Collections;

public class FloatingBoxScript : MonoBehaviour {
	float timer;
	float maxRange;
	// Use this for initialization
	void Start () {
		timer = 0;
		maxRange = 150;
	}
	
	// Update is called once per frame
	void Update () {
		timer += 1;
		if (timer > maxRange) {
			timer = 0;
		}
		transform.position += (maxRange/2-timer)*(Vector3.right * Time.deltaTime);
	}
}
