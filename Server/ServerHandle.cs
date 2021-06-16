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

		public static void MessageRecieved(int _fromClient, Packet _packet)
		{
			ETGModConsole.Log(_packet.ReadString());
		}
	}
}
