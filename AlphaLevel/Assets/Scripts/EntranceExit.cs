/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System.Collections;

public class EntranceExit : MonoBehaviour {

    //Test variable
    public bool isExit = false;

    //String name of next level
    public string nextLevel;
    //Destination coordinates
    public float x = 0f;
    public float y = 0f;
    public float z = 0f;

    //Cooldown timer
    public float introCountdownMax = 1.5f;
    private float introCountdown = 0f;
    public float exitCountdownMax = 1.5f;
    private float exitCountdown = 0f;

    GameObject player;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
        introCountdown = introCountdownMax;
        exitCountdown = exitCountdownMax;
	}
	
	// Update is called once per frame
	void Update () {
        if (introCountdown > 0)
        {
            introCountdown -= Time.deltaTime;
        }
        else if (isExit)
        {
            exitCountdown -= Time.deltaTime;
        }

        if (exitCountdown <= 0)
        {
            //Application.LoadLevel(nextLevel);
            player.transform.position = new Vector3(x, y, z);
        }
	}

    void OnCollisionEnter (Collision col)
    {
        if (col.gameObject == player)
        {
            if (introCountdown <= 0)
            {
                isExit = true;
            }
        }
    }

    //void OnCollisionExit(Collision col)
    //{
    //    if (col.gameObject == player)
    //    {
    //        isExit = false;
    //    }
    //}
}
