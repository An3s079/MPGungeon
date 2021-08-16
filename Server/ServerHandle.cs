using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace MpGungeon.Server
{
	class ServerHandle
	{
		private static string p1Identity;
		public static void WelcomeReceived(int _fromClient, Packet _packet)
		{
			int _clientIdCheck = _packet.ReadInt();
			string username = _packet.ReadString();
			string identity = _packet.ReadString();
			Vector2 pos = _packet.ReadVector2();
			if (_fromClient == 1)
			{
				p1Identity = identity;
				ETGModConsole.Log($"Player \"{username}\" connected successfully and is now player {_fromClient}. (this is you)");
			}
			else
				ETGModConsole.Log($"Player \"{username}\" connected successfully and is now player {_fromClient}.");

			if (_fromClient != _clientIdCheck)
				ETGModConsole.Log($"Player \"{username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
			if(_fromClient != 1)
				Server.clients[_fromClient].SendIntoGame("null", identity, pos);
			if (_fromClient == 2)
				Server.clients[1].SendIntoGame("null", p1Identity, GameManager.Instance.PrimaryPlayer.specRigidbody.Position.GetPixelVector2());
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
				var id = _packet.ReadInt();
				var pos = _packet.ReadVector2();
				packet.Write(id);
				packet.Write(pos);
				ServerSend.SendUDPDataToAll(_fromClient, packet);
			};
		}
	}
}
