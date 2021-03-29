using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MPGungeon.MPServer.Canvas;

namespace MPGungeon.MPServer
{
    //appears to set the value of PlayerPrefab in networkManager, only purpose it seems.
    class MPServer : MonoBehaviour
    {
        private GameObject _networkManager;

        private void Start()
        {
            _networkManager = new GameObject("NetworkManager");
            _networkManager.AddComponent<NetworkManager>();
            _networkManager.AddComponent<ThreadManager>();

            GameObject playerPrefab = new GameObject(
                "PlayerPrefab",
                typeof(Player)
            )
            {
                layer = 9,
            };

            NetworkManager.Instance.playerPrefab = playerPrefab;

            DontDestroyOnLoad(playerPrefab);
            DontDestroyOnLoad(_networkManager);
        }

        void Update()
        {
            if(GameManager.Instance.IsPaused == true)
                OptionsPanel.Panel.SetActive(true, false);
            else
                OptionsPanel.Panel.SetActive(false, true);
        }
    }
}
