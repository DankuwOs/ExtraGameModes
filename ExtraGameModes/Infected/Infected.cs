using System;
using System.Runtime.CompilerServices;
using System.Timers;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VTOLVR.Multiplayer;

namespace ExtraGameModes.Infected
{
    public class Infected : MonoBehaviour
    {

        static float InfectBase = 180;
        public static float InfectExtend = 60;
        public static bool Running = false;
        
        public static double InfectDelay = 15;

        public static GameObject TMPGameObject;
        public static GameObject CameraGameObject;

        public static void ActorKilled(Actor actor)
        {
            if (actor.team == Teams.Allied)
            {
                ToEnemyTeamChange(actor.GetNetEntity().owner);
            }
            
            if (Running)
                ExtendCountdown();
        }

        public static void ToAlliedTeamChange(Friend friend)
        {
            VTOLMPSceneManager.instance.RPC_TeamChanged(friend.Id, (int)Teams.Allied);
        }
        public static void ToEnemyTeamChange(Friend friend)
        {
            VTOLMPSceneManager.instance.RPC_TeamChanged(friend.Id, (int)Teams.Enemy);
            CheckWin("Enemy");
        }

        public static void CheckWin(string team)
        {
            switch (team)
            {
                case "Allied":
                    if (VTOLMPSceneManager.instance.GetPlayers(Teams.Allied).Length > 0)
                    {
                        AlliedWin();
                    }
                    break;
                case "Enemy":
                {
                    if (VTOLMPSceneManager.instance.GetPlayers(Teams.Allied).Length == 0)
                    {
                        EnemyWin();
                    }
                    break;
                }
            }
            

            
        }

        public static GameObject CreateLabel()
        {
            var camera = CameraGameObject;
            if (camera == null)
            {
                camera = FindObjectOfType<VRSDKSwitcher>().gameObject.transform.Find("[CameraRig]/Camera (eye)")
                    .gameObject;
            }

            var tmpObject = new GameObject("TMPObject");
            var tmp = tmpObject.AddComponent<VTTextMeshPro>();
            tmp.transform.SetParent(camera.transform);
            
            tmp.transform.localRotation = Quaternion.Euler(0, 0, 0);
            
            tmp.transform.localPosition = Vector3.zero;
            tmp.transform.localPosition += new Vector3(0, -0.12f, 0.42f);

            tmp.verticalAlignment = VerticalAlignmentOptions.Middle;
            tmp.horizontalAlignment = HorizontalAlignmentOptions.Geometry;
            tmp.fontSize = 0.2f;
            tmp.sortingOrder = 1;
            tmp.margin = new Vector4(99.8f, 0, 99.8f, 0);

            return tmpObject;
        }

        public static void StartCountdown()
        {
            InfectBase = 180;
            Running = true;
            if (VTOLMPSceneManager.instance.localPlayer.team == Teams.Allied)
                EnemyInfected();
        }

        public static void ExtendCountdown()
        {
            InfectBase += InfectExtend;
        }

        public void Update()
        {
            if (Running)
            {
                InfectBase -= Time.deltaTime;

                if (InfectBase <= 0)
                {
                    CheckWin("Allied");
                    Running = false;
                }

            }
        }
        
        
        
        public static void AlliedSpawnObjective()
        {
            if (TMPGameObject == null)
            {
                TMPGameObject = CreateLabel();
            }
            var tmp = TMPGameObject.GetComponent<VTTextMeshPro>();
            
            tmp.text = "INFECTED: You or a loved one will be infected in " + InfectDelay + " seconds. Survive for " +
                       InfectBase + " seconds!";
            var timer = new Timer();
            timer.Interval = 7.5f * 1000;
            timer.Start();
            timer.AutoReset = false;
            timer.Elapsed += (sender, args) =>
            {
                tmp.text = "";
                timer.Dispose();
            };
        }

        public static void EnemySpawnObjective()
        {
            if (TMPGameObject == null)
            {
                TMPGameObject = CreateLabel();
            }
            var tmp = TMPGameObject.GetComponent<VTTextMeshPro>();
            tmp.text = "You've been infected, kill non infected people to move them to your team!";
            Timer timer = new Timer();
            timer.Interval = 7.5f * 1000;
            timer.Start();
            timer.AutoReset = false;
            timer.Elapsed += (sender, args) =>
            {
                tmp.text = "";
                timer.Dispose();
            };
        }
        
        public static void EnemyInfected()
        {
            if (TMPGameObject == null)
            {
                TMPGameObject = CreateLabel();
            }
            var tmp = TMPGameObject.GetComponent<VTTextMeshPro>();
            tmp.text = "An infected has been chosen!";
            Timer timer = new Timer();
            timer.Interval = 7.5f * 1000;
            timer.Start();
            timer.AutoReset = false;
            timer.Elapsed += (sender, args) =>
            {
                tmp.text = "";
                timer.Dispose();
            };
        }
        
        
        public static void AlliedWin()
        {
            if (TMPGameObject == null)
            {
                TMPGameObject = CreateLabel();
            }
            var tmp = TMPGameObject.GetComponent<VTTextMeshPro>();
            tmp.text = "Survivors win!";
            Timer timer = new Timer();
            timer.Interval = 7.5f * 1000;
            timer.Start();
            timer.AutoReset = false;
            timer.Elapsed += (sender, args) =>
            {
                tmp.text = "";
                timer.Dispose();
            };
        }

        public static void EnemyWin()
        {
            if (TMPGameObject == null)
            {
                TMPGameObject = CreateLabel();
            }
            var tmp = TMPGameObject.GetComponent<VTTextMeshPro>();
            tmp.text = "Infected win!";
            Timer timer = new Timer();
            timer.Interval = 7.5f * 1000;
            timer.Start();
            timer.AutoReset = false;
            timer.Elapsed += (sender, args) =>
            {
                tmp.text = "";
                timer.Dispose();
            };
        }
    }
}