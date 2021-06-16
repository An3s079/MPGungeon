using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPGungeon.Client
{
	class ClientHandle
	{
		public static void Welcome(Packet _packet)
		{
			string _msg = _packet.ReadString();
			int _myID = _packet.ReadInt();

			ETGModConsole.Log("Message from server: " + _msg);
			Client.instance.myId = _myID;

			ClientSend.WelcomeRecieved();
		}

		public static void Message(Packet _packet)
		{
			string _msg = _packet.ReadString();

			ETGModConsole.Log("Message from server: " + _msg);
		}
	}
}
