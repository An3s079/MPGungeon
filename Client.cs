using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

class Client : MonoBehaviour
{

    int port = 4444;
    string ip = "127.0.0.1";

    // The id we use to identify our messages
    short messageID = 1002;

    NetworkClient client;

    public void CreateClient()
    {
        client = new NetworkClient();
        RegisterHandlers();
        client.Connect(ip, port);
    }

    void RegisterHandlers()
    {
        client.RegisterHandler(messageID, OnMessageReceived);
        client.RegisterHandler(MsgType.Connect, OnConnected);
        client.RegisterHandler(MsgType.Disconnect, OnDisconnected);
    }


    void OnConnected(NetworkMessage message)
    {
        ETGModConsole.Log("Successfully joined server!");
    }

    void OnDisconnected(NetworkMessage message)
    {
        var messageContainer = new StringMessage("Player left the server");
        NetworkServer.SendToAll(messageID, messageContainer);
    }

    void OnMessageReceived(NetworkMessage netMessage)
    {
        var beginMessage = netMessage.ReadMessage<StringMessage>();
        ETGModConsole.Log(beginMessage.value);
    }
}

