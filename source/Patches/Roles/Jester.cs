using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace TownOfUs.Roles
{
    public class Jester : Role
    {
        public bool VotedOut;


        public Jester(PlayerControl player) : base(player)
        {
            Name = "Jester";
            ImpostorText = () => "Get voted out";
            TaskText = () => "Get voted out!\nFake Tasks:";
            Color = Patches.Colors.Jester;
            RoleType = RoleEnum.Jester;
            Faction = Faction.Neutral;
        }

        internal override bool EABBNOODFGL(ShipStatus __instance)
        {
            if (!VotedOut || !Player.Data.IsDead && !Player.Data.Disconnected) return true;
            Utils.EndGame();
            return false;
        }

        public void Wins()
        {
            //System.Console.WriteLine("Reached Here - Jester edition");
            VotedOut = true;
        }

        public void Loses()
        {
            Player.Data.Role.TeamType = RoleTeamTypes.Impostor;
        }
    }
}