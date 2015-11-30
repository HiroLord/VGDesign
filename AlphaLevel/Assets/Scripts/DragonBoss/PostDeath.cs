/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */


using UnityEngine;
using System.Collections;

public class PostDeath : MonoBehaviour {

	public GameObject exit;
	Vector3 endPos;
	public bool isDead = false;

	// Use this for initialization
	void Start () {
		endPos = exit.transform.position;
		exit.transform.position = new Vector3 (exit.transform.position.x, -10f, exit.transform.position.z);
	
	}
	
	// Update is called once per frame
	void Update () {
		if (isDead) {
			if (exit.transform.position != endPos) {
				exit.transform.position = Vector3.Lerp (exit.transform.position, endPos, Time.deltaTime * 0.5f);
			}
		}
	}
}
