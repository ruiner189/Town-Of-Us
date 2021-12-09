using Hazel;
using Reactor;
using System.Linq;
using TownOfUs.Patches.Buttons;
using UnityEngine;

namespace TownOfUs.Roles
{
    public class Engineer : Role
    {
        public ModdedButton VentButton;
        public ModdedButton FixButton;
        public Engineer(PlayerControl player) : base(player)
        {
            Name = "Engineer";
            ImpostorText = () => "Maintain important systems on the ship";
            TaskText = () => "Vent and fix a sabotage from anywhere!";
            Color = Patches.Colors.Engineer;
            RoleType = RoleEnum.Engineer;
            if (CustomGameOptions.RoleProgressionOn) UsedThisRound = true;

            VentButton = new ModdedButton(player);
            VentButton.ButtonType = ButtonType.VentButton;
            VentButton.ButtonTarget = ButtonTarget.Vent;
            VentButton.UseDefault = true;
            VentButton.SetCooldown(button => { return 0.35f; });
            VentButton.SetCooldownValue(0f);
            VentButton.SetAction(ModdedButton.VentAction);
            VentButton.SetEnabled(VentButtonEnabled);
            VentButton.RegisterButton();

            FixButton = new ModdedButton(player);
            FixButton.UseDefault = true;
            FixButton.ButtonType = ButtonType.AbilityButton;
            FixButton.ButtonTarget = ButtonTarget.None;
            FixButton.Sprite = TownOfUs.EngineerFix;
            FixButton.SetAction(FixAction);
            FixButton.SetEnabled(FixEnabled);
            FixButton.SetCooldown(button => { return 10; });
            FixButton.RegisterButton();
        }

        public bool VentButtonEnabled(ModdedButton button)
        {
            if (!button.Show()) return false;
            if (PlayerControl.AllPlayerControls.Count <= 1) return false;
            if (button.GetCooldown() > 0 && button.IsCoolingDown()) return false;

            var player = button.Player;
            if (player.CanMove || player.inVent)
                return button.ClosestVent != null;
            return false;
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

        public bool FixEnabled(ModdedButton button)
        {
            if (PlayerControl.AllPlayerControls.Count <= 1) return false;
            if (PlayerControl.LocalPlayer == null) return false;
            if (PlayerControl.LocalPlayer.Data == null) return false;
            if (!PlayerControl.LocalPlayer.Is(RoleEnum.Engineer)) return false;

            var role = Role.GetRole<Engineer>(PlayerControl.LocalPlayer);


            if (PlayerControl.LocalPlayer.Data.IsDead) return false;
            if (!ShipStatus.Instance) return false;
            var system = ShipStatus.Instance.Systems[SystemTypes.Sabotage].Cast<SabotageSystemType>();
            if (system == null) return false;
            var specials = system.specials.ToArray();
            var dummyActive = system.dummy.IsActive;
            var sabActive = specials.Any(s => s.IsActive);
            if (sabActive & !dummyActive & !role.UsedThisRound)
            {
                return true;
            }

            return false;
        }

        public bool FixAction(ModdedButton button)
        {
            PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"Engineer Fix Start");
            if (!PlayerControl.LocalPlayer.Is(RoleEnum.Engineer)) return true;
            PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"Engineer 2");

            if (!PlayerControl.LocalPlayer.CanMove) return false;
            if (PlayerControl.LocalPlayer.Data.IsDead) return false;
            PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"Engineer 3");

            if (!button.Enabled()) return false;

            if (UsedThisRound) return false;
            PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"Engineer 4");

            var system = ShipStatus.Instance.Systems[SystemTypes.Sabotage].Cast<SabotageSystemType>();
            var specials = system.specials.ToArray();
            var dummyActive = system.dummy.IsActive;
            var sabActive = specials.Any(s => s.IsActive);
            PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"Engineer 5");
            if (!sabActive | dummyActive) return false;
            UsedThisRound = true;
            PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"Engineer 6");

            switch (PlayerControl.GameOptions.MapId)
            {
                case 0:
                case 3:
                    var comms1 = ShipStatus.Instance.Systems[SystemTypes.Comms].Cast<HudOverrideSystemType>();
                    if (comms1.IsActive) return FixComms();
                    var reactor1 = ShipStatus.Instance.Systems[SystemTypes.Reactor].Cast<ReactorSystemType>();
                    if (reactor1.IsActive) return FixReactor(SystemTypes.Reactor);
                    var oxygen1 = ShipStatus.Instance.Systems[SystemTypes.LifeSupp].Cast<LifeSuppSystemType>();
                    if (oxygen1.IsActive) return FixOxygen();
                    var lights1 = ShipStatus.Instance.Systems[SystemTypes.Electrical].Cast<SwitchSystem>();
                    if (lights1.IsActive) return FixLights(lights1);

                    break;
                case 1:
                    var comms2 = ShipStatus.Instance.Systems[SystemTypes.Comms].Cast<HqHudSystemType>();
                    if (comms2.IsActive) return FixMiraComms();
                    var reactor2 = ShipStatus.Instance.Systems[SystemTypes.Reactor].Cast<ReactorSystemType>();
                    if (reactor2.IsActive) return FixReactor(SystemTypes.Reactor);
                    var oxygen2 = ShipStatus.Instance.Systems[SystemTypes.LifeSupp].Cast<LifeSuppSystemType>();
                    if (oxygen2.IsActive) return FixOxygen();
                    var lights2 = ShipStatus.Instance.Systems[SystemTypes.Electrical].Cast<SwitchSystem>();
                    if (lights2.IsActive) return FixLights(lights2);
                    break;

                case 2:
                    var comms3 = ShipStatus.Instance.Systems[SystemTypes.Comms].Cast<HudOverrideSystemType>();
                    if (comms3.IsActive) return FixComms();
                    var seismic = ShipStatus.Instance.Systems[SystemTypes.Laboratory].Cast<ReactorSystemType>();
                    if (seismic.IsActive) return FixReactor(SystemTypes.Laboratory);
                    var lights3 = ShipStatus.Instance.Systems[SystemTypes.Electrical].Cast<SwitchSystem>();
                    if (lights3.IsActive) return FixLights(lights3);
                    break;
                case 4:
                    var comms4 = ShipStatus.Instance.Systems[SystemTypes.Comms].Cast<HudOverrideSystemType>();
                    if (comms4.IsActive) return FixComms();
                    var reactor = ShipStatus.Instance.Systems[SystemTypes.Reactor].Cast<HeliSabotageSystem>();
                    if (reactor.IsActive) return FixAirshipReactor();
                    var lights4 = ShipStatus.Instance.Systems[SystemTypes.Electrical].Cast<SwitchSystem>();
                    if (lights4.IsActive) return FixLights(lights4);
                    break;
            }
            PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"Engineer 7");

            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)CustomRPC.EngineerFix, SendOption.Reliable, -1);
            writer.Write(PlayerControl.LocalPlayer.NetId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"Engineer 8");

            return false;
        }

        private static bool FixComms()
        {
            ShipStatus.Instance.RpcRepairSystem(SystemTypes.Comms, 0);
            return false;
        }

        private static bool FixMiraComms()
        {
            ShipStatus.Instance.RpcRepairSystem(SystemTypes.Comms, 16 | 0);
            ShipStatus.Instance.RpcRepairSystem(SystemTypes.Comms, 16 | 1);
            return false;
        }

        private static bool FixAirshipReactor()
        {
            ShipStatus.Instance.RpcRepairSystem(SystemTypes.Reactor, 16 | 0);
            ShipStatus.Instance.RpcRepairSystem(SystemTypes.Reactor, 16 | 1);
            return false;
        }

        private static bool FixReactor(SystemTypes system)
        {
            ShipStatus.Instance.RpcRepairSystem(system, 16);
            return false;
        }

        private static bool FixOxygen()
        {
            ShipStatus.Instance.RpcRepairSystem(SystemTypes.LifeSupp, 16);
            return false;
        }

        private static bool FixLights(SwitchSystem lights)
        {
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)CustomRPC.FixLights, SendOption.Reliable, -1);
            AmongUsClient.Instance.FinishRpcImmediately(writer);

            lights.ActualSwitches = lights.ExpectedSwitches;

            return false;
        }

    }
}