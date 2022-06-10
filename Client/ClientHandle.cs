using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;
namespace MpGungeon.Client
{
	class ClientHandle
	{
		public static void Welcome(Packet _packet)
		{
			string _msg = _packet.ReadString();
			int _myID = _packet.ReadInt();

			ETGModConsole.Log("Message from server: " + _msg);
			Client.instance.myId = _myID;
			Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
			ClientSend.WelcomeRecieved();
			var obj = GameManager.Instance.PrimaryPlayer.gameObject.GetOrAddComponent<NetworkedObject>();
			obj.ID = _myID;
		}

		public static void Message(Packet _packet)
		{
			string msg = _packet.ReadString();
			ETGModConsole.Log(msg);
		}

		public static void SetObjPos(Packet _packet)
		{
			int _id = _packet.ReadInt();
			Vector3 _position = _packet.ReadVector2();
			Manager.objects[_id].transform.position = _position;
		}
		public static void SetObjAnim(Packet _packet)
		{
			int _id = _packet.ReadInt();
			try
			{
				string clip = _packet.ReadString();
				var animator = Manager.objects[_id].GetComponentInChildren<tk2dSpriteAnimator>();
				if (Manager.objects[_id].GetComponent<PlayerController>() != null)
				{
					animator = Manager.objects[_id].GetComponent<PlayerController>().spriteAnimator;
				}
				animator.Play(clip);
			}catch(Exception e)
			{
				ETGModConsole.Log("looking for key: " + _id + "\n have id's: ");
				foreach (var key in Manager.objects)
					{
					
						ETGModConsole.Log(key.Key.ToString()); 
					}
				ETGModConsole.Log("error setting anim: " + e);
			}
		}
		public static void SpawnPlayer(Packet _packet)
		{
			int _id = _packet.ReadInt();
			string _username = _packet.ReadString();
			string character = _packet.ReadString();
			Vector3 _position = _packet.ReadVector3();
			Manager.SpawnPlayer(_id, "null name", character, _position);
		}

		internal static void SetObjPosRemainder(Packet _packet)
		{
			int _id = _packet.ReadInt();
			Vector3 _position = _packet.ReadVector2();
			Vector3 remainder = _packet.ReadVector2();
			Manager.objects[_id].GetComponent<PlayerController>().specRigidbody.Position = new Position(new IntVector2((int)_position.x, (int)_position.y), remainder);
		}

		internal static void SetObjFlip(Packet _packet)
		{
			int _id = _packet.ReadInt();
			bool flipped = _packet.ReadBool();
			var player = Manager.objects[_id].GetComponent<PlayerController>();
			player.sprite.FlipX = flipped;
			
			if (flipped)
				player.sprite.gameObject.transform.localPosition = new Vector3(player.sprite.GetBounds().extents.x * 2f, 0f, 0f);
			else
				player.sprite.gameObject.transform.localPosition = Vector3.zero;

		}
	}
}
