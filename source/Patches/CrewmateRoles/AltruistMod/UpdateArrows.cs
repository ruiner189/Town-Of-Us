using HarmonyLib;
using Reactor.Extensions;
using TownOfUs.Extensions;
using TownOfUs.Roles;

namespace TownOfUs.CrewmateRoles.AltruistMod
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    public class UpdateArrows
    {
        public static void Postfix(PlayerControl __instance)
        {
            if (Coroutine.Arrow != null)
            {
                if (LobbyBehaviour.Instance || MeetingHud.Instance || PlayerControl.LocalPlayer.Data.IsDead ||
                    Coroutine.Target.Data.IsDead)
                {
                    Coroutine.Arrow.gameObject.Destroy();
                    Coroutine.Target = null;
                    return;
                }

                Coroutine.Arrow.target = Coroutine.Target.transform.position;
            }
            if(Altruist.AltruistArrow != null) {
                if (LobbyBehaviour.Instance || MeetingHud.Instance || PlayerControl.LocalPlayer.Data.IsDead)
                {
                    Altruist.AltruistArrow.Destroy();
                    Altruist.AltruistArrow = null;
                }
                Altruist.AltruistArrow.update();
            }
        }
    }
}