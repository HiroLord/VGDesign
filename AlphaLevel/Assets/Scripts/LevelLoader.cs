/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour 
{
	public string nextLevel;
	public Vector3 startPosition;

	private int count = 0;

	private bool host = true;
	public bool Ready = false;
	public bool Host {
		set {
			this.host = value;
		}
	}

	// Use this for initialization
	void Start () {
		GameObject obj = GameObject.Find ("NetworkManager");
		if (obj) {
			obj.GetComponent<NetworkManager> ().levelLoader = this;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Transititon() {
		GameObject obj = GameObject.Find ("NetworkManager");
		if (obj) {
			obj.GetComponent<NetworkManager> ().ClearEnemies();
		}
		foreach (GameObject p in GameObject.FindGameObjectsWithTag("Player")) {
			//p.transform.position = new Vector3(0, 1f, 6);
			p.transform.position = startPosition;
			p.GetComponent<Rigidbody>().MovePosition(startPosition);
		}
		if (nextLevel == "BossLevel") {
			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().orthographicSize = 9;
		} else if (nextLevel == "PostBoss") {
			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().orthographicSize = 5;
		}
		Application.LoadLevel (nextLevel);
	}

	void OnTriggerEnter(Collider col)
	{
		if(host && col.tag == "Player") {
			count += 1;
			if (count == GameObject.FindGameObjectsWithTag("Player").Length) {
				Ready = true;
				Transititon();
			}
		}
	}

	void OnTriggerExit(Collider col) {
		if (host && col.tag == "Player") {
			count -= 1;
		}
	}
}
