using SDG.Unturned;
using System;
using System.Reflection;
using UnityEngine;

namespace HInj
{
    public struct pVel
    {
        public Vector3 point;
        public SteamPlayer User;
    }

    public class Aimbot : MonoBehaviour
    {
        public static FieldInfo Pitch, Yaw;
        public static pVel pPredict = new pVel();

        //Startup
        public void Awake()
        {
            Pitch = typeof(PlayerLook).GetField("_pitch", BindingFlags.Instance | BindingFlags.NonPublic);
            Yaw = typeof(PlayerLook).GetField("_yaw", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        //Find players hitbox by specified name
        public static Vector3 GetHitbox(Transform target, string hitbox)
        {
            foreach (Transform cur in target.transform.GetComponentsInChildren<Transform>())
                if (cur.name.Contains(hitbox))
                    return cur.position + new Vector3(0f, 0.4f, 0f);

            return Vector3.zero;
        }

        //Gets the distance between each plane, rather than the total distance
        public static Vector3 VectorDistance(Vector3 v1, Vector3 v2)
        {
            return new Vector3((v1.x - v2.x), (v1.y - v2.y), (v1.z - v2.z));
        }

        //Calculate bullet drop to a point
        //BUG: Drop will be more innacurate the higher the slope to target
        //Rotation in the math maybe??
        public float DropCalc(Vector3 point)
        {
            float treturn = 0f;

            if (Vector3.Distance(point, Player.player.look.aim.position) < 5f)
                return 0f;

            ItemGunAsset firearm = (ItemGunAsset)Player.player.equipment.asset;

            Quaternion quaternion = Quaternion.LookRotation(point - Player.player.look.transform.position, Player.player.look.transform.up);
            Vector3 targetForward = quaternion * Vector3.forward;

            BulletInfo bulletInfo = new BulletInfo();
            bulletInfo.pos = Player.player.look.transform.position;
            bulletInfo.dir = targetForward.normalized;

            float num = firearm.ballisticDrop;
            bulletInfo.barrelAsset = Player.player.equipment.thirdModel.gameObject.GetComponent<Attachments>().barrelAsset;
            if (bulletInfo.barrelAsset != null)
                num *= bulletInfo.barrelAsset.ballisticDrop;

            int ticker = 0;
            while (++ticker < firearm.ballisticSteps)
            {
                bulletInfo.pos += bulletInfo.dir * firearm.ballisticTravel;
                bulletInfo.dir.y -= num;
                bulletInfo.dir.Normalize();

                if (Vector3.Distance(
                    new Vector3(point.x, 0f, point.z),
                    new Vector3(bulletInfo.pos.x, 0f, bulletInfo.pos.z))
                    < firearm.ballisticTravel
                    )
                {
                    treturn = bulletInfo.pos.y - point.y;
                    break;
                }
            }

            if (treturn < 0)
                treturn -= treturn * 2;
            else
                treturn = 0f;

            return treturn;
        }

        //Called every tick
        public void Update()
        {
            if (!Global.AimEnabled || Global.AllOff || Global.AimSettings.RageBot || !Global.AimSettings.LegitBot)
                return;

            if (!Provider.isConnected)
                return;

            if (Provider.clients.Count < 1)
                return;

            //Is holding gun
            if (!(Player.player?.equipment?.asset is ItemGunAsset))
                return;

            if (!Input.GetKey(KeyCode.Mouse1))
                return;

            Camera m = Camera.main;

            SteamPlayer targ = null;
            float maxDist = ((ItemGunAsset)Player.player.equipment.asset).range, bestFov = 300, trgDst = 0f;
            Vector3 position = m.transform.position, forward = m.transform.forward, AimAt = Vector3.zero;

            foreach (SteamPlayer i in Provider.clients)
            {
                if (i.player == Player.player)
                    continue;
                if (i.player.quests.groupID == Player.player.quests.groupID)
                    continue;
                if (i.player.life.isDead)
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
                pPredict.User = null;
                return;
            }

            AimAt = GetHitbox(targ.player.transform, "Skull");

            //Not all players have a skull, or is majorly displaced when prone
            if (AimAt == Vector3.zero || targ.player.stance.prone)
                AimAt = targ.player.transform.position + new Vector3(0, targ.player.look.heightLook / 1.45f, 0);

            if (Global.AimSettings.VisibleCheck)
            {
                RaycastHit hit;
                Physics.Raycast(m.transform.position, AimAt - m.transform.position, out hit, trgDst + 0.35f, RayMasks.DAMAGE_CLIENT);

                if (hit.transform == null || !hit.transform.CompareTag("Enemy"))
                    return;
            }

            //Unity engine will use player sensitivity when we use it's own functions for math calculations
            float save = Player.player.look.sensitivity;
            Player.player.look.sensitivity = Global.AimSettings.AimSpeed;

            //Apply drop calculation if server is using ballistics
            if (Global.AimSettings.DropCalculator && Provider.modeConfigData.Gameplay.Ballistics)
                AimAt.y += DropCalc(AimAt);

            //Get rotation angles, ez bypass to doing math
            Quaternion quaternion = Quaternion.LookRotation(AimAt - m.transform.position);
            Quaternion quaternion2 = Quaternion.RotateTowards(m.transform.rotation, quaternion, Global.AimSettings.AimSpeed);

            //Reset sensitivty
            Player.player.look.sensitivity = save;

            float xVal = quaternion2.eulerAngles.x;
            if (xVal <= 90f)
                xVal += 90f;
            else if (xVal > 180f)
                xVal -= 270f;

            //Todo: Rotate using camera
            Yaw.SetValue(Player.player.look, quaternion2.eulerAngles.y);
            Pitch.SetValue(Player.player.look, xVal);
        }
    }
}
