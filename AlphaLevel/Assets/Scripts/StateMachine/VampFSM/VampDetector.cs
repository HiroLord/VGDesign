using UnityEngine;
using System.Collections;

public class VampDetector : MonoBehaviour {
	
	VampBehavior ogre;
	// Use this for initialization
	void Start () {
		ogre = GetComponentInParent<VampBehavior> ();
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
			ogre.currTargetID = col.gameObject.GetComponent<PlayerInputManager>().playerID;
		}
	}
}
