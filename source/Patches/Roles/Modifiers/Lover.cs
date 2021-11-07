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
            TaskText = () =>
                "You are in Love with " + OtherLover.Player.name;
            Color = Colors.Lovers;
            ModifierType = ModifierEnum.Lover;
        }

        /**
        public Lover(PlayerControl player, int num, bool loverImpostor) : base(player)
        {
            var imp = num == 2 && loverImpostor;
            Name = imp ? "Loving Impostor" : "Lover";
            Color = Patches.Colors.Lovers;
            ImpostorText = () =>
                "You are in " + ColorString + "Love</color> with " + ColorString + OtherLover.Player.name;
            TaskText = () => $"Stay alive with your love {OtherLover.Player.name} \n and win together";
            RoleType = imp ? RoleEnum.LoverImpostor : RoleEnum.Lover;
            Num = num;
            LoverImpostor = loverImpostor;
            Scale = imp ? 2.3f : 1f;
            Faction = imp ? Faction.Impostors : Faction.Crewmates;
        }
        */

        public Lover OtherLover { get; set; }
        public bool LoveCoupleWins { get; set; }
        public int Num { get; set; }
        public bool LoverImpostor { get; set; }

        /*
        protected override void IntroPrefix(IntroCutscene._CoBegin_d__14 __instance)
        {
            var loverTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
            loverTeam.Add(PlayerControl.LocalPlayer);
            loverTeam.Add(OtherLover.Player);
            __instance.yourTeam = loverTeam;
        }
        */

        public static void Gen(List<PlayerControl> crewmates, List<PlayerControl> impostors)
        {
            PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"Lovers 1 {crewmates} -- {impostors}");
            // Check to make sure there is enough players for lovers
            if (crewmates.Count <= 1 && impostors.Count < 1) return;

            PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"Lovers 2");
            // Chooses a completely random player as the first lover.
            var allPlayers = new List<PlayerControl>();
            allPlayers.AddRange(crewmates);
            allPlayers.AddRange(impostors);
            var num = Random.RandomRangeInt(0, allPlayers.Count);
            var firstLover = allPlayers[num];
            allPlayers.Remove(firstLover);

            var isImpostor = impostors.Contains(firstLover);

            PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"Lovers 3 : {num}");
            // Chooses second lover. If first is an impostor, the second will not be.
            PlayerControl secondLover;
            if (isImpostor) {
                num = Random.RandomRangeInt(0, crewmates.Count);
                secondLover = crewmates[num];
            } else {
                num = Random.RandomRangeInt(0, allPlayers.Count);
                secondLover = allPlayers[num];
            }

            PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"Lovers 4 : {num}");
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte) CustomRPC.SetCouple, SendOption.Reliable, -1);
            writer.Write(firstLover.PlayerId);
            writer.Write(secondLover.PlayerId);
            var lover1 = new Lover(firstLover);
            var lover2 = new Lover(secondLover);

            lover1.OtherLover = lover2;
            lover2.OtherLover = lover1;

            AmongUsClient.Instance.FinishRpcImmediately(writer);
            PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"Lovers 5 {writer}");
        }

        internal bool EABBNOODFGL(ShipStatus __instance)
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