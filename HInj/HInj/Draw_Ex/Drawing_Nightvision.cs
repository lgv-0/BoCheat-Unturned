using SDG.Unturned;

namespace HInj
{
    public partial class Drawing
    {
        public static void UpdateNightvision()
        {
            if ((Global.VisSettings.NightVision && Global.VisualsEnabled && !Global.AllOff) && !Hooks.askScreenshot.NeedingSpy)
            {
                if (LevelLighting.vision != ELightingVision.MILITARY)
                {
                    LevelLighting.vision = ELightingVision.MILITARY;
                    LevelLighting.updateLighting();
                    LevelLighting.updateLocal();
                    PlayerLifeUI.updateGrayscale();
                    Global.LastUsedNight = true;
                }
            }
            else
            {
                if (Global.LastUsedNight)
                {
                    LevelLighting.vision = ELightingVision.NONE;
                    LevelLighting.updateLighting();
                    LevelLighting.updateLocal();
                    PlayerLifeUI.updateGrayscale();
                    Global.LastUsedNight = false;
                }
            }
        }
    }
}
