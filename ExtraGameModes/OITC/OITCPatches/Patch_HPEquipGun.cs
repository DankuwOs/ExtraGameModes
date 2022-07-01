using ExtraGameModes;
using Harmony;
using static ExtraGameModes.OITC.OneInTheChamber;

[HarmonyPatch(typeof(HPEquipGun), "OnEquip")]
public static class WeaponManagerPatch
{
    public static void Postfix(HPEquipGun __instance)
    {
        if (CycleGameModes.CurrentGameMode != "OITC") 
            return;
        
        var shortName = __instance.shortName;
        switch (shortName)
        {
            case "Vulcan":
            case "GAU-22":
            case "Rail Gun":
                
                break;
            
            default: return;
        }

        if (__instance.weaponManager.actor.isPlayer || __instance.weaponManager.actor.isHumanPlayer)
            EquippedGun(__instance);
    }
}