using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
using Harmony;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VTOLVR.Multiplayer;
using static VTOLVR.Multiplayer.VTOLMPSceneManager;

namespace ExtraGameModes.Infected
{
    public class Infected : MonoBehaviour
    {
        private const float InfectBase = 180;
        public static float InfectTimer = InfectBase;
        private const float InfectExtend = 60;
        public static bool Running = false;
        public const double InfectDelay = 15;

        private static GameObject _tmpGameObject;
        public static GameObject CameraGameObject;

        public static UnityEvent<ulong> FirstInfectedPlayer = new UnityEvent<ulong>();
        public static UnityEvent<int> TeamWon = new UnityEvent<int>();
        public static UnityEvent<bool, float, float> InfectedTimer = new UnityEvent<bool, float, float>();


        public static void ActorKilled(Actor actor)
        {
            
            if (actor.team != Teams.Allied || actor != instance.localPlayer.vehicleActor) return;
            
            ToEnemyTeamChange();
            
            if (Running)
                ExtendCountdown();
        }

        public static void ToAlliedTeamChange()
        {
            if (instance.localPlayer.selectedSlot != -1)
                instance.VacateSlot();
            
            List<VehicleSlotListItem> items = Traverse.Create(typeof(VTOLMPBriefingRoom)).Field("slotUIs").GetValue() as List<VehicleSlotListItem>;
            if (items != null)
            {
                items.Clear();
                Traverse.Create(typeof(VTOLMPBriefingRoom)).Field("slotUIs")
                    .SetValue(items); // Clear the slot UI so no no bad do thing :~)
            }
            
            instance.RequestTeam(Teams.Allied);
            
            for (int i = 0; i < instance.alliedSlots.Count; i++)
            {
                if (instance.alliedSlots[i].player == null)
                {
                    instance.RequestSlot(instance.alliedSlots[i]); // player will probably get a vehicle they don't want. lmao idiot
                    break;
                }
            }

        }
        public static void ToEnemyTeamChange()
        {
            if (instance.localPlayer.selectedSlot != -1)
                instance.VacateSlot();
            
            List<VehicleSlotListItem> items = Traverse.Create(typeof(VTOLMPBriefingRoom)).Field("slotUIs").GetValue() as List<VehicleSlotListItem>;
            if (items != null)
            {
                items.Clear();
                Traverse.Create(typeof(VTOLMPBriefingRoom)).Field("slotUIs")
                    .SetValue(items); // Clear the slot UI so no no bad do thing :~)
            }

            instance.RequestTeam(Teams.Enemy);

            for (int i = 0; i < instance.enemySlots.Count; i++)
            {
                if (instance.enemySlots[i].player == null)
                {
                    instance.RequestSlot(instance.enemySlots[i]);
                    break;
                }
            }
            
            CheckWin("Enemy");
        }
        
        public static void CheckWin(string team)
        {
            switch (team)
            {
                case "Allied":
                    if (instance.alliedSlots.Count(slot => slot.player != null) > 0)
                    {
                        Debug.Log("Allied win");
                        
                        AlliedWin();
                        ResetValues();
                    }
                    break;
                case "Enemy":
                {
                    if (instance.alliedSlots.Count(slot => slot.player != null) == 0)
                    {
                        Debug.Log("Enemy win");
                        EnemyWin();
                        ResetValues();
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
        
        public static void SetText(string text, float time)
        {
            if (_tmpGameObject == null)
            {
                _tmpGameObject = CreateLabel();
            }
            
            var tmp = _tmpGameObject.GetComponent<VTTextMeshPro>();
            tmp.text = text;
            
            var timer = new Timer();
            timer.Interval = time * 1000;
            timer.Start();
            timer.AutoReset = false;
            timer.Elapsed += (sender, args) =>
            {
                tmp.text = "";
                timer.Dispose();
            };
        }

        public static void StartCountdown()
        {
            InfectTimer = InfectBase;
            
            Running = true;
            if (instance.localPlayer.team == Teams.Allied)
                EnemyInfected();
            
            if (VTOLMPUtils.IsMPAndHost())
            {
                if (InfectedTimer == null)
                    InfectedTimer = new UnityEvent<bool, float, float>();
                InfectedTimer.Invoke(Running, InfectTimer, InfectBase);
            }
        }

        public static void ExtendCountdown()
        {
            InfectTimer += InfectExtend;

            if (VTOLMPUtils.IsMPAndHost())
            {
                if (InfectedTimer == null)
                    InfectedTimer = new UnityEvent<bool, float, float>();
                InfectedTimer.Invoke(Running, InfectTimer, InfectBase);
            }
        }

        public static VTOLMPTeamSelectUI TeamSelectUI;

        public static void InfectedUpdate()
        {
            if (!Running) return;
            InfectTimer -= Time.deltaTime;

            if (InfectTimer <= 0)
            {
                CheckWin("Allied");
                
                ResetValues();
                
                if (VTOLMPUtils.IsMPAndHost())
                {
                    if (InfectedTimer == null)
                        InfectedTimer = new UnityEvent<bool, float, float>();
                    InfectedTimer.Invoke(Running, InfectTimer, InfectBase);
                }
            }
        }

        public static void ResetValues()
        {
            Running = false;
            InfectTimer = InfectBase;
        }
        
        public static void AlliedSpawnObjective()
        {
            SetText("INFECTED: You or a loved one will be infected in " + InfectDelay + " seconds. Survive for " +
                    InfectBase + " seconds!", 7.5f);
        }

        public static void EnemySpawnObjective()
        {
            SetText("You've been infected, kill non infected people to move them to your team!", 7.5f);
        }
        
        public static void EnemyInfected()
        {
            SetText("An infected has been chosen!", 7.5f);
        }
        
        
        public static void AlliedWin()
        {
            if (VTOLMPUtils.IsMPAndHost())
            {
                if (TeamWon == null)
                    TeamWon = new UnityEvent<int>();
                TeamWon.Invoke(0);
            }

            SetText("Survivors win!", 15f);
        }

        public static void EnemyWin()
        {
            if (VTOLMPUtils.IsMPAndHost())
            {
                if (TeamWon == null)
                    TeamWon = new UnityEvent<int>();
                TeamWon.Invoke(1);
            }

            SetText("Infected win!", 15f);
        }



        
    }
}