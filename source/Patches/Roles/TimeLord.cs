using System;
using TownOfUs.CrewmateRoles.TimeLordMod;
using UnityEngine;

namespace TownOfUs.Roles
{
    public class TimeLord : Role
    {
        public TimeLord(PlayerControl player) : base(player)
        {
            Name = "Time Lord";
            ImpostorText = () => "Rewind Time";
            TaskText = () => "Rewind Time!";
            Color = Patches.Colors.TimeLord;
            RoleType = RoleEnum.TimeLord;
            Scale = 1.4f;
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