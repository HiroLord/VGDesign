using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {
	public float health;
	public float maxHealth;
	public float vision;
	public float attackCooldown;
	public float speed;

	public GameObject guardTarget;
	public float patrolDistance;
	public float strayDistance;
	//public Transform anchor;

	public GameObject player;
	public Vector3 destination;

	public int state;
	
	// Use this for initialization
	void Start () {
		maxHealth = health = 100;
		vision = 20f;
		strayDistance = 10f;
		player = GameObject.Find ("Player");
		speed = .1f;
		guardTarget = GameObject.Find ("Treasure");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			state = 1; //attacking
		} else if (Input.GetKeyDown (KeyCode.Alpha2)) {
			state = 2; //fleeing
		} else if (Input.GetKeyDown (KeyCode.Alpha0)) {
			state = 0; // patrol
		}

		if (state == 1) {
			float moving = (Vector3.Distance(transform.position,guardTarget.transform.position) < strayDistance) ? speed : 0;
			destination = player.transform.position;
			transform.position += moving * Vector3.Normalize(destination - transform.position);
			if(health < (.3*maxHealth)){
				state = 2;
			}
		}
		if (state == 2){
			health = maxHealth;
			destination = guardTarget.transform.position;
			transform.position += speed * Vector3.Normalize(destination - transform.position);
			if(Vector3.Distance(transform.position,destination) < patrolDistance){
				state = 0;
			}
		}
		if (state == 0){
				//patrol
		}
	}

	Vector3 intercept(GameObject other){
		Movement target = other.GetComponent<Movement> ();
		float targetSpeed = 5f;

		return (new Vector3(0,0,0));
	}
}
