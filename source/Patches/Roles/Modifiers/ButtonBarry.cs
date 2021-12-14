using Hazel;
using TownOfUs.Patches.Buttons;
using UnityEngine;

namespace TownOfUs.Roles.Modifiers
{
    public class ButtonBarry : Modifier
    {

        public bool ButtonUsed;

        public ModdedButton Button;

        public ButtonBarry(PlayerControl player) : base(player)
        {
            Name = "Button Barry";
            TaskText = () => "Call a button from anywhere!";
            Color = new Color(0.9f, 0f, 1f, 1f);
            ModifierType = ModifierEnum.ButtonBarry;

            Button = new ModdedButton(player);
            Button.UseDefault = false;
            Button.ButtonType = ButtonType.AbilityButton;
            Button.ButtonTarget = ButtonTarget.None;
            Button.Sprite = TownOfUs.ButtonSprite;
            Button.SetAction(ButtonAction);
            Button.SetCooldown(button => 0);
            Button.SetCooldownValue(0);
            Button.SetPosition(Utility.HudAlignment.BottomLeft, GetOffset());
            Button.RegisterButton();
        }

        public bool ButtonAction(ModdedButton button)
        {
            ButtonUsed = true;
            RpcButtonBarryMeeting();
            SetupMeeting();
            return false;
        }

        public bool ButtonEnabled(ModdedButton button)
        {
            if (!ModdedButton.DefaultEnabled(button)) return false;
            if (ButtonUsed) return false;
            return true;
        }

        public static void SetupMeeting()
        {
            if (AmongUsClient.Instance.AmHost)
            {
                MeetingRoomManager.Instance.reporter = PlayerControl.LocalPlayer;
                MeetingRoomManager.Instance.target = null;
                AmongUsClient.Instance.DisconnectHandlers.AddUnique(
                    MeetingRoomManager.Instance.Cast<IDisconnectHandler>());
                if (ShipStatus.Instance.CheckTaskCompletion()) return;
                DestroyableSingleton<HudManager>.Instance.OpenMeetingRoom(PlayerControl.LocalPlayer);
                PlayerControl.LocalPlayer.RpcStartMeeting(null);
            }
        }

        public static void RpcButtonBarryMeeting()
        {
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
            (byte)CustomRPC.BarryButton, SendOption.Reliable, -1);
            writer.Write(PlayerControl.LocalPlayer.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }

        private int GetOffset()
        {
            Role role = Role.GetRole(Player);
            if (role == null) return 0;
            if (role.RoleType == RoleEnum.Glitch || role.RoleType == RoleEnum.Morphling) return 2;
            return 0;
        }
    }
}