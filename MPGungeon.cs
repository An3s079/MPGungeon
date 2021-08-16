using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Gungeon;
using UnityEngine;
using System.Globalization;
using MpGungeon.Server;

namespace MpGungeon
{
	public class MpGungeon : ETGModule
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
				ETGModConsole.Commands.GetGroup("mp").AddUnit("msg", this.msg);
			}
			catch (Exception e)
			{
				AdvancedLogging.LogError("mod Broke heres why: " + e);
			}
			AdvancedLogging.Log($"{MOD_NAME} v{VERSION} started successfully.", new Color32(19, 235, 155, 255), HaveModIcon: true);
		}

		private void msg(string[] obj)
		{
			Client.ClientSend.Message(obj);

		}

		private void StartClient(string[] obj)
		{
			if (GameManager.Instance.PrimaryPlayer != null)
			{
				if (obj.Length > 0)
					Client.Client.instance.ConnectToServer(obj[0]);
				else
					Client.Client.instance.ConnectToServer("127.0.0.1");
			}
			else
				AdvancedLogging.LogError("Primary player cannot be null!");
		}

		private void StartServer(string[] obj)
		{
			if (GameManager.Instance.PrimaryPlayer != null)
			{
				Server.Server.Start();
				Client.Client.instance.ConnectToServer();
			}
			else
				AdvancedLogging.LogError("Primary player cannot be null!");
		}

		public override void Exit() { }
		public override void Init() { }


	}

	

}
