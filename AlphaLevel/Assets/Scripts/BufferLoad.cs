/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BufferLoad : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () {
		foreach (GameObject p in GameObject.FindGameObjectsWithTag("Player")) {
			//p.transform.position = new Vector3(0, 1f, 6);
			p.transform.position = new Vector3(-13, 3, 12);
			p.GetComponent<Rigidbody>().MovePosition(new Vector3(-13, 3, 12));
		}
		Application.LoadLevel ("IslandStart");
	}
}
