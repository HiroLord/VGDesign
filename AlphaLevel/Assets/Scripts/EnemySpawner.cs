using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour 
{

	public GameObject enemy;
	public float spawnTime = 5f;
	public Transform[] spawnPoints;
	public int amount = 5;
	private int count = 0;
	// Use this for initialization
	void Start () 
	{
		InvokeRepeating ("Spawn", spawnTime, spawnTime);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void Spawn()
	{
		if(count++ >= amount)
		{
			return;
		}
		else
		{
			int index = Random.Range (0, spawnPoints.Length);
			Instantiate (enemy, spawnPoints [index].position, spawnPoints [index].rotation);
		}
	}
}
