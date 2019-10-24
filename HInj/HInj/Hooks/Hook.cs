using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Harmony;
using SDG.Provider;
using SDG.Unturned;

namespace HInj
{
    public class _Hook
    {
        public static HarmonyInstance hInstance = null;

        //Todo: Instead of doing this copy+paste 700 times
        //      Make it happen at runtime (https://github.com/pardeike/Harmony/wiki/Target-Method-Annotations)

        public static void HookFunctions()
        {
            hInstance = HarmonyInstance.Create("main");

            //Spy Detour///////////////////////////////////////////////
            MethodInfo originalMethod = typeof(Player).GetMethod("askScreenshot", BindingFlags.Public | BindingFlags.Instance);

            MethodInfo prefixMethod, postfixMethod, transpiler;
            PatchTools.GetPatches(typeof(Hooks.askScreenshot), out prefixMethod, out postfixMethod, out transpiler);

            PatchProcessor patcher = new PatchProcessor(hInstance,
                new List<MethodBase> { originalMethod },
                new HarmonyMethod(prefixMethod), //Prefix
                null, //Postfix
                null //Transpiler
                );

            patcher.Patch();

            //Aim Detour/////////////////////////////////////////////
            originalMethod = typeof(PlayerLook).GetMethod("updateAim", BindingFlags.Public | BindingFlags.Instance);

            prefixMethod = null; postfixMethod = null; transpiler = null;
            PatchTools.GetPatches(typeof(Hooks.UpdateAim), out prefixMethod, out postfixMethod, out transpiler);

            patcher = new PatchProcessor(hInstance,
                new List<MethodBase> { originalMethod },
                new HarmonyMethod(prefixMethod), //Prefix
                null, //Postfix
                null //Transpiler
                );

            patcher.Patch();

            //Stun Detour/////////////////////////////////////////////
            originalMethod = typeof(PlayerUI).GetMethod("stun", BindingFlags.Public | BindingFlags.Static);

            prefixMethod = null; postfixMethod = null; transpiler = null;
            PatchTools.GetPatches(typeof(Hooks.Stun), out prefixMethod, out postfixMethod, out transpiler);

            patcher = new PatchProcessor(hInstance,
                new List<MethodBase> { originalMethod },
                new HarmonyMethod(prefixMethod), //Prefix
                null, //Postfix
                null //Transpiler
                );

            patcher.Patch();

            //Underwater Detour/////////////////////////////////////////////
            originalMethod = typeof(LevelLighting).GetMethod("isPositionUnderwater", BindingFlags.Public | BindingFlags.Static);

            prefixMethod = null; postfixMethod = null; transpiler = null;
            PatchTools.GetPatches(typeof(Hooks.IsPositionUnderwater), out prefixMethod, out postfixMethod, out transpiler);

            patcher = new PatchProcessor(hInstance,
                new List<MethodBase> { originalMethod },
                new HarmonyMethod(prefixMethod), //Prefix
                new HarmonyMethod(postfixMethod), //Postfix
                null //Transpiler
                );

            patcher.Patch();

            //refreshDynamicMap Detour/////////////////////////////////////////////
            originalMethod = typeof(PlayerDashboardInformationUI).GetMethod("refreshDynamicMap", BindingFlags.Public | BindingFlags.Static);

            prefixMethod = null; postfixMethod = null; transpiler = null;
            PatchTools.GetPatches(typeof(Hooks.refreshDynamicMap), out prefixMethod, out postfixMethod, out transpiler);

            patcher = new PatchProcessor(hInstance,
                new List<MethodBase> { originalMethod },
                new HarmonyMethod(prefixMethod), //Prefix
                new HarmonyMethod(postfixMethod), //Postfix
                null //Transpiler
                );

            patcher.Patch();

            //sendRaycast Detour/////////////////////////////////////////////
            originalMethod = typeof(PlayerInput).GetMethod("sendRaycast", BindingFlags.Instance | BindingFlags.Public);

            prefixMethod = null; postfixMethod = null; transpiler = null;
            PatchTools.GetPatches(typeof(Hooks.sendRaycast), out prefixMethod, out postfixMethod, out transpiler);

            patcher = new PatchProcessor(hInstance,
                new List<MethodBase> { originalMethod },
                new HarmonyMethod(prefixMethod), //Prefix
                null, //Postfix
                null //Transpiler
                );

            patcher.Patch();

            //Ballistics Detour/////////////////////////////////////////////
            originalMethod = typeof(UseableGun).GetMethod("ballistics", BindingFlags.Instance | BindingFlags.NonPublic);

            prefixMethod = null; postfixMethod = null; transpiler = null;
            PatchTools.GetPatches(typeof(Hooks.Ballistics), out prefixMethod, out postfixMethod, out transpiler);

            patcher = new PatchProcessor(hInstance,
                new List<MethodBase> { originalMethod },
                new HarmonyMethod(prefixMethod), //Prefix
                null, //Postfix
                null //Transpiler
                );

            patcher.Patch();

            //sendChat Detour/////////////////////////////////////////////
            originalMethod = typeof(ChatManager).GetMethod("sendChat", BindingFlags.Static | BindingFlags.Public);

            prefixMethod = null; postfixMethod = null; transpiler = null;
            PatchTools.GetPatches(typeof(Hooks.sendChat), out prefixMethod, out postfixMethod, out transpiler);

            patcher = new PatchProcessor(hInstance,
                new List<MethodBase> { originalMethod },
                new HarmonyMethod(prefixMethod), //Prefix
                null, //Postfix
                null //Transpiler
                );

            patcher.Patch();
            Hooks.sendChat.spammer.Start();

            //onLevelLoaded Detour/////////////////////////////////////////////
            originalMethod = typeof(Provider).GetMethod("onLevelLoaded", BindingFlags.Static | BindingFlags.NonPublic);

            prefixMethod = null; postfixMethod = null; transpiler = null;
            PatchTools.GetPatches(typeof(Hooks.lvlLoaded), out prefixMethod, out postfixMethod, out transpiler);

            patcher = new PatchProcessor(hInstance,
                new List<MethodBase> { originalMethod },
                new HarmonyMethod(prefixMethod), //Prefix
                null, //Postfix
                null //Transpiler
                );

            patcher.Patch();

            //UpdateLighting Detour/////////////////////////////////////////////
            originalMethod = typeof(LevelLighting).GetMethod("updateLighting", BindingFlags.Public | BindingFlags.Static);

            prefixMethod = null; postfixMethod = null; transpiler = null;
            PatchTools.GetPatches(typeof(Hooks.updLighting), out prefixMethod, out postfixMethod, out transpiler);

            patcher = new PatchProcessor(hInstance,
                new List<MethodBase> { originalMethod },
                new HarmonyMethod(prefixMethod), //Prefix
                new HarmonyMethod(postfixMethod), //Postfix
                null //Transpiler
                );

            patcher.Patch();

            //DoorAnimation Detour/////////////////////////////////////////////
            originalMethod = typeof(InteractableDoor).GetMethod("updateToggle", BindingFlags.Public | BindingFlags.Instance);

            prefixMethod = null; postfixMethod = null; transpiler = null;
            PatchTools.GetPatches(typeof(Hooks.DoorAnimation), out prefixMethod, out postfixMethod, out transpiler);

            patcher = new PatchProcessor(hInstance,
                new List<MethodBase> { originalMethod },
                new HarmonyMethod(prefixMethod), //Prefix
                null, //Postfix
                null //Transpiler
                );

            patcher.Patch();

            //InvResult Detour/////////////////////////////////////////////
            originalMethod = typeof(TempSteamworksEconomy).GetMethod("onInventoryResultReady", BindingFlags.NonPublic | BindingFlags.Instance);

            prefixMethod = null; postfixMethod = null; transpiler = null;
            PatchTools.GetPatches(typeof(Skins.onInvResultReady), out prefixMethod, out postfixMethod, out transpiler);

            patcher = new PatchProcessor(hInstance,
                new List<MethodBase> { originalMethod },
                new HarmonyMethod(prefixMethod), //Prefix
                null, //Postfix
                null //Transpiler
                );

            patcher.Patch();
        }
    }
}