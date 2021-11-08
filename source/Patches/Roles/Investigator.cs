using System.Collections.Generic;
using TownOfUs.CrewmateRoles.InvestigatorMod;
using UnityEngine;

namespace TownOfUs.Roles
{
    public class Investigator : Role
    {
        public readonly List<Footprint> AllPrints = new List<Footprint>();


        public Investigator(PlayerControl player) : base(player)
        {
            Name = "Investigator";
            ImpostorText = () => "Find all imposters by examining footprints";
            TaskText = () => "You can see everyone's footprints.";
            Color = Patches.Colors.Investigator;
            RoleType = RoleEnum.Investigator;
            Scale = 1.4f;
        }
    }
}