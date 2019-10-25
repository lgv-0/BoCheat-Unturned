using Harmony;
using SDG.Unturned;

namespace HInj.Hooks
{
    [HarmonyPatch(typeof(LevelLighting), "updateLighting")]
    public class updLighting
    {
        public static float Btime = 0;
        public static void Prefix(ref float ____time)
        {
            if ((!Global.MiscSettings.AlwaysDay && !Global.VisSettings.NightMode) || Global.AllOff || askScreenshot.NeedingSpy)
            {
                if (askScreenshot.NeedingSpy)
                    askScreenshot.TimeReady = true;
                return;
            }

            Btime = ____time;

            if (Global.MiscSettings.AlwaysDay)
                ____time = (uint)(LightingManager.cycle * LevelLighting.transition);
            else if (Global.VisSettings.NightMode)
                ____time = 0.75f;
        }

        public static void Postfix(ref float ____time)
        {
            if ((!Global.MiscSettings.AlwaysDay && !Global.VisSettings.NightMode) || Global.AllOff)
                return;

            ____time = Btime;
        }
    }
}
