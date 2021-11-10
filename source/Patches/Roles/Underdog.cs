using System.Linq;
using TownOfUs.Patches.Buttons;

namespace TownOfUs.Roles
{
    public class Underdog : Role
    {
        public Underdog(PlayerControl player) : base(player)
        {
            Name = "Underdog";
            ImpostorText = () => "Use your comeback power to win";
            TaskText = () => "long kill cooldown when 2 imps, short when 1 imp";
            Color = Patches.Colors.Impostor;
            RoleType = RoleEnum.Underdog;
            Faction = Faction.Impostors;

            GenerateKillButton();
            KillButton.SetCooldown(MaxTimer);
        }

        public float MaxTimer(ModdedButton button) => PlayerControl.GameOptions.KillCooldown * (
            LastImp() ? 0.5f : 1.5f
        );

        private bool LastImp()
        {
            return PlayerControl.AllPlayerControls.ToArray()
                .Count(x => x.Data.Role.IsImpostor && !x.Data.IsDead) == 1;
        }


    }
}
