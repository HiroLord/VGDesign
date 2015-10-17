using UnityEngine;
using System.Collections;

public class LevelLoad : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("1")) {
			Application.LoadLevel ("IceBiome");
		} else if (Input.GetKeyDown ("2")) {
			Application.LoadLevel ("FireForestBiome");
		} else if (Input.GetKeyDown ("3")) {
			Application.LoadLevel ("StormBiome");
		} else if (Input.GetKeyDown ("4")) {
			Application.LoadLevel ("SwampBiome");
		} else if (Input.GetKeyDown ("5")) {
			Application.LoadLevel ("SavannahBiome");
		}
	}
}