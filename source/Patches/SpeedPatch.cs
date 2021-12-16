using HarmonyLib;
using Reactor;
using TownOfUs.Extensions;
using TownOfUs.Roles;
using TownOfUs.Roles.Modifiers;

namespace TownOfUs.Patches
{
    [HarmonyPatch]
    public static class SpeedPatch
    {
        
        [HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.FixedUpdate))]
        [HarmonyPostfix]
        public static void PostfixPhysics(PlayerPhysics __instance)
        {
            if (__instance.AmOwner && GameData.Instance && __instance.myPlayer.CanMove)
            {
                float speedMultiplier = 1;
                var modifier = Modifier.GetModifier(__instance.myPlayer);
                if (modifier != null)
                    speedMultiplier = modifier.SpeedFactor;
                var role = Role.GetRole(__instance.myPlayer);
                if (role != null && role.OverrideSpeed)
                    speedMultiplier = role.SpeedFactor;
                __instance.body.velocity *= speedMultiplier;

            }

        }

        [HarmonyPatch(typeof(CustomNetworkTransform), nameof(CustomNetworkTransform.FixedUpdate))]
        [HarmonyPostfix]
        public static void PostfixNetwork(CustomNetworkTransform __instance)
        {
            if (__instance.AmOwner && __instance.interpolateMovement != 0.0f)
            {

                var player = __instance.gameObject.GetComponent<PlayerControl>();
                Modifier modifier = Modifier.GetModifier(player);
                if (modifier != null)
                {
                    __instance.body.velocity *= modifier.SpeedFactor;
                }
            }
        }
         
    }

}