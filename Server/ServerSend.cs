using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

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

		public static void SpawnPlayer(int _toClient, Player _player)
		{
			using (Packet _packet = new Packet((int)ServerPackets.SpawnPlayer))
			{
				_packet.Write(_player.id);
				_packet.Write(_player.username);
				_packet.Write(_player.identity);
				_packet.Write(new Vector3((int)43, 17.4f, 0));

				SendTCPDataToAll(_toClient, _packet);
			}
		}


		#endregion

	}
}
