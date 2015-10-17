using UnityEngine;
using System.Collections;

public class AgentNav : MonoBehaviour {

	public Transform target;
	NavMeshAgent agent;

	// Use this for initialization
	void Start () 
	{
		agent = GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		agent.SetDestination (target.position);
	}
}
