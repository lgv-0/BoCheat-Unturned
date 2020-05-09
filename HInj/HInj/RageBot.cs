using SDG.Unturned;
using System.Collections.Generic;
using UnityEngine;

namespace HInj
{
    //Used as the target-finder for the rage mode.
    public class RageBot : MonoBehaviour
    {
        public static List<SteamPlayer> mTargets = new List<SteamPlayer>();
        public static SteamPlayer FocusTarget = null;
        
        //Called every tick
        public void Update()
        {
            if (Global.AllOff || !Global.AimSettings.RageBot || !Global.AimEnabled)
                return;

            if (!Provider.isConnected)
                return;

            if (Provider.clients.Count < 1)
                return;

            //Is holding gun
            if (!(Player.player?.equipment?.asset is ItemGunAsset))
                return;

            SteamPlayer targ = null;
            Vector3 position = Camera.main.transform.position, forward = Camera.main.transform.forward;
            float maxDist = ((ItemGunAsset)Player.player.equipment.asset).range, bestFov = 300, trgDst = 0f;
            foreach (SteamPlayer i in Global.AimSettings.RageAll ? Provider.clients : mTargets)
            {
                if (i.player == Player.player)
                    continue;
                if (i.player.life.isDead)
                    continue;
                if (!Friend.isTarget(i))
                    continue;

                float distance = Vector3.Distance(i.player.transform.position, position);

                if (!(distance < maxDist))
                    continue;

                Vector3 wtsp = Drawing.Drawing_Hk.WorldToScreen(i.player.transform.position);

                if (wtsp.z > -8)
                {
                    float fov = Mathf.Abs(Vector2.Distance(new Vector2(wtsp.x, Screen.height - wtsp.y), new Vector2((Screen.width / 2), (Screen.height / 2))));

                    if (fov < bestFov)
                    {
                        bestFov = fov;
                        targ = i;
                        trgDst = distance;
                    }
                }
            }

            if (targ == null)
            {
                if (FocusTarget != null)
                {
                    FocusTarget = null;
                    Toolkit.SendMessageTip("Lost Lock");
                }
                return;
            }

            FocusTarget = targ;
            Toolkit.SendMessageTip("Aimbot Lock: " + targ.playerID.characterName);

            if (!Global.AimSettings.RBypassOne)
            {
                Vector3 PosTo = Vector3.Lerp(Camera.main.transform.position, FocusTarget.player.look.aim.position, 0.99f);
                Player.player.look.aim.position = PosTo;
                Ray ray = new Ray(Player.player.look.aim.position, Player.player.look.aim.forward);
                RaycastInfo raycastInfo = DamageTool.raycast(ray, 2f, RayMasks.DAMAGE_CLIENT);
                if ((UnityEngine.Object)raycastInfo.player != (UnityEngine.Object)null)
                    Player.player.input.sendRaycast(raycastInfo);
            }
        }
    }
}
