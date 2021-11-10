using System.Collections.Generic;
using System.Linq;
using Hazel;
using Reactor;
using TownOfUs.ImpostorRoles.CamouflageMod;
using TownOfUs.Patches;
using TownOfUs.Roles.Modifiers;
using UnityEngine;

namespace TownOfUs.Roles
{
    public class Lover : Modifier
    {
        public Lover(PlayerControl player) : base(player)
        {
            Name = "Lover";
            SymbolName = "♥";
            TaskText = () =>
                "You are in Love with " + OtherLover.Player.name;
            Color = Colors.Lovers;
            ModifierType = ModifierEnum.Lover;
        }

        public Lover OtherLover { get; set; }
        public bool LoveCoupleWins { get; set; }
        public int Num { get; set; }
        public bool LoverImpostor { get; set; }

        public override List<PlayerControl> GetTeammates()
        {
            var loverTeam = new List<PlayerControl>
            {
                PlayerControl.LocalPlayer,
                OtherLover.Player
            };
            return loverTeam;
        }

        public static void Gen(List<PlayerControl> canHaveModifiers)
        {
            List<PlayerControl> crewmates = new List<PlayerControl>();
            List<PlayerControl> impostors = new List<PlayerControl>();

            foreach(var player in canHaveModifiers)
            {
                if (player.Data.Role.IsImpostor)
                    impostors.Add(player);
                else crewmates.Add(player);
            }

            // Check to make sure there is enough players for lovers
            if (crewmates.Count <= 1 && impostors.Count < 1) return;

            // Chooses a completely random player as the first lover.
            var num = Random.RandomRangeInt(0, canHaveModifiers.Count);
            var firstLover = canHaveModifiers[num];
            canHaveModifiers.Remove(firstLover);

            var isImpostor = impostors.Contains(firstLover);

            // Chooses second lover. If first is an impostor, the second will not be.
            PlayerControl secondLover;
            if (isImpostor) {
                num = Random.RandomRangeInt(0, crewmates.Count);
                secondLover = crewmates[num];
            } else {
                num = Random.RandomRangeInt(0, canHaveModifiers.Count);
                secondLover = canHaveModifiers[num];
            }
            canHaveModifiers.Remove(secondLover);

            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte) CustomRPC.SetCouple, SendOption.Reliable, -1);
            writer.Write(firstLover.PlayerId);
            writer.Write(secondLover.PlayerId);
            var lover1 = new Lover(firstLover);
            var lover2 = new Lover(secondLover);

            lover1.OtherLover = lover2;
            lover2.OtherLover = lover1;

            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }

        internal override bool EABBNOODFGL(ShipStatus __instance)
        {
            if (FourPeopleLeft()) return false;

            if (CheckLoversWin())
            {
                //System.Console.WriteLine("LOVERS WIN");
                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte) CustomRPC.LoveWin, SendOption.Reliable, -1);
                writer.Write(Player.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                Win();
                Utils.EndGame();
                return false;
            }

            return true;
        }

        private bool FourPeopleLeft()
        {
            var players = PlayerControl.AllPlayerControls.ToArray();
            var alives = players.Where(x => !x.Data.IsDead).ToList();
            var lover1 = Player;
            var lover2 = OtherLover.Player;
            {
                return !lover1.Data.IsDead && !lover2.Data.IsDead &&
                       alives.Count() == 4 && LoverImpostor;
            }
        }

        private bool CheckLoversWin()
        {
            //System.Console.WriteLine("CHECKWIN");
            var players = PlayerControl.AllPlayerControls.ToArray();
            var alives = players.Where(x => !x.Data.IsDead).ToList();
            var lover1 = Player;
            var lover2 = OtherLover.Player;

            return !lover1.Data.IsDead && !lover2.Data.IsDead &&
                   (alives.Count == 3) | (alives.Count == 2);
        }

        public void Win()
        {
            if (Role.AllRoles.Where(x => x.RoleType == RoleEnum.Jester).Any(x => ((Jester) x).VotedOut)) return;
            LoveCoupleWins = true;
            OtherLover.LoveCoupleWins = true;
        }
    }
}