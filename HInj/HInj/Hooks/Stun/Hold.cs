using Harmony;
using SDG.Unturned;
using System.Reflection;
using UnityEngine;

namespace HInj.Hooks
{
    public class Stun
    {
        //Custom stun
        public static bool Prefix([HarmonyArgument("amount")] float amount)
        {
            if (Global.AllOff)
                return true;

            Ex(Global.VisSettings.ReduceFlash ? 0.08f : amount * 5f);
            return false;
        }

        private static void Ex(float inc)
        {
            typeof(PlayerUI).GetField("stunColor", BindingFlags.NonPublic | BindingFlags.Static)
             .SetValue(null, new Color(1f, 1f, 1f, inc));

            ((Component)MainCamera.instance).GetComponent<AudioSource>()
                .PlayOneShot((AudioClip)Resources.Load("Sounds/General/Stun"), inc);
        }
    }
}
