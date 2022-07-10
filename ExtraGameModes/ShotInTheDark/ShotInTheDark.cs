using ExtraGameModes;
using ExtraGameModes.Infected;
using Harmony;
using UnityEngine;
using VTOLVR.Multiplayer;

[HarmonyPatch(typeof(CameraFogSettings), "OnPreRender")]
public static class CameraFogSettingsPatch
{

    public static void Postfix(CameraFogSettings __instance)
    {
        if (CycleGameModes.CurrentGameMode != "Shot in the Dark" || VTOLMPSceneManager.instance.localPlayer.vehicleActor == null)
            return;

        
        RenderSettings.fogDensity = 0.004f;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
    }
}