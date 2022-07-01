using ExtraGameModes;
using ExtraGameModes.Infected;
using Harmony;
using Steamworks.ServerList;
using VTOLVR.Multiplayer;

[HarmonyPatch(typeof(VTOLMPBriefingRoom), "Start")]
public static class BriefingRoomPatch
{
    public static void Postfix(VTOLMPBriefingRoom __instance)
    {
        if (CycleGameModes.CurrentGameMode != "Infected")
            return;
        
        Infected.ToAlliedTeamChange(VTOLMPSceneManager.instance.localPlayer.steamUser);
    }
}