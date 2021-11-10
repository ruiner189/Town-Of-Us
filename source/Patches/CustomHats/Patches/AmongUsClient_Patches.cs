using HarmonyLib;
using UnityEngine;

namespace TownOfUs.Patches.CustomHats.Patches
{
    [HarmonyPatch(typeof(HatManager), nameof(HatManager.GetHatById))]
    public class AmongUsClient_Patches
    {
        private static bool _executed;
        public static void Prefix(HatManager __instance)
        {
            if (!_executed)
            {
                _executed = true;
                HatLoader.LoadHats(__instance);
            }
        }        
    }
}