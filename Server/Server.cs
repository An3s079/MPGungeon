using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using SGUI;

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
		private static UdpClient udpListener;
		public static void Start(int _MaxPlayers = 3, int _Port = 34197)
		{
			MaxPlayers = _MaxPlayers;
			Port = _Port;

			ETGModConsole.Log("Starting server."); //we should throw some loading image or something here idk
			InitializeServerData();

			tcpListener = new TcpListener(IPAddress.Any, Port);
			tcpListener.Start();
			tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);

			udpListener = new UdpClient(_Port);
			udpListener.BeginReceive(UDPRecieveCallback, null);
			//string CodeString = new WebClient().DownloadString("http://icanhazip.com").Replace("\\r\\n", "").Replace("\\n", "").Trim();

			//CodeString = CodeString.Replace(".", "Sa");
			//CodeString = CodeString.Replace("0", "cR");
			//CodeString = CodeString.Replace("1", "b");
			//CodeString = CodeString.Replace("2", "t");
			//CodeString = CodeString.Replace("3", "FM");
			//CodeString = CodeString.Replace("4", "$%");
			//CodeString = CodeString.Replace("6", "e!");
			//CodeString = CodeString.Replace("7", "~Y");
			//CodeString = CodeString.Replace("8", "p-");
			//CodeString = CodeString.Insert(0, RandomString(5));
			//CodeString = CodeString.Insert(CodeString.Length, RandomString(5));

			ETGModConsole.Log("Server started on port " + Port); //+ CodeString);
			//AdvancedLogging.LogButton("Press here to write code to console textbox for copying.", UnityEngine.Color.green).OnClick += (SGUI.SButton button) =>
			//{
			//	STextField field = (STextField)ETGModConsole.Instance.GUI[1];
			//	field.Text = CodeString;
			//};
		}
		private static string RandomString(int length)
		{
			const string pool = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz01234567890123456789";
			var builder = new StringBuilder();
			Random r = new Random();
			for (var i = 0; i < length; i++)
			{
				var c = pool[r.Next(0, pool.Length)];
				builder.Append(c);
			}

			return builder.ToString();
		}

		private static void UDPRecieveCallback(IAsyncResult _result)
		{
			try
			{
				IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
				byte[] data = udpListener.EndReceive(_result, ref clientEndPoint);
				udpListener.BeginReceive(UDPRecieveCallback, null);

				if (data.Length < 4)
				{
					return;
				}

				using (Packet packet = new Packet(data))
				{
					int clientId = packet.ReadInt();

					if (clientId == 0)
					{
						return;
					}
					if (clients[clientId].udp.endpoint == null) // new clients initial connection message
					{
						clients[clientId].udp.Connect(clientEndPoint);
						return;
					}

					if(clients[clientId].udp.endpoint.ToString() == clientEndPoint.ToString())
					{
						clients[clientId].udp.HandleData(packet);
					}
				}

			}catch(Exception e)
			{
				ETGModConsole.Log("error recieving UDP data: " + e);
			}
				
		}

		public static void SendUDPData(IPEndPoint _clientEndPoint, Packet _packet)
		{
			try
			{
				if(_clientEndPoint != null)
				{
					udpListener.BeginSend(_packet.ToArray(), _packet.Length(), _clientEndPoint, null, null);
				}
			}catch(Exception e)
			{
				ETGModConsole.Log("Error sending data to " + _clientEndPoint + "via UDP: " + e);
			}
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
				{ (int)ClientPackets.udpTestReceived, ServerHandle.UDPTestRecieved },
				{ (int)ClientPackets.message, ServerHandle.MessageRecieved },
			};
		}
	}
}
