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

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnTriggerEnter(Collider col)
	{
		if(col.tag == "Player")
		{
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
	}
}
