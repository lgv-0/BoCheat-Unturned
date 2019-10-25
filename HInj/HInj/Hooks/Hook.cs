using System.Collections.Generic;
using System.Reflection;
using Harmony;
using SDG.Unturned;

namespace HInj
{
    public class _Hook
    {
        public static HarmonyInstance hInstance = null;

        public static void HookFunctions()
        {
            hInstance = HarmonyInstance.Create("main");

            hInstance.PatchAll();

            Hooks.sendChat.spammer.Start();

            //Spy
            MethodInfo originalMethod = typeof(Player).GetMethod("askScreenshot", BindingFlags.Public | BindingFlags.Instance);

            MethodInfo prefixMethod, postfixMethod, transpiler;
            PatchTools.GetPatches(typeof(Hooks.askScreenshot), out prefixMethod, out postfixMethod, out transpiler);

            PatchProcessor patcher = new PatchProcessor(hInstance,
                new List<MethodBase> { originalMethod },
                new HarmonyMethod(prefixMethod),
                null,
                null
                );

            patcher.Patch();
        }
    }
}
