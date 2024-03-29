﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace MpGungeon.Server
{
	class Client
	{
		public static int dataBufferSize = 4096;

		public int id;
		public TCP tcp;
		public UDP udp;
		public Player player;

		public Client(int _clientID)
		{
			id = _clientID;
			tcp = new TCP(id);
			udp = new UDP(id);
			
		}
		public class TCP
		{
			public TcpClient socket;
			private readonly int id;
			private NetworkStream stream;
			private Packet receivedData;
			private byte[] receiveBuffer;

			public TCP(int _id)
			{
				id = _id;
			}

			public void Connect (TcpClient _socket)
			{
				socket = _socket;
				{
					socket.ReceiveBufferSize = dataBufferSize;
					socket.SendBufferSize = dataBufferSize;
				}
				stream = socket.GetStream();

				receivedData = new Packet();
				receiveBuffer = new byte[dataBufferSize];

				stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
				ServerSend.Welcome(id, "Congration. You successfully connected to the server");
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
				catch(Exception e)
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
							Server.packetHandlers[_packetId](id, _packet);
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
			public IPEndPoint endpoint;
			private int id;

			public UDP(int _id)
			{
				id = _id;
			}

			public void Connect(IPEndPoint _endPoint)
			{
				endpoint = _endPoint;
			}

			public void SendData(Packet _packet)
			{
				Server.SendUDPData(endpoint, _packet);
			}

			public void HandleData(Packet _packetData)
			{
				int _packetLength = _packetData.ReadInt();
				byte[] _packetBytes = _packetData.ReadBytes(_packetLength);

				ThreadManager.ExecuteOnMainThread(() =>
				{
					using (Packet _packet = new Packet(_packetBytes))
					{
						int _packetId = _packet.ReadInt();
						Server.packetHandlers[_packetId](id, _packet);
					}
				});
			}
		}

		public void SendIntoGame(string _playerName, string identity, Vector2 pos)
		{
			player = new Player(id, _playerName, pos, identity);

            foreach (Client _client in Server.clients.Values)
            {
                if (_client.player != null)
                {
                    if (_client.id != id)
                    {
                        ServerSend.SpawnPlayer(id, _client.player);
                    }
                }
            }

            foreach (Client _client in Server.clients.Values)
            {
                if (_client.player != null)
                {	
                    ServerSend.SpawnPlayer(_client.id, player);
                }
            }
		}
	}
}
