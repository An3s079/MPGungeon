using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.Networking.NetworkSystem;

class Host : MonoBehaviour
    {
        NetworkClient LocalClient;
        static int port = 4444;

        // The id we use to identify our messages 
        short messageID = 1002;
        

        public void SetupServer()
        {
            NetworkServer.Listen(port);
            ETGModConsole.Log("Sucessfully Created Server");
            RegisterHandlers();
        }

        public void SetupLocalClient()
        {
            LocalClient = ClientScene.ConnectLocalServer();
            LocalClient.RegisterHandler(messageID, OnMessageReceived);
            LocalClient.RegisterHandler(MsgType.Connect, OnConnectedLocalClient);
            MPNetworkManager.isAtStartup = false;
        }
        void OnConnectedLocalClient(NetworkMessage message)
        {
            ETGModConsole.Log("Successfully joined server!");
        }

    void OnApplicationQuit()
        {
            NetworkServer.Shutdown();
        }

        private void RegisterHandlers()
        {
            NetworkServer.RegisterHandler(MsgType.Connect, OnClientConnected);
            NetworkServer.RegisterHandler(MsgType.Disconnect, OnClientDisconnected);

            NetworkServer.RegisterHandler(messageID, OnMessageReceived);
        }

        private void RegisterHandler(short t, NetworkMessageDelegate handler)
        {
            NetworkServer.RegisterHandler(t, handler);
        }

        void OnClientConnected(NetworkMessage netMessage)
        {
            var messageContainer = new StringMessage("Thanks for joining!");

            NetworkServer.SendToClient(netMessage.conn.connectionId, messageID, messageContainer);

            // Send a message to all the clients connected (in case we ever decide to do more than 2p)
            var messageToAll = new StringMessage("A new player has connected to the server");
            NetworkServer.SendToAll(messageID, messageToAll);
        }

        void OnClientDisconnected(NetworkMessage netMessage)
        {

        }

        void OnMessageReceived(NetworkMessage netMessage)
        {

            var beginMessage = netMessage.ReadMessage<StringMessage>();
            ETGModConsole.Log(beginMessage.value);
        }
    }

