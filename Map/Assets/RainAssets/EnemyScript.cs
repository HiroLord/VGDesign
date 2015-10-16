using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {
	public Transform actor;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Vector3.Distance(actor.position,transform.position) > 5f){
			transform.position += .1f* Vector3.Normalize(actor.position - transform.position);
		}
	}
}
