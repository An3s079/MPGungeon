﻿using System;
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
			players.Add(_id, _player);
		}
    }
}
