using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Gungeon;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using System.Globalization;
using Ionic.Zip;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

namespace MPGungeon
{
	public class Module : ETGModule
	{

		public static readonly string MOD_NAME = "Gungeon Multiplayer";
		public static readonly string VERSION = "1.0.0";
		public static readonly string TEXT_COLOR = "#00FFFF";
		NetworkClient client = new NetworkClient();
		public override void Start()
		{
			try
			{
			
				GameObject NetworkController = new GameObject("Network Manager");
				NetworkController.AddComponent<MPNetworkManager>();

				ETGModConsole.Commands.AddGroup("mp", args =>
				{
					Log("Gungeon Multiplayer mod made by An3s :)");
				});

				ETGModConsole.Commands.GetGroup("mp").AddUnit("help", args =>
				{
					ETGModConsole.Log("You will be prompted when you must enter something in the console");
				});

				ETGModConsole.Commands.GetGroup("mp").AddUnit("serverport", this.GetInputServerPort);
				ETGModConsole.Commands.GetGroup("mp").AddUnit("host", this.HostGame);
				ETGModConsole.Commands.GetGroup("mp").AddUnit("join", this.JoinGame);
				ETGModConsole.Commands.GetGroup("mp").AddUnit("test", this.Test);
			}
			catch (Exception e)
			{
				ETGModConsole.Log("mod Broke heres why: " + e);
			}
			Log($"{MOD_NAME} v{VERSION} started successfully.", TEXT_COLOR);
			Log("<color=#ff0000ff>Type in \"mp host\" to host a game and \"mp join\" to join a game</color>");
		}

        private void Test(string[] obj)
        {
			var message = new StringMessage("haha yes");
			NetworkServer.SendToAll(1002,	message);
		}

        private void HostGame(string[] arg)
        {
			if(MPNetworkManager.isAtStartup == true)
				MPNetworkManager.GetServerPort();
		}

        private void JoinGame(string[] arg)
        {
			Client client = new Client();
			if (MPNetworkManager.isAtStartup == true)
				client.CreateClient();
		}

        private void GetInputServerPort(string[] arg)
		{
			if (MPNetworkManager.isAtStartup == true)
			{
				Host host = new Host();
				host.SetupServer();
				host.SetupLocalClient();
			}
		}


		public static void Log(string text, string color = "FFFFFF")
		{
			ETGModConsole.Log($"<color={color}>{text}</color>");
		}

		public override void Exit() { }
		public override void Init() { }


	}
}
