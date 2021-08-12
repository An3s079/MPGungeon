using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPGungeon.Server
{
	class ServerHandle
	{
		public static void WelcomeReceived(int _fromClient, Packet _packet)
		{
			int _clientIdCheck = _packet.ReadInt();
			string username = _packet.ReadString();

			ETGModConsole.Log($"Player \"{username}\" connected successfully and is now player {_fromClient}.");
			
			if(_fromClient != _clientIdCheck)
				ETGModConsole.Log($"Player \"{username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
		}

		public static void UDPTestRecieved(int _fromClient, Packet _packet)
		{
			string msg = _packet.ReadString();
			ETGModConsole.Log("recieved packet via udp: "+ msg);
		}

		internal static void MessageRecieved(int _fromClient, Packet _packet)
		{
			using(Packet packet = new Packet((int)ServerPackets.messageReceived))
			{
				var msg = _packet.ReadString();
				packet.Write("Message from user " + _fromClient + ": " + msg);
				ServerSend.SendTCPDataToAll(_fromClient, packet);
			}
		}
	}
}
