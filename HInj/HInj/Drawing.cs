using SDG.Unturned;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace HInj
{
    public partial class Drawing
    {
        public static AssetBundle f = AssetBundle.LoadFromFile("bocheat.main");
        public static Shader Wireframe = f.LoadAsset<Shader>("assets/wireframe.shader");
        //Col: _WireColor, _BaseColor
        //Int: _WireThickness(0-800), _WireSmoothness(0-200)
        public static Shader Seethrough_Full = f.LoadAsset<Shader>("assets/seethrough_full.shader");
        //Change material.color
        public static Shader Silhouette = f.LoadAsset<Shader>("assets/silhouette.shader");
        //Col: _Color, _OutlineColor
        //Fl: _Outline (0.0f->0.19f)
        public static Shader Chams = f.LoadAsset<Shader>("assets/chams.shader");
        //Col: _VisibleColor, _NonVisibleColor
        public static Shader defaultPlayerShader = null;

        public class Drawing_Hk : MonoBehaviour
        {
            static Camera Main;
            static GameObject lightGameObject = new GameObject("nightmodelight");
            static Light lightComp = lightGameObject.AddComponent<Light>();
            static Transform aur = null;

            //W2S that abuses the camera's w2s and for some reason still needs vector3???????
            public static Vector3 WorldToScreen(Vector3 Pos)
            {
                Vector3 Y = Main.WorldToScreenPoint(Pos);
                Y.y = Screen.height - Y.y;
                return Y;
            }

            //Box/Line color set
            private static Texture2D texture2D = null;
            public static void UpdateTextureColor(Color color)
            {
                texture2D.SetPixel(0, 0, color);
                texture2D.Apply();
            }

            public static void BoxRect(Rect rect)
            {
                GUI.DrawTexture(rect, texture2D);
            }

            public static void DrawBox(Vector2 pos, Vector2 size, float thick)
            {
                BoxRect(new Rect(pos.x, pos.y, size.x, thick));
                BoxRect(new Rect(pos.x, pos.y, thick, size.y));
                BoxRect(new Rect(pos.x + size.x, pos.y, thick, size.y));
                BoxRect(new Rect(pos.x, pos.y + size.y, size.x + thick, thick));
            }

            //Called when game is rendering/handling GUI
            public void OnGUI()
            {
                if (!Provider.isConnected || Global.AllOff)
                    return;

                if (Provider.clients.Count < 1)
                    return;

                if (Player.player == null)
                    return;

                /////////////BEGIN_LIGHTINGUPDATE////////////////

                UpdateNightvision();

                /////////////END_LIGHTINGUPDATE//////////////////

                if (Hooks.askScreenshot.NeedingSpy)
                {
                    Hooks.askScreenshot.ESPReady = true;
                    return;
                }

                //Force box color to yellow
                if (texture2D == null)
                {
                    texture2D = new Texture2D(1, 1);
                    UpdateTextureColor(Color.yellow);
                }

                if (!Global.VisualsEnabled)
                    return;

                if (!(Player.player?.transform != null))
                    return;

                Main = Camera.main;

                GUIStyle AStyle = GUI.skin.GetStyle("Label");
                int BackupSize = AStyle.fontSize;

                ///////////////BEGIN_DRAWUI/////////////////

                if (Global.VisSettings.Players)
                    DrawPlayers();

                if (Global.VisSettings.Items)
                    DrawItems();

                ///////////////END_DRAWUI//////////////////
                AStyle.fontSize = BackupSize;
                AStyle.alignment = TextAnchor.MiddleLeft;
            }

            //Startup
            public void Awake()
            {
                lightComp.color = Color.cyan;
                lightComp.type = LightType.Directional;
                lightComp.transform.localEulerAngles = new Vector3(90, 0, 0);
                DontDestroyOnLoad(lightGameObject);
            }

            public static void ResetColorfuls()
            {
                foreach (originalTextures p in Backup)
                    if (p.originalRender != null)
                    {
                        Renderer f = p.originalRender;
                        f.material.shader = p.originalShader;
                        f.material.color = p.originalCol;
                    }

                if (defaultPlayerShader != null)
                    foreach (SteamPlayer x in Provider.clients)
                        if (x.player != null && x.player.transform != null)
                            foreach (Renderer f in x.player.transform.GetComponentsInChildren<Renderer>())
                                if (f.material.shader == Chams)
                                    f.material.shader = defaultPlayerShader;

                Backup.Clear();
            }

            static int badmethoddontask = 0;
            public static void OccaionalCheck()
            {
                List<originalTextures> temp = new List<originalTextures>();
                foreach (originalTextures x in Backup)
                    if (x.originalRender == null)
                        temp.Add(x);
                foreach (originalTextures s in temp)
                    Backup.Remove(s);
            }

            //Called every tick
            public void Update()
            {
                if (!Provider.isConnected || Global.AllOff || Hooks.askScreenshot.NeedingSpy || !Global.VisualsEnabled)
                {
                    lightGameObject.SetActive(false);
                    ResetColorfuls();
                    if (aur != null)
                        aur.gameObject.SetActive(false);
                    if (Hooks.askScreenshot.NeedingSpy)
                    {
                        Hooks.askScreenshot.ChamReady = true;
                        Toolkit.HideChats("BoCheat");
                        if (Global.AimSettings.RageBot)
                        {
                            Toolkit.SendMessageTip("Missing Space", true);
                            Global.AimSettings.RageBot = false;
                            System.Console.WriteLine("Auto disabling ragebot for spy");
                        }
                    }
                    return;
                }

                if (badmethoddontask++ > 285)
                {
                    badmethoddontask = 0;
                    OccaionalCheck();
                    if (!Global.VisSettings.Chams)
                        ResetColorfuls();
                }

                if (Provider.clients.Count < 1)
                    return;

                if (Player.player == null)
                    return;

                if (Global.VisSettings.Chams)
                    if (VehicleManager.vehicles != null)
                        foreach (InteractableVehicle r in VehicleManager.vehicles)
                            foreach (Renderer x in r.gameObject.GetComponentsInChildren<Renderer>())
                            {
                                if (x.material.shader == Seethrough_Full)
                                    continue;

                                if (x.tag == "Small" || x.tag == "Enemy" || x.tag == "Item")
                                    continue;

                                if (x.name.Contains("Seat") || x.name.Contains("light") || x.name.Contains("Sir"))
                                    continue;

                                Backup.Add(new originalTextures(x, x.material.shader, x.material.color));
                                x.material.shader = Seethrough_Full;

                                if (!r.isLocked || r.lockedGroup == Player.player.quests.groupID || r.lockedOwner == Provider.client)
                                    x.material.color = Color.cyan;
                                else
                                    x.material.color = Color.gray;
                            }

                if (Global.VisSettings.NightMode)
                {
                    if (aur == null)
                        aur = (Transform)typeof(LevelLighting).GetField("auroraBorealisTransform", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
                    aur.gameObject.SetActive(true);
                    aur.GetComponent<MeshRenderer>().material.SetFloat("_Intensity", 85);

                    lightComp.intensity = Global.VisSettings.NightLight;
                    lightGameObject.SetActive(true);
                }
                else
                    lightGameObject.SetActive(false);
            }

            public struct originalTextures
            {
                //Was using gameobject but that created issues with one-way windows
                public Renderer originalRender;
                public Shader originalShader;
                public Color originalCol;
                public originalTextures(Renderer x, Shader y, Color z)
                {
                    originalRender = x;
                    originalShader = y;
                    originalCol = z;
                }
            }
            public static List<originalTextures> Backup = new List<originalTextures>();
        }
    }
}
