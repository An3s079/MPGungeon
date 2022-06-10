using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace MpGungeon.Client
{
	class Manager : MonoBehaviour
	{

        public static Dictionary<int, GameObject> players = new Dictionary<int, GameObject>();
		public static Dictionary<int, GameObject> objects = new Dictionary<int, GameObject>();
		public static void SpawnPlayer(int _id, string _username, string character, Vector3 _position)
		{
			if(character.ToLower() == "pilot")
			{
				character = "rogue";
			}
			GameObject _player = (GameObject)BraveResources.Load("Player" + character);
			var player = Instantiate(_player, _position, Quaternion.identity);
			var controller = player.GetComponent<PlayerController>();
			controller.HasShadow = true;
			controller.usingForcedInput = true;
			var i = controller.specRigidbody.GetPixelColliders();
			foreach (var col in i)
			{
				col.Enabled = false;
			}
			controller.enabled = false;
			SpriteOutlineManager.AddOutlineToSprite(controller.sprite, Color.black);
			controller.ShadowObject.GetComponent<Renderer>().enabled = true;
			
			players.Add(_id, player);
			objects.Add(_id, player);
		}

		public static int GetNewIDForObject()
		{
			return objects.Keys.ToArray()[objects.Keys.Count - 1] + 1;
		}

	}
}
