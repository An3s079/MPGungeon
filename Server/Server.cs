using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace MPGungeon.Server
{
	class Server
	{
		public static int MaxPlayers; //in case we ever get to adding more than 2
		public static int Port;

		public static Dictionary<int, Client> clients = new Dictionary<int, Client>();
		public delegate void PacketHandler(int _fromClient, Packet _packet);
		public static Dictionary<int, PacketHandler> packetHandlers = new Dictionary<int, PacketHandler>();

		private static TcpListener tcpListener;

		public static void Start(int _MaxPlayers = 3, int _Port = 26950)
		{
			MaxPlayers = _MaxPlayers;
			Port = _Port;

			ETGModConsole.Log("Starting server."); //we should throw some loading image or something here idk
			InitializeServerData();

			tcpListener = new TcpListener(IPAddress.Any, Port);
			tcpListener.Start();
			tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);

			ETGModConsole.Log("Server started on port " + Port);
		}

		private static void TCPConnectCallback(IAsyncResult _result)
		{
			TcpClient client = tcpListener.EndAcceptTcpClient(_result);
			tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);

			ETGModConsole.Log($"Incoming connection from {client.Client.RemoteEndPoint}");

			for (int i = 1; i <= MaxPlayers; i++)
			{
				if(clients[i].tcp.socket is null)
				{
					clients[i].tcp.Connect(client);
					return;
				}
			}
			AdvancedLogging.LogError($"{client.Client.RemoteEndPoint} failed to connect: Server full.");
		}

		private static void InitializeServerData()
		{
			for(int i = 1; i <= MaxPlayers; i++)
				clients.Add(i, new Client(i));

			packetHandlers = new Dictionary<int, PacketHandler>()
			{
				{ (int)ClientPackets.welcomeReceived, ServerHandle.WelcomeReceived},
				//{ (int)ClientPackets.MessageRecieved, ServerHandle.MessageRecieved }
			};
		}
	}
}
