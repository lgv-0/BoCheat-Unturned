using UnityEngine;
using SDG.Unturned;
using System.Reflection;

namespace HInj
{
    public static class Toolkit
    {
        public static FieldInfo TooltipLastMessage, TooltipIsMessaged, TooltipMessageLabel2, TooltipMessageProgress2_0,
            TooltipMessageProgress2_1, TooltipMessageIcon2, groupId, chatHist, chatPreviews;

        public static void GetFields()
        {
            TooltipLastMessage = typeof(PlayerUI).GetField("lastMessage", BindingFlags.NonPublic | BindingFlags.Static);
            TooltipIsMessaged = typeof(PlayerUI).GetField("isMessaged", BindingFlags.NonPublic | BindingFlags.Static);
            TooltipMessageLabel2 = typeof(PlayerUI).GetField("messageLabel2", BindingFlags.NonPublic | BindingFlags.Static);
            TooltipMessageIcon2 = typeof(PlayerUI).GetField("messageIcon2", BindingFlags.NonPublic | BindingFlags.Static);
            TooltipMessageProgress2_0 = typeof(PlayerUI).GetField("messageProgress2_0", BindingFlags.NonPublic | BindingFlags.Static);
            TooltipMessageProgress2_1 = typeof(PlayerUI).GetField("messageProgress2_1", BindingFlags.NonPublic | BindingFlags.Static);
            groupId = typeof(PlayerQuests).GetField("<groupID>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance);
            chatHist = typeof(PlayerLifeUI).GetField("chatHistoryLabels", BindingFlags.NonPublic | BindingFlags.Static);
            chatPreviews = typeof(PlayerLifeUI).GetField("chatPreviewLabels", BindingFlags.NonPublic | BindingFlags.Static);
        }

        //Sends a tooltip message at the bottom of the player screen
        public static void SendMessageTip(string Message, bool FullClear)
        {
            PlayerUI.messageBox2.sizeOffset_Y = 50;
            PlayerUI.messageBox2.positionOffset_Y = -80 - PlayerUI.messageBox2.sizeOffset_Y;
            PlayerUI.messageBox2.isVisible = true;

            TooltipLastMessage.SetValue(null, Time.realtimeSinceStartup);

            TooltipIsMessaged.SetValue(null, true);

            SleekLabel u = (SleekLabel)TooltipMessageLabel2.GetValue(null);
            u.text = Message;
            TooltipMessageLabel2.SetValue(null, u);

            if (FullClear)
            {
                SleekImageTexture IconText = (SleekImageTexture)TooltipMessageIcon2.GetValue(null);
                IconText.isVisible = false;
                TooltipMessageIcon2.SetValue(null, IconText);

                SleekProgress Prog = (SleekProgress)TooltipMessageProgress2_0.GetValue(null);
                Prog.isVisible = false;
                TooltipMessageProgress2_0.SetValue(null, Prog);

                Prog = (SleekProgress)TooltipMessageProgress2_1.GetValue(null);
                Prog.isVisible = false;
                TooltipMessageProgress2_1.SetValue(null, Prog);
            }
        }

        public static void PatchCreditMenu()
        {
            FieldInfo dat = typeof(MenuCreditsUI).GetField("credits", BindingFlags.NonPublic | BindingFlags.Static);
            CreditsContributorContributionPair[] credits = (CreditsContributorContributionPair[])dat.GetValue(null);

            //Make a new list
            credits = new CreditsContributorContributionPair[]
            {
                new CreditsContributorContributionPair("lgv-0", "uh"),
                new CreditsContributorContributionPair("Cervine", "oh")
            };

            dat.SetValue(null, credits);
        }

        //Display message in chat
        public static void ChatMsg(string msg, Color? col = null)
        {
            ChatManager.receiveChatMessage(new Steamworks.CSteamID(0),
                "https://i.imgur.com/AYX6G8f.jpg",
                EChatMode.GLOBAL,
                col ?? Color.yellow,
                true,
                "<color=cyan>BoCheat:</color> " + msg);
        }

        //Blank out chats containing specific match
        public static void HideChats(string Match)
        {
            SleekChat[] x = (SleekChat[])chatHist.GetValue(null);
            SleekChat[] y = (SleekChat[])chatPreviews.GetValue(null);
            ReceivedChatMessage p = new ReceivedChatMessage(null, null, EChatMode.GLOBAL, Color.white, false, "");
            for (int uhwhy = 0; uhwhy < ChatManager.receivedChatHistory.Count; uhwhy++)
                if (ChatManager.receivedChatHistory[uhwhy].contents.Contains(Match))
                    ChatManager.receivedChatHistory[uhwhy] = p;
            for (int uhwhy = 0; uhwhy < y.Length; uhwhy++)
                if (y[uhwhy].representingChatMessage.contents.Contains(Match))
                    y[uhwhy].representingChatMessage = p;
            for (int uhwhy = 0; uhwhy < x.Length; uhwhy++)
                if (x[uhwhy].representingChatMessage.contents.Contains(Match))
                    x[uhwhy].representingChatMessage = p;
        }
    }
}
