/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System.Collections;

public class SampleAgentScript : MonoBehaviour {

	private Transform currTarget;
	private int currPoint;
	public Transform[] points;
	private int pointsLen;
	NavMeshAgent agent;
	Animator anim;

	// Use this for initialization
	void Start () 
	{
		agent = GetComponent<NavMeshAgent> ();
		anim = GetComponent<Animator> ();
		//points = new Transform[3];
		currPoint = 0;
		currTarget = points [currPoint];
		pointsLen = points.Length;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(agent.remainingDistance < 2)
		{
//			print ("Change?");
			currPoint++;
			if(currPoint >= pointsLen)
				currPoint = 0;
			currTarget = points[currPoint];
			//anim.SetFloat ("Speed", 0.0f);
		}
		agent.SetDestination (currTarget.position);
		anim.SetFloat ("Speed", 1.0f);
	}
}
