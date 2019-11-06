using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Threading;

namespace HInj
{
    public class Friend
    {
        public static Thread friendWatch = new Thread(FriendThread);
        public static List<Steamworks.CSteamID> FriendsCurrent = new List<Steamworks.CSteamID>();
        public static List<Steamworks.CSteamID> FriendsTemp = new List<Steamworks.CSteamID>();
        static bool CurrentlySwapping = false;

        public static void FriendThread()
        {
            while (true)
            {
                Thread.Sleep(TimeSpan.FromSeconds(30));
                if (Global.AimSettings.ignoreSteamFriends)
                    try
                    {
                        FriendsTemp = FriendsCurrent;
                        CurrentlySwapping = true;
                        FriendsCurrent.Clear();

                        for (int i = 0; i < Steamworks.SteamFriends.GetFriendCount(Steamworks.EFriendFlags.k_EFriendFlagImmediate); i++)
                            FriendsCurrent.Add(Steamworks.SteamFriends.GetFriendByIndex(i, Steamworks.EFriendFlags.k_EFriendFlagImmediate));

                        CurrentlySwapping = false;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
            }
        }

        public static bool isTarget(SteamPlayer Check)
        {
            if (Player.player.quests.isMemberOfAGroup)
                if (Check.player.quests.groupID == Player.player.quests.groupID)
                    return false;

            if (Global.AimSettings.ignoreSteamFriends)
                if (!CurrentlySwapping)
                {
                    if (FriendsCurrent.Contains(Check.playerID.steamID))
                        return false;
                }
                else if (FriendsTemp.Contains(Check.playerID.steamID))
                    return false;

            return true;
        }
    }
}
