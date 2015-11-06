using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	public GameObject instance;
	private GameObject[] players = new GameObject[256];
	public Movement player;

	private TcpClient client;
	private bool connected = false;
	private byte[] recvBuffer = new byte[256];
	private int recvBufferSize = 0;

	private int timeBetween = 60;
	
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
	
	// Update is called once per frame
	void Update () {
		if (!connected) {
			if (Input.GetKey ("h")) {
				Connect (true);
			} else if (Input.GetKey ("c")) {
				Connect (false);
			}
		} else {
			StreamReader reader = new StreamReader (client.GetStream ());
			while (client.GetStream().DataAvailable) {
				recvBuffer[recvBufferSize] = (byte)client.GetStream().ReadByte();
				recvBufferSize += 1;
				Debug.Log ("Read data");
			}
			if (recvBufferSize > 0) {
				int msgID = ReadByte ();
				switch(msgID) {
				case 0:
					// Send our information
					WriteByte (1);
					break;
				case 2:
					int newPID = ReadByte ();
					if (players[newPID] == null) { break; }
					float newX = ReadFloat ();
					float oldY = players[newPID].transform.position.y;
					float newZ = ReadFloat ();
					Debug.Log("New position: " + newX.ToString() + ", " + newZ.ToString());
					players[newPID].transform.position.Set(newX, oldY, newZ);
					break;
				case 10:
					int crPID = ReadByte ();
					float crX = ReadFloat ();
					float crZ = ReadFloat ();
					GameObject newPlayer = (GameObject)Instantiate (instance, new Vector3(crX, 0, crZ), new Quaternion(0,0,0,0));
					players[crPID] = newPlayer;
					break;
				}
			}

			timeBetween -= 1;
			if (timeBetween < 1) {
				timeBetween = 60;
				WriteByte (2);
				WriteFloat (player.transform.position.x);
				WriteFloat (player.transform.position.y);
			}
		}
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