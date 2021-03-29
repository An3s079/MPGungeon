using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Gungeon;
using UnityEngine;
using System.Globalization;
using MPGungeon.MPServer.Canvas;
namespace MPGungeon
{
	public class Module : ETGModule
	{
		//seems like server and client might be seperate dll's in the HK mod.
		public static readonly string MOD_NAME = "Gungeon Multiplayer";
		public static readonly string VERSION = "1.0.0";
		public static readonly string TEXT_COLOR = "#00FFFF";
		public static Dictionary<byte[], string> textureCache = new Dictionary<byte[], string>(new MPServer.Util.ByteArrayComparer());

		public override void Start()
		{
			try
			{
			
				 
            // Initialize texture cache
            // This will allow us to easily send textures to th e server when asked to.
            Log("Listing saved textures :");
            string cacheDir = Path.Combine(Application.dataPath, "SkinCache");
            Directory.CreateDirectory(cacheDir);
            string[] files = Directory.GetFiles(cacheDir);
            foreach (string filePath in files)
            {
                string filename = Path.GetFileName(filePath);
                Log(filename);

                byte[] hash = new byte[20];
                for (int i = 0; i < 40; i += 2)
                {
                    hash[i / 2] = Convert.ToByte(filename.Substring(i, 2), 16);
                }

                textureCache[hash] = filePath;
            }
				
				GameManager.Instance.gameObject.AddComponent<MPServer.MPServer>();
				GUIController.Instance.BuildMenus();
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
