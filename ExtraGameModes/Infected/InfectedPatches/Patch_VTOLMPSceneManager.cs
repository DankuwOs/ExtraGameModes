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
using TMPro;
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
        if (CycleGameModes.CurrentGameMode != "Infected")
            return;
        
        var syncObj = new GameObject("InfectedSyncObj");
        var netEntity = __instance.netEntity;
        syncObj.transform.parent = netEntity.gameObject.transform;
        syncObj.AddComponent<InfectPlayerSync>();
        syncObj.AddComponent<TimerSync>();
        syncObj.AddComponent<WinSync>();
        netEntity.netSyncs.Add(syncObj.GetComponents<VTNetSyncRPCOnly>());
    }
}
[HarmonyPatch(typeof(VTOLMPSceneManager), "SetupVehicleSlots")]
public static class VTOLMPSceneManagerPatch2
{
    public static void Postfix(VTOLMPSceneManager __instance)
    {
        if (CycleGameModes.CurrentGameMode != "Infected")
            return;

        var allied = __instance.alliedSlots;
        var enemy = __instance.enemySlots;

        var alliedList = new List<VTOLMPSceneManager.VehicleSlot>();
        var enemyList = new List<VTOLMPSceneManager.VehicleSlot>();
        
        var alliedIdx = 0;
        for (int i = 0; i < allied.Count; i++)
        {
            alliedIdx += 1;
            VTOLMPSceneManager.VehicleSlot slot = new VTOLMPSceneManager.VehicleSlot
            {
                designation = allied[i].designation,
                vehicleName = allied[i].vehicleName,
                spawnID = allied[i].spawnID,
                team = allied[i].team,
                idx = allied.Last().idx + alliedIdx,
                slotTitle = allied[i].slotTitle,
                slotID = allied[i].slotID,
            };
            Debug.Log("Adding slot: " + slot.slotID + " " + slot.vehicleName + " " + slot.designation + " " +
                      slot.team + " " + slot.spawnID + " " + slot.idx + " " + slot.slotTitle + " IDK what these mean");
            alliedList.Add(slot);
        }

        var enemyIdx = 0;
        for (int i = 0; i < enemy.Count; i++)
        {
            enemyIdx += 1;
            VTOLMPSceneManager.VehicleSlot slot = new VTOLMPSceneManager.VehicleSlot
            {
                designation = enemy[i].designation,
                vehicleName = enemy[i].vehicleName,
                spawnID = enemy[i].spawnID,
                team = enemy[i].team,
                idx = enemy.Last().idx + enemyIdx,
                slotTitle = enemy[i].slotTitle,
                slotID = enemy[i].slotID
            };
            Debug.Log("Adding slot: " + slot.slotID + " " + slot.vehicleName + " " + slot.designation + " " +
                      slot.team + " " + slot.spawnID + " " + slot.idx + " " + slot.slotTitle + " IDK what these mean");
            enemyList.Add(slot);
        }
        
        enemy.AddRange(enemyList);
        allied.AddRange(alliedList);
    }
}