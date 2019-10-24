using SDG.Unturned;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace HInj.Hooks
{
    public class Ballistics
    {
        public static bool Prefix(UseableGun __instance, ref List<BulletInfo> ___bullets, ref int ___hitmarkerIndex, ref ParticleSystem ___tracerEmitter,
            ref Attachments ___thirdAttachments)
        {
            if (Global.AllOff || Hooks.askScreenshot.NeedingSpy)
            {
                ___tracerEmitter.startColor = Color.gray;
                return true;
            }
            if (!Global.AimEnabled || (Global.AimSettings.RageBot && RageBot.FocusTarget != null && !Global.AimSettings.RBypassOne))
                return true;

            if ((__instance.equippedGunAsset.projectile == null) && (___bullets != null))
            {
                if (__instance.channel.isOwner)
                {
                    RaycastInfo info2 = null;
                    float dst = 0f;

                    if (Global.AimSettings.RageBot)
                        if (RageBot.FocusTarget != null)
                        {
                            Vector3 AimAt = Aimbot.GetHitbox(RageBot.FocusTarget.player.transform, "Skull");

                            if (AimAt == Vector3.zero || RageBot.FocusTarget.player.stance.prone)
                                AimAt = RageBot.FocusTarget.player.transform.position + new Vector3(0, 0.005f, 0);

                            Ray prx = new Ray(Player.player.look.aim.position, (AimAt - Player.player.look.aim.position).normalized);
                            dst = Vector3.Distance(AimAt, Player.player.look.aim.position);
                            info2 = DamageTool.raycast(prx,
                                dst,
                                RayMasks.ENEMY,
                                Player.player);
                        }


                    for (int i = 0; i < ___bullets.Count; i++)
                    {
                        BulletInfo info = ___bullets[i];
                        if (Global.AimSettings.RageBot && RageBot.FocusTarget != null && info2 != null && info2.player == RageBot.FocusTarget.player)
                        {
                            if (Global.AimSettings.RBypassOne)
                            {
                                if (!((info.steps * __instance.equippedGunAsset.ballisticTravel) < dst))
                                {
                                    Player.player.input.sendRaycast(info2);
                                    Player.player.input.sendRaycast(info2);
                                    info.steps = 254;
                                    goto prx;
                                }
                            }
                        }

                        byte pellets = info.magazineAsset.pellets;
                        if (__instance.channel.isOwner)
                        {
                            EPlayerHit nONE = EPlayerHit.NONE;
                            if (pellets > 1)
                                ___hitmarkerIndex = info.pellet;
                            else if (OptionsSettings.hitmarker)
                            {
                                ___hitmarkerIndex++;
                                if (___hitmarkerIndex >= PlayerLifeUI.hitmarkers.Length)
                                    ___hitmarkerIndex = 0;
                            }
                            else
                                ___hitmarkerIndex = 0;

                            Ray ray = new Ray(info.pos, info.dir);
                            if (info2 == null)
                            {
                                info2 = DamageTool.raycast(ray,
                                    !Provider.modeConfigData.Gameplay.Ballistics ? __instance.equippedGunAsset.range : __instance.equippedGunAsset.ballisticTravel,
                                    RayMasks.DAMAGE_CLIENT,
                                    __instance.player);
                            }

                            if (((info2.player != null) && (__instance.equippedGunAsset.playerDamageMultiplier.damage > 1f)) && (!__instance.player.quests.isMemberOfSameGroupAs(info2.player) && Provider.isPvP))
                            {
                                if (nONE != EPlayerHit.CRITICAL)
                                    nONE = (info2.limb != ELimb.SKULL) ? EPlayerHit.ENTITIY : EPlayerHit.CRITICAL;
                                PlayerUI.hitmark(___hitmarkerIndex, info2.point, pellets > 1, (info2.limb != ELimb.SKULL) ? EPlayerHit.ENTITIY : EPlayerHit.CRITICAL);
                            }
                            else if (((info2.zombie != null) && (__instance.equippedGunAsset.zombieDamageMultiplier.damage > 1f)) || ((info2.animal != null) && (__instance.equippedGunAsset.animalDamageMultiplier.damage > 1f)))
                            {
                                if (nONE != EPlayerHit.CRITICAL)
                                    nONE = (info2.limb != ELimb.SKULL) ? EPlayerHit.ENTITIY : EPlayerHit.CRITICAL;
                                PlayerUI.hitmark(___hitmarkerIndex, info2.point, pellets > 1, (info2.limb != ELimb.SKULL) ? EPlayerHit.ENTITIY : EPlayerHit.CRITICAL);
                            }
                            else if (((info2.transform != null) && info2.transform.CompareTag("Barricade")) && (__instance.equippedGunAsset.barricadeDamage > 1f))
                            {
                                ushort num3;
                                if (ushort.TryParse(info2.transform.name, NumberStyles.Any, CultureInfo.InvariantCulture, out num3))
                                {
                                    ItemBarricadeAsset asset = (ItemBarricadeAsset)Assets.find(EAssetType.ITEM, num3);
                                    if ((asset != null) && (asset.isVulnerable || ((ItemWeaponAsset)__instance.player.equipment.asset).isInvulnerable))
                                    {
                                        if (nONE == EPlayerHit.NONE)
                                            nONE = EPlayerHit.BUILD;
                                        PlayerUI.hitmark(___hitmarkerIndex, info2.point, pellets > 1, EPlayerHit.BUILD);
                                    }
                                }
                            }
                            else if (((info2.transform != null) && info2.transform.CompareTag("Structure")) && (__instance.equippedGunAsset.structureDamage > 1f))
                            {
                                ushort num4;
                                if (ushort.TryParse(info2.transform.name, NumberStyles.Any, CultureInfo.InvariantCulture, out num4))
                                {
                                    ItemStructureAsset asset2 = (ItemStructureAsset)Assets.find(EAssetType.ITEM, num4);
                                    if ((asset2 != null) && (asset2.isVulnerable || ((ItemWeaponAsset)__instance.player.equipment.asset).isInvulnerable))
                                    {
                                        if (nONE == EPlayerHit.NONE)
                                            nONE = EPlayerHit.BUILD;
                                        PlayerUI.hitmark(___hitmarkerIndex, info2.point, pellets > 1, EPlayerHit.BUILD);
                                    }
                                }
                            }
                            else if (((info2.vehicle != null) && !info2.vehicle.isDead) && (__instance.equippedGunAsset.vehicleDamage > 1f))
                            {
                                if (((info2.vehicle.asset != null) && info2.vehicle.canBeDamaged) && (info2.vehicle.asset.isVulnerable || ((ItemWeaponAsset)__instance.player.equipment.asset).isInvulnerable))
                                {
                                    if (nONE == EPlayerHit.NONE)
                                        nONE = EPlayerHit.BUILD;
                                    PlayerUI.hitmark(___hitmarkerIndex, info2.point, pellets > 1, EPlayerHit.BUILD);
                                }
                            }
                            else if (((info2.transform != null) && info2.transform.CompareTag("Resource")) && (__instance.equippedGunAsset.resourceDamage > 1f))
                            {
                                byte num5;
                                byte num6;
                                ushort num7;
                                if (ResourceManager.tryGetRegion(info2.transform, out num5, out num6, out num7))
                                {
                                    ResourceSpawnpoint spawnpoint = ResourceManager.getResourceSpawnpoint(num5, num6, num7);
                                    if (((spawnpoint != null) && !spawnpoint.isDead) && (spawnpoint.asset.bladeID == ((ItemWeaponAsset)__instance.player.equipment.asset).bladeID))
                                    {
                                        if (nONE == EPlayerHit.NONE)
                                            nONE = EPlayerHit.BUILD;
                                        PlayerUI.hitmark(___hitmarkerIndex, info2.point, pellets > 1, EPlayerHit.BUILD);
                                    }
                                }
                            }
                            else if ((info2.transform != null) && (__instance.equippedGunAsset.objectDamage > 1f))
                            {
                                InteractableObjectRubble componentInParent = info2.transform.GetComponentInParent<InteractableObjectRubble>();
                                if (componentInParent != null)
                                {
                                    info2.transform = componentInParent.transform;
                                    info2.section = componentInParent.getSection(info2.collider.transform);
                                    if (!componentInParent.isSectionDead(info2.section) && (componentInParent.asset.rubbleIsVulnerable || ((ItemWeaponAsset)__instance.player.equipment.asset).isInvulnerable))
                                    {
                                        if (nONE == EPlayerHit.NONE)
                                            nONE = EPlayerHit.BUILD;
                                        PlayerUI.hitmark(___hitmarkerIndex, info2.point, pellets > 1, EPlayerHit.BUILD);
                                    }
                                }
                            }



                            if (Provider.modeConfigData.Gameplay.Ballistics)
                            {
                                if ((info.steps > 0) || (__instance.equippedGunAsset.ballisticSteps <= 1))
                                    if (__instance.equippedGunAsset.ballisticTravel < 32f)
                                        trace(info.pos + ((Vector3)(info.dir * 32f)), info.dir, ref ___tracerEmitter, ref __instance, ref ___thirdAttachments);
                                    else
                                        trace(info.pos + ((Vector3)(info.dir * UnityEngine.Random.Range(32f, __instance.equippedGunAsset.ballisticTravel))), info.dir, ref ___tracerEmitter, ref __instance, ref ___thirdAttachments);
                            }
                            else if (__instance.equippedGunAsset.range < 32f)
                                trace(ray.origin + ((Vector3)(ray.direction * 32f)), ray.direction, ref ___tracerEmitter, ref __instance, ref ___thirdAttachments);
                            else
                                trace(ray.origin + ((Vector3)(ray.direction * UnityEngine.Random.Range(32f, Mathf.Min(64f, __instance.equippedGunAsset.range)))), ray.direction, ref ___tracerEmitter, ref __instance, ref ___thirdAttachments);

                            float ballisticDrop = __instance.equippedGunAsset.ballisticDrop;
                            if (info.barrelAsset != null)
                                ballisticDrop *= info.barrelAsset.ballisticDrop;

                            info.pos += (Vector3)(info.dir * __instance.equippedGunAsset.ballisticTravel);
                            if (!Global.AimSettings.RageBot)
                                if (!Global.AimSettings.NoDrop)
                                    info.dir.y -= ballisticDrop;

                            info.dir.Normalize();

                            if (Global.AimSettings.RageBot && RageBot.FocusTarget != null)
                                goto prx;

                            if (!__instance.player.input.isRaycastInvalid(info2))
                            {
                                if (nONE != EPlayerHit.NONE)
                                {
                                    int num9;
                                    if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Accuracy_Hit", out num9))
                                        Provider.provider.statisticsService.userStatisticsService.setStatistic("Accuracy_Hit", (int)(num9 + 1));
                                    if ((nONE == EPlayerHit.CRITICAL) && Provider.provider.statisticsService.userStatisticsService.getStatistic("Headshots", out num9))
                                        Provider.provider.statisticsService.userStatisticsService.setStatistic("Headshots", (int)(num9 + 1));
                                }
                                __instance.player.input.sendRaycast(info2);
                                info.steps = 0xfe;
                            }
                        }
                    }
                }

                prx:
                if (__instance.player.equipment.asset != null)
                    if (Provider.modeConfigData.Gameplay.Ballistics)
                    {
                        for (int k = ___bullets.Count - 1; k >= 0; k--)
                        {
                            BulletInfo info5 = ___bullets[k];
                            info5.steps = (byte)(info5.steps + 1);
                            if (info5.steps >= __instance.equippedGunAsset.ballisticSteps)
                                ___bullets.RemoveAt(k);
                        }
                    }
                    else
                        ___bullets.Clear();
            }
            return false;
        }

        static void trace(Vector3 pos, Vector3 dir, ref ParticleSystem tracerEmitter, ref UseableGun instancem, ref Attachments thirdAttach)
        {
            if ((tracerEmitter != null) && (((thirdAttach.barrelModel == null) || !thirdAttach.barrelAsset.isBraked) || (instancem.player.equipment.state[0x10] <= 0)))
            {
                tracerEmitter.transform.position = pos;
                tracerEmitter.transform.rotation = Quaternion.LookRotation(dir);
                tracerEmitter.Emit(1);
                tracerEmitter.startColor = Color.magenta;
            }
        }
    }
}
