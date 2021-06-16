using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Gungeon;
using UnityEngine;
using System.Globalization;
using MPGungeon.Server;

namespace MPGungeon
{
	public class MPGungeon : ETGModule
	{
		
		/*
		 * we should totally add a buncha images in our logging, mhm yep.
		 */

		public static readonly string MOD_NAME = "Gungeon Multiplayer";
		public static readonly string VERSION = "1.0.0";

		public static ETGModuleMetadata metadata = new ETGModuleMetadata();

		public override void Start()
		{
			try
			{
				ETGModMainBehaviour.Instance.gameObject.AddComponent<Client.Client>();
				ETGModMainBehaviour.Instance.gameObject.AddComponent<Client.ThreadManager>();
				ETGModMainBehaviour.Instance.gameObject.AddComponent<Server.ThreadManager>();
				metadata = this.Metadata;
				ETGModConsole.Commands.AddGroup("mp", args =>
				{
				});

				ETGModConsole.Commands.GetGroup("mp").AddUnit("startserver", this.StartServer);
				ETGModConsole.Commands.GetGroup("mp").AddUnit("startclient", this.StartClient);
				ETGModConsole.Commands.GetGroup("mp").AddUnit("sayserver", this.Say);
				ETGModConsole.Commands.GetGroup("mp").AddUnit("sayclient", this.SayClient);
			}
			catch (Exception e)
			{
				AdvancedLogging.LogError("mod Broke heres why: " + e);
			}
			AdvancedLogging.Log($"{MOD_NAME} v{VERSION} started successfully.", new Color32(19, 235, 155, 255), HaveModIcon: true);
		}

		private void SayClient(string[] obj)
		{
			using (Client.Packet _packet = new Client.Packet((int)Client.ClientPackets.MessageRecieved))
			{
				_packet.Write(string.Join(" ", obj));

				Client.ClientSend.SendTCPData(_packet);
			}
		}

		private void Say(string[] obj)
		{
			using (Packet _packet = new Packet((int)ServerPackets.Message))
			{
				_packet.Write(string.Join(" ", obj));


				ServerSend.SendTCPDataToAll(_packet);
			}
			
		}

		private void StartClient(string[] obj)
		{
			
			Client.Client.instance.ConnectToServer();
		}

		private void StartServer(string[] obj)
		{
			Server.Server.Start();
		}

		public override void Exit() { }
		public override void Init() { }


	}

	

}
