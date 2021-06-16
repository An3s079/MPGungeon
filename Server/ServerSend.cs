using System;
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
		public static void SendTCPDataToAll(Packet _packet)
		{
			_packet.WriteLength();
			for (int i = 1; i <= Server.MaxPlayers; i++)
			{
				Server.clients[i].tcp.SendData(_packet);
			}
		}
		private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
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
		public static void Welcome(int _ToCLient, string _Message)
		{
			using (Packet _packet = new Packet((int)ServerPackets.welcome))
			{
				_packet.Write(_Message);
				_packet.Write(_ToCLient);

				SendTCPData(_ToCLient, _packet);
			}
		}


	}
}
