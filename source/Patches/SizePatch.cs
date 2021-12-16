using HarmonyLib;
using TownOfUs.Extensions;
using TownOfUs.Roles;
using TownOfUs.Roles.Modifiers;
using UnityEngine;

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
                Vector3 size = new Vector3(0,0,0);
                var modifier = Modifier.GetModifier(player);
                if(modifier != null)
                {
                    size = modifier.SizeFactor;
                }
                var role = Role.GetRole(player);
                if(role != null && role.OverrideSize)
                {
                    size = role.SizeFactor;
                }
                if (size.x == 0 && Modifier.DefaultSize.x != 0)
                    player.transform.localScale = Modifier.DefaultSize;
                else if (size.x != 0)
                    player.transform.localScale = size;
            }
        }
        
    }

}