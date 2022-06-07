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
            GameObject _player = new GameObject();
			ItemAPI.ItemBuilder.AddSpriteToObject("player2", "MpGungeon/Resources/RedSquare", _player);
			Instantiate(_player, _position, Quaternion.identity);
			var animator = _player.GetOrAddComponent<tk2dSpriteAnimator>();
			character = character.ToLower();
			if (character == "pilot")
			{
				character = "rogue";
			}
			var prefab = (GameObject)BraveResources.Load("Playerrogue.prefab");//Resources.Load("Assets/Resources/CHARACTERDB:" + args[0] + ".prefab");
			animator.Library.clips = prefab.GetComponent<PlayerController>().spriteAnimator.Library.clips;
			ETGModConsole.Log(prefab.GetComponent<PlayerController>().characterIdentity.ToString());
			players.Add(_id, _player);
			objects.Add(_id, _player);
		}

		public static int GetNewIDForObject()
		{
			return objects.Keys.ToArray()[objects.Keys.Count - 1] + 1;
		}

	}
}
