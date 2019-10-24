using SDG.Unturned;
using UnityEngine;

namespace HInj
{
    public partial class Drawing
    {
        public static void DrawItems()
        {
            foreach (ItemRegion i in ItemManager.regions)
                foreach (ItemDrop drop in i.drops)
                {
                    if (drop.interactableItem.asset is ItemGunAsset)
                    {
                        EItemRarity rar = ((ItemGunAsset)drop.interactableItem.asset).rarity;
                        if (rar == EItemRarity.COMMON || rar == EItemRarity.UNCOMMON || rar == EItemRarity.RARE)
                            continue;

                        float dist = Vector3.Distance(drop.model.position, Player.player.transform.position);
                        if (dist > 160)
                            continue;

                        Vector3 Pos = Drawing_Hk.WorldToScreen(drop.model.position);
                        if (Pos.x < 0 || Pos.y < 0 || Pos.x > Screen.width || Pos.y > Screen.height || Pos.z < 0)
                            continue;

                        int Size = (int)(500 / dist);
                        if (Size < 7)
                            Size = 7;
                        if (Size > 11)
                            Size = 11;

                        GUIStyle centeredStyle = GUI.skin.GetStyle("Label");
                        centeredStyle.alignment = TextAnchor.UpperCenter;
                        centeredStyle.fontSize = Size;
                        centeredStyle.richText = false;
                        GUI.Label(new Rect(Pos.x - 60, Pos.y - 10, 120, 20), drop.interactableItem.asset.itemName, centeredStyle);
                    }
                }
        }
    }
}
