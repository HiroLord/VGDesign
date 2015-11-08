/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System.Collections;

public class CameraMoves : MonoBehaviour {
	
	public Transform target;
	public float smoothing = .5f;
	
	Vector3 offset;
	
	void Start() {
		offset = transform.position - target.position;
	}
	
	void FixedUpdate() {
		Vector3 targetCamPos = target.position + offset;
		transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing);
	}
}
