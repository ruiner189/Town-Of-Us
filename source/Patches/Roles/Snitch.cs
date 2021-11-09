using System.Collections.Generic;
using TownOfUs.ImpostorRoles.CamouflageMod;
using UnityEngine;

namespace TownOfUs.Roles
{
    public class Snitch : Role
    {
        public List<ArrowBehaviour> ImpArrows = new List<ArrowBehaviour>();

        public List<ArrowBehaviour> SnitchArrows = new List<ArrowBehaviour>();

        public List<PlayerControl> SnitchTargets = new List<PlayerControl>();

        public Snitch(PlayerControl player) : base(player)
        {
            Name = "Snitch";
            ImpostorText = () => "Complete all your tasks to discover the Impostors";
            TaskText = () =>
                TasksDone
                    ? "Find the arrows pointing to the Impostors!"
                    : "Complete all your tasks to discover the Impostors!";
            Color = Patches.Colors.Snitch;
            Hidden = !CustomGameOptions.SnitchOnLaunch;
            RoleType = RoleEnum.Snitch;
        }

        public bool OneTaskLeft => TasksLeft <= 1;
        public bool TasksDone => TasksLeft <= 0;

        protected override void OnTierUp() { return; }

        internal override bool Criteria()
        {
            return OneTaskLeft && PlayerControl.LocalPlayer.Data.IsImpostor ||
                   base.Criteria();
        }

        internal override bool SelfCriteria()
        {
            if (Local)
            {
                if (CustomGameOptions.SnitchOnLaunch) return base.SelfCriteria();
                return OneTaskLeft;
            }
            return base.SelfCriteria();
        }

        internal override bool RoleCriteria()
        {
            var localPlayer = PlayerControl.LocalPlayer;
            if (localPlayer.Data.IsImpostor)
            {
                return OneTaskLeft;
            }
            else if (Role.GetRole(localPlayer).Faction == Faction.Neutral)
            {
                return OneTaskLeft && CustomGameOptions.SnitchSeesNeutrals;
            }
            return false;
        }

        
        protected override string NameText(bool revealTasks, bool revealRole, bool revealModifier, bool revealLover, PlayerVoteArea player = null)
        {
            if (CamouflageUnCamouflage.IsCamoed && player == null) return "";
            if (PlayerControl.LocalPlayer.Data.IsDead) return base.NameText(revealTasks, revealRole, revealModifier, revealLover, player);
            if (OneTaskLeft || !Hidden) return base.NameText(revealTasks, revealRole, revealModifier, revealLover, player);
            
            // Shows snitch as crewmate
            var PlayerName = base.NameText(revealTasks, false, revealModifier, revealLover, player);

            Player.nameText.color = Color.white;
            if (player != null) player.NameText.color = Color.white;
            if (player != null && (MeetingHud.Instance.state == MeetingHud.VoteStates.Proceeding ||
                                   MeetingHud.Instance.state == MeetingHud.VoteStates.Results)) return PlayerName;
            if (!CustomGameOptions.RoleUnderName && player == null) return PlayerName;
            Player.nameText.transform.localPosition = new Vector3(
                0f,
                Player.Data.HatId == 0U ? 1.5f : 2.0f,
                -0.5f
            );
            return PlayerName + "\n" + "Crewmate";
        }
    }
}