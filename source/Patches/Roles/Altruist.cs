using TownOfUs.Patches;
using UnityEngine;

namespace TownOfUs.Roles
{
    public class Altruist : Role
    {
        public bool CurrentlyReviving;
        public DeadBody CurrentTarget;

        public static Arrow AltruistArrow;

        public bool ReviveUsed;

        public Altruist(PlayerControl player) : base(player)
        {
            Name = "Altruist";
            ImpostorText = () => "Sacrifice yourself to save another";
            TaskText = () => "Revive a dead body at the cost of your own life.";
            Color = Patches.Colors.Altruist;
            RoleType = RoleEnum.Altruist;
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