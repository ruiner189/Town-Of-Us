using Hazel;
using System;
using TownOfUs.CrewmateRoles.TimeLordMod;
using TownOfUs.Patches.Buttons;
using UnityEngine;

namespace TownOfUs.Roles
{
    public class TimeLord : Role
    {

        public ModdedButton RewindButton;
        public TimeLord(PlayerControl player) : base(player)
        {
            Name = "Time Lord";
            ImpostorText = () => "Rewind Time";
            TaskText = () => "Rewind Time!";
            Color = Patches.Colors.TimeLord;
            RoleType = RoleEnum.TimeLord;
            Scale = 1.4f;

            RewindButton = new ModdedButton(player);
            RewindButton.ButtonType = ButtonType.AbilityButton;
            RewindButton.ButtonTarget = ButtonTarget.None;
            RewindButton.SetCooldown((button => { return CustomGameOptions.RewindCooldown; }));
            RewindButton.SetEnabled(RewindEnabled);
            RewindButton.SetAction(RewindAction);
            RewindButton.Sprite = TownOfUs.Rewind;
            RewindButton.RegisterButton();
        }

        public bool RewindAction(ModdedButton button)
        {
            if (!button.Enabled()) return false;
            StartStop.StartRewind(this);
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)CustomRPC.Rewind, SendOption.Reliable, -1);
            writer.Write(PlayerControl.LocalPlayer.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            return false;
        }


        public bool RewindEnabled(ModdedButton button)
        {
            if (!ModdedButton.DefaultEnabled(button)) return false;
            if (RecordRewind.rewinding) return false;
            return true;
        }

        public float GetRewindDuration()
        {
            var value = CustomGameOptions.RewindDuration;
            if (Tier4) return value * 1.25f;
            if (Tier3) return value * 1.125f;
            return value;
        }

        public DateTime StartRewind { get; set; }
        public DateTime FinishRewind { get; set; }

        public float TimeLordRewindTimer(TimeLord timeLord)
        {
            var utcNow = DateTime.UtcNow;


            TimeSpan timespan;
            float num;

            if (RecordRewind.rewinding)
            {
                timespan = utcNow - StartRewind;
                num = timeLord.GetRewindDuration() * 1000f / 3f;
            }
            else
            {
                timespan = utcNow - FinishRewind;
                num = CustomGameOptions.RewindCooldown * 1000f;
            }


            var flag2 = num - (float) timespan.TotalMilliseconds < 0f;
            if (flag2) return 0;
            return (num - (float) timespan.TotalMilliseconds) / 1000f;
        }

        protected override void OnTierUp() {
            if (Tier2) base.OnTierUp();
        }

        public float GetCooldown()
        {
            return RecordRewind.rewinding ? CustomGameOptions.RewindDuration : CustomGameOptions.SheriffKillCd;
        }
    }
}