using System.Linq;
using HarmonyLib;
using InnerNet;
using TownOfUs.Roles;

namespace TownOfUs.NeutralRoles.GlitchMod
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    internal class Update
    {
        private static void Prefix(HudManager __instance)
        {
            foreach (var role in Role.AllRoles.Where(r =>  r.RoleType == RoleEnum.Glitch))
            {
                Glitch glitch = (Glitch)role;
                if (AmongUsClient.Instance.GameState == InnerNetClient.GameStates.Started)
                    if (glitch != null)
                        if (PlayerControl.LocalPlayer.Is(RoleEnum.Glitch))
                            glitch.Update(__instance);
                glitch.UpdateHack();
            }
        }
    }
}