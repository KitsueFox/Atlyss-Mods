using System;
using MelonLoader;
using HarmonyLib;

namespace BlackyFox.FoxDenServerPlugin
{
    public abstract class FoxDenServerPluginPatch
    {
        [HarmonyPatch(typeof(PartyObjectBehavior), "SerializeSyncVars")]
        public class Patch_PartyObjectBehavior
        {
            static bool Prefix(PartyObjectBehavior __instance)
            {
                __instance.Network_maxPartyLimit = 8;
                return true;
            }
        }
        [HarmonyPatch(typeof(GameManager), "Start")]
        public static class gamemanager
        {
            private static void Postfix(ref GameManager __instance)
            {
                try
                {
                    __instance._statLogics._maxMainLevel = 50;
                }
                catch (Exception e)
                {
                    MelonLogger.Error(e);
                }
            }
        }
    }
}