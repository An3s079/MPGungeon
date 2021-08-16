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

        public static void SpawnPlayer(int _id, string _username, string character, Vector3 _position)
        {
            GameObject _player = new GameObject();
			ItemAPI.ItemBuilder.AddSpriteToObject("player2", "MpGungeon/Resources/RedSquare", _player);
			Instantiate(_player, _position, Quaternion.identity);
			//if (_id == Client.instance.myId)
			//{
			//	_player = Instantiate(BraveResources.Load("Player" + character, ".prefab") as GameObject, _position, Quaternion.identity);
			//}
			//else
			//{
			//	_player = Instantiate(BraveResources.Load("Player" + character, ".prefab") as GameObject, _position, Quaternion.identity);
			//}

			//_player.GetComponent<PlayerManager>().id = _id;
			//_player.GetComponent<PlayerManager>().username = _username;
			//FieldInfo myFieldInfo = typeof(PlayerController).GetField("m_activeActions",
			//	BindingFlags.NonPublic | BindingFlags.Instance);

			//myFieldInfo.SetValue(_player.GetComponent<PlayerController>(), new GungeonActions());
			players.Add(_id, _player);
		}
    }
}
