using HarmonyLib;
using TownOfUs.Roles;
using UnityEngine;

namespace TownOfUs.CrewmateRoles.MedicMod
{

    [HarmonyPatch(typeof(Object), nameof(Object.Destroy), typeof(Object))]
    public static class HUDClose
    {
        public static void Postfix(Object obj)
        {
            if (ExileController.Instance == null || obj != ExileController.Instance.gameObject) return;
            foreach (var role in Role.GetRoles(RoleEnum.Medic))
            {
                var medic = (Medic)role;
                if (CustomGameOptions.RoleProgressionOn && medic.GetTier4)
                {
                    if (medic.UsedAbility)
                    {
                        if (medic.ShieldedPlayer == null || medic.ShieldedPlayer.Data.IsDead)
                        {
                            medic.UsedAbility = false;
                        }
                    }
                }
            }
        }
    }
}