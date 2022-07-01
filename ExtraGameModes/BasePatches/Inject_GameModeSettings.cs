using System;
using System.Runtime.CompilerServices;
using ExtraGameModes;
using Harmony;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VTOLVR.Multiplayer;

[HarmonyPatch(typeof(VTMPScenarioSettings), "SetupScenarioSettings")]
public class Inject_ArmorySettings
{
    public static void Postfix(VTMPScenarioSettings __instance)
    {
        if (!doneSettings)
        {
            GameObject settingsTemplate = __instance.envOptionsUIobj.gameObject;
            GameObject gameModeSettings = GameObject.Instantiate(settingsTemplate, settingsTemplate.transform.parent);
            gameModeSettings.SetActive(true);
            gameModeSettings.transform.localPosition = new Vector3(-322, -397, 0);


            gameModeSettings.transform.Find("envTitle").GetComponent<Text>().text = "Game Mode";
            var text = gameModeSettings.transform.Find("envText").GetComponent<Text>();
            
            text.text = Main.Modes[0];
            
            VRInteractable[] interactables = gameModeSettings.GetComponentsInChildren<VRInteractable>();
            foreach (VRInteractable interactable in interactables)
            {
                // Persistent listerners can't be removed :~(
                
                if (interactable.gameObject.name.Contains("Next"))
                {
                    interactable.interactableName = "Next Gamemode";
                    interactable.OnInteract = new UnityEvent();
                    interactable.OnInteract.AddListener(delegate
                    {
                        text.text = CycleGameModes.Next(text.text);
                    });
                }
                
                if (interactable.gameObject.name.Contains("Prev"))
                {
                    interactable.interactableName = "Previous Gamemode";
                    interactable.OnInteract = new UnityEvent();
                    interactable.OnInteract.AddListener(delegate
                    {
                        text.text = CycleGameModes.Previous(text.text);
                    });
                }
            }
            
            doneSettings = true;
            
            gameModeSettings.GetComponentInParent<VRPointInteractableCanvas>().RefreshInteractables();
        }
    }

    public static bool doneSettings = false;
}