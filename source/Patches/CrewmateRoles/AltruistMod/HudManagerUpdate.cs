using HarmonyLib;
using Reactor;
using TownOfUs.Patches;
using TownOfUs.Roles;
using UnityEngine;

namespace TownOfUs.CrewmateRoles.AltruistMod
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class HudManagerUpdate
    {
        public static void Postfix(HudManager __instance)
        {
            if (PlayerControl.AllPlayerControls.Count <= 1) return;
            if (PlayerControl.LocalPlayer == null) return;
            if (PlayerControl.LocalPlayer.Data == null) return;
            if (!PlayerControl.LocalPlayer.Is(RoleEnum.Altruist)) return;

            var role = Role.GetRole<Altruist>(PlayerControl.LocalPlayer);

            var data = PlayerControl.LocalPlayer.Data;
            var isDead = data.IsDead;
            var truePosition = PlayerControl.LocalPlayer.GetTruePosition();
            var maxDistance = GameOptionsData.KillDistances[PlayerControl.GameOptions.KillDistance];
            var flag = (PlayerControl.GameOptions.GhostsDoTasks || !data.IsDead) &&
                       (!AmongUsClient.Instance || !AmongUsClient.Instance.IsGameOver) &&
                       PlayerControl.LocalPlayer.CanMove;
            var allocs = Physics2D.OverlapCircleAll(truePosition, maxDistance,
                LayerMask.GetMask(new[] { "Players", "Ghost" }));

            var allocs2 = Physics2D.OverlapCircleAll(truePosition, maxDistance * 10,
                LayerMask.GetMask(new[] { "Players", "Ghost" }));

            var killButton = __instance.KillButton;
            DeadBody closestBody = null;
            var closestDistance = float.MaxValue;
            int arrowDistance = role.GetArrowDistance();

            if (arrowDistance > 0)
            {
                foreach (var collider2D in allocs2)
                {
                    if (!flag || isDead || collider2D.tag != "DeadBody") continue;
                    var component = collider2D.GetComponent<DeadBody>();


                    if (!(Vector2.Distance(truePosition, component.TruePosition) <=
                          arrowDistance)) continue;

                    var distance = Vector2.Distance(truePosition, component.TruePosition);
                    if (!(distance < closestDistance)) continue;
                    closestBody = component;
                    closestDistance = distance;
                }

                if (closestBody != null)
                {
                    if (Altruist.AltruistArrow == null)
                        Altruist.AltruistArrow = new Arrow();
                    if (Altruist.AltruistArrow.GetDeadBodyTarget() != closestBody)
                    {
                        Altruist.AltruistArrow.SetTarget(closestBody);
                    }
                }
            }

            

        }
    }
}