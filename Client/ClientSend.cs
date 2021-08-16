using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MpGungeon.Client
{
	class ClientSend : MonoBehaviour
	{
		public static void SendTCPData(Packet _packet)
		{
			_packet.WriteLength();
			Client.instance.tcp.SendData(_packet);
		}

		public static void SendUDPData(Packet _packet)
		{
			_packet.WriteLength();
			Client.instance.udp.SendData(_packet);
		}

		#region packets, gonna have lotsa these lol
		public static void WelcomeRecieved()
		{
			using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
			{
				_packet.Write(Client.instance.myId);
				_packet.Write("Null name");
				_packet.Write(GameManager.Instance.PrimaryPlayer.characterIdentity.ToString());
				_packet.Write(GameManager.Instance.PrimaryPlayer.specRigidbody.Position.GetPixelVector2());
				SendTCPData(_packet);
			}
		}

		public static void Message(string[] _msg)
		{
			using (Packet packet = new Packet((int)ClientPackets.message))
			{
				var msg = String.Join(" ", _msg);
				packet.Write(msg);
				SendTCPData(packet);
				ETGModConsole.Log("message sent to other users: " + msg);
			}
		}

		public static void SendPlayerPos(Vector2 _pos, int _id)
		{
			using (Packet packet = new Packet((int)ClientPackets.PlayerMovement))
			{
				packet.Write(_id);
				packet.Write(_pos);
				SendUDPData(packet);
			}
		}
		#endregion
	}
}
