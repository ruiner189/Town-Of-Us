using Hazel;
using Reactor;
using System;
using TownOfUs.Patches.Buttons;
using UnityEngine;

namespace TownOfUs.Roles
{
    public class Camouflager : Role

    {
        public KillButton _camouflageButton;
        public bool Enabled;
        public DateTime LastCamouflaged;
        public float TimeRemaining;

        public ModdedButton CamouflageButton;

        public Camouflager(PlayerControl player) : base(player)
        {
            Name = "Camouflager";
            ImpostorText = () => "Camouflage and turn everyone grey";
            TaskText = () => "Camouflage and get secret kills";
            Color = Patches.Colors.Impostor;
            RoleType = RoleEnum.Camouflager;
            Faction = Faction.Impostors;

            CamouflageButton = new ModdedButton(player);
            CamouflageButton.ButtonType = ButtonType.AbilityButton;
            CamouflageButton.ButtonTarget = ButtonTarget.None;
            CamouflageButton.Sprite = TownOfUs.Camouflage;
            CamouflageButton.SetAction(CamouflageAction);
            CamouflageButton.SetCooldown(button => {return CustomGameOptions.CamouflagerCd + CustomGameOptions.CamouflagerDuration;});
            CamouflageButton.SetDuration(button => {return CustomGameOptions.CamouflagerDuration;});
            CamouflageButton.SetActionEnd(CamouflageActionEnd);
            CamouflageButton.RegisterButton();

            GenerateKillButton();
        }

        public bool CamouflageAction(ModdedButton button)
        {
            if (!button.Enabled()) return false;

            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)CustomRPC.Camouflage,
                SendOption.Reliable, -1);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            Utils.Camouflage();
            return false;
        }

        public void CamouflageActionEnd(ModdedButton button)
        {
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
            (byte)CustomRPC.UnCamouflage,
            SendOption.Reliable, -1);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            Utils.UnCamouflage();
        }

        

        public bool Camouflaged => TimeRemaining > 0f;

        public void Camouflage()
        {
            Enabled = true;
            TimeRemaining -= Time.deltaTime;
            Utils.Camouflage();
        }

        public void UnCamouflage()
        {
            Enabled = false;
            LastCamouflaged = DateTime.UtcNow;
            Utils.UnCamouflage();
        }
    }
}