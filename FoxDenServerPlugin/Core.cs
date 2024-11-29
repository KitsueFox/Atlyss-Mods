using MelonLoader;
using System;

namespace BlackyFox.FoxDenServerPlugin
{
    public static class BuildInfo
    {
        public const string Name = "FoxDenServerPlugin";
        public const string Author = "BlackyFox";
        public const string Company = null;
        public const string Version = "0.1.0.0";
        public const string DownloadLink = null;
    }
    
    public class FoxDenServerPluginCore : MelonMod
    {
        public override void OnInitializeMelon()
        {
            var harmony = new HarmonyLib.Harmony("ModAwareMultiplayer");
            try
            {
                harmony.PatchAll();
                LoggerInstance.Msg($"FoxDenServerPlugin loaded successfully!");
            }
            catch (Exception e)
            {
                LoggerInstance.Msg(e.ToString());
            }
        }
    }
}