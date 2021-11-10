using HarmonyLib;
using Hazel;
using Reactor;
using TownOfUs.Patches.Buttons;
using TownOfUs.Roles;
using TownOfUs.Roles.Modifiers;
using UnityEngine;

namespace TownOfUs.CrewmateRoles.MedicMod
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.CmdCheckMurder))]
    public class StopKill
    {
        public static void BreakShield(byte medicId, byte playerId, bool notify, bool breakShield)
        {
            if (notify)
            {
                if (PlayerControl.LocalPlayer.PlayerId == playerId &&
                    CustomGameOptions.NotificationShield == NotificationOptions.Shielded)
                    Coroutines.Start(Utils.FlashCoroutine(new Color(0f, 0.5f, 0f, 1f)));

                if (PlayerControl.LocalPlayer.PlayerId == medicId &&
                    CustomGameOptions.NotificationShield == NotificationOptions.Medic)
                    Coroutines.Start(Utils.FlashCoroutine(new Color(0f, 0.5f, 0f, 1f)));

                if (CustomGameOptions.NotificationShield == NotificationOptions.Everyone)
                    Coroutines.Start(Utils.FlashCoroutine(new Color(0f, 0.5f, 0f, 1f)));
            }
            if (!breakShield)
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
        public static bool Prefix(PlayerControl __instance, PlayerControl target)
        {
            if (target == null) return true;
            if (target.IsShielded())
            {
                Medic.RpcInteractWithShield(target, true, true);
                if (!Medic.InteractWithShield(target, true, true))
                {
                    PlayerControl.LocalPlayer.SetKillTimer(PlayerControl.GameOptions.KillCooldown);
                }
                return false;
            }
            Diseased diseased = Modifier.GetModifier<Diseased>(target);
            if(diseased != null)
            {
                foreach(var button in ModdedButton.GetAllModdedButtonsFromPlayer(__instance))
                {
                    button.ResetCooldownValue();
                    button.SetCooldownValue(button.GetCooldown() * 2);
                }
            }
            return true;
        }
    }
}