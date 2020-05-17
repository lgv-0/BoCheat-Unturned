using Harmony;
using SDG.Unturned;
using System;
using UnityEngine;

namespace HInj.Hooks
{
    [HarmonyPatch(typeof(LevelLighting), "updateLighting")]
    public class updLighting
    {
        public static void Prefix(ref float ___dayVolume, ref float ___nightVolume)
        {
            if ((!Global.MiscSettings.AlwaysDay && !Global.VisSettings.NightMode) || Global.AllOff || askScreenshot.NeedingSpy)
            {
                if (askScreenshot.NeedingSpy)
                    askScreenshot.TimeReady = true;
                return;
            }

            Console.WriteLine($"{LevelLighting.time} < {LevelLighting.bias} < {LevelLighting.azimuth} < {LevelLighting.transition}");
        }

        public static void Postfix()
        {
            if ((!Global.MiscSettings.AlwaysDay && !Global.VisSettings.NightMode) || Global.AllOff)
                return;
        }
    }
}
