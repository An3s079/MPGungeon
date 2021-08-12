﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPGungeon.Server
{
	class ServerSend
	{
		private static void SendTCPData(int _ToClient, Packet _packet)
		{
			_packet.WriteLength();
			Server.clients[_ToClient].tcp.SendData(_packet);
		}

		private static void SendUDPData(int _ToClient, Packet _packet)
		{
			_packet.WriteLength();
			Server.clients[_ToClient].udp.SendData(_packet);
		}

		public static void SendTCPDataToAll(Packet _packet)
		{
			_packet.WriteLength();
			for (int i = 1; i <= Server.MaxPlayers; i++)
			{
				Server.clients[i].tcp.SendData(_packet);
			}
		}
		public static void SendTCPDataToAll(int _exceptClient, Packet _packet)
		{
			_packet.WriteLength();
			for (int i = 1; i <= Server.MaxPlayers; i++)
			{
				if (i != _exceptClient)
				{
					Server.clients[i].tcp.SendData(_packet);
				}
			}
		}

		public static void SendUDPDataToAll(Packet _packet)
		{
			_packet.WriteLength();
			for (int i = 1; i <= Server.MaxPlayers; i++)
			{
				Server.clients[i].udp.SendData(_packet);
			}
		}
		public static void SendUDPDataToAll(int _exceptClient, Packet _packet)
		{
			_packet.WriteLength();
			for (int i = 1; i <= Server.MaxPlayers; i++)
			{
				if (i != _exceptClient)
				{
					Server.clients[i].udp.SendData(_packet);
				}
			}
		}

		#region packets, we gonna have a lot of these lol
		public static void Welcome(int _ToCLient, string _Message)
		{
			using (Packet _packet = new Packet((int)ServerPackets.welcome))
			{
				_packet.Write(_Message);
				_packet.Write(_ToCLient);

				SendTCPData(_ToCLient, _packet);
			}
		}

		public static void UDPTest(int _toClient)
		{
			using(Packet packet = new Packet((int)ServerPackets.udpTest))
			{
				packet.Write("yay, it worked");

				SendUDPData(_toClient, packet);
			}
		}


		#endregion

	}
}
