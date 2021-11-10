using Hazel;
using Reactor.Extensions;
using System;
using TownOfUs.Patches.Buttons;

namespace TownOfUs.Roles
{
    public class Undertaker : Role
    {
        public KillButton _dragDropButton;
        public ModdedButton DragButton;
        public ModdedButton VentButton;

        public Undertaker(PlayerControl player) : base(player)
        {
            Name = "Undertaker";
            ImpostorText = () => "Drag bodies and hide them";
            TaskText = () => "Drag bodies around to hide them from being reported";
            Color = Patches.Colors.Impostor;
            RoleType = RoleEnum.Undertaker;
            Faction = Faction.Impostors;

            DragButton = new ModdedButton(player);
            DragButton.ButtonType = ButtonType.AbilityButton;
            DragButton.ButtonTarget = ButtonTarget.DeadBody;
            DragButton.Sprite = TownOfUs.DragSprite;
            DragButton.SetAction(DragAction);
            DragButton.SetCooldown(button => CustomGameOptions.DragCd);
            DragButton.SetDuration(button => 10f);
            DragButton.SetActionEnd(DragActionEnd);
            DragButton.SetEnabled(DragEnable);
            DragButton.RegisterButton();

            VentButton = new ModdedButton(player);
            VentButton.ButtonType = ButtonType.VentButton;
            VentButton.ButtonTarget = ButtonTarget.Vent;
            VentButton.UseDefault = true;
            VentButton.SetCooldown(button => { return 0.35f; });
            VentButton.SetCooldownValue(0f);
            VentButton.SetAction(ModdedButton.VentAction);
            VentButton.SetEnabled(VentButtonEnabled);
            VentButton.RegisterButton();

            GenerateKillButton();
        }

        public bool DragAction(ModdedButton button)
        {
            if (!button.Enabled()) return false;

            if (CurrentlyDragging == null)
            {
                var target = button.ClosestBody;
                var playerId = target.ParentId;

                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.Drag, SendOption.Reliable, -1);
                writer.Write(PlayerControl.LocalPlayer.PlayerId);
                writer.Write(playerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);

                CurrentlyDragging = target;
                DragButton.Sprite = TownOfUs.DropSprite;
            } else
            {
                StopDragging();
                DragButton.SkipAction = true;
            }

            return false;
        }

        public bool VentButtonEnabled(ModdedButton button)
        {
            if (!button.Show()) return false;
            if (PlayerControl.AllPlayerControls.Count <= 1) return false;
            if (button.GetCooldown() > 0 && button.IsCoolingDown()) return false;
            if (CurrentlyDragging != null) return false;

            var player = button.Player;
            if (player.CanMove || player.inVent)
                return button.ClosestVent != null;
            return false;
        }
        public bool DragEnable(ModdedButton button)
        {
            if (!button.Show()) return false;

            if (button.ActionActive) return true;
            return ModdedButton.DefaultEnabled(button);
        }

        public void DragActionEnd(ModdedButton button)
        {
            if(CurrentlyDragging != null)
                StopDragging();
        }

        private void StopDragging()
        {
            DragButton.ResetCooldownValue();

            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)CustomRPC.Drop, SendOption.Reliable, -1);
            writer.Write(PlayerControl.LocalPlayer.PlayerId);
            var position = CurrentlyDragging.TruePosition;
            writer.Write(position);
            writer.Write(0);
            AmongUsClient.Instance.FinishRpcImmediately(writer);

            var body = CurrentlyDragging;
            body.bodyRenderer.material.SetFloat("_Outline", 0f);
            CurrentlyDragging = null;
            
            DragButton.Sprite = TownOfUs.DragSprite;
        }

        public DeadBody CurrentlyDragging { get; set; }
    }
}