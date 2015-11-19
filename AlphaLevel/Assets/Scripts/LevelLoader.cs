using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour 
{

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
		print ("Collision!");
		if(col.tag == "Player")
		{
			foreach (GameObject p in GameObject.FindGameObjectsWithTag("Player")) {
				p.transform.position = new Vector3(0, 1f, 6);
				p.GetComponent<Rigidbody>().MovePosition(new Vector3(0,1f,6));
			}
			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().orthographicSize = 9;
			Application.LoadLevel (1);
		}
	}
}
