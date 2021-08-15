using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Net;

namespace MPGungeon.Client
{
	class Client : MonoBehaviour
	{
		public static Client instance;
		public static int dataBufferSize = 4096;

		public string ip = "127.0.0.1";
		public int port = 34197;
		public int myId = 0;
		public TCP tcp;
		public UDP udp;

		private delegate void PacketHandler(Packet _packet);
		private static Dictionary<int, PacketHandler> packetHandlers;

		private void Awake()
		{
			if (instance == null)
				instance = this;
			else if(instance != this)
				Destroy(this);
		}

		private void Start()
		{
			tcp = new TCP();
			udp = new UDP();
		}

		void Update()
		{
			if (GameManager.Instance.PrimaryPlayer != null)
			{
				PlayerController p = GameManager.Instance.PrimaryPlayer;
				if (p.Velocity.x > 0 || p.Velocity.y > 0)
				{
					ClientSend.SendPlayerPos(p.specRigidbody.Position.GetPixelVector2(), myId);
				}
			}
		}

		public void ConnectToServer(string CodeString = null)
		{
			try
			{
				if (CodeString != null)
				{
					var code = CodeString;
					//code = code.Remove(0, 5);

					//code = code.Remove(code.Length - 5, 5);

					//code = code.Replace("Sa", ".");
					//code = code.Replace("cR", "0");
					//code = code.Replace("b", "1");
					//code = code.Replace("t", "2");
					//code = code.Replace("FM", "3");
					//code = code.Replace("$%", "4");
					//code = code.Replace("e!", "6");
					//code = code.Replace("~Y", "7");
					//code = code.Replace("p-", "8");
					//ETGModConsole.Log(code);
					ip = code;
				}
				InitializeClientData();

				tcp.Connect();
			}catch(Exception e)
			{
				ETGModConsole.Log(e.ToString());
			}
		}

		public class TCP
		{
			public TcpClient socket;

			private NetworkStream stream;
			private Packet receivedData;
			private byte[] receiveBuffer;

			public void Connect()
			{
				try
				{
					ETGModConsole.Log("Attempting to connect to server...");
					socket = new TcpClient
					{
						ReceiveBufferSize = dataBufferSize,
						SendBufferSize = dataBufferSize
					};

					receiveBuffer = new byte[dataBufferSize];
					socket.BeginConnect(instance.ip, instance.port, ConnectCallback, socket);
				}
				catch (Exception e)
				{
					ETGModConsole.Log(e.ToString());
				}
			}

			private void ConnectCallback(IAsyncResult _result)
			{
				socket.EndConnect(_result);
				
				if (!socket.Connected)
					return;

				stream = socket.GetStream();

				receivedData = new Packet();

				stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
			}
			public void SendData(Packet _packet)
			{
				try
				{
					if(socket != null)
					{
						stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
					}

				}catch(Exception e)
				{
					AdvancedLogging.LogError(e);
				}
			}

			
			private void ReceiveCallback(IAsyncResult _result)
			{
				try
				{
					int _byteLength = stream.EndRead(_result);

					if (_byteLength <= 0)
						return;

					byte[] _data = new byte[_byteLength];
					Array.Copy(receiveBuffer, _data, _byteLength);

					receivedData.Reset(HandleData(_data));

					stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
				}
				catch (Exception e)
				{
					AdvancedLogging.LogError(e);
				}
			}

			private bool HandleData(byte[] _data)
			{
				int _packetLength = 0;

				receivedData.SetBytes(_data);

				if (receivedData.UnreadLength() >= 4)
				{
					_packetLength = receivedData.ReadInt();
					if (_packetLength <= 0)
					{
						return true;
					}
				}

				while (_packetLength > 0 && _packetLength <= receivedData.UnreadLength())
				{
					byte[] _packetBytes = receivedData.ReadBytes(_packetLength);
					ThreadManager.ExecuteOnMainThread(() =>
					{
						using (Packet _packet = new Packet(_packetBytes))
						{
							int _packetId = _packet.ReadInt();
							packetHandlers[_packetId](_packet);
						}
					});

					_packetLength = 0;
					if (receivedData.UnreadLength() >= 4)
					{
						_packetLength = receivedData.ReadInt();
						if (_packetLength <= 0)
						{
							return true;
						}
					}
				}

				if (_packetLength <= 1)
				{
					return true;
				}

				return false;
			}

		}

		public class UDP
		{
			public UdpClient socket;
			public IPEndPoint endPoint;

			public UDP()
			{
				endPoint = new IPEndPoint(IPAddress.Parse(instance.ip), instance.port);
			}

			public void Connect(int _localPort)
			{
				socket = new UdpClient(_localPort);
				
				socket.Connect(endPoint);
				socket.BeginReceive(RecieveCallback, null);

				using (Packet _packet = new Packet())
				{
					SendData(_packet);
				}
				ETGModConsole.Log("udp connected");
			}
			public void SendData(Packet _packet)
			{
				try
				{
					_packet.InsertInt(instance.myId);
					if (socket != null)
					{
						socket.BeginSend(_packet.ToArray(), _packet.Length(), null, null);
					}

				}
				catch(Exception e)
				{
					ETGModConsole.Log("Error sending data to server via UDP: " + e);
				}
			}

			private void RecieveCallback(IAsyncResult _result)
			{
				try
				{
					byte[] data = socket.EndReceive(_result, ref endPoint);
					socket.BeginReceive(RecieveCallback, null);

					if(data.Length < 4)
					{
						ETGModConsole.Log("data too short");
						//add disconnect later
						return;
					}
					HandleData(data);
				}
				catch
				{
					//add disconnect later
				}
			}

			private void HandleData(byte[] _data)
			{
				using (Packet _packet = new Packet(_data))
				{
					int _packetLength = _packet.ReadInt();
					_data = _packet.ReadBytes(_packetLength);
				}

				ThreadManager.ExecuteOnMainThread(() =>
				{
					using (Packet _packet = new Packet(_data))
					{
						int _packetId = _packet.ReadInt();
						packetHandlers[_packetId](_packet);
					}
				});
			}
			
		}
		private void InitializeClientData()
		{
			packetHandlers = new Dictionary<int, PacketHandler>()
			{
				{ (int)ServerPackets.welcome , ClientHandle.Welcome},
				{ (int)ServerPackets.messageReceived , ClientHandle.Message},
				{ (int)ServerPackets.SpawnPlayer, ClientHandle.SpawnPlayer},
				{ (int)ServerPackets.PlayerPosition, ClientHandle.SetPlayerPos},
			};

		}
	}

}
	