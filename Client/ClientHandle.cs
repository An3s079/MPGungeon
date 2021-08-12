using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

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

		public static void UDPTest(Packet _packet)
		{
			string msg = _packet.ReadString();
			ETGModConsole.Log(msg);
			ClientSend.UDPTestRecieved();
		}

		public static void Message(Packet _packet)
		{
			string msg = _packet.ReadString();
			ETGModConsole.Log(msg);
		}
	}
}
