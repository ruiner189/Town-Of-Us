using Hazel;
using TownOfUs.CrewmateRoles.MedicMod;
using TownOfUs.Patches.Buttons;
using UnityEngine;

namespace TownOfUs.Roles
{
    public class Medic : Role
    {
        private ModdedButton ShieldButton;
        public bool UsedAbility { get; set; } = false;
        public PlayerControl ShieldedPlayer { get; set; }
        public PlayerControl exShielded { get; set; }

        public Medic(PlayerControl player) : base(player)
        {
            Name = "Medic";
            ImpostorText = () => "Create a shield to protect a crewmate";
            TaskText = () => "Protect a crewmate with a shield";
            Color = Patches.Colors.Medic;
            RoleType = RoleEnum.Medic;
            ShieldedPlayer = null;

            ShieldButton = new ModdedButton(player);
            ShieldButton.Sprite = TownOfUs.MedicSprite;
            ShieldButton.ButtonType = ButtonType.AbilityButton;
            ShieldButton.SetCooldown(button => { return 20; });
            ShieldButton.SetShow(MedicButtonShow);
            ShieldButton.SetAction(MedicButtonAction);
            ShieldButton.RegisterButton();
        }



        public bool MedicButtonAction(ModdedButton button)
        {
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)CustomRPC.Protect, SendOption.Reliable, -1);
            writer.Write(PlayerControl.LocalPlayer.PlayerId);
            writer.Write(button.ClosestPlayer.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);

            ShieldedPlayer = button.ClosestPlayer;
            UsedAbility = true;
            return false;
        }

        public bool MedicButtonShow(ModdedButton button)
        {
            if (!ModdedButton.DefaultShow(button)) return false;
            if (UsedAbility) return false;

            return true;
        }

        public static bool InteractWithShield(PlayerControl target, bool shouldNotify, bool tryToBreakShield)
        {
            var medic = target.GetMedic();
            if(medic != null)
            {
                bool canBreakShield = CustomGameOptions.ShieldBreaks || (CustomGameOptions.RoleProgressionOn && !medic.GetTier3);
                StopKill.BreakShield(medic.Player.PlayerId, target.PlayerId, shouldNotify, canBreakShield && tryToBreakShield);
                return canBreakShield && tryToBreakShield;
            }
            return false;

        }

        public static void RpcInteractWithShield(PlayerControl target, bool shouldNotify, bool tryToBreakShield)
        {
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
            (byte)CustomRPC.AttemptSound, SendOption.Reliable, -1);
            writer.Write(target.PlayerId);
            writer.Write(shouldNotify);
            writer.Write(tryToBreakShield);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }


    }
}