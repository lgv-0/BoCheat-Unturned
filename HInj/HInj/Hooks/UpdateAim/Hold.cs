using Harmony;
using SDG.Unturned;
using UnityEngine;

namespace HInj.Hooks
{
    [HarmonyPatch(typeof(PlayerLook), "updateAim", new System.Type[] { typeof(float) })]
    public class UpdateAim
    {
        public static bool Prefix(float delta)
        {
            if (!Global.MiscSettings.NoRecoilSway || Global.AllOff)
                return true;

            //Force aim specifically to have no sway
            if (Player.player.movement.getVehicle() != null && Player.player.movement.getVehicle().passengers[Player.player.movement.getSeat()].turret != null && Player.player.movement.getVehicle().passengers[Player.player.movement.getSeat()].turret.useAimCamera)
            {
                Passenger passenger = Player.player.movement.getVehicle().passengers[Player.player.movement.getSeat()];
                if (passenger.turretAim != null)
                {
                    Player.player.look.aim.position = passenger.turretAim.position;
                    Player.player.look.aim.rotation = passenger.turretAim.rotation;
                }
            }
            else
            {
                Player.player.look.aim.localPosition = Vector3.Lerp(Player.player.look.aim.localPosition, Vector3.up * Player.player.look.heightLook, 4f * delta);
                if (Player.player.stance.stance == EPlayerStance.SITTING || Player.player.stance.stance == EPlayerStance.DRIVING)
                {
                    Player.player.look.aim.parent.localRotation = Quaternion.Euler(0f, Player.player.look.yaw, 0f);
                }
                else
                {
                    Player.player.look.aim.parent.localRotation = Quaternion.Lerp(Player.player.look.aim.parent.localRotation, Quaternion.Euler(0f, 0f, (float)Player.player.animator.lean * HumanAnimator.LEAN), 4f * delta);
                }

                if (Global.MiscSettings.NoRecoilSway)
                    Player.player.look.aim.localRotation = Quaternion.Euler(Player.player.look.pitch - 90f, 0f, 0f);
                else
                    Player.player.look.aim.localRotation = Quaternion.Euler(Player.player.look.pitch - 90f + Player.player.animator.viewSway.x, Player.player.animator.viewSway.y, 0f);
            }

            //Force our view to have no sway
            if (Global.MiscSettings.NoRecoilSway)
                Player.player.animator.viewSway = new Vector3(0f, 0f, 0f);

            return false;
        }
    }
}
