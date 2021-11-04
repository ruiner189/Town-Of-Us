using System.Linq;
using HarmonyLib;
using Reactor;
using TownOfUs.Roles;
using UnityEngine;

namespace TownOfUs.Patches
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.CompleteTask))]
    public class CompleteTask
    {
        public static void Postfix(PlayerControl __instance)
        {
            var role = Role.GetRole(__instance);
            if (role.Faction != Faction.Crewmates && role.RoleType != RoleEnum.Phantom) return;

            var taskinfos = __instance.Data.Tasks.ToArray();
            var tasksLeft = taskinfos.Count(x => !x.Complete);
            var totalTasks = taskinfos.Count();

            role.TasksLeft = tasksLeft;

            if (!CustomGameOptions.RoleProgressionOn) return;
            if (__instance.Data.IsDead) return;

            int tier1 = (int) (totalTasks * 0.75);
            int tier2 = (int) (totalTasks * 0.5);
            int tier3 = (int) (totalTasks * 0.25);
            int tier4 = 0;

            if (tasksLeft == tier1) {
                role.SetTier1(true);
            } else if (tasksLeft == tier2) {
                role.SetTier2(true);
            } else if (tasksLeft == tier3) {
                role.SetTier3(true);
            } else if (tasksLeft == tier4) {
                role.SetTier4(true);
            }
        }
    }
}
