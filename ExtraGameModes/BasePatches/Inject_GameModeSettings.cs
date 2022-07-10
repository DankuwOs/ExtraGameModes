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
        if (Main.settingsPrefab == null)
        {
            Debug.Log("[Extra Game Modes] Settings prefab is null");
            return;
        }
        
        var settingsObj = GameObject.Instantiate(Main.settingsPrefab, __instance.transform.Find("displayObj"));

        var settings = settingsObj.GetComponent<SettingsObject>();

        var title = settings.title;
        var text = settings.description;
        
        title.text = "Game Mode";
        text.text  = Main.Modes[0];

        
        foreach (VRInteractable interactable in settings.vrInteractables)
        {
            if (interactable.gameObject.name.Contains("Next"))
            {
                interactable.OnInteract = new UnityEvent();
                interactable.OnInteract.AddListener(delegate { text.text = CycleGameModes.Next(text.text); });
            }

            if (interactable.gameObject.name.Contains("Prev"))
            {
                interactable.OnInteract = new UnityEvent();
                interactable.OnInteract.AddListener(delegate { text.text = CycleGameModes.Previous(text.text); });
            }
        }

        settings.GetComponentInParent<VRPointInteractableCanvas>().RefreshInteractables();
    }
}