/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NetworkManager : MonoBehaviour {

	public String IPAddress = "192.168.1.115";

	public int[] hostCode = {0,0,0,0};
	private int myPlayerID = 0;

	public List<EnemyNetwork> enemies = new List<EnemyNetwork> ();

	public GameObject instance;
	private PlayerInputManager[] players = new PlayerInputManager[256];
	public PlayerInputManager player;
	
	private TcpClient client;
	private bool connected = false;
	private byte[] recvBuffer = new byte[256];
	private int recvBufferSize = 0;
	private bool host = true;
	public bool Host {
		get {
			return host;
		}
		set {
			host = value;
		}
	}

	private float timeBetween = 1;

	public LevelLoader levelLoader;

	private bool confirmed = false;
	
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (gameObject);
	}

	private int sortByName (GameObject a, GameObject b) {
		return (a.name.Length).CompareTo(b.name.Length);
	}

	public void AddEnemy(EnemyNetwork enemy) {
		enemies.Add (enemy);
		enemy.original = host;

		enemies = enemies.OrderBy(go=>go.name).ToList();
	}

	public void RemoveEnemy(EnemyNetwork enemy) {
		enemies.Remove (enemy);
	}

	public void ClearEnemies() {
		enemies.Clear();
	}

	bool prefetched = false;

	bool PreFetch() {
		prefetched = Security.PrefetchSocketPolicy (IPAddress, 25001, 3000);
		return prefetched;
	}

	void OnLevelWasLoaded(int level) {
		if (level == 1) {
			player = GameObject.Find ("Player").GetComponent<PlayerInputManager> ();
			player.PlayerID = myPlayerID;
			players[myPlayerID] = player;
			if (connected) {
				string hostString = "Host Code: ";
				for (var i=0; i<4; i++) {
					hostString += hostCode[i];
				}
				GameObject.Find ("HostPanel").GetComponentInChildren<Text>().text = hostString;
			}
		}
	}
	
	public void Connect(bool host) {
		if (Application.isWebPlayer && !prefetched) {
			PreFetch ();
			Connect (host);
			return;
		}
		this.host = host;
		if (levelLoader) {
			levelLoader.Host = host;
		}
		try {
			client = new TcpClient(IPAddress, 25001);
			client.ReceiveTimeout = 0;
			WriteByte (host ? 1 : 0);
			Debug.Log ("Connected!");
			connected = true;
		}
		catch(SocketException ex) {
			Debug.Log(ex.Message);
		}
		foreach (EnemyNetwork enemy in enemies) {
			enemy.original = host;
		}
	}
	
	void FixedUpdate() {
		while (CanHandleMsg()) {
			int msgID = ReadByte ();
			switch(msgID) {
			case 254:
				// Send our information
				WriteByte (1);
				WriteByte (host ? 1 : 0);
				for (int i=0; i<4; i++) {
					WriteByte (hostCode[i]);
				}
				break;
			case 1: // My own player ID
				int pID = ReadByte();
				if (pID < 1) {
					client.Close();
					connected = false;
					return;
				}
				Application.LoadLevel ("IslandStart");
				for (int i=0; i<4; i++) {
					hostCode[i] = ReadByte ();
				}
				Debug.Log (hostCode);
				myPlayerID = pID;
				confirmed = true;
				break;
			case 2: // Updating player position
				int newPID = ReadByte ();
				float newX = ReadFloat ();
				float oldY = players[newPID].transform.position.y;
				float newZ = ReadFloat ();
				if (players[newPID] == null) { break; }
				//Debug.Log("New position: " + newX.ToString() + ", " + newZ.ToString());
				players[newPID].SnapTo(newX, newZ);
				break;
			case 3: // Player is moving or stopping
				int movPID = ReadByte ();
				float newH = ReadFloat ();
				float newV = ReadFloat ();
				int newShoot = ReadByte();
				if (players[movPID] == null) { break; }
				players[movPID].setInputs(newH, newV, newShoot);
				break;
			case 4: // Player is turning
				int turnPID = ReadByte ();
				float newTurn = ReadFloat ();
				if (players[turnPID] == null) { break; }
				players[turnPID].setRotation(newTurn);
				break;
			case 5: // Enemy state has changed
				int enemyID = ReadByte();
				int eState = ReadByte ();
				Debug.Log ("New enemy state " + eState.ToString());
				enemies[enemyID].SetFromEState(eState);
				break;
			case 6: // Enemy health has changed
				int enemyHID = ReadByte ();
				float enemyDH = ReadFloat ();
				Debug.Log ("Enemy health change " + enemyDH.ToString());
				enemies[enemyHID].changeHealthHard(-((int)enemyDH));
				break;
			case 7: // Enemy position update
				Debug.Log ("New enemy position.");
				int enemyPID = ReadByte ();
				float enemyX = ReadFloat ();
				float enemyZ = ReadFloat ();
				Debug.Log (enemyX);
				float oldEY = enemies[enemyPID].transform.position.y;
				enemies[enemyPID].transform.position = new Vector3(enemyX, oldEY, enemyZ);
				break;
			case 8: // Reviving player
				Debug.Log ("Reviving player!");
				int revID = ReadByte ();
				if (players[revID] == null) { break; }
				players[revID].getMove().Revive();
				break;
			case 9: // Enemy state has changed
				int enemyTID = ReadByte();
				int enemyTargetID = ReadByte();
				Debug.Log ("New enemy target " + enemyTargetID.ToString());
				if (enemyTargetID > 0) {
					enemies[enemyTID].currTarget = players[enemyTargetID].transform;
					enemies[enemyTID].CurrTargetID = enemyTID;
				}
				break;
			case 10: // New player
				Debug.Log("New Player!");
				int crPID = ReadByte ();
				float crX = ReadFloat ();
				float crZ = ReadFloat ();
				GameObject newPlayer = (GameObject)Instantiate (instance, new Vector3(crX, 1, crZ), new Quaternion(0,0,0,0));
				players[crPID] = newPlayer.GetComponent<PlayerInputManager>();
				players[crPID].PlayerID = crPID;
				players[crPID].setIsPlayer(false);
				break;
			case 11:
				levelLoader.StartFade();
				break;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!connected) {
			/*
			if (Input.GetKey ("h")) {
				Connect (true);
			} else if (Input.GetKey ("c")) {
				Connect (false);
			}
			*/
		} else {
			while (client.GetStream().DataAvailable) {
				recvBuffer[recvBufferSize] = (byte)client.GetStream().ReadByte();
				recvBufferSize += 1;
				//Debug.Log ("Read data. Buffer size: " + recvBufferSize.ToString());
				String data = "Data:";
				for (int i=0; i<recvBufferSize; i++) {
					data+= " " + recvBuffer[i].ToString();
				}
			}

			if (!confirmed) {
				return;
			}

			if (levelLoader.Ready) {
				WriteByte (11);
			}
			
			if (player.NeedsUpdate()) {
				WriteByte (3);
				WriteFloat (player.getH());
				WriteFloat (player.getV());
				WriteByte (player.getShooting() ? 1 : 0);
			}
			
			if (player.HasTurned()) {
				WriteByte (4);
				WriteFloat (player.getRotation());
			}
			
			if (player.ReviveBtn()) {
				// Find a player that needs reviving and do just that
				foreach (PlayerInputManager pl in players) {
					if (pl && pl.getMove ().GetDead()) {
						if (Vector3.Distance(pl.transform.position, player.transform.position) < 3f) {
							pl.getMove().Revive();
							WriteByte (8);
							WriteByte (pl.PlayerID);
						}
					}
				}
			}
			
			timeBetween -= Time.deltaTime;
			if (timeBetween < Time.deltaTime) {
				timeBetween = 60 * Time.deltaTime;
				WriteByte (2);
				WriteFloat (player.transform.position.x);
				WriteFloat (player.transform.position.z);
			}

			for (int e=0; e<enemies.Count; e++) {
				if (enemies[e] == null) {
					continue;
				}
				// Not right; needs to be changed to enemies on non-hosts
				int deltaHealth = enemies[e].getHealthDiff();
				if (deltaHealth != 0) {
					Debug.Log ("Health changed!");
					WriteByte(6);
					WriteByte(e);
					WriteFloat(deltaHealth);
				}
				if (enemies[e].original) {
					if (enemies[e].getChangedState()) {
						Debug.Log ("State change!");
						WriteByte (5);
						WriteByte (e);
						WriteByte (enemies[e].getEState());
						//WriteByte (enemies[e].currTargetID);
					}
					if (enemies[e].TargetChanged()) {
						WriteByte (9);
						WriteByte (e);
						WriteByte (enemies[e].CurrTargetID);
					}
					if (enemies[e].NeedsUpdate()) {
						WriteByte (7);
						WriteByte (e);
						WriteFloat (enemies[e].transform.position.x);
						WriteFloat (enemies[e].transform.position.z);
					}
				}
			}
		}
	}
	
	private bool CanHandleMsg() {
		if (!connected || recvBufferSize < 1) {
			return false;
		}
		int msgID = PeekByte ();
		int sizeM = 257;
		switch (msgID) {
		case 254:
			sizeM = 0;
			break;
		case 1:
			sizeM = 5;
			break;
		case 2:
			sizeM = 9;
			break;
		case 3:
			sizeM = 10;
			break;
		case 4:
			sizeM = 5;
			break;
		case 5:
			sizeM = 2;
			break;
		case 6:
			sizeM = 2;
			break;
		case 7:
			sizeM = 9;
			break;
		case 8:
			sizeM = 1;
			break;
		case 9:
			sizeM = 2;
			break;
		case 10:
			sizeM = 9;
			break;
		case 11:
			sizeM = 0;
			break;
		default:
			Debug.Log ("MSG ID " + msgID.ToString() + " does not exist.");
			recvBufferSize = 0;
			break;
		}
		if (sizeM < recvBufferSize) {
			//Debug.Log("Handling message " + msgID.ToString());
			return true;
		}
		//Debug.Log ("Waiting to handle message " + msgID.ToString ());
		return false;
	}
	
	private int PeekByte() {
		return recvBuffer [0];
	}
	
	private int ReadByte() {
		return ShiftData (1)[0];
	}
	
	private byte[] ShiftData(int amnt) {
		byte[] output = new byte[amnt];
		for (int i=0; i<amnt; i++) {
			output[i] = recvBuffer[i];
		}
		for (int i=0; i<recvBufferSize - amnt; i++) {
			recvBuffer[i] = recvBuffer[i+amnt];
		}
		recvBufferSize -= amnt;
		return output;
	}
	
	private String ReadString() {
		int length = ReadByte ();
		return System.Text.Encoding.UTF8.GetString (ShiftData (length));
	}
	
	private float ReadFloat() {
		byte[] bytes = ShiftData (4);
		return BitConverter.ToSingle (bytes, 0);
	}
	
	private void WriteFloat(float f) {
		NetworkStream stream = client.GetStream ();
		byte[] buffer = BitConverter.GetBytes(f);
		stream.Write (buffer, 0, buffer.Length);
		stream.Flush ();
	}
	
	private void WriteByte(int data) {
		NetworkStream stream = client.GetStream ();
		stream.WriteByte ((byte)data);
		stream.Flush ();
	}
	
	private void WriteString(string data) {
		NetworkStream stream = client.GetStream ();
		stream.WriteByte ((byte)data.Length);
		stream.Write (System.Text.Encoding.UTF8.GetBytes (data), 0, data.Length);
		stream.Flush ();
	}
}