using Hazel;
using Il2CppSystem.Collections.Generic;
using Reactor;
using System;
using System.Collections;
using System.Linq;
using TownOfUs.CrewmateRoles.InvestigatorMod;
using TownOfUs.CrewmateRoles.SnitchMod;
using TownOfUs.NeutralRoles.ShifterMod;
using TownOfUs.Patches.Buttons;
using TownOfUs.Roles.Modifiers;
using UnityEngine;

namespace TownOfUs.Roles
{
    public class Shifter : Role
    {
        public ModdedButton ShiftButton;
        public PlayerControl Target;
        public Shifter(PlayerControl player) : base(player)
        {
            Name = "Shifter";
            ImpostorText = () => "Shift around different roles";
            TaskText = () => "Steal other people's roles.\nFake Tasks:";
            Color = Patches.Colors.Shifter;
            RoleType = RoleEnum.Shifter;
            Faction = Faction.Neutral;

            ShiftButton = new ModdedButton(player);
            ShiftButton.ButtonType = ButtonType.AbilityButton;
            ShiftButton.ButtonTarget = ButtonTarget.Player;
            ShiftButton.Sprite = TownOfUs.Shift;
            ShiftButton.SetAction(ShiftAction);
            ShiftButton.SetActionEnd(ShiftActionEnd);
            ShiftButton.SetCooldown(button => CustomGameOptions.ShifterCd);
            ShiftButton.SetDuration(button => CustomGameOptions.ShifterDuration);
            ShiftButton.ResetCooldownAtMeetingEnd = CustomGameOptions.ShifterResetMeeting;
            ShiftButton.RegisterButton();
        }

        public bool ShiftAction(ModdedButton button)
        {
            var target = button.ClosestPlayer;
            if (target.IsShielded())
            {
                Medic.RpcInteractWithShield(target, true, true);
                if(!Medic.InteractWithShield(target, true, true))
                    button.ResetCooldown = false;
                return false;
            }

            Target = target;
            if (button.GetDuration() > 0)
                return false;
            RpcShift();
            Shift(this, Target);
            return false;
        }

        public void ShiftActionEnd(ModdedButton button)
        {
            RpcShift();
            Shift(this, Target);
        }

        public static Role CloneRole(Role original, PlayerControl player)
        {
            Role oldRole = Role.GetRole(player);
            RoleDictionary.Remove(player.PlayerId);
            switch (original.RoleType)
            {
                case RoleEnum.Sheriff:
                    return new Sheriff(player);
                case RoleEnum.Jester:
                    return new Jester(player);
                case RoleEnum.Engineer:
                    return new Engineer(player);
                case RoleEnum.Mayor:
                    return new Mayor(player);
                case RoleEnum.Swapper:
                    return new Swapper(player);
                case RoleEnum.Investigator:
                    return new Investigator(player);
                case RoleEnum.TimeLord:
                    return new TimeLord(player);
                case RoleEnum.Medic:
                    Medic medic = new Medic(player);
                    Medic oldMedic = (Medic)original;
                    medic.ShieldedPlayer = oldMedic.ShieldedPlayer;
                    medic.exShielded = oldMedic.exShielded;
                    return medic;
                case RoleEnum.Seer:
                    return new Seer(player);
                case RoleEnum.Executioner:
                    return new Executioner(player);
                case RoleEnum.Spy:
                    return new Spy(player);
                case RoleEnum.Snitch:
                    return new Snitch(player);
                case RoleEnum.Arsonist:
                    Arsonist newArso = new Arsonist(player);
                    Arsonist oldArso = (Arsonist) original;
                    newArso.DousedPlayers = oldArso.DousedPlayers;
                    return newArso;
                case RoleEnum.Crewmate:
                    return new Crewmate(player);
                case RoleEnum.Altruist:
                    return new Altruist(player);
                case RoleEnum.Shifter:
                    return new Shifter(player);
                case RoleEnum.Phantom:
                    return new Phantom(player);
                case RoleEnum.Miner:
                    return new Miner(player);
                case RoleEnum.Swooper:
                    return new Swooper(player);
                case RoleEnum.Morphling:
                    return new Morphling(player);
                case RoleEnum.Camouflager:
                    return new Camouflager(player);
                case RoleEnum.Janitor:
                    return new Janitor(player);
                case RoleEnum.Undertaker:
                    return new Undertaker(player);
                case RoleEnum.Assassin:
                    return new Assassin(player);
                case RoleEnum.Underdog:
                    return new Underdog(player);
                case RoleEnum.Glitch:
                    return new Glitch(player);
                case RoleEnum.Impostor:
                    return new Impostor(player);
                case RoleEnum.None:
                default:
                    RoleDictionary.Add(player.PlayerId, oldRole);
                    return original;
            }
        }

        public static Modifier CloneModifier(Modifier original, PlayerControl player)
        {
            Modifier oldModifier = Modifier.GetModifier(player);
            Modifier.ModifierDictionary.Remove(player.PlayerId);
            switch (original.ModifierType)
            {
                case ModifierEnum.Torch:
                    return new Torch(player);
                case ModifierEnum.Diseased:
                    return new Diseased(player);
                case ModifierEnum.Flash:
                    return new Flash(player);
                case ModifierEnum.Tiebreaker:
                    return new Tiebreaker(player);
                case ModifierEnum.Drunk:
                    return new Drunk(player);
                case ModifierEnum.BigBoi:
                    return new BigBoi(player);
                case ModifierEnum.ButtonBarry:
                    return new ButtonBarry(player);
                case ModifierEnum.Lover:
                    Lover lover = new Lover(player);
                    Lover oldLover = (Lover)original;
                    Lover otherLover = oldLover.OtherLover;
                    lover.OtherLover = otherLover;
                    otherLover.OtherLover = lover;
                    return lover;
                default:
                    Modifier.ModifierDictionary.Add(player.PlayerId, oldModifier);
                    return original;
            }

        }

        public static void Shift(Shifter shifterRole, PlayerControl target)
        {
            var targetsOldRole = Role.GetRole(target);
            var roleType = targetsOldRole.RoleType;

            var shifter = shifterRole.Player;
            List<PlayerTask> tasks1, tasks2;
            List<GameData.TaskInfo> taskinfos1, taskinfos2;

            var swapTasks = true;
            var snitch = false;

            Role shiftersNewRole = shifterRole;
            Role targetsNewRole  = targetsOldRole;

            if (targetsOldRole.Faction == Faction.Crewmates || (targetsOldRole.Faction == Faction.Neutral && targetsOldRole.RoleType != RoleEnum.Glitch))
            {
                shiftersNewRole = CloneRole(targetsOldRole, shifter);

                if (roleType == RoleEnum.Investigator) Footprint.DestroyAll((Investigator) targetsOldRole);
                if (roleType == RoleEnum.Snitch) CompleteTask.Postfix(shifter);

                var targetOldModifier = Modifier.GetModifier(target);
                var shifterOldModifier = Modifier.GetModifier(shifter);

                if (targetOldModifier != null)
                    CloneModifier(targetOldModifier, shifter);
                else
                    Modifier.ModifierDictionary.Remove(shifter.PlayerId);

                if (shifterOldModifier != null)
                    CloneModifier(shifterOldModifier, target);
                else
                    Modifier.ModifierDictionary.Remove(target.PlayerId);


                snitch = roleType == RoleEnum.Snitch;

                foreach (var exeRole in Role.AllRoles.Where(x => x.RoleType == RoleEnum.Executioner))
                {
                    var executioner = (Executioner)exeRole;
                    var exeTarget = executioner.target;
                    if (exeTarget == target)
                    {
                        executioner.target.nameText.color = Color.white;
                        executioner.target = shifter;
                        executioner.RegenTask();
                    }
                }

                if (CustomGameOptions.WhoShifts == ShiftEnum.NonImpostors ||
                    targetsOldRole.RoleType == RoleEnum.Crewmate && CustomGameOptions.WhoShifts == ShiftEnum.RegularCrewmates)
                {
                    targetsNewRole = CloneRole(shifterRole, target);
                }
                else
                {
                    RoleDictionary.Remove(target.PlayerId);
                    targetsNewRole = new Crewmate(target);
                }
            } 
            else
            {
                shifter.MurderPlayer(shifter);
                swapTasks = false;
            }

            if (swapTasks)
            {
                tasks1 = target.myTasks;
                taskinfos1 = target.Data.Tasks;
                tasks2 = shifter.myTasks;
                taskinfos2 = shifter.Data.Tasks;

                shifter.myTasks = tasks1;
                shifter.Data.Tasks = taskinfos1;
                target.myTasks = tasks2;
                target.Data.Tasks = taskinfos2;

                if (target.AmOwner) Coroutines.Start(ShowShift());

                if (snitch)
                {
                    var snitchRole = Role.GetRole<Snitch>(shifter);
                    snitchRole.ImpArrows.DestroyAll();
                    snitchRole.SnitchArrows.DestroyAll();
                    snitchRole.SnitchTargets.Clear();
                    CompleteTask.Postfix(shifter);
                    if (target.AmOwner)
                        foreach (var player in PlayerControl.AllPlayerControls)
                            player.nameText.color = Color.white;
                }
            }

            if (shifter.AmOwner || target.AmOwner)
            {
                Lights.SetLights();
                ModdedButton.GetAllModdedButtonsFromRole(targetsNewRole).ForEach(button => button.ResetCooldownValue());
                ModdedButton.GetAllModdedButtonsFromRole(shiftersNewRole).ForEach(button => button.ResetCooldownValue());
            }

            targetsNewRole.RegenTask();
            shiftersNewRole.RegenTask();
            
        }
    

        public void RpcShift()
        {

            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)CustomRPC.Shift, SendOption.Reliable, -1);
            writer.Write(Player.PlayerId);
            writer.Write(Target.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }


        public static IEnumerator ShowShift()
        {
            var wait = new WaitForSeconds(0.83333336f);
            var hud = DestroyableSingleton<HudManager>.Instance;
            var overlay = hud.KillOverlay;
            var transform = overlay.flameParent.transform;
            var flame = transform.GetChild(0).gameObject;
            var renderer = flame.GetComponent<SpriteRenderer>();

            renderer.sprite = TownOfUs.ShiftKill;
            var background = overlay.background;
            overlay.flameParent.SetActive(true);
            yield return new WaitForLerp(0.16666667f,
                delegate (float t) { overlay.flameParent.transform.localScale = new Vector3(1f, t, 1f); });
            yield return new WaitForSeconds(1f);
            yield return new WaitForLerp(0.16666667f,
                delegate (float t) { overlay.flameParent.transform.localScale = new Vector3(1f, 1f - t, 1f); });
            overlay.flameParent.SetActive(false);
            overlay.showAll = null;
            renderer.sprite = TownOfUs.NormalKill;
        }


        public void Loses()
        {
            //Player.Data.Role.IsImpostor = true;
        }
    }

    public enum ShiftEnum
    {
        NonImpostors,
        RegularCrewmates,
        Nobody
    }
}