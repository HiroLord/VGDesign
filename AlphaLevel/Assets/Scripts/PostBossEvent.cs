using UnityEngine;
using System.Collections;

public class PostBossEvent : MonoBehaviour {
	public Rigidbody player;
	public GameObject geyser;
	public GameObject marker;
	public Transform tempObjects;
	bool active;
	int timer;
	int geyserRate;
	int geyserDelay;//between mark and geyser
	int geyserLife; //time geyser lasts

	float rangeFromPlayerX;
	float rangeFromPlayerZ;
	// Use this for initialization
	void Start () {
		tempObjects = GameObject.Find ("TempObjects").transform;
		geyser = GameObject.Find ("FireGeyser");
		marker = GameObject.Find ("GeyserMarker");
		player = GameObject.Find ("Player").GetComponent<Rigidbody> ();
		active = false;
		timer = 0;
		geyserRate = 50;
		geyserDelay = 15;
		geyserLife = 60;

		rangeFromPlayerX = 3;
		rangeFromPlayerZ = 3;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			active = !active;
		}
		if (active) {
			timer += 1;
			if(timer > geyserRate){
				Vector3 randomPosition = new Vector3(Random.Range(player.position.x - rangeFromPlayerX,player.position.x + rangeFromPlayerX),
				                                     player.position.y,
				                                     Random.Range(player.position.z - rangeFromPlayerZ,player.position.z + rangeFromPlayerZ));
				if (active) {
					if(GameObject.Find("GeyserMarker(Clone)") == null){
						GameObject newMarker = GameObject.Instantiate (marker, randomPosition, Quaternion.identity) as GameObject;
						newMarker.transform.parent = tempObjects;
					}
					else if(timer > geyserRate + geyserDelay
					        && GameObject.Find("FireGeyser(Clone)") == null){
						GameObject newGeyser = GameObject.Instantiate (geyser, GameObject.Find ("GeyserMarker(Clone)").transform.position, Quaternion.LookRotation(Vector3.up)) as GameObject;
						newGeyser.transform.parent = tempObjects;
					} else if( timer > geyserRate + geyserDelay + geyserLife){
						timer = 0;
						GameObject.Destroy(GameObject.Find("GeyserMarker(Clone)"));
						GameObject.Destroy (GameObject.Find("FireGeyser(Clone)"));
					}
				}
			}
		}
	}//eoupdate





}
