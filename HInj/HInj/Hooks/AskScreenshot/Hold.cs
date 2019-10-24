using Harmony;
using SDG.Unturned;
using System;
using UnityEngine;

namespace HInj.Hooks
{
    //Screenshot detour
    public class askScreenshot
    {
        public static bool Prefix([HarmonyArgument("steamID")] Steamworks.CSteamID steamID)
        {
            Console.WriteLine("Spy! Major excuses time my man.");

            if (Global.MiscSettings.NoSpy && Global.MiscSettings.CleanSpy)
                Global.MiscSettings.NoSpy = false;

            if (Global.AllOff)
                return true;

            if (Global.MiscSettings.NoSpy)
                return false;

            SpyQ++;

            return false;
        }

        public static bool NeedingSpy = false,
            ESPReady = false,
            ChamReady = false,
            TimeReady = false,
            GlowReady = false;
        public static void SpyUpd()
        {
            if (SpyQ > 0)
                NeedingSpy = true;

            if (NeedingSpy && ESPReady && ChamReady && TimeReady && GlowReady)
            {
                DoRealSpy();
                SpyQ--;
                NeedingSpy = false;
                ESPReady = false;
                ChamReady = false;
                TimeReady = false;
                GlowReady = false;
            }
        }

        public static int SpyQ = 0;
        public static void DoRealSpy()
        {
            Texture2D screenshotRaw = null, screenshotFinal = null;
            byte[] data;
            float heightRatio;
            Color[] newColors;
            Color[] oldColors;
            float widthRatio;

            if ((screenshotRaw != null) && ((screenshotRaw.width != Screen.width) || (screenshotRaw.height != Screen.height)))
            {
                UnityEngine.Object.DestroyImmediate(screenshotRaw);
                screenshotRaw = null;
            }
            if (screenshotRaw == null)
            {
                screenshotRaw = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
                screenshotRaw.name = "Screenshot_Raw";
                screenshotRaw.hideFlags = HideFlags.HideAndDontSave;
            }
            screenshotRaw.ReadPixels(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), 0, 0, false);
            if (screenshotFinal == null)
            {
                screenshotFinal = new Texture2D(640, 480, TextureFormat.RGB24, false);
                screenshotFinal.name = "Screenshot_Final";
                screenshotFinal.hideFlags = HideFlags.HideAndDontSave;
            }
            oldColors = screenshotRaw.GetPixels();
            newColors = new Color[screenshotFinal.width * screenshotFinal.height];
            widthRatio = ((float)screenshotRaw.width) / ((float)screenshotFinal.width);
            heightRatio = ((float)screenshotRaw.height) / ((float)screenshotFinal.height);
            for (int i = 0; i < screenshotFinal.height; i++)
            {
                int num3 = ((int)(i * heightRatio)) * screenshotRaw.width;
                int num4 = i * screenshotFinal.width;
                for (int j = 0; j < screenshotFinal.width; j++)
                {
                    int num6 = (int)(j * widthRatio);
                    newColors[num4 + j] = oldColors[num3 + num6];
                }
            }
            screenshotFinal.SetPixels(newColors);
            screenshotFinal.filterMode = FilterMode.Trilinear;
            data = screenshotFinal.EncodeToJPG(0x17);
            if (data.Length < 0x7530)
            {
                Player.player.channel.longBinaryData = true;
                Player.player.channel.openWrite();
                Player.player.channel.write(data);
                Player.player.channel.closeWrite("tellScreenshotRelay", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER);
                Player.player.channel.longBinaryData = false;
            }
        }
    }
}