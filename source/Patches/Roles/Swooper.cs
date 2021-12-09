using Hazel;
using System;
using TownOfUs.Patches.Buttons;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TownOfUs.Roles
{
    public class Swooper : Role
    {
        public ModdedButton SwoopButton;
        public Swooper(PlayerControl player) : base(player)
        {
            Name = "Swooper";
            ImpostorText = () => "Turn invisible temporarily";
            TaskText = () => "Turn invisible and sneakily kill";
            Color = Patches.Colors.Impostor;
            RoleType = RoleEnum.Swooper;
            Faction = Faction.Impostors;

            SwoopButton = new ModdedButton(player);
            SwoopButton.ButtonType = ButtonType.AbilityButton;
            SwoopButton.ButtonTarget = ButtonTarget.None;
            SwoopButton.Sprite = TownOfUs.SwoopSprite;
            SwoopButton.SetAction(SwoopAction);
            SwoopButton.SetActionEnd(SwoopActionEnd);
            SwoopButton.SetCooldown(button => CustomGameOptions.SwoopCd);
            SwoopButton.SetDuration(button => CustomGameOptions.SwoopDuration);
            SwoopButton.RegisterButton();

            GenerateKillButton();
        }

        public bool SwoopAction(ModdedButton button)
        {
            RpcSwoop();
            Swoop();
            return false;
        }

        public void SwoopActionEnd(ModdedButton button)
        {
            RpcUnSwoop();
            UnSwoop();
        }

        public void Swoop()
        {
            var color = Color.clear;
            if (PlayerControl.LocalPlayer.Data.Role.IsImpostor || PlayerControl.LocalPlayer.Data.IsDead) color.a = 0.1f;

            Player.MyRend.color = color;
            Utils.Morph(Player, new GameData.PlayerOutfit());
        }

        public void RpcSwoop()
        {
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
            (byte)CustomRPC.Swoop, SendOption.Reliable, -1);
            var position = PlayerControl.LocalPlayer.transform.position;
            writer.Write(Player.PlayerId);
            writer.Write(true);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }


        public void UnSwoop()
        {
            Player.MyRend.color = Color.white;
            Utils.Morph(Player, Player.Data.DefaultOutfit);
        }

        public void RpcUnSwoop()
        {
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
            (byte)CustomRPC.Swoop, SendOption.Reliable, -1);
            var position = PlayerControl.LocalPlayer.transform.position;
            writer.Write(Player.PlayerId);
            writer.Write(false);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }
    }
}