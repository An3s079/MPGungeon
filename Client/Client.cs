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
		public int port = 26950;
		public int myId = 0;
		public TCP tcp;

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
		}

		public void ConnectToServer()
		{
			InitializeClientData();

			tcp.Connect();
		}

		public class TCP
		{
			public TcpClient socket;

			private NetworkStream stream;
			private Packet receivedData;
			private byte[] receiveBuffer;

			public void Connect()
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
		private void InitializeClientData()
		{
			packetHandlers = new Dictionary<int, PacketHandler>()
			{
				{ (int)ServerPackets.welcome , ClientHandle.Welcome},
				//{ (int)ServerPackets.Message , ClientHandle.Message}
			};

		}
	}

}
	