using Harmony;
using SDG.Unturned;
using UnityEngine;

namespace HInj.Hooks
{
    [HarmonyPatch(typeof(InteractableDoor), "updateToggle", new System.Type[] { typeof(bool) })]
    public class DoorAnimation
    {
        //DOOR
        public static bool Prefix(bool newOpen, InteractableDoor __instance, ref bool ____isOpen, ref Transform ___barrierTransform,
            ref BoxCollider ___placeholderCollider)
        {
            ____isOpen = newOpen;

            AudioSource r = __instance.GetComponent<AudioSource>();
            r.pitch = 1.25f;
            r.Play();

            ___barrierTransform.gameObject.SetActive(!newOpen);

            Animation x = __instance.GetComponent<Animation>();
            x["Open"].speed = 2f;
            x["Close"].speed = 2f;

            if (newOpen)
                x.Play("Open");
            else
                x.Play("Close");

            return false;
        }
        //STUCK
    }
}
