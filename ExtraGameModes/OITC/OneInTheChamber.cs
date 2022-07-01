using UnityEngine;
using VTOLVR.Multiplayer;

namespace ExtraGameModes.OITC
{
    public class OneInTheChamber
    {
        public static HPEquipGun equippedGun;

        public static void ActorKilled(Actor actor)
        {
            if (CycleGameModes.CurrentGameMode != "OITC")
                return;

            var creditActor = actor.killedByActor;

            Debug.Log(creditActor.name + " killed " + actor.name);

            if (creditActor == null || equippedGun == null)
            {
                Debug.Log("No credit actor or gun");
                return;
            }

            if (creditActor == equippedGun.weaponManager.actor)
            {
                if (equippedGun.shortName == "Rail Gun")
                    equippedGun.gun.currentAmmo += 1;
                else
                    equippedGun.gun.currentAmmo += 40;
            }
        }

        public static void EquippedGun(HPEquipGun gun)
        {
            equippedGun = gun;

            equippedGun.gun.currentAmmo = equippedGun.shortName == "Rail Gun" ? 1 : 40;
        }
    }
}