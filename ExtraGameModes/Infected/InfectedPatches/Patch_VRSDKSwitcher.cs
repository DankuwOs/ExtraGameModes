using ExtraGameModes;
using ExtraGameModes.Infected;
using Harmony;

[HarmonyPatch(typeof(VRSDKSwitcher), "Awake")]
public static class PlayerSpawnPatch
{

    public static void Postfix(PlayerSpawn __instance)
    {
        if (CycleGameModes.CurrentGameMode != "Infected")
            return;
        
        Infected.CameraGameObject = __instance.gameObject.transform.Find("[CameraRig]/Camera (eye)").gameObject;
    }
}