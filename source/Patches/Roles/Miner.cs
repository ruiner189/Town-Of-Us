using Hazel;
using Reactor.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using TownOfUs.Patches.Buttons;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TownOfUs.Roles
{
    public class Miner : Role
    {
        public readonly List<Vent> Vents = new List<Vent>();

        public ModdedButton MineButton;

        public Miner(PlayerControl player) : base(player)
        {
            Name = "Miner";
            ImpostorText = () => "From the top, make it drop, that's a vent";
            TaskText = () => "From the top, make it drop, that's a vent";
            Color = Patches.Colors.Impostor;
            RoleType = RoleEnum.Miner;
            Faction = Faction.Impostors;

            MineButton = new ModdedButton(player);
            MineButton.ButtonType = ButtonType.AbilityButton;
            MineButton.ButtonTarget = ButtonTarget.None;
            MineButton.Sprite = TownOfUs.MineSprite;
            MineButton.SetAction(MineAction);
            MineButton.SetCooldown(button => CustomGameOptions.MineCd);
            MineButton.name = "mineButton";
            MineButton.RegisterButton();

            GenerateKillButton();
        }

        public bool MineAction(ModdedButton button)
        {
            var position = Player.transform.position;
            var id = GetAvailableId();
            SpawnVent(id, this, position, 0.01f);
            RpcSpawnVent();
            return false;
        }

        public static void SpawnVent(int ventId, Miner role, Vector2 position, float zAxis)
        {
            var ventPrefab = Object.FindObjectOfType<Vent>();
            var vent = Object.Instantiate(ventPrefab, ventPrefab.transform.parent);
            vent.Id = ventId;
            vent.transform.position = new Vector3(position.x, position.y, zAxis);

            if (role.Vents.Count > 0)
            {
                var leftVent = role.Vents[^1];
                vent.Left = leftVent;
                leftVent.Right = vent;
            }
            else
            {
                vent.Left = null;
            }

            vent.Right = null;
            vent.Center = null;

            var allVents = ShipStatus.Instance.AllVents.ToList();
            allVents.Add(vent);
            ShipStatus.Instance.AllVents = allVents.ToArray();

            role.Vents.Add(vent);
        }

        public void RpcSpawnVent()
        {
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
            (byte)CustomRPC.Mine, SendOption.Reliable, -1);
            var position = Player.transform.position;
            var id = GetAvailableId();
            writer.Write(id);
            writer.Write(Player.PlayerId);
            writer.Write(position);
            writer.Write(0.01f);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }

        public static int GetAvailableId()
        {
            var id = 0;

            while (true)
            {
                if (ShipStatus.Instance.AllVents.All(v => v.Id != id)) return id;
                id++;
            }
        }

        public bool CanPlace { get; set; }
        public Vector2 VentSize { get; set; }

    }
}