using Harmony;
using SDG.Framework.Translations;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Text;

namespace HInj.Hooks
{
    [HarmonyPatch(typeof(Provider), "onLevelLoaded", new Type[] { typeof(int) })]
    public class lvlLoaded
    {
        public static bool Prefix(int level,
            ref List<SDG.Framework.Modules.Module> ___critMods,
            ref StringBuilder ___modBuilder,
            ref byte[] ____serverPasswordHash)
        {
            if (level == 2)
            {
                Provider.isLoadingUGC = false;
                if (Provider.isConnected)
                {
                    int num12;
                    byte num13 = 1;
                    ___critMods.Clear();
                    ___modBuilder.Length = 0;
                    SDG.Framework.Modules.ModuleHook.getRequiredModules(___critMods);
                    for (int m = 0; m < ___critMods.Count; m++)
                    {
                        ___modBuilder.Append(___critMods[m].config.Name);
                        ___modBuilder.Append(",");
                        ___modBuilder.Append(___critMods[m].config.Version_Internal);
                        if (m < (___critMods.Count - 1))
                        {
                            ___modBuilder.Append(";");
                        }
                    }

                    object[] objects = new object[] {
                            (byte) 2, Characters.selected,
                        Provider.clientName,
                        Characters.active.name,
                        ____serverPasswordHash, Level.hash, ReadWrite.appOut(), num13,
                        Provider.APP_VERSION_PACKED,
                        Provider.isPro,
                        ((float) Provider.currentServerInfo.ping) / 1000f,
                        Characters.active.nick,
                        Characters.active.group,
                        Characters.active.face,
                        Characters.active.hair,
                        Characters.active.beard,
                        Characters.active.skin,
                        Characters.active.color,
                        Characters.active.markerColor,
                        Characters.active.hand,
                        Characters.active.packageShirt,
                        Characters.active.packagePants,
                        Characters.active.packageHat,
                        Characters.active.packageBackpack,
                        Characters.active.packageVest,
                        Characters.active.packageMask,
                        Characters.active.packageGlasses,
                        Characters.packageSkins.ToArray(),
                        (byte) Characters.active.skillset, ___modBuilder.ToString(), Translator.language, Lobbies.currentLobby, Level.packedVersion,
                        LocalHwid.getHwid()
                         };

                    //Allows us to auto-detect inventory selected items without additional work.
                    Skins.SkinsUsing = Characters.packageSkins;
                    ulong[] Converting = new ulong[7] {
                        Characters.active.packageShirt,
                        Characters.active.packagePants,
                        Characters.active.packageHat,
                        Characters.active.packageBackpack,
                        Characters.active.packageVest,
                        Characters.active.packageMask,
                        Characters.active.packageGlasses
                    };
                    foreach (Skins.Sk t in Skins.SkinList)
                        for (int p = 0; p < 7; p++)
                            if (Converting[p] == t.instanceId)
                                Skins.Clothes[p] = t.itemDef;

                    Console.WriteLine("P:" + Provider.APP_VERSION_PACKED + " | L:" + Level.packedVersion);

                    byte[] packet = SteamPacker.getBytes(0, out num12, objects);
                    Provider.send(Provider.server, ESteamPacket.CONNECT, packet, num12, 0);
                    return false;
                }
            }
            return true;
        }
    }
}
