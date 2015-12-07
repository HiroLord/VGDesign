/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour 
{
	private BackgroundMusicManager bg;
	public string nextLevel;
	public Vector3 startPosition;

	private GameObject[] players;
	private Text fail;
	private int count = 0;
	public Image fade;
	private bool setFade;
	private float setAlpha;
	private bool startFade;
	private float startAlpha;
	private bool gameOver = false;

	private bool host = true;
	private int ready = 0;
	public bool Ready {
		get {
			if (ready != 1) {
				return false;
			} else {
				ready = 2;
				return true;
			}
		}
	}
	public bool Host {
		set {
			this.host = value;
		}
	}

	// Use this for initialization
	void Start () 
	{
		players = GameObject.FindGameObjectsWithTag ("Player");
		GameObject f = GameObject.FindGameObjectWithTag ("Failure");
		if(f != null)
		{
			print ("Got text");
			fail = f.GetComponent<Text>();
		}
		bg = GameObject.Find ("BackgroundMusic").GetComponent<BackgroundMusicManager>();
		GameObject obj = GameObject.FindWithTag ("Fade");
		if (obj != null)
			fade = obj.GetComponent<Image> ();
		setFade = false;
		startFade = true;
		gameOver = false;
		fade.color = Color.black;
		setAlpha = 0.0f;
		startAlpha = 1.0f;
		GameObject obj2 = GameObject.Find ("NetworkManager");
		if (obj2) {
			NetworkManager man = obj2.GetComponent<NetworkManager> ();
			man.levelLoader = this;
			host = man.Host;

		}

	}

	void resize() {
		Vector2 rect = new Vector2 (Screen.width, Screen.height);
		fade.rectTransform.sizeDelta = rect;
	}

	public void StartFade() {
		setFade = true;
		resize ();
	}

	// Update is called once per frame
	void Update () {
		if(setFade)
		{
			if(setAlpha >= 1.0f)
			{
				if(gameOver)
				{
					Color col = fail.color;
					col.a = 0.0f;
					fail.color = col;
				}
				Transititon();
				setFade = false;
			}
			else
			{
				Color col = fade.color;
				setAlpha += Time.deltaTime;
				col.a = setAlpha;
				fade.color = col;
				if(gameOver)
				{
					Color col2 = fail.color;
					col2.a = setAlpha;
					fail.color = col2;
				}
			}
		}
		if(startFade)
		{
			//print ("Next fading");
			if(startAlpha >= 0.0f)
			{
				Color col = fade.color;
				startAlpha -= Time.deltaTime;
				col.a = startAlpha;
				fade.color = col;
			}
			else
				startFade = false;
		}
		if (!gameOver) {
			bool dead = true;
			foreach (GameObject p in players) {
				Player pla = p.GetComponent<Player> ();
				if (!pla.isDead) {
					dead = false;
				}
			}
			if (dead) {
				gameOver = true;
				StartFade ();
			}
		}
	
	}

	public void Transititon() {
		Debug.Log ("transition");
		if(gameOver){
			gameOver = false;
			nextLevel = "IslandStart";
		}
		bg.Level (nextLevel);
		GameObject obj = GameObject.Find ("NetworkManager");
		if (obj) {
			obj.GetComponent<NetworkManager> ().ClearEnemies();
		}
		foreach (GameObject p in GameObject.FindGameObjectsWithTag("Player")) {
			//p.transform.position = new Vector3(0, 1f, 6);
			p.transform.position = startPosition;
			p.GetComponent<Rigidbody>().MovePosition(startPosition);
		}
		if (nextLevel == "BossLevel") {
			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().orthographicSize = 9;
		} else if (nextLevel == "PostBoss") {
			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().orthographicSize = 5;
		}
		Application.LoadLevel (nextLevel);
	}

	void OnTriggerEnter(Collider col)
	{
		if(host && col.tag == "Player") {
			count += 1;
			if (count == GameObject.FindGameObjectsWithTag("Player").Length) {
				StartFade();
				ready = 1;
			}
		}
	}

	void OnTriggerExit(Collider col) {
		if (host && col.tag == "Player") {
			count -= 1;
		}
	}
}
