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

			if(_fromClient == 1)
				ETGModConsole.Log($"Player \"{username}\" connected successfully and is now player {_fromClient}. (this is you)");
			else
				ETGModConsole.Log($"Player \"{username}\" connected successfully and is now player {_fromClient}.");

			if (_fromClient != _clientIdCheck)
				ETGModConsole.Log($"Player \"{username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
			if(_fromClient != 1)
				Server.clients[_fromClient].SendIntoGame("null");
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

		internal static void PlayerMovement(int _fromClient, Packet _packet)
		{
			using (Packet packet = new Packet((int)ServerPackets.PlayerPosition))
			{
				var pos = _packet.ReadVector2();
				ServerSend.SendUDPDataToAll(_fromClient, packet);
			};
		}
	}
}
