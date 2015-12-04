/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	public String IPAddress = "128.61.31.181";

	public EnemyNetwork[] enemies;

	public GameObject instance;
	private PlayerInputManager[] players = new PlayerInputManager[256];
	public PlayerInputManager player;
	
	private TcpClient client;
	private bool connected = false;
	private byte[] recvBuffer = new byte[256];
	private int recvBufferSize = 0;
	
	private float timeBetween = 1;
	
	// Use this for initialization
	void Start () {
		Debug.Log ("NetworkingManager initialized");
		DontDestroyOnLoad (gameObject);
	}
	
	void Connect(bool host) {
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
	}
	
	void FixedUpdate() {
		while (CanHandleMsg()) {
			int msgID = ReadByte ();
			switch(msgID) {
			case 254:
				// Send our information
				WriteByte (1);
				WriteByte (9);
				WriteByte (2);
				WriteByte (1);
				WriteByte (8);
				break;
			case 1: // My own player ID
				int pID = ReadByte();
				player.playerID = pID;
				players[pID] = player;
				break;
			case 2: // Updating player position
				int newPID = ReadByte ();
				float newX = ReadFloat ();
				float oldY = players[newPID].transform.position.y;
				float newZ = ReadFloat ();
				if (players[newPID] == null) { break; }
				Debug.Log("New position: " + newX.ToString() + ", " + newZ.ToString());
				players[newPID].GetComponent<Rigidbody> ().MovePosition(new Vector3(newX, oldY, newZ));
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
				int enemyTargetID = ReadByte();
				Debug.Log ("New enemy state " + eState.ToString());
				enemies[enemyID].SetFromEState(eState);
				if (enemyTargetID > 0) {
					enemies[enemyID].currTarget = players[enemyTargetID].transform;
				}
				break;
			case 6: // Enemy health has changed
				int enemyHID = ReadByte ();
				int enemyDH = ReadByte ();
				Debug.Log ("Enemy health change " + enemyDH.ToString());
				enemies[enemyHID].changeHealth(-enemyDH);
				break;
			case 7: // Enemy position update
				Debug.Log ("New enemy position.");
				int enemyPID = ReadByte ();
				float enemyX = ReadFloat ();
				float enemyZ = ReadFloat ();
				float oldEY = enemies[enemyPID].transform.position.y;
				enemies[enemyPID].transform.position = new Vector3(enemyX, oldEY, enemyZ);
				break;
			case 8: // Reviving player
				Debug.Log ("Reviving player!");
				int revID = ReadByte ();
				if (revID > 0) {
					players[revID].getMove().Revive();
				}
				break;
			case 10: // New player
				Debug.Log("New Player!");
				int crPID = ReadByte ();
				float crX = ReadFloat ();
				float crZ = ReadFloat ();
				GameObject newPlayer = (GameObject)Instantiate (instance, new Vector3(crX, 0, crZ), new Quaternion(0,0,0,0));
				players[crPID] = newPlayer.GetComponent<PlayerInputManager>();
				players[crPID].playerID = crPID;
				players[crPID].setIsPlayer(false);
				break;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!connected) {
			if (Input.GetKey ("h")) {
				Connect (true);
			} else if (Input.GetKey ("c")) {
				Connect (false);
				foreach (EnemyNetwork enemy in enemies) {
					enemy.original = false;
				}
			}
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
					if (pl.getMove ().GetDead()) {
						pl.getMove().Revive();
						WriteByte (8);
						WriteByte (player.playerID);
					}
				}
			}
			
			timeBetween -= Time.deltaTime;
			if (timeBetween < Time.deltaTime) {
				timeBetween = 45 * Time.deltaTime;
				WriteByte (2);
				WriteFloat (player.transform.position.x);
				WriteFloat (player.transform.position.z);
			}

			for (int e=0; e<enemies.Length; e++) {
				if (enemies[e] == null) {
					continue;
				}
				if (enemies[e].original) {
					if (enemies[e].getChangedState()) {
						Debug.Log ("State change!");
						WriteByte (5);
						WriteByte (e);
						WriteByte (enemies[e].getEState());
						WriteByte (enemies[e].currTargetID);
					}
					if (enemies[e].NeedsUpdate()) {
						WriteByte (7);
						WriteByte (e);
						WriteFloat (enemies[e].transform.position.x);
						WriteFloat (enemies[e].transform.position.z);
					}
					// Not right; needs to be changed to enemies on non-hosts
					int deltaHealth = 0; //enemies[e].getHealthDiff();
					if (deltaHealth > 0) {
						WriteByte(6);
						WriteByte(e);
						WriteByte(deltaHealth);
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
			sizeM = 1;
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
			sizeM = 3;
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
		case 10:
			sizeM = 9;
			break;
		default:
			Debug.Log ("MSG ID " + msgID.ToString() + " does not exist.");
			recvBufferSize = 0;
			break;
		}
		if (sizeM < recvBufferSize) {
			Debug.Log("Handling message " + msgID.ToString());
			return true;
		}
		Debug.Log ("Cannot handle message " + msgID.ToString ());
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