using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Gungeon;
using UnityEngine;
using System.Globalization;
using UnityEngine.Networking;

namespace MPGungeon
{
	public class Module : ETGModule
	{
		//seems like server and client might be seperate dll's in the HK mod.
		public static readonly string MOD_NAME = "Gungeon Multiplayer";
		public static readonly string VERSION = "1.0.0";
		public static readonly string TEXT_COLOR = "#00FFFF";
		public override void Start()
		{
			try
			{
			
				
				ETGModConsole.Commands.AddGroup("mp", args =>
				{
					Log("Gungeon Multiplayer mod made by An3s and Glorfindel (ScionOfMemory) :)");
				});

				
			}
			catch (Exception e)
			{
				ETGModConsole.Log("mod Broke heres why: " + e);
			}
			Log($"{MOD_NAME} v{VERSION} started successfully.", TEXT_COLOR);
		}


		public static void Log(string text, string color = "FFFFFF")
		{
			ETGModConsole.Log($"<color={color}>{text}</color>");
		}

		public override void Exit() { }
		public override void Init() { }


	}
}
