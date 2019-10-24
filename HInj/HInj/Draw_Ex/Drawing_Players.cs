using SDG.Unturned;
using UnityEngine;

namespace HInj
{
    public partial class Drawing
    {
        public static void DrawPlayers()
        {
            foreach (SteamPlayer i in Provider.clients)
            {
                if (i.player == Player.player)
                    continue;
                //Ensure player is not in our group
                if (i.player.quests.groupID == Player.player.quests.groupID)
                    continue;
                if (i.player.life.isDead)
                    continue;

                float dist = Vector3.Distance(i.player.transform.position, Player.player.transform.position);
                if (dist > Global.VisSettings.Distance)
                    continue;

                Vector3 Pos = Drawing_Hk.WorldToScreen(i.player.transform.position);
                if (Pos.x < 0 || Pos.y < 0 || Pos.x > Screen.width || Pos.y > Screen.height || Pos.z < 0)
                    continue;

                int Size = (int)(160f / dist);
                if (Size < 7)
                    Size = 7;
                if (Size > 13)
                    Size = 13;

                GUIStyle centeredStyle = GUI.skin.GetStyle("Label");
                centeredStyle.alignment = TextAnchor.UpperCenter;
                centeredStyle.fontSize = Size;
                centeredStyle.richText = true;
                GUI.Label(new Rect(Pos.x - 80, Pos.y - 4, 160, 2f * Size), "<color=yellow>" + i.player.name + "</color>", centeredStyle);

                if (i?.player?.equipment?.asset?.itemName != null)
                    GUI.Label(new Rect(Pos.x - 80, (Pos.y - 10) + (2f * Size), 160, 2f * Size), "<color=white>" + i.player.equipment.asset.itemName + "</color>", centeredStyle);

                if (Global.VisSettings.Chams)
                    foreach (Renderer t in i.player.transform.GetComponentsInChildren<Renderer>())
                        if (t.material.shader != Chams)
                            if (t.name.Contains("Model"))
                            {
                                if (defaultPlayerShader == null)
                                    defaultPlayerShader = t.material.shader;
                                t.material.shader = Chams;
                            }

                Vector3 PosPlusTop = Drawing_Hk.WorldToScreen(i.player.transform.position + new Vector3(0, i.player.look.heightLook, 0));
                if (PosPlusTop.z < 0)
                    continue;

                if (Global.VisSettings.Box)
                {
                    float Offset = Mathf.Abs(Pos.y - PosPlusTop.y) - 2f;
                    Drawing_Hk.DrawBox(
                        new Vector2(PosPlusTop.x - (Offset / 2), PosPlusTop.y),
                        new Vector2(Offset, Pos.y - PosPlusTop.y),
                        1f
                        );
                }
            }
        }
    }
}
