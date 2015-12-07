using UnityEngine;
using System.Collections;

public class BackgroundMusicManager : MonoBehaviour {
	public AudioClip[] music;
	public AudioSource src;

	public void Level(string name) {
		Debug.Log (name);
		if (name.Equals ("IslandStart")) {
			swap (0);
		} else if (name.Equals ("level1")) {
			swap (1);
		} else if (name.Equals ("BossLevel")) {
			swap (2);
		} else if (name.Equals ("level3")) {
			swap (3);
		}
	}


	void swap(int n) {
		src.Stop ();
		src.clip = music [n];
		src.loop = true;
		src.Play ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
