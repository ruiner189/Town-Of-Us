using Hazel;
using Reactor;
using TownOfUs.CrewmateRoles.AltruistMod;
using TownOfUs.Patches;
using TownOfUs.Patches.Buttons;
using UnityEngine;
using Coroutine = TownOfUs.CrewmateRoles.AltruistMod.Coroutine;

namespace TownOfUs.Roles
{
    public class Altruist : Role
    {
        public bool CurrentlyReviving;
        public static Arrow AltruistArrow;

        public bool ReviveUsed;

        public ModdedButton ReviveButton;

        public Altruist(PlayerControl player) : base(player)
        {
            Name = "Altruist";
            ImpostorText = () => "Sacrifice yourself to save another";
            TaskText = () => "Revive a dead body at the cost of your own life.";
            Color = Patches.Colors.Altruist;
            RoleType = RoleEnum.Altruist;
            ReviveButton = new ModdedButton(player);
            ReviveButton.ButtonType = ButtonType.AbilityButton;
            ReviveButton.ButtonTarget = ButtonTarget.DeadBody;
            ReviveButton.SetAction(TryToRevive);
            ReviveButton.SetCooldown(button => { return 30; });
            ReviveButton.Sprite = TownOfUs.ReviveSprite;
            ReviveButton.RegisterButton();
        }

        public bool TryToRevive(ModdedButton button)
        {
            var target = button.ClosestBody;
            var flag = PlayerControl.LocalPlayer.Is(RoleEnum.Altruist);
            if (!flag) return true;
            if (!PlayerControl.LocalPlayer.CanMove) return false;
            if (PlayerControl.LocalPlayer.Data.IsDead) return false;
            var role = Role.GetRole<Altruist>(PlayerControl.LocalPlayer);

            if(button.IsCoolingDown()) return false;
            if (!button.Enabled()) return false;
            var maxDistance = GameOptionsData.KillDistances[PlayerControl.GameOptions.KillDistance];
            if (role == null)
                return false;
            if (button.ClosestBody == null)
                return false;
            if (Vector2.Distance(target.TruePosition,
                PlayerControl.LocalPlayer.GetTruePosition()) > maxDistance) return false;
            var playerId = button.ClosestBody.ParentId;

            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)CustomRPC.AltruistRevive, SendOption.Reliable, -1);
            writer.Write(PlayerControl.LocalPlayer.PlayerId);
            writer.Write(playerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);

            Coroutines.Start(Coroutine.AltruistRevive(target, role));
            return false;
        }

        public int GetArrowDistance()
        {
            if (!CustomGameOptions.RoleProgressionOn) return 0;
            int distance = 0;
            if (Tier1) distance += 5;
            if (Tier2) distance += 1;
            if (Tier3) distance += 1;
            if (Tier4) distance += 2;
            return distance;
        }
    }
}