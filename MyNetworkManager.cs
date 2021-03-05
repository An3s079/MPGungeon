using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using Microsoft.VisualBasic;
using System.Collections;
using WindowsInput;
using System.Collections.Generic;
using UnityEngine.Networking.NetworkSystem;

public class MPNetworkManager : MonoBehaviour
    {
        public static bool isAtStartup = true;

        public static void GetServerPort()
        {
            ETGModConsole.Log("Type in the console: mp serverport yourport , with yourport being replaced with any numbers");
        }
        
        //it was gonna be more earlier lol, then i split it all up
}

