using Harmony;
using SDG.Unturned;

namespace HInj.Hooks
{
    [HarmonyPatch(typeof(PlayerInput), "sendRaycast", new System.Type[] { typeof(RaycastInfo) })]
    public class sendRaycast
    {
        public static bool Prefix(ref RaycastInfo info)
        {
            if (Global.AllOff)
                return true;

            if ((info.zombie != null || info.player != null) && (Global.AimSettings.ForceHit || Global.AimSettings.RageBot))
                info.limb = ELimb.SKULL;

            return true;
        }
    }
}
