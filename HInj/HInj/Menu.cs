using SDG.Unturned;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace HInj
{
    public class Menu : MonoBehaviour
    {
        public static Rect MainMenuSize = new Rect(10, 10, 150, 130);
        public static Rect AimbotMenuSize = new Rect(160, 10, 200, 190);
        public static Rect VisualsMenuSize = new Rect(360, 10, 200, 300);
        public static Rect MiscMenuSize = new Rect(560, 10, 160, 310);
        public static Rect VehicleMenuSize = new Rect(720, 10, 130, 195);
        public static Rect ItemMenuSize = new Rect(10, 200, 160, 400);

        public void OnGUI()
        {
            if (Global.AllOff || Hooks.askScreenshot.NeedingSpy)
                return;

            if (MenuDashboardUI.active)
            {
                PlayerPauseUI.active = false;
                return;
            }

            if (!MenuPauseUI.active && !PlayerPauseUI.active)
                return;

            MainMenuSize = GUI.Window(15, MainMenuSize, Menu_Main, "<size=12>BoCheat</size>");

            if (Global.AimEnabled)
                AimbotMenuSize = GUI.Window(16, AimbotMenuSize, Menu_Aimbot, "<size=12>Aimbot</size>");

            if (Global.VisualsEnabled)
                VisualsMenuSize = GUI.Window(17, VisualsMenuSize, Menu_Visuals, "<size=12>Visuals</size>");

            if (Global.MiscEnabled)
                MiscMenuSize = GUI.Window(18, MiscMenuSize, Menu_Misc, "<size=12>Misc</size>");

            if (Global.VehicleEnabled)
                VehicleMenuSize = GUI.Window(19, VehicleMenuSize, Menu_Vehicle, "<size=12>Vehicles</size>");

            if (Global.ItemEnabled)
            {
                ItemMenuSize = GUI.Window(20, ItemMenuSize, Menu_Item, "<size=12>Items</size>");

                iFind = GUI.Window(21, iFind, ItemSearchMenu, "<size=12>Item-Searching</size>");
            }
        }

        void Menu_Main(int id)
        {
            Global.AimEnabled = GUILayout.Toggle(Global.AimEnabled, "Aimbot");

            Global.VisualsEnabled = GUILayout.Toggle(Global.VisualsEnabled, "Visuals");

            Global.VehicleEnabled = GUILayout.Toggle(Global.VehicleEnabled, "Vehicles");

            Global.ItemEnabled = GUILayout.Toggle(Global.ItemEnabled, "Items");

            Global.MiscEnabled = GUILayout.Toggle(Global.MiscEnabled, "Miscellaneous");

            //GUI.DragWindow();
        }

        void Menu_Aimbot(int id)
        {
            Global.AimSettings.LegitBot = GUILayout.Toggle(Global.AimSettings.LegitBot, "LegitBot");

            Global.AimSettings.VisibleCheck = GUILayout.Toggle(Global.AimSettings.VisibleCheck, "Visible Check");

            Global.AimSettings.DropCalculator = GUILayout.Toggle(Global.AimSettings.DropCalculator, "Drop Calculator");

            Global.AimSettings.ForceHit = GUILayout.Toggle(Global.AimSettings.ForceHit, "Force Hit");

            Global.AimSettings.NoDrop = GUILayout.Toggle(Global.AimSettings.NoDrop, "No Drop");

            Global.AimSettings.RBypassOne = GUILayout.Toggle(Global.AimSettings.RBypassOne, "Rage-Bypass 1");

            GUILayout.Label("Aim Speed: " + Global.AimSettings.AimSpeed.ToString("F0"));

            Global.AimSettings.AimSpeed = GUILayout.HorizontalSlider(Global.AimSettings.AimSpeed, 1f, 100f);
        }

        void Menu_Visuals(int id)
        {
            Global.VisSettings.NightVision = GUILayout.Toggle(Global.VisSettings.NightVision, "Nightvision");

            Global.VisSettings.ReduceFlash = GUILayout.Toggle(Global.VisSettings.ReduceFlash, "Reduced Flash");

            Global.VisSettings.NightMode = GUILayout.Toggle(Global.VisSettings.NightMode, "Night Mode");

            Global.VisSettings.betterWater = GUILayout.Toggle(Global.VisSettings.betterWater, "Better Water");

            Global.VisSettings.Players = GUILayout.Toggle(Global.VisSettings.Players, "Players");

            Global.VisSettings.Items = GUILayout.Toggle(Global.VisSettings.Items, "Items");

            Global.VisSettings.Glow = GUILayout.Toggle(Global.VisSettings.Glow, "Glow");

            Global.VisSettings.Box = GUILayout.Toggle(Global.VisSettings.Box, "Box");

            Global.VisSettings.Chams = GUILayout.Toggle(Global.VisSettings.Chams, "Chams");

            GUILayout.Label("Night Light: " + Global.VisSettings.NightLight.ToString("F2"));

            Global.VisSettings.NightLight = GUILayout.HorizontalSlider(Global.VisSettings.NightLight, 0f, 1.5f);

            GUILayout.Label("P-Distance: " + Global.VisSettings.Distance.ToString("F0"));

            Global.VisSettings.Distance = GUILayout.HorizontalSlider(Global.VisSettings.Distance, 1f, 650f);
        }

        void Menu_Misc(int id)
        {
            Global.MiscSettings.ForceThirdperson = GUILayout.Toggle(Global.MiscSettings.ForceThirdperson, "Override Thirdperson");

            Global.MiscSettings.ForceSatellite = GUILayout.Toggle(Global.MiscSettings.ForceSatellite, "Force Satellite (GPS)");

            Global.MiscSettings.ForceCompass = GUILayout.Toggle(Global.MiscSettings.ForceCompass, "Force Compass");

            Global.MiscSettings.NoRecoilSway = GUILayout.Toggle(Global.MiscSettings.NoRecoilSway, "No Recoil");

            Global.MiscSettings.BypassTimers = GUILayout.Toggle(Global.MiscSettings.BypassTimers, "Bypass Timers");

            Global.MiscSettings.FastSalvage = GUILayout.Toggle(Global.MiscSettings.FastSalvage, "Fast Salvage");

            Global.MiscSettings.NoFog = GUILayout.Toggle(Global.MiscSettings.NoFog, "No Fog");

            Global.MiscSettings.AlwaysDay = GUILayout.Toggle(Global.MiscSettings.AlwaysDay, "Always Late-Morning");

            Global.MiscSettings.ShowMapAll = GUILayout.Toggle(Global.MiscSettings.ShowMapAll, "Map reveal");

            Global.MiscSettings.Jesus = GUILayout.Toggle(Global.MiscSettings.Jesus, "Jesus");

            Global.MiscSettings.NoSpy = GUILayout.Toggle(Global.MiscSettings.NoSpy, "No Spy");

            Global.MiscSettings.CleanSpy = GUILayout.Toggle(Global.MiscSettings.CleanSpy, "Clean Spy");

            if (GUILayout.Button("Freecam"))
                Miscellaneous.ToggleFreecam();
        }

        void Menu_Vehicle(int id)
        {
            Global.VehicleSettings.Fly = GUILayout.Toggle(Global.VehicleSettings.Fly, "Fly");

            GUILayout.Label("V-Lift: " + Global.VehicleSettings.VehicLift.ToString("F0"));

            Global.VehicleSettings.VehicLift = GUILayout.HorizontalSlider(Global.VehicleSettings.VehicLift, 1f, 30f);

            Global.VehicleSettings.Ping = GUILayout.Toggle(Global.VehicleSettings.Ping, "Ping");

            Global.VehicleSettings.BypassOne = GUILayout.Toggle(Global.VehicleSettings.BypassOne, "Bypass 1");

            GUILayout.Label("Ping-Force: " + Global.VehicleSettings.PingForce.ToString("F3"));

            Global.VehicleSettings.PingForce = GUILayout.HorizontalSlider(Global.VehicleSettings.PingForce, 0f, 17.5f);

            Global.VehicleSettings.Sink = GUILayout.Toggle(Global.VehicleSettings.Sink, "Sink");
        }

        void Menu_Item(int id)
        {
            Global.ItemSettings.GrabAll = GUILayout.Toggle(Global.ItemSettings.GrabAll, "Grab All");

            Global.ItemSettings.GrabGuns = GUILayout.Toggle(Global.ItemSettings.GrabGuns, "Guns");

            Global.ItemSettings.GrabClips = GUILayout.Toggle(Global.ItemSettings.GrabClips, "Clips");

            Global.ItemSettings.GrabMedical = GUILayout.Toggle(Global.ItemSettings.GrabMedical, "Medical");

            Global.ItemSettings.GrabFood = GUILayout.Toggle(Global.ItemSettings.GrabFood, "Food");

            Global.ItemSettings.AutoArrow = GUILayout.Toggle(Global.ItemSettings.AutoArrow, "Auto-Arrow");

            Global.ItemSettings.AutoClothes = GUILayout.Toggle(Global.ItemSettings.AutoClothes, "Auto-Clothes");

            Global.ItemSettings.AutoMedical = GUILayout.Toggle(Global.ItemSettings.AutoMedical, "Auto-Medical");

            GUILayout.Label("* Filtering");

            GUILayout.BeginVertical("Box");

            if (GUILayout.Button(Global.ItemSettings.TargetRarity ? "Target Rarity" : "Minimum Rarity"))
                Global.ItemSettings.TargetRarity = !Global.ItemSettings.TargetRarity;

            Global.ItemSettings.FilterRarity = GUILayout.SelectionGrid(Global.ItemSettings.FilterRarity,
                new string[6] { "Common", "Uncommon", "Rare", "Epic", "Legendary", "Mythical" },
                1);

            GUILayout.EndVertical();
        }

        public static Rect iFind = new Rect(170, 340, 330, 260);
        public static string rt = "I-name/ID", rt2 = "3";
        Vector2 scrollPos;
        void ItemSearchMenu(int id)
        {
            GUILayout.BeginHorizontal();
            GUI.SetNextControlName("txfrx1");
            rt = GUILayout.TextField(rt, GUILayout.Width(240));
            GUI.SetNextControlName("txfrx2");
            rt2 = GUILayout.TextField(rt2, GUILayout.Width(30));

            //Error with keycode detection here, didn't really read cause it was working
            if (GUILayout.Button("Add") || (Event.current.isKey && Event.current.keyCode == KeyCode.Return && (GUI.GetNameOfFocusedControl() == "txfrx1" || GUI.GetNameOfFocusedControl() == "txfrx2")))
            {
                Asset a = null;
                if (Regex.Matches(rt, @"[a-zA-Z]").Count < 1)
                    a = Assets.find(EAssetType.ITEM, ushort.Parse(rt));
                else
                    a = (new List<ItemAsset>(Assets.find(EAssetType.ITEM).Cast<ItemAsset>())).
                        Where(i => i.itemName != null)
                        .OrderBy(i => i.itemName.Length)
                        .FirstOrDefault(i => i.itemName.ToLower().Contains(rt.ToLower()));

                if (a == null)
                    return;

                Global.ItemSettings.CustomItems.Add(new Items.itemSs((Regex.Matches(rt, @"[a-zA-Z]").Count < 1) ? ushort.Parse(rt) : ((ItemAsset)a).id, ((ItemAsset)a).itemName, int.Parse(rt2)));
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginVertical();
            scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Width(330), GUILayout.Height(250));
            Items.itemSs qRemove = new Items.itemSs(0, "", 0);
            foreach (Items.itemSs z in Global.ItemSettings.CustomItems)
            {
                GUILayout.BeginHorizontal(GUILayout.Height(10));

                GUILayout.Label(z.iName, GUILayout.Width(187));
                GUILayout.Label(z.itemID.ToString(), GUILayout.Width(50));
                GUILayout.Label(z.Max.ToString(), GUILayout.Width(25));
                if (GUILayout.Button("X", GUILayout.Width(30)))
                    qRemove = z;

                GUILayout.EndHorizontal();
            }
            if (qRemove.itemID != 0)
                Global.ItemSettings.CustomItems.Remove(qRemove);
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }
    }
}
