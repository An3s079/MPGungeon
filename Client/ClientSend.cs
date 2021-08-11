using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MPGungeon.Client
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

				SendTCPData(_packet);
			}
		}

		public static void UDPTestRecieved()
		{
			using (Packet packet = new Packet((int)ClientPackets.udpTestRecieved))
			{
				packet.Write("Recieved UDP Packet.");
				SendUDPData(packet);
			}
		}
		#endregion
	}
}
