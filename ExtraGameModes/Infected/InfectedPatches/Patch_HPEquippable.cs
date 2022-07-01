using ExtraGameModes;
using Harmony;
using UnityEngine;

[HarmonyPatch(typeof(HPEquippable), "Awake")]
public static class HPEquippablePatch
{
    public static void Prefix(HPEquippable __instance)
    {
        if (CycleGameModes.CurrentGameMode != "Infected" || CycleGameModes.CurrentGameMode != "OITC")
            return;

        var shortName = __instance.shortName;
        if (shortName != "GAU-22" || shortName != "Vulcan" || shortName != "Rail Gun")
            Object.DestroyImmediate(__instance.gameObject, true);
    }
}