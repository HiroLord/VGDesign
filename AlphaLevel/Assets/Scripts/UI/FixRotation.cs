/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FixRotation : MonoBehaviour {
	void LateUpdate () {
		Quaternion rotation = Quaternion.LookRotation(Vector3.up , Vector3.forward);
		transform.rotation = rotation;
	}
}
