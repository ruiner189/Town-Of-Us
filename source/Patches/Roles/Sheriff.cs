using Hazel;
using System;
using TownOfUs.CrewmateRoles.MedicMod;
using TownOfUs.Patches.Buttons;
using UnityEngine;

namespace TownOfUs.Roles
{
    public class Sheriff : Role
    {

        public Sheriff(PlayerControl player) : base(player)
        {
            Name = "Sheriff";
            ImpostorText = () => "Shoot the <color=#FF0000FF>Impostor</color>";
            TaskText = () => "Kill off the impostor but don't kill crewmates.";
            Color = Patches.Colors.Sheriff;
            RoleType = RoleEnum.Sheriff;

            KillButton = new ModdedButton(player);
            KillButton.ButtonType = ButtonType.KillButton;
            KillButton.ButtonTarget = ButtonTarget.Player;
            KillButton.UseDefault = true;
            KillButton.SetCooldown(button => { return CustomGameOptions.SheriffKillCd; });
            KillButton.SetAction(SheriffKillAction);
            KillButton.RegisterButton();
        }
        public bool FirstRound { get; set; } = false;

        public bool SheriffKillAction(ModdedButton button)
        {
            var target = KillButton.ClosestPlayer;
            if (target.IsShielded())
            {
                Medic.RpcInteractWithShield(target, true, true);
                if(!Medic.InteractWithShield(target, true, true))
                {
                    button.ResetCooldown = false;
                }
                return false;
            }
            var canKill = target.Data.Role.IsImpostor ||
                    target.Is(RoleEnum.Jester) && CustomGameOptions.SheriffKillsJester ||
                    target.Is(RoleEnum.Glitch) && CustomGameOptions.SheriffKillsGlitch ||
                    target.Is(RoleEnum.Arsonist) && CustomGameOptions.SheriffKillsArsonist ||
                    target.Is(RoleEnum.Sheriff) && CustomGameOptions.SheriffKillsSheriff;

            if (!canKill)
            {
                if (CustomGameOptions.SheriffKillOther)
                    Utils.RpcMurderPlayer(PlayerControl.LocalPlayer, target);
                Utils.RpcMurderPlayer(PlayerControl.LocalPlayer, PlayerControl.LocalPlayer);
            }
            else
            {
                Utils.RpcMurderPlayer(PlayerControl.LocalPlayer, target);
            }
            
            return false;
        }

        public bool SheriffKillShow(ModdedButton button)
        {
            if (!ModdedButton.DefaultShow(button)) return false;
            if (!CustomGameOptions.SheriffFirstRoundOn && !FirstRound) return false;
            return true;
        }

        public Boolean SheriffKillButtonEnabled(ModdedButton button)
        {
            if (PlayerControl.AllPlayerControls.Count <= 1) return false;
            if (PlayerControl.LocalPlayer == null) return false;
            if (PlayerControl.LocalPlayer.Data == null) return false;
            if (PlayerControl.AllPlayerControls.Count <= 1) return false;
            if (button.ClosestPlayer == null) return false;
            
            var isDead = PlayerControl.LocalPlayer.Data.IsDead;
            var firstRound = CustomGameOptions.SheriffFirstRoundOn && FirstRound;

            if (isDead || firstRound)
                return false;

            return true;
        }


        internal override bool Criteria()
        {
            return CustomGameOptions.ShowSheriff || base.Criteria();
        }
    }
}