using Harmony;
using SDG.Unturned;

namespace HInj.Hooks
{
    public class sendRaycast
    {
        public static bool Prefix([HarmonyArgument("info")] ref RaycastInfo info)
        {
            if (Global.AllOff)
                return true;

            if ((info.zombie != null || info.player != null) && (Global.AimSettings.ForceHit || Global.AimSettings.RageBot))
                info.limb = ELimb.SKULL;

            return true;
        }
    }
}
