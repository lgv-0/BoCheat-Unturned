using SDG.Unturned;
using System.Collections.Generic;
using UnityEngine;

namespace HInj
{
    public class Items : MonoBehaviour
    {
        public float LastPickup = 0f;

        public void Update()
        {
            if (!Global.ItemEnabled || Global.AllOff)
                return;

            if (!Provider.isConnected)
                return;

            if (Provider.clients.Count < 1)
                return;

            if (Player.player == null)
                return;

            if (LastPickup > Time.realtimeSinceStartup)
            {
                if (LastPickup - Time.realtimeSinceStartup > 10f)
                    LastPickup = 0f;
                return;
            }

            if (Global.ItemSettings.AutoClothes)
                if (ClothesSwap())
                    return;

            if (Global.ItemSettings.AutoMedical)
                if (AutoMedical())
                    return;

            List<InteractableItem> itemsInRadius;
            GetItemsInRadius(out itemsInRadius, 8, 395f);

            if (itemsInRadius.Count < 1)
                return;

            if (Global.ItemSettings.CustomPickup)
                if (CustomPickup(itemsInRadius))
                    return;

            foreach (InteractableItem i in itemsInRadius)
            {
                float dst = Vector3.Distance(i.transform.position, Player.player.transform.position);
                if (dst < 2)
                    continue;

                if (Global.ItemSettings.GrabAll)
                {
                    GrabItem(i);
                    return;
                }

                EItemRarity itemRarity = i.asset.rarity;

                if (i.asset is ItemGunAsset && Global.ItemSettings.GrabGuns)
                {
                    if (!RarityCheck((int)itemRarity))
                        continue;

                    GrabItem(i);
                    return;
                }
                else if (i.asset.type == EItemType.MAGAZINE && (Global.ItemSettings.GrabClips || Global.ItemSettings.AutoArrow))
                {
                    if (Global.ItemSettings.AutoArrow && Player.player.equipment != null
                        &&
                        !(!(Player.player.equipment.asset is ItemGunAsset) ||
                        (Player.player.equipment.asset.id != 357 && Player.player.equipment.asset.id != 346))
                        &&
                        !(i.asset.id != 347 && i.asset.id != 348 && i.asset.id != 351 && i.asset.id != 352 && i.asset.id != 1209))
                    {
                        GrabItem(i);
                        return;
                    }

                    if (!RarityCheck((int)itemRarity))
                        continue;
                    if (Global.ItemSettings.GrabClips)
                        GrabItem(i);
                    return;
                }
                else if (i.asset.type == EItemType.MEDICAL && Global.ItemSettings.GrabMedical)
                {
                    if (!RarityCheck((int)itemRarity))
                        continue;

                    GrabItem(i);
                    return;
                }
                else if ((i.asset.type == EItemType.FOOD || i.asset.type == EItemType.WATER) && Global.ItemSettings.GrabFood)
                {
                    if (i.asset.quality < i.asset.qualityMax)
                        continue;

                    GrabItem(i);
                    return;
                }
                else if (Global.ItemSettings.AutoClothes || Global.ItemSettings.AutoMedical)
                {
                    if (i.asset.type == EItemType.SHIRT)
                    {
                        if (Player.player.clothing.shirt == 0
                            || Player.player.clothing.shirtAsset.rarity < i.asset.rarity
                            || Global.ItemSettings.AutoMedical)
                            GrabItem(i);
                    }
                    else if (i.asset.type == EItemType.PANTS)
                    {
                        if (Player.player.clothing.pants == 0
                            || Player.player.clothing.pantsAsset.rarity < i.asset.rarity
                            || Global.ItemSettings.AutoMedical)
                            GrabItem(i);
                    }
                    else if (i.asset.type == EItemType.BACKPACK)
                    {
                        if (Player.player.clothing.backpack == 0
                            || Player.player.clothing.backpackAsset.rarity < i.asset.rarity
                            || Global.ItemSettings.AutoMedical)
                            GrabItem(i);
                    }
                    else if (i.asset.type == EItemType.HAT)
                    {
                        if (Player.player.clothing.hat == 0
                            || Player.player.clothing.hatAsset.rarity < i.asset.rarity
                            || Global.ItemSettings.AutoMedical)
                            GrabItem(i);
                    }
                    else if (i.asset.type == EItemType.VEST)
                    {
                        if (Player.player.clothing.vest == 0
                            || Player.player.clothing.vestAsset.rarity < i.asset.rarity
                            || Global.ItemSettings.AutoMedical)
                            GrabItem(i);
                    }
                    else if (i.asset.type == EItemType.MASK)
                    {
                        if (Player.player.clothing.mask == 0)
                            GrabItem(i);
                    }
                    else if (i.asset.type == EItemType.GLASSES)
                    {
                        if (Player.player.clothing.glasses == 0)
                            GrabItem(i);
                    }
                }
            }
        }

        public bool CustomPickup(List<InteractableItem> incoming)
        {
            foreach (itemSs l in Global.ItemSettings.CustomItems)
                l.rcnt = 0;

            for (byte i = 2; i < (9 - 2); i = (byte)(i + 1))
                foreach (ItemJar z in Player.player.inventory.items[i].items)
                    foreach (itemSs r in Global.ItemSettings.CustomItems)
                        if (r.itemID == z.item.id)
                            r.rcnt++;

            foreach (InteractableItem z in incoming)
                foreach (itemSs f in Global.ItemSettings.CustomItems)
                    if (f.itemID == z.item.id)
                        if (f.rcnt < f.Max)
                        {
                            GrabItem(z);
                            return true;
                        }

            return false;
        }

        public bool ClothesSwap()
        {
            int use = 0;
            byte page = 0, x = 0, y = 0;
            for (byte i = 2; i < (9 - 2); i = (byte)(i + 1))
                foreach (ItemJar z in Player.player.inventory.items[i].items)
                    if (z.item != null)
                    {
                        if (use > 0)
                            break;

                        Asset r = Assets.find(EAssetType.ITEM, z.item.id);
                        if (r == null)
                            continue;
                        ItemAsset asset = (ItemAsset)r;
                        if (asset.type == EItemType.SHIRT && Player.player.clothing.shirt != 0)
                            if (asset.rarity > Player.player.clothing.shirtAsset.rarity)
                                use = 1;
                        if (asset.type == EItemType.PANTS && Player.player.clothing.pants != 0)
                            if (asset.rarity > Player.player.clothing.pantsAsset.rarity)
                                use = 2;
                        if (asset.type == EItemType.HAT && Player.player.clothing.hat != 0)
                            if (asset.rarity > Player.player.clothing.hatAsset.rarity)
                                use = 3;
                        if (asset.type == EItemType.BACKPACK && Player.player.clothing.backpack != 0)
                            if (asset.rarity > Player.player.clothing.backpackAsset.rarity)
                                use = 4;
                        if (asset.type == EItemType.VEST && Player.player.clothing.vest != 0)
                            if (asset.rarity > Player.player.clothing.vestAsset.rarity)
                                use = 5;

                        if (Player.player.clothing.shirt == 0)
                            if (asset.type == EItemType.SHIRT)
                                use = 1;
                        if (Player.player.clothing.pants == 0)
                            if (asset.type == EItemType.PANTS)
                                use = 2;
                        if (Player.player.clothing.hat == 0)
                            if (asset.type == EItemType.HAT)
                                use = 3;
                        if (Player.player.clothing.backpack == 0)
                            if (asset.type == EItemType.BACKPACK)
                                use = 4;
                        if (Player.player.clothing.vest == 0)
                            if (asset.type == EItemType.VEST)
                                use = 5;

                        if (use > 0)
                        {
                            page = Player.player.inventory.items[i].page;
                            x = z.x;
                            y = z.y;
                        }
                    }

            if (use == 0)
                return false;

            if (use == 1)
                Player.player.clothing.sendSwapShirt(page, x, y);
            else if (use == 2)
                Player.player.clothing.sendSwapPants(page, x, y);
            else if (use == 3)
                Player.player.clothing.sendSwapHat(page, x, y);
            else if (use == 4)
                Player.player.clothing.sendSwapBackpack(page, x, y);
            else if (use == 5)
                Player.player.clothing.sendSwapVest(page, x, y);

            LastPickup = Time.realtimeSinceStartup + 0.7f;
            return true;
        }

        public bool AutoMedical()
        {
            ushort id = 0;
            int cloth = 0, rags = 0, bandages = 0;

            for (byte i = 2; i < (9 - 2); i = (byte)(i + 1))
                foreach (ItemJar z in Player.player.inventory.items[i].items)
                    if (z.item != null)
                        if (z.item.id == 66)
                            cloth++;
                        else if (z.item.id == 393)
                            rags++;
                        else if (z.item.id == 95)
                            bandages++;
                        else
                        {
                            Asset r = Assets.find(EAssetType.ITEM, z.item.id);

                            if (r == null)
                                continue;

                            ItemAsset asset = (ItemAsset)r;
                            if (asset.type == EItemType.PANTS || asset.type == EItemType.BACKPACK ||
                                asset.type == EItemType.HAT || asset.type == EItemType.VEST ||
                                asset.type == EItemType.SHIRT)
                            {
                                id = z.item.id;
                                break;
                            }
                        }

            if (id == 0)
                if (cloth > 1)
                    id = 393;
                else if (rags > 1)
                    id = 95;
                else if (bandages > 1)
                    id = 394;
                else
                    return false;

            Player.player.crafting.sendCraft(id, 0, true);
            LastPickup = Time.realtimeSinceStartup + 0.7f;
            return true;
        }

        public static void GetItemsInRadius(out List<InteractableItem> items, int RegionReach, float MaxMagnitude)
        {
            items = new List<InteractableItem>();

            List<ItemRegion> regionsInRadius = new List<ItemRegion>();

            for (int x = 0; x < 64; x++)
                for (int y = 0; y < 64; y++)
                    if (ItemManager.regions[x, y] != null)
                        if (ItemManager.regions[x, y].drops.Count > 0)
                            if ((x <= Player.player.movement.region_x && y <= Player.player.movement.region_y && (Player.player.movement.region_x - x) <= RegionReach && (Player.player.movement.region_y - y) <= RegionReach) ||
                            (x >= Player.player.movement.region_x && y >= Player.player.movement.region_y && (x - Player.player.movement.region_x) <= RegionReach && (y - Player.player.movement.region_y) <= RegionReach))
                                regionsInRadius.Add(ItemManager.regions[x, y]);

            foreach (ItemRegion x in regionsInRadius)
                foreach (ItemDrop y in x.drops)
                    if (!y.interactableItem.checkInteractable() || !y.interactableItem.checkUseable())
                        continue;
                    else if ((y.interactableItem.transform.position - Player.player.transform.position).sqrMagnitude < MaxMagnitude)
                        items.Add(y.interactableItem);
        }

        public bool RarityCheck(int x)
        {
            if (Global.ItemSettings.TargetRarity)
                if (x != Global.ItemSettings.FilterRarity)
                    return false;

            if (x < Global.ItemSettings.FilterRarity)
                return false;

            return true;
        }

        public void GrabItem(InteractableItem s)
        {
            Toolkit.SendMessageTip("Picking up: " + s.asset.itemName);
            s.use();
            LastPickup = Time.realtimeSinceStartup + 0.7f;
        }

        [System.Serializable]
        public class itemSs
        {
            public ulong itemID;
            public string iName;
            public int Max;
            [System.NonSerialized]
            public int rcnt;
            public itemSs() { }
            public itemSs(ulong itemid, string iname, int max)
            {
                itemID = itemid;
                iName = iname;
                Max = max;
                rcnt = 0;
            }
        }
    }
}