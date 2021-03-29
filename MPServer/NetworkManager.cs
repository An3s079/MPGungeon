﻿using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace MPGungeon.MPServer
{
    //starts and stops server, etc, i think
    class NetworkManager : MonoBehaviour
    {
        public static NetworkManager Instance;

        private const int MaxPlayers = 50;
        private const int Port = 26950;

        public GameObject playerPrefab;

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != null)
            {
                Log("Instance already exists, destroying object.");
                Destroy(Instance);
            }
        }

        private void Start()
        {
            QualitySettings.vSyncCount = 0;

            Server.Start(MaxPlayers, Port);
        }

        private void OnApplicationQuit()
        {
            for (byte id = 1; id <= MaxPlayers; id++)
            {
                if (Server.clients[id].player != null)
                {
                    Log($"Calling ServerSend.DisconnectPlayer({id})");
                    ServerSend.DisconnectPlayer(id);
                }
            }

            Log("Disconnected All Players");

            Server.Stop();
        }

        public Player InstantiatePlayer(Vector3 position, Vector3 scale)
        {
            GameObject playerObj = Instantiate(playerPrefab);
            Player playerComponent = playerObj.GetComponent<Player>();
            playerComponent.transform.position = position;
            playerComponent.transform.localScale = scale;
            return playerComponent;
        }

        private static void Log(object message) => Debug.Log("[Network Manager] " + message);
    }
}