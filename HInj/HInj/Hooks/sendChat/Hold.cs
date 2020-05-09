using Harmony;
using SDG.Unturned;
using System;
using System.Threading;
using UnityEngine;

namespace HInj.Hooks
{
    [HarmonyPatch(typeof(ChatManager), "sendChat", new System.Type[] { typeof(EChatMode), typeof(string) })]
    public class sendChat
    {
        public static Thread spammer = new Thread(xThread);
        static string spamString = "";
        static bool isSpamming = false;

        private static void xThread()
        {
            while (true)
            {
                Thread.Sleep(275);
                if (!isSpamming)
                    continue;
                ChatManager.instance.channel.send("askChat", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[2]
                {
                    (byte)0,
                    spamString
                });
            }
        }

        public static bool Prefix(EChatMode mode, string text, ref ChatManager ___manager)
        {
            //Todo: Big switch statement with default array
            if (text.Contains("_spam"))
            {
                string[] array = text.Split(' ');
                if (array.Length > 1)
                {
                    if (array[1] == "start")
                        isSpamming = true;
                    else if (array[1] == "stop")
                        isSpamming = false;
                    else
                        spamString = text.Remove(0, 6);
                }
            }
            else if (text.Contains("_tp"))
            {
                string[] array = text.Split(' ');
                if (array.Length > 1)
                {
                    SteamPlayer t = PlayerTool.getSteamPlayer(array[1]);
                    if (t != null)
                        Player.player.transform.position = t.player.transform.position;
                    else
                        Toolkit.ChatMsg("Could not find player");
                }
            }
            else if (text.Contains("_rageon"))
            {
                Toolkit.ChatMsg("It's bo time baby.", Color.green);
                Global.AimSettings.RageBot = true;
            }
            else if (text.Contains("_rageoff"))
                Global.AimSettings.RageBot = false;
            else if (text.Contains("_rageall"))
            {
                Global.AimSettings.RageAll = !Global.AimSettings.RageAll;
                Toolkit.ChatMsg("Set " + Global.AimSettings.RageAll);
            }
            else if (text.Contains("_ragetarget"))
            {
                string[] array = text.Split(' ');
                if (array.Length > 1)
                {
                    SteamPlayer tr = PlayerTool.getSteamPlayer(array[1]);
                    if (tr != null)
                    {
                        if (RageBot.mTargets.Contains(tr))
                        {
                            Toolkit.ChatMsg("De-target @" + tr.playerID.characterName, Color.yellow);
                            RageBot.mTargets.Remove(tr);
                        }
                        else
                        {
                            RageBot.mTargets.Add(tr);
                            Toolkit.ChatMsg("Target @" + tr.playerID.characterName + ", send em to bo-town boy!", Color.green);
                        }
                    }
                    else
                        Toolkit.ChatMsg("No player found for " + array[1], Color.red);
                }
            }
            else if (text.Contains("_off"))
                Global.AllOff = true;
            else if (text.Contains("_on"))
                Global.AllOff = false;
            else if (text.Contains("_hide"))
            {
                Toolkit.HideChats("BoCheat");
                Toolkit.SendMessageTip("Missing Space");
            }
            else
            {
                object[] arguments = new object[] { (byte)mode, text };
                ___manager.channel.send("askChat", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, arguments);
            }
            return false;
        }
    }
}
