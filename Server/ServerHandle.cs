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
				ETGModConsole.Log($"Player \"{username}\" connected successfully and is now player {_fromClient}. (this is you)");
			}
			else
				ETGModConsole.Log($"Player \"{username}\" connected successfully and is now player {_fromClient}.");
			Server.clients[1].SendIntoGame("null", GameManager.Instance.PrimaryPlayer.characterIdentity.ToString(), GameManager.Instance.PrimaryPlayer.specRigidbody.Position.GetPixelVector2());
			Server.clients[_fromClient].SendIntoGame("null", identity, pos);

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

		internal static void ObjectMovement(int _fromClient, Packet _packet)
		{
			using (Packet packet = new Packet((int)ServerPackets.ObjectPosition))
			{
				var id = _packet.ReadInt();
				var pos = _packet.ReadVector2();
				packet.Write(id);
				packet.Write(pos);
				ServerSend.SendUDPDataToAll(_fromClient, packet);
			};
		}

		internal static void ObjAnim(int _fromClient, Packet _packet)
		{
			using (Packet packet = new Packet((int)ServerPackets.ObjAnimSet))
			{
				var id = _packet.ReadInt();
				var clip = _packet.ReadString();
				packet.Write(id);
				packet.Write(clip);
				ServerSend.SendTCPDataToAll(_fromClient, packet);
			};
		}

		internal static void ObjectRemain(int _fromClient, Packet _packet)
		{
			using (Packet packet = new Packet((int)ServerPackets.ObjectRemain))
			{
				var id = _packet.ReadInt();
				var pos = _packet.ReadVector2();
				var rem = _packet.ReadVector2();
				packet.Write(id);
				packet.Write(pos);
				packet.Write(rem);
				ServerSend.SendUDPDataToAll(_fromClient, packet);
			};
		}

		internal static void Flip(int _fromClient, Packet _packet)
		{
			using (Packet packet = new Packet((int)ServerPackets.flip))
			{
				var id = _packet.ReadInt();
				var flip = _packet.ReadBool();
				packet.Write(id);
				packet.Write(flip);
				ServerSend.SendTCPDataToAll(_fromClient, packet);
			};
		}
	}
}
