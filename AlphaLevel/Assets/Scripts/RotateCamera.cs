using UnityEngine;
using System.Collections;

public class RotateCamera : MonoBehaviour {

	public float xFocus, yFocus, zFocus;
	public float maxSpeed = 15f;
	Vector3 pos;
	float speed = -9f;

	// Use this for initialization
	void Start () {
		pos = new Vector3 (xFocus, yFocus, zFocus);
	}
	
	// Update is called once per frame
	void Update () {
		if (speed > 0f) {
			transform.RotateAround (pos, Vector3.up, speed * Time.deltaTime);
		}
		if (speed < maxSpeed) {
			speed += Time.deltaTime * 2f;
		} else {
			speed = maxSpeed;
		}
	}
}
