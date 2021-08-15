using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MPGungeon.Client
{
	class Manager : MonoBehaviour
	{

        public static Dictionary<int, PlayerController> players = new Dictionary<int, PlayerController>();

        public static void SpawnPlayer(int _id, string _username, Vector3 _position)
        {
            GameObject _player;
            if (_id == Client.instance.myId)
            {
                _player = Instantiate(GameManager.LastUsedPlayerPrefab, _position, Quaternion.identity);
            }
            else
            {
                _player = Instantiate(GameManager.LastUsedPlayerPrefab, _position, Quaternion.identity);
            }

            //_player.GetComponent<PlayerManager>().id = _id;
            //_player.GetComponent<PlayerManager>().username = _username;
            players.Add(_id, _player.GetComponent<PlayerController>());
        }
    }
}
