using UnityEngine;
using System.Collections;

public class LevelLoad : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("1")) {
			Application.LoadLevel (0);
		} else if (Input.GetKeyDown ("2")) {
			Application.LoadLevel (1);
		} else if (Input.GetKeyDown ("3")) {
			Application.LoadLevel (2);
		} else if (Input.GetKeyDown ("4")) {
			Application.LoadLevel(3);
		} else if (Input.GetKeyDown ("5")) {
			Application.LoadLevel(4);
		}
	}
}