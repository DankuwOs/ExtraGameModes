using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExtraGameModes;
using ExtraGameModes.Infected;
using ExtraGameModes.Infected.InfectedSyncs;
using Harmony;
using Rewired.Data.Mapping;
using Steamworks.Data;
using UnityEngine;
using UnityEngine.Events;
using VTNetworking;
using VTOLVR.Multiplayer;
using Random = System.Random;

[HarmonyPatch(typeof(VTOLMPSceneManager), "RPC_BeginScenario")]
public static class VTOLMPSceneManagerPatch
{
    public static void Postfix(VTOLMPSceneManager __instance)
    {
        if (CycleGameModes.CurrentGameMode != "Infected" || !VTOLMPUtils.IsMPAndHost())
            return;
        
        if (__instance.localPlayer.team == Teams.Enemy)
            Infected.ToAlliedTeamChange();
        

        var random = new Random();

        InfectPlayer(
            VTOLMPLobbyManager.instance.connectedPlayers[
                random.Next(VTOLMPLobbyManager.instance.connectedPlayers.Count)]);
    }

    public static async Task InfectPlayer(PlayerInfo infos)
    {
        await Task.Delay(TimeSpan.FromSeconds(Infected.InfectDelay));

        Infected.FirstInfectedPlayer.Invoke(infos.steamUser.Id);
    }
}

[HarmonyPatch(typeof(VTOLMPSceneManager), "Awake")]
public static class VTOLMPSceneManagerPatch1
{
    public static void Postfix(VTOLMPSceneManager __instance)
    {
        var syncObj = new GameObject("InfectedSyncObj");
        var netEntity = __instance.netEntity;
        syncObj.transform.parent = netEntity.gameObject.transform;
        syncObj.AddComponent<InfectPlayerSync>();
        syncObj.AddComponent<TimerSync>();
        syncObj.AddComponent<WinSync>();
        netEntity.netSyncs.Add(syncObj.GetComponents<VTNetSyncRPCOnly>());
    }
}