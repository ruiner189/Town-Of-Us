using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Hazel;
using Reactor;
using Reactor.Extensions;
using TownOfUs.CrewmateRoles.AltruistMod;
using TownOfUs.CrewmateRoles.MedicMod;
using TownOfUs.CrewmateRoles.SwapperMod;
using TownOfUs.CrewmateRoles.TimeLordMod;
using TownOfUs.CustomOption;
using TownOfUs.Extensions;
using TownOfUs.ImpostorRoles.AssassinMod;
using TownOfUs.ImpostorRoles.MinerMod;
using TownOfUs.NeutralRoles.ExecutionerMod;
using TownOfUs.NeutralRoles.PhantomMod;
using TownOfUs.Roles;
using TownOfUs.Roles.Modifiers;
using UnhollowerBaseLib;
using UnityEngine;
using Coroutine = TownOfUs.ImpostorRoles.JanitorMod.Coroutine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random; //using Il2CppSystem;

namespace TownOfUs
{
    public static class RpcHandling
    {
        private static readonly List<(Type, Faction, CustomRPC, int)> RolePool = new List<(Type, Faction, CustomRPC, int)>();
        private static readonly List<(Type, CustomRPC, Faction[], int)> ModifierPool = new List<(Type, CustomRPC, Faction[], int)>();
        private static bool PhantomOn;
        private static int NeutralsLeft;

        internal static bool Check(int probability)
        {
            if (probability == 0) return false;
            if (probability == 100) return true;
            var num = Random.RandomRangeInt(1, 101);
            return num <= probability;
        }

        private static void SortRoles(List<(Type, CustomRPC, int)> roles, int max = int.MaxValue)
        {
            roles.Shuffle();
            roles.Sort((a, b) =>
            {
                var a_ = a.Item3 == 100 ? 0 : 100;
                var b_ = b.Item3 == 100 ? 0 : 100;
                return a_.CompareTo(b_);
            });
            if (roles.Count > max)
                while (roles.Count > max)
                    roles.RemoveAt(roles.Count - 1);
        }

        private static void GenEachRole(List<PlayerControl> impostors)
        {
            var crewmates = Utils.GetCrewmates(impostors);
            PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"impostor count: {impostors.Count}");
            PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"crewmate count: {crewmates.Count}");
            crewmates.Shuffle();
            impostors.Shuffle();
            PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"Lists are now shuffled!");

            if (Check(CustomGameOptions.VanillaGame))
            {
                RolePool.Clear();
                ModifierPool.Clear();
                PhantomOn = false;
            }

            PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"Selecting crewmate and neutral roles!");

            NeutralsLeft = CustomGameOptions.MaxNeutralRoles;
            var executionerList = new List<PlayerControl>();

            foreach (var player in crewmates)
            {
                GenCrewmateOrNeutralRole(player, executionerList);
            }

            PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"Selecting impostor roles!");

            foreach (var player in impostors)
            {
                GenImpostorRole(player);
            }


            if (executionerList.Count > 0)
            {
                var targets = Utils.GetCrewmates(impostors).Where(
                    crewmate => Role.GetRole(crewmate)?.Faction == Faction.Crewmates
                ).ToList();
                foreach (var executioner in executionerList)
                {
                    GenExecutionerRole(executioner, targets);
                }
            }

            GenModifiers();

            if (PhantomOn)
            {
                var vanilla = PlayerControl.AllPlayerControls.ToArray().Where(x => x.Is(RoleEnum.Crewmate)).ToList();
                var toChooseFrom = crewmates.Count > 0
                    ? crewmates
                    : PlayerControl.AllPlayerControls.ToArray().Where(x => x.Is(Faction.Crewmates) && !x.IsLover())
                        .ToList();
                var rand = Random.RandomRangeInt(0, toChooseFrom.Count);
                var pc = toChooseFrom[rand];

                SetPhantom.WillBePhantom = pc;

                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.SetPhantom, SendOption.Reliable, -1);
                writer.Write(pc.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
            }
            else
            {
                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.SetPhantom, SendOption.Reliable, -1);
                writer.Write(byte.MaxValue);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
            }
            PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"Made it to the end!");
        }

        private static void GenCrewmateOrNeutralRole(PlayerControl player, List<PlayerControl> executionerList = null)
        {
            var crewmate = (typeof(Crewmate), Faction.Crewmates, CustomRPC.SetCrewmate, 0);
            (Type, Faction, CustomRPC, int) items = crewmate;
            if (RolePool.Count > 0)
            {
                items = RolePool.FirstOrDefault(role =>
                {
                    if (role.Item2 == Faction.Crewmates) return true;
                    if (role.Item2 == Faction.Neutral)
                    {
                        if (NeutralsLeft == 0) return false;
                        if (executionerList == null && role.Item3 == CustomRPC.SetExecutioner) return false;
                        NeutralsLeft--;
                        return true;
                    }
                    return false;
                });
                if (items.Item1 == null) items = crewmate;
                else
                {
                    RolePool.Remove(items);
                }
            }
            else items = crewmate;

            if (items.Item3 == CustomRPC.SetExecutioner)
            {
                executionerList.Add(player);
                return;
            }
            Role.Gen<Role>(items.Item1, player, items.Item3);
        }

        private static void GenExecutionerRole(PlayerControl player, List<PlayerControl> targets)
        {
            if (targets.Count > 0)
            {
                var exec = Role.Gen<Executioner>(
                    typeof(Executioner),
                    player,
                    CustomRPC.SetExecutioner
                );
                var target = exec.target = targets[Random.RandomRangeInt(0, targets.Count)];

                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.SetTarget, SendOption.Reliable, -1);
                writer.Write(player.PlayerId);
                writer.Write(target.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                targets.Remove(target);
            }
            else
            {
                GenCrewmateOrNeutralRole(player);
            }
        }

        private static void GenImpostorRole(PlayerControl player)
        {
            var impostor = (typeof(Impostor), Faction.Impostors, CustomRPC.SetImpostor, 0);
            (Type, Faction, CustomRPC, int) items = impostor;
            if (RolePool.Count > 0)
            {
                items = RolePool.FirstOrDefault(role =>
                {
                    if (role.Item2 == Faction.Impostors) return true;
                    return false;
                });
                if (items.Item1 == null) items = impostor;
                else
                {
                    RolePool.Remove(items);
                }
            }
            else items = impostor;

            Role.Gen<Role>(items.Item1, player, items.Item3);
        }

        private static void GenModifiers()
        {

            var canHaveModifier = PlayerControl.AllPlayerControls.ToArray().ToList();
            canHaveModifier.Shuffle();

            foreach (var (type, rpc, factions, _) in ModifierPool)
            {
                if (canHaveModifier.Count == 0) break;

                if (rpc == CustomRPC.SetCouple)
                {
                    if (canHaveModifier.Count == 1) continue;
                    Lover.Gen(canHaveModifier);
                }
                else
                {
                    var player = canHaveModifier.First(p => {
                        var role = Role.GetRole(p);
                        if (role == null) return false;
                        if (factions.Contains(role.Faction)) return true;
                        return false;
                    });
                    if (player != null)
                    {
                        Role.Gen<Modifier>(type, player, rpc);
                        canHaveModifier.Remove(player);
                    }
                }
            }
        }


        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.HandleRpc))]
        public static class HandleRpc
        {
            public static void Postfix([HarmonyArgument(0)] byte callId, [HarmonyArgument(1)] MessageReader reader)
            {
                //if (callId >= 43) //System.Console.WriteLine("Received " + callId);
                byte readByte, readByte1, readByte2;
                sbyte readSByte, readSByte2;
                switch ((CustomRPC)callId)
                {
                    case CustomRPC.SetMayor:
                        readByte = reader.ReadByte();
                        new Mayor(Utils.PlayerById(readByte));
                        break;

                    case CustomRPC.SetJester:
                        readByte = reader.ReadByte();
                        new Jester(Utils.PlayerById(readByte));
                        break;

                    case CustomRPC.SetSheriff:
                        readByte = reader.ReadByte();
                        new Sheriff(Utils.PlayerById(readByte));
                        break;

                    case CustomRPC.SetEngineer:
                        readByte = reader.ReadByte();
                        new Engineer(Utils.PlayerById(readByte));
                        break;


                    case CustomRPC.SetJanitor:
                        new Janitor(Utils.PlayerById(reader.ReadByte()));

                        break;

                    case CustomRPC.SetSwapper:
                        readByte = reader.ReadByte();
                        new Swapper(Utils.PlayerById(readByte));
                        break;

                    case CustomRPC.SetShifter:
                        readByte = reader.ReadByte();
                        new Shifter(Utils.PlayerById(readByte));
                        break;

                    case CustomRPC.SetInvestigator:
                        readByte = reader.ReadByte();
                        new Investigator(Utils.PlayerById(readByte));
                        break;

                    case CustomRPC.SetTimeLord:
                        readByte = reader.ReadByte();
                        new TimeLord(Utils.PlayerById(readByte));
                        break;

                    case CustomRPC.SetTorch:
                        readByte = reader.ReadByte();
                        new Torch(Utils.PlayerById(readByte));
                        break;
                    case CustomRPC.SetDiseased:
                        readByte = reader.ReadByte();
                        new Diseased(Utils.PlayerById(readByte));
                        break;
                    case CustomRPC.SetFlash:
                        readByte = reader.ReadByte();
                        new Flash(Utils.PlayerById(readByte));
                        break;

                    case CustomRPC.SetMedic:
                        readByte = reader.ReadByte();
                        new Medic(Utils.PlayerById(readByte));
                        break;
                    case CustomRPC.SetMorphling:
                        readByte = reader.ReadByte();
                        new Morphling(Utils.PlayerById(readByte));
                        break;

                    case CustomRPC.LoveWin:
                        var winnerlover = Utils.PlayerById(reader.ReadByte());
                        Modifier.GetModifier<Lover>(winnerlover).Win();
                        break;


                    case CustomRPC.JesterLose:
                        foreach (var role in Role.AllRoles)
                            if (role.RoleType == RoleEnum.Jester)
                                ((Jester)role).Loses();

                        break;
                    case CustomRPC.PhantomLose:
                        foreach (var role in Role.AllRoles)
                            if (role.RoleType == RoleEnum.Phantom)
                                ((Phantom)role).Loses();

                        break;


                    case CustomRPC.GlitchLose:
                        foreach (var role in Role.AllRoles)
                            if (role.RoleType == RoleEnum.Glitch)
                                ((Glitch)role).Loses();

                        break;

                    case CustomRPC.ShifterLose:
                        foreach (var role in Role.AllRoles)
                            if (role.RoleType == RoleEnum.Shifter)
                                ((Shifter)role).Loses();

                        break;

                    case CustomRPC.ExecutionerLose:
                        foreach (var role in Role.AllRoles)
                            if (role.RoleType == RoleEnum.Executioner)
                                ((Executioner)role).Loses();

                        break;

                    case CustomRPC.NobodyWins:
                        Role.NobodyWinsFunc();
                        break;

                    case CustomRPC.SetCouple:
                        var id = reader.ReadByte();
                        var id2 = reader.ReadByte();
                        var lover1 = Utils.PlayerById(id);
                        var lover2 = Utils.PlayerById(id2);

                        var modifierLover1 = new Lover(lover1);
                        var modifierLover2 = new Lover(lover2);

                        modifierLover1.OtherLover = modifierLover2;
                        modifierLover2.OtherLover = modifierLover1;

                        break;

                    case CustomRPC.Start:
                        /*
                        EngineerMod.PerformKill.UsedThisRound = false;
                        EngineerMod.PerformKill.SabotageTime = DateTime.UtcNow.AddSeconds(-100);
                        */
                        Utils.ShowDeadBodies = false;
                        Murder.KilledPlayers.Clear();
                        Role.NobodyWins = false;
                        RecordRewind.points.Clear();
                        break;

                    case CustomRPC.JanitorClean:
                        readByte1 = reader.ReadByte();
                        var janitorPlayer = Utils.PlayerById(readByte1);
                        var janitorRole = Role.GetRole<Janitor>(janitorPlayer);
                        readByte = reader.ReadByte();
                        var deadBodies = Object.FindObjectsOfType<DeadBody>();
                        foreach (var body in deadBodies)
                            if (body.ParentId == readByte)
                                Coroutines.Start(Coroutine.CleanCoroutine(body, janitorRole));

                        break;
                    case CustomRPC.EngineerFix:
                        var engineer = Utils.PlayerById(reader.ReadByte());
                        Role.GetRole<Engineer>(engineer).UsedThisRound = true;
                        break;


                    case CustomRPC.FixLights:
                        var lights = ShipStatus.Instance.Systems[SystemTypes.Electrical].Cast<SwitchSystem>();
                        lights.ActualSwitches = lights.ExpectedSwitches;
                        break;

                    case CustomRPC.SetExtraVotes:

                        var mayor = Utils.PlayerById(reader.ReadByte());
                        var mayorRole = Role.GetRole<Mayor>(mayor);
                        mayorRole.ExtraVotes = reader.ReadBytesAndSize().ToList();
                        if (!mayor.Is(RoleEnum.Mayor)) mayorRole.VoteBank -= mayorRole.ExtraVotes.Count;

                        break;

                    case CustomRPC.SetSwaps:
                        readSByte = reader.ReadSByte();
                        SwapVotes.Swap1 =
                            MeetingHud.Instance.playerStates.FirstOrDefault(x => x.TargetPlayerId == readSByte);
                        readSByte2 = reader.ReadSByte();
                        SwapVotes.Swap2 =
                            MeetingHud.Instance.playerStates.FirstOrDefault(x => x.TargetPlayerId == readSByte2);
                        PluginSingleton<TownOfUs>.Instance.Log.LogMessage("Bytes received - " + readSByte + " - " +
                                                                          readSByte2);
                        break;

                    case CustomRPC.Shift:
                        readByte1 = reader.ReadByte();
                        readByte2 = reader.ReadByte();
                        var shifter = Utils.PlayerById(readByte1);
                        var other = Utils.PlayerById(readByte2);
                        Shifter.Shift(Role.GetRole<Shifter>(shifter), other);
                        break;
                    case CustomRPC.Rewind:
                        readByte = reader.ReadByte();
                        var TimeLordPlayer = Utils.PlayerById(readByte);
                        var TimeLordRole = Role.GetRole<TimeLord>(TimeLordPlayer);
                        StartStop.StartRewind(TimeLordRole);
                        break;
                    case CustomRPC.Protect:
                        readByte1 = reader.ReadByte();
                        readByte2 = reader.ReadByte();

                        var medic = Utils.PlayerById(readByte1);
                        var shield = Utils.PlayerById(readByte2);
                        Role.GetRole<Medic>(medic).ShieldedPlayer = shield;
                        Role.GetRole<Medic>(medic).UsedAbility = true;
                        break;
                    case CustomRPC.RewindRevive:
                        readByte = reader.ReadByte();
                        RecordRewind.ReviveBody(Utils.PlayerById(readByte));
                        break;
                    case CustomRPC.AttemptSound:
                        readByte1 = reader.ReadByte();
                        shield = Utils.PlayerById(readByte1);
                        bool notifyMedic = reader.ReadBoolean();
                        bool tryToBreakShield = reader.ReadBoolean();

                        Medic.InteractWithShield(shield, notifyMedic, tryToBreakShield);
                        break;
                    case CustomRPC.SetGlitch:
                        var GlitchId = reader.ReadByte();
                        var GlitchPlayer = Utils.PlayerById(GlitchId);
                        new Glitch(GlitchPlayer);
                        break;
                    case CustomRPC.BypassKill:
                        var killer = Utils.PlayerById(reader.ReadByte());
                        var target = Utils.PlayerById(reader.ReadByte());

                        Utils.MurderPlayer(killer, target);
                        break;
                    case CustomRPC.AssassinKill:
                        var toDie = Utils.PlayerById(reader.ReadByte());
                        AssassinKill.MurderPlayer(toDie);
                        break;
                    case CustomRPC.SetMimic:
                        var glitchPlayer = Utils.PlayerById(reader.ReadByte());
                        var mimicPlayer = Utils.PlayerById(reader.ReadByte());
                        var glitchRole = Role.GetRole<Glitch>(glitchPlayer);
                        if (glitchPlayer == mimicPlayer)
                            glitchRole.IsUsingMimic = false;
                        else
                            glitchRole.IsUsingMimic = true;
                        Utils.Morph(glitchPlayer, mimicPlayer.Data.DefaultOutfit);
                        break;

                    case CustomRPC.RpcResetAnim:
                        var animPlayer = Utils.PlayerById(reader.ReadByte());
                        var theGlitchRole = Role.GetRole<Glitch>(animPlayer);
                        theGlitchRole.IsUsingMimic = false;
                        Utils.Morph(theGlitchRole.Player, theGlitchRole.Player.Data.DefaultOutfit);
                        break;
                    case CustomRPC.GlitchWin:
                        var theGlitch = Role.AllRoles.FirstOrDefault(x => x.RoleType == RoleEnum.Glitch);
                        ((Glitch)theGlitch)?.Wins();
                        break;
                    case CustomRPC.SetHacked:
                        var glitchPlayer2 = Utils.PlayerById(reader.ReadByte());
                        var hackPlayer = Utils.PlayerById(reader.ReadByte());
                        Utils.Hack(glitchPlayer2, hackPlayer);
                        break;
                    case CustomRPC.RemoveHacked:
                        var glitchPlayer3 = Utils.PlayerById(reader.ReadByte());
                        var hackPlayer2 = Utils.PlayerById(reader.ReadByte());
                        Utils.RemoveHack(glitchPlayer3, hackPlayer2);
                        break;
                    case CustomRPC.Investigate:
                        var seer = Utils.PlayerById(reader.ReadByte());
                        var otherPlayer = Utils.PlayerById(reader.ReadByte());
                        Role.GetRole<Seer>(seer).Investigated.Add(otherPlayer.PlayerId);
                        Role.GetRole<Seer>(seer).LastInvestigated = DateTime.UtcNow;
                        break;
                    case CustomRPC.SetSeer:
                        new Seer(Utils.PlayerById(reader.ReadByte()));
                        break;
                    case CustomRPC.Morph:
                        var morphling = Utils.PlayerById(reader.ReadByte());
                        var morphTarget = Utils.PlayerById(reader.ReadByte());
                        Utils.Morph(morphling, morphTarget.Data.DefaultOutfit);
                        break;
                    case CustomRPC.SetExecutioner:
                        new Executioner(Utils.PlayerById(reader.ReadByte()));
                        break;
                    case CustomRPC.SetTarget:
                        var executioner = Utils.PlayerById(reader.ReadByte());
                        var exeTarget = Utils.PlayerById(reader.ReadByte());
                        var exeRole = Role.GetRole<Executioner>(executioner);
                        exeRole.target = exeTarget;
                        break;
                    case CustomRPC.SetCamouflager:
                        new Camouflager(Utils.PlayerById(reader.ReadByte()));
                        break;
                    case CustomRPC.Camouflage:
                        Utils.Camouflage();
                        break;
                    case CustomRPC.UnCamouflage:
                        Utils.UnCamouflage();
                        break;
                    case CustomRPC.SetSpy:
                        new Spy(Utils.PlayerById(reader.ReadByte()));
                        break;
                    case CustomRPC.ExecutionerToJester:
                        TargetColor.ExeToJes(Utils.PlayerById(reader.ReadByte()));
                        break;
                    case CustomRPC.SetSnitch:
                        new Snitch(Utils.PlayerById(reader.ReadByte()));
                        break;
                    case CustomRPC.SetMiner:
                        new Miner(Utils.PlayerById(reader.ReadByte()));
                        break;
                    case CustomRPC.Mine:
                        var ventId = reader.ReadInt32();
                        var miner = Utils.PlayerById(reader.ReadByte());
                        var minerRole = Role.GetRole<Miner>(miner);
                        var pos = reader.ReadVector2();
                        var zAxis = reader.ReadSingle();
                        Miner.SpawnVent(ventId, minerRole, pos, zAxis);
                        break;
                    case CustomRPC.SetSwooper:
                        new Swooper(Utils.PlayerById(reader.ReadByte()));
                        break;
                    case CustomRPC.Swoop:
                        var swooper = Utils.PlayerById(reader.ReadByte());
                        var swooperRole = Role.GetRole<Swooper>(swooper);
                        var invisible = reader.ReadBoolean();
                        if (invisible)
                            swooperRole.Swoop();
                        else
                            swooperRole.UnSwoop();
                        break;
                    case CustomRPC.SetTiebreaker:
                        new Tiebreaker(Utils.PlayerById(reader.ReadByte()));
                        break;
                    case CustomRPC.SetDrunk:
                        new Drunk(Utils.PlayerById(reader.ReadByte()));
                        break;
                    case CustomRPC.SetArsonist:
                        new Arsonist(Utils.PlayerById(reader.ReadByte()));
                        break;
                    case CustomRPC.Douse:
                        var arsonist = Utils.PlayerById(reader.ReadByte());
                        var douseTarget = Utils.PlayerById(reader.ReadByte());
                        var arsonistRole = Role.GetRole<Arsonist>(arsonist);
                        arsonistRole.DousedPlayers.Add(douseTarget.PlayerId);
                        break;
                    case CustomRPC.Ignite:
                        var theArsonist = Utils.PlayerById(reader.ReadByte());
                        var theArsonistRole = Role.GetRole<Arsonist>(theArsonist);
                        Arsonist.Ignite(theArsonistRole);
                        break;

                    case CustomRPC.ArsonistWin:
                        var theArsonistTheRole = Role.AllRoles.FirstOrDefault(x => x.RoleType == RoleEnum.Arsonist);
                        ((Arsonist)theArsonistTheRole)?.Wins();
                        break;
                    case CustomRPC.ArsonistLose:
                        foreach (var role in Role.AllRoles)
                            if (role.RoleType == RoleEnum.Arsonist)
                                ((Arsonist)role).Loses();

                        break;
                    case CustomRPC.SetImpostor:
                        new Impostor(Utils.PlayerById(reader.ReadByte()));
                        break;
                    case CustomRPC.SetCrewmate:
                        new Crewmate(Utils.PlayerById(reader.ReadByte()));
                        break;
                    case CustomRPC.SyncCustomSettings:
                        Rpc.ReceiveRpc(reader);
                        break;
                    case CustomRPC.SetAltruist:
                        new Altruist(Utils.PlayerById(reader.ReadByte()));
                        break;
                    case CustomRPC.SetBigBoi:
                        new BigBoi(Utils.PlayerById(reader.ReadByte()));
                        break;
                    case CustomRPC.AltruistRevive:
                        readByte1 = reader.ReadByte();
                        var altruistPlayer = Utils.PlayerById(readByte1);
                        var altruistRole = Role.GetRole<Altruist>(altruistPlayer);
                        readByte = reader.ReadByte();
                        var theDeadBodies = Object.FindObjectsOfType<DeadBody>();
                        foreach (var body in theDeadBodies)
                            if (body.ParentId == readByte)
                            {
                                if (body.ParentId == PlayerControl.LocalPlayer.PlayerId)
                                    Coroutines.Start(Utils.FlashCoroutine(altruistRole.Color,
                                        CustomGameOptions.ReviveDuration, 0.5f));

                                Coroutines.Start(
                                    global::TownOfUs.CrewmateRoles.AltruistMod.Coroutine.AltruistRevive(body,
                                        altruistRole));
                            }

                        break;
                    case CustomRPC.FixAnimation:
                        var player = Utils.PlayerById(reader.ReadByte());
                        player.MyPhysics.ResetMoveState();
                        player.Collider.enabled = true;
                        player.moveable = true;
                        player.NetTransform.enabled = true;
                        break;
                    case CustomRPC.SetButtonBarry:
                        new ButtonBarry(Utils.PlayerById(reader.ReadByte()));
                        break;
                    case CustomRPC.BarryButton:
                        var buttonBarry = Utils.PlayerById(reader.ReadByte());
                        if (AmongUsClient.Instance.AmHost)
                        {
                            MeetingRoomManager.Instance.reporter = buttonBarry;
                            MeetingRoomManager.Instance.target = null;
                            AmongUsClient.Instance.DisconnectHandlers.AddUnique(MeetingRoomManager.Instance
                                .Cast<IDisconnectHandler>());
                            if (ShipStatus.Instance.CheckTaskCompletion()) return;

                            DestroyableSingleton<HudManager>.Instance.OpenMeetingRoom(buttonBarry);
                            buttonBarry.RpcStartMeeting(null);
                        }

                        break;

                    case CustomRPC.SetUndertaker:
                        new Undertaker(Utils.PlayerById(reader.ReadByte()));
                        break;
                    case CustomRPC.Drag:
                        readByte1 = reader.ReadByte();
                        var dienerPlayer = Utils.PlayerById(readByte1);
                        var dienerRole = Role.GetRole<Undertaker>(dienerPlayer);
                        readByte = reader.ReadByte();
                        var dienerBodies = Object.FindObjectsOfType<DeadBody>();
                        foreach (var body in dienerBodies)
                            if (body.ParentId == readByte)
                                dienerRole.CurrentlyDragging = body;
                        break;
                    case CustomRPC.Drop:
                        readByte1 = reader.ReadByte();
                        var v2 = reader.ReadVector2();
                        var v2z = reader.ReadSingle();
                        var dienerPlayer2 = Utils.PlayerById(readByte1);
                        var dienerRole2 = Role.GetRole<Undertaker>(dienerPlayer2);
                        var body2 = dienerRole2.CurrentlyDragging;
                        dienerRole2.CurrentlyDragging = null;
                        body2.transform.position = v2;
                        break;
                    case CustomRPC.SetAssassin:
                        new Assassin(Utils.PlayerById(reader.ReadByte()));
                        break;
                    case CustomRPC.SetUnderdog:
                        new Underdog(Utils.PlayerById(reader.ReadByte()));
                        break;
                    case CustomRPC.SetPhantom:
                        readByte = reader.ReadByte();
                        SetPhantom.WillBePhantom = readByte == byte.MaxValue ? null : Utils.PlayerById(readByte);
                        break;
                    case CustomRPC.PhantomDied:
                        var phantom = SetPhantom.WillBePhantom;
                        Role.RoleDictionary.Remove(phantom.PlayerId);
                        var phantomRole = new Phantom(phantom);
                        phantomRole.RegenTask();
                        phantom.gameObject.layer = LayerMask.NameToLayer("Players");
                        SetPhantom.RemoveTasks(phantom);
                        SetPhantom.AddCollider(phantomRole);
                        PlayerControl.LocalPlayer.MyPhysics.ResetMoveState();
                        System.Console.WriteLine("Become Phantom - Users");
                        break;
                    case CustomRPC.CatchPhantom:
                        var phantomPlayer = Utils.PlayerById(reader.ReadByte());
                        Role.GetRole<Phantom>(phantomPlayer).Caught = true;
                        break;
                    case CustomRPC.PhantomWin:
                        Role.GetRole<Phantom>(Utils.PlayerById(reader.ReadByte())).CompletedTasks = true;
                        break;

                    case CustomRPC.AddMayorVoteBank:
                        Role.GetRole<Mayor>(Utils.PlayerById(reader.ReadByte())).VoteBank += reader.ReadInt32();
                        break;
                }
            }
        }


        [HarmonyPatch(typeof(RoleManager), nameof(RoleManager.SelectRoles))]
        public static class SelectRoles
        {
            public static void Postfix(RoleManager __instance)
            {
                ClearData();
                GenerateRolePool();
                GenerateModifiers();
                var impostors = new List<PlayerControl>();
                foreach (var player in PlayerControl.AllPlayerControls)
                {
                    if (player.Data == null) continue;
                    if (player.Data.Role == null) continue;
                    if (player.Data.Role.IsImpostor) impostors.Add(player);
                }
                GenEachRole(impostors);
            }

            private static void ClearData()
            {
                Utils.ShowDeadBodies = false;
                Role.NobodyWins = false;
                RolePool.Clear();
                ModifierPool.Clear();
                RecordRewind.points.Clear();
                Murder.KilledPlayers.Clear();
            }

            private static void GenerateRolePool()
            {
                RolePool.Clear();
                // Crew Roles
                for (int i = 0; i < CustomGameOptions.MayorMax; i++)
                    if (Check(CustomGameOptions.MayorOn))
                        RolePool.Add((typeof(Mayor), Faction.Crewmates, CustomRPC.SetMayor, CustomGameOptions.MayorOn));

                for (int i = 0; i < CustomGameOptions.SheriffMax; i++)
                    if (Check(CustomGameOptions.SheriffOn))
                        RolePool.Add((typeof(Sheriff), Faction.Crewmates, CustomRPC.SetSheriff, CustomGameOptions.SheriffOn));

                for (int i = 0; i < CustomGameOptions.EngineerMax; i++)
                    if (Check(CustomGameOptions.EngineerOn))
                        RolePool.Add((typeof(Engineer), Faction.Crewmates, CustomRPC.SetEngineer, CustomGameOptions.EngineerOn));

                for (int i = 0; i < CustomGameOptions.SwapperMax; i++)
                    if (Check(CustomGameOptions.SwapperOn))
                        RolePool.Add((typeof(Swapper), Faction.Crewmates, CustomRPC.SetSwapper, CustomGameOptions.SwapperOn));

                for (int i = 0; i < CustomGameOptions.InvestigatorMax; i++)
                    if (Check(CustomGameOptions.InvestigatorOn))
                        RolePool.Add((typeof(Investigator), Faction.Crewmates, CustomRPC.SetInvestigator, CustomGameOptions.InvestigatorOn));

                for (int i = 0; i < CustomGameOptions.TimeLordMax; i++)
                    if (Check(CustomGameOptions.TimeLordOn))
                        RolePool.Add((typeof(TimeLord), Faction.Crewmates, CustomRPC.SetTimeLord, CustomGameOptions.TimeLordOn));

                for (int i = 0; i < CustomGameOptions.MedicMax; i++)
                    if (Check(CustomGameOptions.MedicOn))
                        RolePool.Add((typeof(Medic), Faction.Crewmates, CustomRPC.SetMedic, CustomGameOptions.MedicOn));

                for (int i = 0; i < CustomGameOptions.SeerMax; i++)
                    if (Check(CustomGameOptions.SeerOn))
                        RolePool.Add((typeof(Seer), Faction.Crewmates, CustomRPC.SetSeer, CustomGameOptions.SeerOn));

                for (int i = 0; i < CustomGameOptions.SpyMax; i++)
                    if (Check(CustomGameOptions.SpyOn))
                        RolePool.Add((typeof(Spy), Faction.Crewmates, CustomRPC.SetSpy, CustomGameOptions.SpyOn));

                for (int i = 0; i < CustomGameOptions.SnitchMax; i++)
                    if (Check(CustomGameOptions.SnitchOn))
                        RolePool.Add((typeof(Snitch), Faction.Crewmates, CustomRPC.SetSnitch, CustomGameOptions.SnitchOn));

                for (int i = 0; i < CustomGameOptions.AltruistMax; i++)
                    if (Check(CustomGameOptions.AltruistOn))
                        RolePool.Add((typeof(Altruist), Faction.Crewmates, CustomRPC.SetAltruist, CustomGameOptions.AltruistOn));

                // Impostor Roles
                for (int i = 0; i < CustomGameOptions.UndertakerMax; i++)
                    if (Check(CustomGameOptions.UndertakerOn))
                        RolePool.Add((typeof(Undertaker), Faction.Impostors, CustomRPC.SetUndertaker, CustomGameOptions.UndertakerOn));

                for (int i = 0; i < CustomGameOptions.AssassinMax; i++)
                    if (Check(CustomGameOptions.AssassinOn))
                        RolePool.Add((typeof(Assassin), Faction.Impostors, CustomRPC.SetAssassin, CustomGameOptions.AssassinOn));

                for (int i = 0; i < CustomGameOptions.UnderdogMax; i++)
                    if (Check(CustomGameOptions.UnderdogOn))
                        RolePool.Add((typeof(Underdog), Faction.Impostors, CustomRPC.SetUnderdog, CustomGameOptions.UnderdogOn));

                for (int i = 0; i < CustomGameOptions.MorphlingMax; i++)
                    if (Check(CustomGameOptions.MorphlingOn))
                        RolePool.Add((typeof(Morphling), Faction.Impostors, CustomRPC.SetMorphling, CustomGameOptions.MorphlingOn));

                for (int i = 0; i < CustomGameOptions.CamouflagerMax; i++)
                    if (Check(CustomGameOptions.CamouflagerOn))
                        RolePool.Add((typeof(Camouflager), Faction.Impostors, CustomRPC.SetCamouflager, CustomGameOptions.CamouflagerOn));

                for (int i = 0; i < CustomGameOptions.MinerMax; i++)
                    if (Check(CustomGameOptions.MinerOn))
                        RolePool.Add((typeof(Miner), Faction.Impostors, CustomRPC.SetMiner, CustomGameOptions.MinerOn));

                for (int i = 0; i < CustomGameOptions.SwooperMax; i++)
                    if (Check(CustomGameOptions.SwooperOn))
                        RolePool.Add((typeof(Swooper), Faction.Impostors, CustomRPC.SetSwooper, CustomGameOptions.SwooperOn));

                for (int i = 0; i < CustomGameOptions.JanitorMax; i++)
                    if (Check(CustomGameOptions.JanitorOn))
                        RolePool.Add((typeof(Janitor), Faction.Impostors, CustomRPC.SetJanitor, CustomGameOptions.JanitorOn));

                // Neutral Roles
                for (int i = 0; i < CustomGameOptions.ArsonistMax; i++)
                    if (Check(CustomGameOptions.ArsonistOn))
                        RolePool.Add((typeof(Arsonist), Faction.Neutral, CustomRPC.SetArsonist, CustomGameOptions.ArsonistOn));

                for (int i = 0; i < CustomGameOptions.ExecutionerMax; i++)
                    if (Check(CustomGameOptions.ExecutionerOn))
                        RolePool.Add((typeof(Executioner), Faction.Neutral, CustomRPC.SetExecutioner, CustomGameOptions.ExecutionerOn));

                for (int i = 0; i < CustomGameOptions.JesterMax; i++)
                    if (Check(CustomGameOptions.JesterOn))
                        RolePool.Add((typeof(Jester), Faction.Neutral, CustomRPC.SetJester, CustomGameOptions.JesterOn));

                for (int i = 0; i < CustomGameOptions.ShifterMax; i++)
                    if (Check(CustomGameOptions.ShifterOn))
                        RolePool.Add((typeof(Shifter), Faction.Neutral, CustomRPC.SetShifter, CustomGameOptions.ShifterOn));

                for (int i = 0; i < CustomGameOptions.GlitchMax; i++)
                    if (Check(CustomGameOptions.GlitchOn))
                        RolePool.Add((typeof(Glitch), Faction.Neutral, CustomRPC.SetGlitch, CustomGameOptions.GlitchOn));
                RolePool.Shuffle();
                RolePool.Sort((a, b) =>
                {
                    var a_ = a.Item4 == 100 ? 0 : 100;
                    var b_ = b.Item4 == 100 ? 0 : 100;
                    return a_.CompareTo(b_);
                });
            }

            private static void GenerateModifiers()
            {
                ModifierPool.Clear();
                var global = new Faction[] { Faction.Crewmates, Faction.Impostors, Faction.Neutral };
                var crewAndNeutrals = new Faction[]{ Faction.Crewmates, Faction.Neutral };

                if (Check(CustomGameOptions.TorchOn))
                    ModifierPool.Add((typeof(Torch), CustomRPC.SetTorch, new[] { Faction.Crewmates }, CustomGameOptions.TorchOn));

                if (Check(CustomGameOptions.DiseasedOn))
                    ModifierPool.Add((typeof(Diseased), CustomRPC.SetDiseased, new []{ Faction.Crewmates}, CustomGameOptions.DiseasedOn));

                if (Check(CustomGameOptions.TiebreakerOn))
                    ModifierPool.Add((typeof(Tiebreaker), CustomRPC.SetTiebreaker, crewAndNeutrals, CustomGameOptions.TiebreakerOn));

                if (Check(CustomGameOptions.FlashOn))
                    ModifierPool.Add((typeof(Flash), CustomRPC.SetFlash, global, CustomGameOptions.FlashOn));

                if (Check(CustomGameOptions.DrunkOn))
                    ModifierPool.Add((typeof(Drunk), CustomRPC.SetDrunk, global, CustomGameOptions.DrunkOn));

                if (Check(CustomGameOptions.BigBoiOn))
                    ModifierPool.Add((typeof(BigBoi), CustomRPC.SetBigBoi, global, CustomGameOptions.BigBoiOn));

                if (Check(CustomGameOptions.ButtonBarryOn))
                    ModifierPool.Add(
                        (typeof(ButtonBarry), CustomRPC.SetButtonBarry, global, CustomGameOptions.ButtonBarryOn));
                if (Check(CustomGameOptions.LoversOn))
                    ModifierPool.Add((typeof(Lover), CustomRPC.SetCouple, global, CustomGameOptions.LoversOn));

                ModifierPool.Shuffle();
            }
        }

    }
}
