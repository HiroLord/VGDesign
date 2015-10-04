using UnityEngine;
using System.Collections;

public class ThirdPersonMovement : MonoBehaviour {

	public Transform target;
	public float smoothing = .5f;
	
	Vector3 offset;
	
	void Start() {
		offset = new Vector3 (0.5f, 1.8f, -1f);
	}
	
	void FixedUpdate() {
		Vector3 targetCamPos = target.position + (target.rotation * offset);
		transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing);
		transform.rotation = target.rotation;
	}
}
