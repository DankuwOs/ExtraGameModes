using System;
using System.Dynamic;
using ExtraGameModes;
using ExtraGameModes.Infected;
using ExtraGameModes.OITC;
using Harmony;
using UnityEngine;
using VTOLVR.Multiplayer;

[HarmonyPatch(typeof(Actor), "Start")]
public static class ActorPatch
{
    public static void Postfix(Actor __instance)
    {
        switch (CycleGameModes.CurrentGameMode)
        {
            case "None":
                
                return;
            
            case "OITC":
                
                __instance.health.OnDeath.AddListener(delegate
                {
                    OneInTheChamber.ActorKilled(__instance);
                });
                break;
            
            case "Gun Game":
                Debug.Log("Gun Game not implemented yet");
                break;
            
            case "Infected":
                
                if (__instance.isPlayer || __instance.isHumanPlayer)
                {
                    __instance.health.OnDeath.AddListener(delegate
                    {
                        Infected.ActorKilled(__instance);
                    });
                    
                    switch (__instance.team)
                    {
                        case Teams.Allied:
                            Infected.AlliedSpawnObjective();
                            break;
                        case Teams.Enemy:
                            Infected.EnemySpawnObjective();
                            break;
                    }

                    var count = __instance.weaponManager.equipCount;
                    for (int i = 0; i < count; i++)
                    {
                        var weapon = __instance.weaponManager.GetEquip(i);
                        var shortName = weapon.shortName;
                        
                        if (shortName != "GAU-22" || shortName != "Vulcan" || shortName != "Rail Gun")
                            __instance.weaponManager.GBreakEquip(i);
                    }
                }

                break;
        }
            
    }
}