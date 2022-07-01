using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
using Rewired;
using UnityEngine;
using VTOLVR.Multiplayer;

namespace ExtraGameModes
{
    public class Main : VTOLMOD
    {
        public static List<String> Modes = new List<string>()
        {
            "None",
            "OITC",
            "Gun Game",
            "Infected"
        };
        
        
        // This method is run once, when the Mod Loader is done initialising this game object
        public override void ModLoaded()
        {
            HarmonyInstance instance = HarmonyInstance.Create("dankuwos.extragamemodes");
            instance.PatchAll();
            
            base.ModLoaded();
        }
    }
}