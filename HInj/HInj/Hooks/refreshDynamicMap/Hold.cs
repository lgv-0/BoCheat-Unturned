using Harmony;
using SDG.Unturned;
using System.Collections.Generic;

namespace HInj.Hooks
{
    [HarmonyPatch(typeof(PlayerDashboardInformationUI), "refreshDynamicMap")]
    class refreshDynamicMap
    {
        public static List<Steamworks.CSteamID> f = new List<Steamworks.CSteamID>();
        public static List<SteamPlayer> Targets = new List<SteamPlayer>();
        static bool tempSwap = false;

        public static void Prefix()
        {
            if (!Global.MiscSettings.ShowMapAll || Global.AllOff)
                return;

            Targets.Clear();
            f.Clear();

            for (int i = 0; i < Provider.clients.Count; i++)
            {
                SteamPlayer player2 = Provider.clients[i];
                if ((player2.model != null) && (player2.playerID.steamID != Provider.client))
                    Targets.Add(player2);
            }

            if (!Player.player.quests.isMemberOfAGroup)
            {
                tempSwap = true;
                Toolkit.groupId.SetValue(Player.player.quests, new Steamworks.CSteamID(999999));
            }

            foreach (SteamPlayer y in Targets)
            {
                f.Add((Steamworks.CSteamID)Toolkit.groupId.GetValue(y.player.quests));
                Toolkit.groupId.SetValue(y.player.quests, Player.player.quests.groupID);
            }
        }

        public static void Postfix()
        {
            if (!Global.MiscSettings.ShowMapAll)
                return;

            if (tempSwap)
            {
                tempSwap = false;
                Toolkit.groupId.SetValue(Player.player.quests, Steamworks.CSteamID.Nil);
            }

            for (int i = 0; i < Targets.Count; i++)
                Toolkit.groupId.SetValue(Targets[i].player.quests, f[i]);
        }
    }
}
