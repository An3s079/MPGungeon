using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MPGungeon.Server
{
	class Player
	{
		public int id;
		public string username;
		public Vector3 position;
		public string identity;
		public Player(int _id, string _username, Vector3 _pos, string _identity)
		{
			id = _id;
			username = _username;
			position = _pos;
			identity = _identity;
		}
	}
}
