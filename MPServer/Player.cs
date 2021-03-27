using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace MPGungeon.MPServer
{
    //contains all the player info, anim, health, inventory (soon), etc
    public class Player : MonoBehaviour
    {

        public byte id;
        public string username;
        public string animation;
        public string activeScene;
        public Vector3 position;
        public Vector3 scale;
        public bool isHost;
        // we also need to get inventory

        //we need to be able to get player identity (might be even harder with stuff like cc's)
        public float health;
        public float maxHealth;

        public List<byte[]> textureHashes = new List<byte[]>();

        public void Initialize(byte id, string username, string animation, float health, float maxHealth, bool isHost)
        {
            this.id = id;
            this.username = username;
            this.animation = animation;
            this.health = health;
            this.maxHealth = maxHealth;
            this.isHost = isHost;
        }

        public void SetPosition(Vector3 position)
        {
            this.position = position;

            ServerSend.PlayerPosition(this);
        }

        public void SetScale(Vector3 scale)
        {
            this.scale = scale;

            ServerSend.PlayerScale(this);
        }

        public void SetAnimation(string animation)
        {
            this.animation = animation;

            ServerSend.PlayerAnimation(this);
        }

        private void Log(object message) => Debug.Log("[Player] " + message);
    }
}
