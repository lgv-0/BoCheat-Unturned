using SDG.Unturned;
using UnityEngine;

namespace HInj
{
    public partial class Drawing
    {
        public static void DrawItems()
        {
            System.Collections.Generic.List<InteractableItem> itemsInRadius;
            Items.GetItemsInRadius(out itemsInRadius, 18, 3950f);

            foreach (InteractableItem drop in itemsInRadius)
            {
                if (drop.asset is ItemGunAsset)
                {
                    EItemRarity rar = ((ItemGunAsset)drop.asset).rarity;
                    if (rar == EItemRarity.COMMON || rar == EItemRarity.UNCOMMON || rar == EItemRarity.RARE)
                        continue;

                    Vector3 Pos = Drawing_Hk.WorldToScreen(drop.transform.position);
                    if (Pos.x < 0 || Pos.y < 0 || Pos.x > Screen.width || Pos.y > Screen.height || Pos.z < 0)
                        continue;

                    int Size = (int)(500 / Vector3.Distance(drop.transform.position, Player.player.transform.position));
                    if (Size < 7)
                        Size = 7;
                    if (Size > 11)
                        Size = 11;

                    GUIStyle centeredStyle = GUI.skin.GetStyle("Label");
                    centeredStyle.alignment = TextAnchor.UpperCenter;
                    centeredStyle.fontSize = Size;
                    centeredStyle.richText = false;
                    GUI.Label(new Rect(Pos.x - 60, Pos.y - 10, 120, 20), drop.asset.itemName, centeredStyle);
                }
            }
        }
    }
}
