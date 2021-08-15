using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using UnityEngine;

namespace MPGungeon.Client
{
	class ClientHandle
	{
		public static void Welcome(Packet _packet)
		{
			string _msg = _packet.ReadString();
			int _myID = _packet.ReadInt();

			ETGModConsole.Log("Message from server: " + _msg);
			Client.instance.myId = _myID;
			Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
			ClientSend.WelcomeRecieved();

			
		}

		public static void Message(Packet _packet)
		{
			string msg = _packet.ReadString();
			ETGModConsole.Log(msg);
		}

		public static void SetPlayerPos(Packet _packet)
		{
			int _id = _packet.ReadInt();
			Vector3 _position = _packet.ReadVector3();

			Manager.players[_id].specRigidbody.Position = new Position(_position);
		}

		public static void SpawnPlayer(Packet _packet)
		{
			int _id = _packet.ReadInt();
			string _username = _packet.ReadString();
			Vector3 _position = _packet.ReadVector3();
			Manager.SpawnPlayer(_id, "null name", _position);
		}
	}
}
