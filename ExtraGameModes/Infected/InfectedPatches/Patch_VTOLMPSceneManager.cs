using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExtraGameModes;
using ExtraGameModes.Infected;
using Harmony;
using Rewired.Data.Mapping;
using Steamworks.Data;
using UnityEngine;
using VTOLVR.Multiplayer;
using Random = System.Random;

[HarmonyPatch(typeof(VTOLMPSceneManager), "RPC_BeginScenario")]
public static class VTOLMPSceneManagerPatch
{
    public static void Postfix(VTOLMPSceneManager __instance)
    {
        if (CycleGameModes.CurrentGameMode != "Infected")
            return;

        foreach (var player in VTOLMPLobbyManager.instance.connectedPlayers)
        {
            Infected.ToAlliedTeamChange(player.steamUser);
        }

        Random random = new Random();

        InfectPlayer(VTOLMPLobbyManager.instance.connectedPlayers[random.Next(VTOLMPLobbyManager.instance.connectedPlayers.Count)]);
    }

    public static async Task InfectPlayer(PlayerInfo infos)
    {
        await Task.Delay(TimeSpan.FromSeconds(Infected.InfectDelay));
        
        VTOLMPSceneManager.instance.RPC_TeamChanged(infos.steamUser.Id, (int)Teams.Enemy);
        
        if (infos == VTOLMPSceneManager.instance.localPlayer)
            Infected.EnemySpawnObjective();
        
        Infected.StartCountdown();
    }
}