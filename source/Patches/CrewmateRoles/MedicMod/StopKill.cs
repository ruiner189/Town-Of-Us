using HarmonyLib;
using Hazel;
using Reactor;
using TownOfUs.Roles;
using UnityEngine;

namespace TownOfUs.CrewmateRoles.MedicMod
{
    [HarmonyPatch(typeof(KillButtonManager), nameof(KillButtonManager.PerformKill))]
    public class StopKill
    {
        public static void BreakShield(byte medicId, byte playerId, bool flag)
        {
            if (PlayerControl.LocalPlayer.PlayerId == playerId &&
                CustomGameOptions.NotificationShield == NotificationOptions.Shielded)
                Coroutines.Start(Utils.FlashCoroutine(new Color(0f, 0.5f, 0f, 1f)));

            if (PlayerControl.LocalPlayer.PlayerId == medicId &&
                CustomGameOptions.NotificationShield == NotificationOptions.Medic)
                Coroutines.Start(Utils.FlashCoroutine(new Color(0f, 0.5f, 0f, 1f)));

            if (CustomGameOptions.NotificationShield == NotificationOptions.Everyone)
                Coroutines.Start(Utils.FlashCoroutine(new Color(0f, 0.5f, 0f, 1f)));

            PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"Flag? {flag}");
            if (!flag)
                return;

            var player = Utils.PlayerById(playerId);
            PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"Player? {player}");
            var medicPlayer = Utils.PlayerById(medicId);
            PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"Medic? {medicPlayer}");
            if (medicPlayer.Is(RoleEnum.Medic))
            {
                var medic = Role.GetRole<Medic>(medicPlayer);
                if(medic.ShieldedPlayer.PlayerId == playerId)
                {
                    medic.ShieldedPlayer = null;
                    medic.exShielded = player;
                    PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"{player.name} is Ex-Shielded");
                }
            }

            player.myRend.material.SetColor("_VisorColor", Palette.VisorColor);
            player.myRend.material.SetFloat("_Outline", 0f);
        }

        [HarmonyPriority(Priority.First)]
        public static bool Prefix(KillButtonManager __instance)
        {
            if (__instance != DestroyableSingleton<HudManager>.Instance.KillButton) return true;
            if (!PlayerControl.LocalPlayer.Data.IsImpostor) return true;
            var target = __instance.CurrentTarget;
            if (target == null) return true;
            if (target.isShielded())
            {
                if (__instance.isActiveAndEnabled && !__instance.isCoolingDown)
                {
                    var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                        (byte) CustomRPC.AttemptSound, SendOption.Reliable, -1);
                    writer.Write(target.getMedic().Player.PlayerId);
                    writer.Write(target.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);

                    if (CustomGameOptions.RoleProgressionOn)
                    {
                        if (!target.getMedic().GetTier3)
                            PlayerControl.LocalPlayer.SetKillTimer(PlayerControl.GameOptions.KillCooldown);
                        PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"{medic.Player.PlayerId}, {target.PlayerId}, {!medic.GetTier3}");
                        BreakShield(target.getMedic().Player.PlayerId, target.PlayerId, !target.getMedic().GetTier3);
                    } else
                    {
                        if (CustomGameOptions.ShieldBreaks)
                            PlayerControl.LocalPlayer.SetKillTimer(PlayerControl.GameOptions.KillCooldown);
                        BreakShield(target.getMedic().Player.PlayerId, target.PlayerId, CustomGameOptions.ShieldBreaks);
                    }

                }


                return false;
            }


            return true;
        }
    }
}