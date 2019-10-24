using Harmony;
using SDG.Unturned;
using System.Diagnostics;
using UnityEngine;

namespace HInj.Hooks
{
    public class IsPositionUnderwater
    {
        static float reg = 0f;
        public static void Prefix([HarmonyArgument("position")] Vector3 position)
        {
            reg = LevelLighting.seaLevel;

            if (!Global.VisSettings.betterWater || Global.AllOff)
                return;

            if (Player.player != null)
                if (Player.player.movement != null)
                    if (Player.player.movement.getVehicle() != null && !Global.VehicleSettings.Sink)
                        return;

            StackTrace stackTrace = new StackTrace();

            string TType = stackTrace.GetFrame(3).GetMethod().DeclaringType.ToString();
            if (TType == "SDG.Unturned.InteractableVehicle")
                return;
            if (TType == "SDG.Unturned.PlayerMovement")
                return;
            if (TType == "SDG.Unturned.PlayerStance")
                return;

            LevelLighting.seaLevel = -20f;
        }

        public static void Postfix([HarmonyArgument("position")] Vector3 position)
        {
            if (Player.player.movement.getVehicle() != null && !Global.VehicleSettings.Sink)
                return;

            LevelLighting.seaLevel = reg;
        }
    }
}
