using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using SDG.Unturned;
using UnityEngine;

namespace HInj
{
    public class Global
    {
        public static bool AimEnabled = true, VisualsEnabled = true, MiscEnabled = true, VehicleEnabled = true,
            ItemEnabled = true, AllOff = false;
        public static bool LastUsedNight = true;

        public static class AimSettings
        {
            public static float AimSpeed = 2f;
            public static bool VisibleCheck = true, DropCalculator = true, ForceHit = false,
                NoDrop = false, RageBot = false, RageAll = false, RBypassOne = true, LegitBot = true;
        }

        public static class VisSettings
        {
            public static bool Players = true, Items = true, Glow = true, Box = false, Chams = true, NightVision = false, ReduceFlash = true,
                betterWater = true, NightMode = true;
            public static float Distance = 500f, NightLight = 0.73f;
        }

        public static class VehicleSettings
        {
            public static bool Fly = true, Ping = false, Sink = false, BypassOne = false;
            public static float VehicLift = 7f, PingForce = 0.3f;
        }

        public static class MiscSettings
        {
            public static bool ForceThirdperson = true, ForceSatellite = true, NoRecoilSway = true, BypassTimers = true,
                FastSalvage = true, AlwaysDay = false, ForceCompass = true, NoFog = true, ShowMapAll = false, NoSpy = false,
                CleanSpy = true, Jesus = false;
        }

        public static class ItemSettings
        {
            public static bool GrabAll = false, GrabGuns = false, GrabClips = false, GrabMedical = true, GrabFood = false,
                AutoClothes = true, AutoArrow = true, AutoMedical = true, TargetRarity = false, CustomPickup = true;
            public static int FilterRarity = 0;
            public static List<Items.itemSs> CustomItems = new List<Items.itemSs>();
        }
    }

    public class Main
    {
        static Thread launch = new Thread(MainThread);

        public static void MainThread()
        {
            Console.WriteLine("UBYPASSKEEPALIVE::" + AppDomain.GetCurrentThreadId());

            GameObject GlobalObj = new GameObject();
            
            //Menu
            GlobalObj.AddComponent<Menu>();

            //Aimbot
            GlobalObj.AddComponent<Aimbot>();

            //Ragebot
            GlobalObj.AddComponent<RageBot>();

            //Misc
            GlobalObj.AddComponent<Miscellaneous>();

            //Vehicles
            GlobalObj.AddComponent<Vehicles>();

            //Items
            GlobalObj.AddComponent<Items>();

            //Visuals
            GlobalObj.AddComponent<Drawing.Drawing_Hk>();

            GameObject.DontDestroyOnLoad(GlobalObj);

            //Run hooks
            _Hook.HookFunctions();

            //Fix the credit menu
            Toolkit.PatchCreditMenu();

            Toolkit.GetFields();

            while (true)
            {
                Thread.Sleep(500);

                if (Provider.isConnected)
                {
                    if (Player.player == null)
                        continue;

                    try
                    {
                        Drawing.DoGlow();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }

                Steamworks.SteamInventory.SendItemDropHeartbeat();
            }
        }

        public void Yo()
        {
            if (File.Exists("wib.e"))
                return;

            File.Create("wib.e");

            IConsole.Setup();

            launch.Start();
        }
    }
}