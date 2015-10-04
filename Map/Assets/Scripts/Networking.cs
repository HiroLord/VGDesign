using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections;

public class Networking : MonoBehaviour {

	private TcpClient client;
	private byte[] recvBuffer = new byte[8142];

	// Use this for initialization
	void Start () {
		try {
			client = new TcpClient("127.0.0.1", 25001);
			SendData("Hello!");
		}
		catch(SocketException ex) {
			Debug.Log(ex.Message);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void SendData(string data)
	{
		StreamWriter writer = new StreamWriter(client.GetStream());
		writer.Write(44);
		writer.Write(55);
		writer.Flush();
	}
}
