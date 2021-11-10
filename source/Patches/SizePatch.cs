using HarmonyLib;
using TownOfUs.Extensions;
using TownOfUs.Roles.Modifiers;

namespace TownOfUs.Patches
{
    [HarmonyPatch]
    public static class SizePatch
    {
        
        [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
        [HarmonyPostfix]
        public static void Postfix(HudManager __instance)
        {
            foreach (var player in PlayerControl.AllPlayerControls.ToArray())
            {
                Modifier modifier = Modifier.GetModifier(player);
                if(modifier != null)
                {
                    player.transform.localScale = modifier.SizeFactor;
                }
            }
        }
        
    }

}