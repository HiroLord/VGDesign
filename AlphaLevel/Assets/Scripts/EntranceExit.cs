/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System.Collections;

public class EntranceExit : MonoBehaviour {

    //Test varaible
    public bool isExit = false;

    //String name of next level
    public string nextLevel;
    //Destination coordinates
    public float x = 0f;
    public float y = 0f;
    public float z = 0f;

    //Cooldown timer
    public float countdownMax = 1f;
    private float countdown = 0f;

    GameObject player;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
        countdown = countdownMax;
	}
	
	// Update is called once per frame
	void Update () {
	    if (countdownMax > 0)
        {
            countdown -= Time.deltaTime;
        }
	}

    void OnCollisionEnter (Collision col)
    {
        if (col.gameObject == player)
        {
            isExit = true;
            if (countdown <= 0)
            {
                //Application.LoadLevel(nextLevel);
                player.transform.position = new Vector3(x, y, z);
                countdown = countdownMax;
            }
        }
    }

    void OnCollisionExit(Collision col)
    {
        if (col.gameObject == player)
        {
            isExit = false;
        }
    }
}
