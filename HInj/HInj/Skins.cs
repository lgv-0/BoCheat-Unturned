using SDG.Unturned;
using System.Collections.Generic;
using System.Linq;
using Harmony;
using SDG.Provider;
using Steamworks;

namespace HInj
{
    public static class Skins
    {
        //Gets set when our client sends the equipped skin list to the server for verification
        public static List<ulong> SkinsUsing = new List<ulong>();
        public static int[] Clothes = new int[7];

        //Forces skins on our side
        public static void SkinUpd()
        {
            //Todo: Make work
            if (Hooks.askScreenshot.NeedingSpy)
            {
                Player.player.clothing.tellVisualToggle(Provider.server, 0, false);
                Player.player.clothing.tellVisualToggle(Provider.server, 1, false);
                Player.player.clothing.tellVisualToggle(Provider.server, 2, false);
            }

            //Curerntly using a rare vehicle skin to detect if we set skins yet or not
            if (Player.player.channel.owner.vehicleSkins.Where(e => e.Value == 83701).Select(e => (KeyValuePair<ushort, int>?)e).FirstOrDefault() == null)
            {
                Player.player.channel.owner.vehicleSkins.Clear();
                Player.player.channel.owner.vehicleSkins.Add(53, 83701);

                foreach (Sk x in SkinList)
                    foreach (ulong p in SkinsUsing)
                        if (x.instanceId == p)
                            if (!Player.player.channel.owner.itemSkins.ContainsKey(x.itemId))
                                Player.player.channel.owner.itemSkins.Add(x.itemId, x.itemDef);
            }

            HumanClothes third = Player.player.clothing.thirdClothes,
                character = Player.player.clothing.characterClothes;
            if (third.visualShirt != Clothes[0] || third.visualHat != Clothes[0])
            {
                third.visualShirt = Clothes[0];
                character.visualShirt = Clothes[0];

                third.visualPants = Clothes[1];
                character.visualPants = Clothes[1];

                third.visualHat = Clothes[2];
                character.visualHat = Clothes[2];

                third.visualBackpack = Clothes[3];
                character.visualBackpack = Clothes[3];

                third.visualVest = Clothes[4];
                character.visualVest = Clothes[4];

                third.visualMask = Clothes[5];
                character.visualMask = Clothes[5];

                third.visualGlasses = Clothes[6];
                character.visualGlasses = Clothes[6];
            }
        }

        //Skin List
        public struct Sk
        {
            public int itemDef;
            public ulong instanceId;
            public ushort itemId;
            public Sk(int itemd, ulong instid, ushort itmid)
            {
                itemDef = itemd;
                instanceId = instid;
                itemId = itmid;
            }
        }
        public static List<Sk> SkinList = new List<Sk>
        {
            new Sk(78701, 1893080729533248718, 1037), //Mythical Burning Lavaflow Heartbreaker
            new Sk(60908, 2990878892708372630, 346), //Mythical Cosmic Tiger Crossbow
            new Sk(61405, 1903258221454729019, 116), //Mythical Glitched Quasar PDW
            new Sk(68404, 2984120955282559885, 116), //Mythical Shiny Chill PDW
            new Sk(62007, 2990878892708367426, 99), //Mythical Bubbling Nightfall Cobra
            new Sk(72213, 2022587755883467346, 1382), //Mythical Melting Voidstream Ekho
            new Sk(68609, 1906626408188285936, 363), //Mythical Electric Nuclear Maplestrike
            new Sk(502114, 1881816022600288704, 1030), //Mythical Confetti Half Fry Frying Pan
            new Sk(71804, 1913382983947536340, 1364), //Mythical Shiny Beastmaw Hell's Fury
            new Sk(73015, 1887496522188223486, 157), //Mythical Radioactive Warhead Rocket Launcher
            new Sk(61811, 1881865484540765015, 297), //Mythical Energized Darkheart Grizzly
            new Sk(64101, 1863853626560531935, 490), //Mythical Holiday Spirit Ornamental Chainsaw
            new Sk(67202, 1648736431487432645, 13), //Mythical Lovely Rainbow Canned Beans
            new Sk(57008, 1898755887675039906, 570), //Mythical Divine Earbuds
            new Sk(78508, 1898753983164308012, 768), //Mythical Cosmic Blue Hawaii Tee
            new Sk(67512, 3714832534435409258, 679), //Mythical Meta Rocket Boots
            new Sk(82905, 1894207263539176535, 1497), //Mythical Glitched Ducky Life Preserver
            new Sk(77613, 1872860825834465619, 758), //Mythical Pyrotechnic Shuttershades
            new Sk(86115, 1892000488232444945, 908), //Mythical Blossoming Dual Katanas
            new Sk(56103, 2036096021502190279, 561) //Mythical Lovely Magic Hat
        };

        //Literal function for putting items in our inventory when steam transmits them
        [HarmonyPatch(typeof(TempSteamworksEconomy), "onInventoryResultReady")]
        public class onInvResultReady
        {
            public static bool Prefix(ref SteamInventoryResultReady_t callback,
                ref SteamInventoryResult_t ___inventoryResult,
                ref Dictionary<ulong, DynamicEconDetails> ___dynamicInventoryDetails,
                TempSteamworksEconomy __instance)
            {
                bool inst = false;
                if ((___inventoryResult != SteamInventoryResult_t.Invalid) && (callback.m_handle == ___inventoryResult))
                {
                    ___dynamicInventoryDetails.Clear();
                    uint num12 = 0;
                    if (SteamInventory.GetResultItems(___inventoryResult, null, ref num12) && (num12 > 0))
                    {
                        SteamItemDetails_t[] _tArray3 = new SteamItemDetails_t[num12];
                        SteamInventory.GetResultItems(___inventoryResult, _tArray3, ref num12);
                        for (uint i = 0; i < num12; i++)
                        {
                            string str7;

                            string str8;
                            uint num14 = 0x400;
                            SteamInventory.GetResultItemProperty(___inventoryResult, i, "tags", out str7, ref num14);
                            uint num15 = 0x400;
                            SteamInventory.GetResultItemProperty(___inventoryResult, i, "dynamic_props", out str8, ref num15);
                            DynamicEconDetails details2 = new DynamicEconDetails
                            {
                                tags = !string.IsNullOrEmpty(str7) ? str7 : string.Empty,
                                dynamic_props = !string.IsNullOrEmpty(str8) ? str8 : string.Empty
                            };
                            ___dynamicInventoryDetails.Add(_tArray3[i].m_itemId.m_SteamItemInstanceID, details2);
                        }

                        __instance.inventoryDetails = new List<SteamItemDetails_t>(_tArray3);

                        inst = true;
                    }

                    if (!inst)
                        __instance.inventoryDetails = new List<SteamItemDetails_t>();
                    
                    //Add custom skins
                    foreach (Sk t in SkinList)
                        __instance.inventoryDetails.Add(new SteamItemDetails_t()
                        {
                            m_iDefinition = new SteamItemDef_t(t.itemDef),
                            m_itemId = new SteamItemInstanceID_t(t.instanceId),
                            m_unFlags = 0,
                            m_unQuantity = 1
                        });

                    __instance.consolidateStacks();
                    __instance.onInventoryRefreshed?.Invoke();
                    __instance.isInventoryAvailable = true;
                    Provider.isLoadingInventory = false;
                    SteamInventory.DestroyResult(___inventoryResult);
                    ___inventoryResult = SteamInventoryResult_t.Invalid;

                    return false;
                }

                return true;
            }
        }
    }
}
