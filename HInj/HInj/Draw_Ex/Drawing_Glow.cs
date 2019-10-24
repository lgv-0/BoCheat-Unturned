using HighlightingSystem;
using SDG.Unturned;
using UnityEngine;

namespace HInj
{
    public partial class Drawing
    {
        public static void DoGlow()
        {
            foreach (SteamPlayer play in Provider.clients)
            {
                if (play.player == Player.player)
                    continue;

                Highlighter Highlighter = play.player.gameObject.GetComponent<Highlighter>();

                if ((Global.VisualsEnabled && Global.VisSettings.Glow) && !Global.AllOff && !Hooks.askScreenshot.NeedingSpy)
                {
                    if (Highlighter == null)
                        Highlighter = play.player.gameObject.AddComponent<Highlighter>();

                    //Deprecated, http://docs.deepdream.games/HighlightingSystem/5.0/#toc9.1upgrading_from_v4.3_to_v5.0
                    //Highlighter.SeeThroughOn();
                    //Highlighter.OccluderOn();

                    //Color blue if in group
                    if (play.player.quests.groupID == Player.player.quests.groupID)
                        Highlighter.ConstantOnImmediate(Color.blue);
                    else
                        Highlighter.ConstantOnImmediate(Color.yellow);
                }
                else if (Highlighter != null)
                {
                    Highlighter.ConstantOffImmediate();
                    MonoBehaviour.Destroy(Highlighter);
                }
            }

            if (Global.VisSettings.Items)
                foreach (ItemRegion region in ItemManager.regions)
                    foreach (ItemDrop drop in region.drops)
                        if (drop.interactableItem.asset is ItemGunAsset)
                        {
                            EItemRarity rar = ((ItemGunAsset)drop.interactableItem.asset).rarity;
                            if (rar == EItemRarity.COMMON || rar == EItemRarity.UNCOMMON || rar == EItemRarity.RARE)
                                continue;

                            Highlighter Highlighter = drop.interactableItem.gameObject.GetComponent<Highlighter>();

                            if (Global.VisualsEnabled && Global.VisSettings.Glow && !Global.AllOff && !Hooks.askScreenshot.NeedingSpy)
                            {
                                if (Highlighter == null)
                                    Highlighter = drop.interactableItem.gameObject.AddComponent<Highlighter>();

                                Highlighter.ConstantOnImmediate(Color.white);
                            }
                            else if (Highlighter != null)
                            {
                                Highlighter.ConstantOffImmediate();
                                MonoBehaviour.Destroy(Highlighter);
                            }
                        }

            if (Hooks.askScreenshot.NeedingSpy)
            {
                System.Threading.Thread.Sleep(85);
                Hooks.askScreenshot.GlowReady = true;
            }
        }
    }
}
