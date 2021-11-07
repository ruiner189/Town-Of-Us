using UnityEngine;

namespace TownOfUs.Roles
{
    public class Engineer : Role
    {
        public Engineer(PlayerControl player) : base(player)
        {
            Name = "Engineer";
            ImpostorText = () => "Maintain important systems on the ship";
            TaskText = () => "Vent and fix a sabotage from anywhere!";
            Color = Patches.Colors.Engineer;
            RoleType = RoleEnum.Engineer;
            if (CustomGameOptions.RoleProgressionOn) UsedThisRound = true;
        }

        public bool UsedThisRound { get; set; } = false;

        public int TotalUses { get; set; } = 0;

        public int RemainingRounds {
            get {
                int Total = 0;
                if (Tier1) Total++;
                if (Tier2) Total++;
                if (Tier3) Total++;
                if (Tier4) Total++;
                return Total - TotalUses;
            }
        }

        protected override void OnTierUp()
        {
            base.OnTierUp();
            if (CustomGameOptions.RoleProgressionOn)
            {
                if (RemainingRounds > 0 && UsedThisRound)
                {
                    UsedThisRound = false;
                    TotalUses++;
                }
            }
        }

    }
}