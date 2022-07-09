using System.Collections.Generic;
using ExtraGameModes;
using Harmony;
using UnityEngine;
using VTOLVR.Multiplayer;

[HarmonyPatch(typeof(VTMPMainMenu), "LaunchMPGameForScenario")]
public class Custom_LobbyData
{
    public static void Postfix()
    {
        if (VTOLMPLobbyManager.isLobbyHost)
        {
            VTOLMPLobbyManager.currentLobby.SetData("Danku_GameMode", CycleGameModes.CurrentGameMode);
        }
        else if (VTOLMPLobbyManager.isInLobby)
        {
            string gameMode = VTOLMPLobbyManager.currentLobby.GetData("Danku_GameMode");
            CycleGameModes.CurrentGameMode = gameMode;
        }
    }
}