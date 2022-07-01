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
            
            switch (CycleGameModes.CurrentGameMode)
            {
                case "Infected":
                    var infectedScenario = VTOLMPLobbyManager.GetScenario(VTOLMPLobbyManager.currentLobby).scenario;
                    
                    // dunno if this works, but sure.
                    infectedScenario.autoPlayerCount = false;
                    
                    infectedScenario.mpPlayerCount = 32; // Doubt you can even go this high, but sure.
                    infectedScenario.overrideAlliedPlayerCount = 32;
                    infectedScenario.overrideEnemyPlayerCount = 16;
                    break;
            }
            
        }
        else if (VTOLMPLobbyManager.isInLobby)
        {
            string gameMode = VTOLMPLobbyManager.currentLobby.GetData("Danku_GameMode");
            CycleGameModes.CurrentGameMode = gameMode;
        }
    }
}