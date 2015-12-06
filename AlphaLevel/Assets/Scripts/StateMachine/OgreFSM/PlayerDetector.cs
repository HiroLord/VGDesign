using UnityEngine;
using System.Collections;

public class PlayerDetector : MonoBehaviour {

	OgreBehavior ogre;
	// Use this for initialization
	void Start () {
		ogre = GetComponentInParent<OgreBehavior> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnTriggerEnter(Collider col)
	{
		if(col.tag == "Player" && ogre.original)
		{
			ogre.playerFound = true;
			//player = col.transform;
			ogre.currTarget = col.transform;
			ogre.CurrTargetID = col.gameObject.GetComponent<PlayerInputManager>().playerID;
		}
	}
}
