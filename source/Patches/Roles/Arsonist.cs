using System;
using System.Collections.Generic;
using System.Linq;
using Hazel;
using Reactor;
using TownOfUs.Patches.Buttons;
using UnityEngine;

namespace TownOfUs.Roles
{
    public class Arsonist : Role
    {
        public bool ArsonistWins;
        public List<byte> DousedPlayers = new List<byte>();
        public bool IgniteUsed;

        public ModdedButton DouseButton;
        public ModdedButton IgniteButton;


        public Arsonist(PlayerControl player) : base(player)
        {
            Name = "Arsonist";
            ImpostorText = () => "Douse players and ignite the light";
            TaskText = () => "Douse players and ignite to kill everyone\nFake Tasks:";
            Color = Patches.Colors.Arsonist;
            RoleType = RoleEnum.Arsonist;
            Faction = Faction.Neutral;

            DouseButton = new ModdedButton(player);
            DouseButton.ButtonType = ButtonType.AbilityButton;
            DouseButton.ButtonTarget = ButtonTarget.Player;
            DouseButton.Sprite = TownOfUs.DouseSprite;
            DouseButton.SetCooldown(button => CustomGameOptions.DouseCd);
            DouseButton.SetAction(DouseAction);
            DouseButton.SetFilter(DouseFilter);
            DouseButton.name = "Douse Button";
            DouseButton.RegisterButton();

            IgniteButton = new ModdedButton(player);
            IgniteButton.name = "Ignite Button";
            IgniteButton.ButtonType = ButtonType.KillButton;
            IgniteButton.ButtonTarget = ButtonTarget.None;
            IgniteButton.Sprite = TownOfUs.IgniteSprite;
            IgniteButton.SetCooldown(button => PlayerControl.GameOptions.KillCooldown);
            IgniteButton.SetEnabled(IgniteEnable);
            IgniteButton.SetAction(IgniteAction);
            IgniteButton.RegisterButton();
            
        }

        // TODO TaskID 3 Polus

        public bool DouseAction(ModdedButton button)
        {
            DousedPlayers.Add(button.ClosestPlayer.PlayerId);
            RpcSetDoused(button.ClosestPlayer);
            return false;
        }

        public List<PlayerControl> DouseFilter(ModdedButton button)
        {
            List<PlayerControl> allPlayers = PlayerControl.AllPlayerControls.ToArray().ToList();
            allPlayers.RemoveAll(player => DousedPlayers.Contains(player.PlayerId));
            return allPlayers;
        }

        public bool IgniteAction(ModdedButton button)
        {
            Ignite(this);
            RpcIgnite();
            return false;
        }

        public bool IgniteEnable(ModdedButton button)
        {
            if (!ModdedButton.DefaultEnabled(button)) return false;
            return CheckEveryoneDoused();
        }

        public static void Ignite(Arsonist arson)
        {
            foreach (var playerId in arson.DousedPlayers)
            {
                var player = Utils.PlayerById(playerId);
                if (
                    player == null ||
                    player.Data.Disconnected ||
                    player.Data.IsDead
                ) continue;
                Utils.MurderPlayer(player, player);
            }

            Utils.MurderPlayer(arson.Player, arson.Player);
            arson.IgniteUsed = true;
        }


        public void RpcSetDoused(PlayerControl target)
        {
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
            (byte)CustomRPC.Douse, SendOption.Reliable, -1);
            writer.Write(Player.PlayerId);
            writer.Write(target.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }

        public void RpcIgnite()
        {
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
            (byte)CustomRPC.Ignite, SendOption.Reliable, -1);
            writer.Write(Player.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }

        internal override bool EABBNOODFGL(ShipStatus __instance)
        {
            if (PlayerControl.AllPlayerControls.ToArray().Count(x => !x.Data.IsDead && !x.Data.Disconnected) == 0)
            {
                var writer = AmongUsClient.Instance.StartRpcImmediately(
                    PlayerControl.LocalPlayer.NetId,
                    (byte) CustomRPC.ArsonistWin,
                    SendOption.Reliable,
                    -1
                );
                writer.Write(Player.PlayerId);
                Wins();
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                Utils.EndGame();
                return false;
            }

            if (IgniteUsed || Player.Data.IsDead) return true;

            return !CustomGameOptions.ArsonistGameEnd;
        }


        public void Wins()
        {
            //System.Console.WriteLine("Reached Here - Glitch Edition");
            ArsonistWins = true;
        }

        public void Loses()
        {
            // Player.Data.IsImpostor = true;
        }

        public bool CheckEveryoneDoused()
        {
            var arsoId = Player.PlayerId;
            foreach (var player in PlayerControl.AllPlayerControls)
            {
                if (
                    player.PlayerId == arsoId ||
                    player.Data.IsDead ||
                    player.Data.Disconnected
                ) continue;
                if (!DousedPlayers.Contains(player.PlayerId)) return false;
            }

            return true;
        }

    }
}
