using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace MPGungeon.MPServer.Canvas
{
    public class OptionsPanel
    {
        public static CanvasPanel Panel;

        private static CanvasToggle _pvpToggle;

        public static void BuildMenu(GameObject canvas)
        {
            Texture2D panelImg = GUIController.Instance.images["Panel_BG"];
            float toggleHeight = 30;
            
            float x = Screen.width / 2.0f - panelImg.width / 2.0f - 30.0f;
            float y = 200.0f;

            if (!GameObject.Find("EventSystem"))
            {
                GameObject eventSystemObj = new GameObject("EventSystem");
                
                EventSystem eventSystem = eventSystemObj.AddComponent<EventSystem>();
                eventSystem.sendNavigationEvents = true;
                eventSystem.pixelDragThreshold = 10;
                
                eventSystemObj.AddComponent<StandaloneInputModule>();

                Object.DontDestroyOnLoad(eventSystemObj);
            }

            Panel = new CanvasPanel(
                canvas,
                panelImg,
                new Vector2(x, y), 
                Vector2.zero,
                new Rect(0, 0, panelImg.width, panelImg.height)
            );
            
            Panel.AddText(
                "Options Text",
                "Options",
                new Vector2(x, y),
                new Vector2(panelImg.width, 60), 
                GUIController.Instance.gungun,
                32,
                FontStyle.Normal,
                TextAnchor.MiddleCenter
            );
            y += 70;
            
            _pvpToggle = Panel.AddToggle(
                "Toggle PvP",
                GUIController.Instance.images["Toggle_BG"],
                GUIController.Instance.images["Checkmark"],
                new Vector2(x, y),
                new Vector2(panelImg.width, 20),
                new Vector2(-80, 0),
                new Rect(0, 0, 150, 20),
                TogglePvP,
                GUIController.Instance.gungun,
                "Enable PvP",
                16
            );
            y += toggleHeight;
            
            Panel.AddToggle(
                "Toggle Spectator",
                GUIController.Instance.images["Toggle_BG"],
                GUIController.Instance.images["Checkmark"],
                new Vector2(x, y),
                new Vector2(panelImg.width, 20),
                new Vector2(-80, 0),
                new Rect(0, 0, 150, 20),
                ToggleSpectator,
                GUIController.Instance.gungun,
                "Spectator Mode",
                16
            );
            y += toggleHeight;
            
            

            Panel.SetActive(false, true);
            
            
            
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnSceneChange;
        }
        

        private static void OnSceneChange(Scene prevScene, Scene nextScene)
        {
            if (nextScene.name == "Menu_Title")
            {
                Panel.SetActive(false, true);
            }
        }
        
        private static void TogglePvP(bool toggleValue)
        {
            if (toggleValue)
            {
                Log("PvP Enabled");
                ServerSettings.PvPEnabled = true;
                ServerSend.PvPEnabled();
            }
            else
            {
                Log("PvP Disabled");
                ServerSettings.PvPEnabled = false;
                ServerSend.PvPEnabled();
            }
        }
        
        private static void ToggleSpectator(bool toggleValue)
        {
            if (toggleValue)
            {
                Log("Spectator Mode Enabled");
                ServerSettings.SpectatorMode = true;
                //ServerSend.PvPEnabled();
            }
            else
            {
                Log("Spectator Mode Disabled");
                ServerSettings.SpectatorMode = false;
                //ServerSend.PvPEnabled();
            }
        }
        
        //private static void ToggleCustomKnight(bool toggleValue)
        //{
        //    if (toggleValue)
        //    {
        //        Log("Custom Knight Enabled");
        //        ServerSettings.CustomKnightIntegration = true;
        //    }
        //    else
        //    {
        //        Log("Custom Knight Disabled");
        //        ServerSettings.CustomKnightIntegration = false;
        //    }
        //}

        private static void Log(object message) => Debug.Log("[Connection Panel] " + message);
    }
}