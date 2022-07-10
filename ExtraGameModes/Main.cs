using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
using Rewired;
using Steamworks.Data;
using UnityEngine;
using VTOLVR.Multiplayer;
using Color = UnityEngine.Color;

namespace ExtraGameModes
{
    public class Main : VTOLMOD
    {
        public static List<String> Modes = new List<string>()
        {
            "None",
            "OITC",
            "Shot in the Dark",
            "Infected"
        };

        public static GameObject settingsPrefab = null;

        // This method is run once, when the Mod Loader is done initialising this game object
        public override void ModLoaded()
        {
            HarmonyInstance instance = HarmonyInstance.Create("dankuwos.extragamemodes");
            instance.PatchAll();
            
            base.ModLoaded();
            
            string pathToBundle = Path.Combine(ModFolder, "settings.splooge");
            settingsPrefab = HelperScripts.FileLoader.GetAssetBundleAsGameObject(pathToBundle, "gameModeObject");
        }

        

        public void Update()
        {
            if (CycleGameModes.CurrentGameMode == "Infected")
                Infected.Infected.InfectedUpdate(); // Infected is has monobehaviour but no work??
        }
    }
}