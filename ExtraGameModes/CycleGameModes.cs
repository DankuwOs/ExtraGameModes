using System;
using System.Linq;
using UnityEngine;
using static ExtraGameModes.Main;

namespace ExtraGameModes
{
    public class CycleGameModes
    {
        public static String CurrentGameMode = "None";
        
        public static String Next(string text)
        {
            if (text == Modes.Last())
            {
                CurrentGameMode = Modes.First();
                return Modes.First();
            }
            CurrentGameMode = Modes[Modes.IndexOf(text) + 1];
            return Modes[Modes.IndexOf(text) + 1];
        }
        
        public static String Previous(string text)
        {
            if (text == Modes.First())
            {
                CurrentGameMode = Modes.Last();
                return Modes.Last();
            }
            CurrentGameMode = Modes[Modes.IndexOf(text) - 1];
            return Modes[Modes.IndexOf(text) - 1];
        }
    }
}