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
	
	public GameObject instance;
	private GameObject[] players = new GameObject[256];
	public PlayerInputManager player;
	
	private TcpClient client;
	private bool connected = false;
	private byte[] recvBuffer = new byte[256];
	private int recvBufferSize = 0;
	
	private float timeBetween = 60;
	
	// Use this for initialization
	void Start () {
		Debug.Log ("NetworkingManager initialized");
	}
	
	void Connect(bool host) {
		try {
			client = new TcpClient("128.61.30.151", 25001);
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
		if (CanHandleMsg()) {
			int msgID = ReadByte ();
			switch(msgID) {
			case 254:
				// Send our information
				WriteByte (1);
				break;
			case 1:
				int pID = ReadByte();
				break;
			case 2:
				int newPID = ReadByte ();
				if (players[newPID] == null) { break; }
				float newX = ReadFloat ();
				float oldY = players[newPID].transform.position.y;
				float newZ = ReadFloat ();
				Debug.Log("New position: " + newX.ToString() + ", " + newZ.ToString());
				players[newPID].GetComponent<Rigidbody> ().MovePosition(new Vector3(newX, oldY, newZ));
				break;
			case 3:
				int movPID = ReadByte ();
				if (players[movPID] == null) { break; }
				float newH = ReadFloat ();
				float newV = ReadFloat ();
				int newShoot = ReadByte();
				players[movPID].GetComponent<PlayerInputManager>().setInputs(newH, newV, newShoot);
				break;
			case 4:
				int turnPID = ReadByte ();
				if (players[turnPID] == null) { break; }
				float newTurn = ReadFloat ();
				players[turnPID].GetComponent<PlayerInputManager>().setRotation(newTurn);
				break;
			case 10:
				Debug.Log("New Player!");
				int crPID = ReadByte ();
				float crX = ReadFloat ();
				float crZ = ReadFloat ();
				GameObject newPlayer = (GameObject)Instantiate (instance, new Vector3(crX, 0, crZ), new Quaternion(0,0,0,0));
				players[crPID] = newPlayer;
				newPlayer.GetComponent<PlayerInputManager>().setIsPlayer(false);
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
			}
		} else {
			while (client.GetStream().DataAvailable) {
				recvBuffer[recvBufferSize] = (byte)client.GetStream().ReadByte();
				recvBufferSize += 1;
				Debug.Log ("Read data. Buffer size: " + recvBufferSize.ToString());
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
				/*for (GameObject pl : players) {

				}*/
			}
			
			timeBetween -= Time.deltaTime;
			if (timeBetween < Time.deltaTime) {
				timeBetween = 120 * Time.deltaTime;
				WriteByte (2);
				WriteFloat (player.transform.position.x);
				WriteFloat (player.transform.position.z);
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