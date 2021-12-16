using HarmonyLib;
using Hazel;
using System;
using System.Collections.Generic;
using System.Linq;
using Reactor.Extensions;
using TMPro;
using TownOfUs.ImpostorRoles.CamouflageMod;
using TownOfUs.Roles.Modifiers;
using UnhollowerBaseLib;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Reactor;
using TownOfUs.Patches.Buttons;
using TownOfUs.Patches;
using TownOfUs.CrewmateRoles.InvestigatorMod;

namespace TownOfUs.Roles
{
    public abstract class Role
    {
        public static readonly Dictionary<byte, Role> RoleDictionary = new();

        public static bool NobodyWins;

        public Func<string> ImpostorText;
        public Func<string> TaskText;
        public ModdedButton KillButton;
        public Vector3 SizeFactor;
        public float SpeedFactor = 1;
        public bool OverrideSize;
        public bool OverrideSpeed;

        protected Role(PlayerControl player)
        {
            Player = player;
            RoleDictionary.Add(player.PlayerId, this);
            ModdedButton.UnregisterAllButtonsFromPlayer(player);
        }

        public String FactionString()
        {
            switch (Faction)
            {
                case Faction.Crewmates:
                    return "Crewmate";
                case Faction.Impostors:
                    return "Impostor";
                case Faction.Neutral:
                    return "Neutral";
                default:
                    return "";
            }
        }

        public Color FactionTitleColor()
        {
            switch (Faction)
            {
                case Faction.Crewmates:
                    return Colors.Crewmate;
                case Faction.Impostors:
                    return Colors.Impostor;
                case Faction.Neutral:
                    return Colors.Shifter;
                default:
                    return Color;
            }
        }

        protected void GenerateKillButton()
        {
            KillButton = new ModdedButton(Player);
            KillButton.ButtonType = ButtonType.KillButton;
            KillButton.ButtonTarget = ButtonTarget.Player;
            KillButton.UseDefault = true;
            KillButton.RegisterButton();
        }

        public static IEnumerable<Role> AllRoles => RoleDictionary.Values.ToList();
        protected internal string Name { get; set; }

        private PlayerControl _player { get; set; }

        public PlayerControl Player
        {
            get => _player;
            set
            {
                if (_player != null) _player.nameText.color = Color.white;

                _player = value;
                PlayerName = value.Data.PlayerName;
            }
        }

        protected float Scale { get; set; } = 1f;
        protected internal Color Color { get; set; }
        protected internal RoleEnum RoleType { get; set; }

        protected internal int TasksLeft => Player.Data.Tasks.ToArray().Count(x => !x.Complete);
        protected internal int TotalTasks => Player.Data.Tasks.Count;

        protected bool Tier1 = false;
        public bool GetTier1 => Tier1;
        public void SetTier1(bool flag) {
            bool oldFlag = Tier1;
            Tier1 = flag;
            if (!oldFlag && flag) OnTierUp();
        }

        protected bool Tier2 = false;
        public bool GetTier2 => Tier2;
        public void SetTier2(bool flag)
        {
            bool oldFlag = Tier2;
            Tier2 = flag;
            if (!oldFlag && flag) OnTierUp();
        }

        protected bool Tier3 = false;
        public bool GetTier3 => Tier3;
        public void SetTier3(bool flag)
        {
            bool oldFlag = Tier3;
            Tier3 = flag;
            if (!oldFlag && flag) OnTierUp();
        }

        protected bool Tier4 = false;

        public bool GetTier4 => Tier4;
        public void SetTier4(bool flag)
        {
            bool oldFlag = Tier4;
            Tier4 = flag;
            if (!oldFlag && flag) OnTierUp();
        }

        public bool Local => PlayerControl.LocalPlayer.PlayerId == Player.PlayerId;

        protected virtual void OnTierUp()
        {
            if (CustomGameOptions.RoleProgressionOn)
            {
                if (Local)
                {
                    if (CustomGameOptions.RoleProgressionFlash)
                    {
                        Coroutines.Start(Utils.FlashCoroutine(Color, 0.5f, 0.2f));
                    }
                }
            }
        }

        protected internal bool Hidden { get; set; } = false;

        protected internal Faction Faction { get; set; } = Faction.Crewmates;

        protected internal Color FactionColor
        {
            get
            {
                return Faction switch
                {
                    Faction.Crewmates => Color.green,
                    Faction.Impostors => Color.red,
                    Faction.Neutral => CustomGameOptions.NeutralRed ? Color.red : Color.grey,
                    _ => Color.white
                };
            }
        }

        public static uint NetId => PlayerControl.LocalPlayer.NetId;
        public string PlayerName { get; set; }

        public string ColorString => "<color=#" + Color.ToHtmlStringRGBA() + ">";

        private bool Equals(Role other)
        {
            return Equals(Player, other.Player) && RoleType == other.RoleType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Role)) return false;
            return Equals((Role)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Player, (int)RoleType);
        }

        //public static T Gen<T>()

        internal virtual bool Criteria()
        {
            Player.nameText.transform.localPosition = new Vector3(
                0f,
                Player.Data.DefaultOutfit.HatId == null ? 1.5f : 2.0f,
                -0.5f
            );
            return (DeadCriteria() || ImpostorCriteria() || LoverCriteria() || SelfCriteria() || RoleCriteria() || Local);
        }

        internal virtual bool ColorCriteria()
        {
            return SelfCriteria() || DeadCriteria() || ImpostorCriteria() || RoleCriteria();
        }

        internal virtual bool DeadCriteria()
        {
            if (PlayerControl.LocalPlayer.Data.IsDead && CustomGameOptions.DeadSeeRoles) return Utils.ShowDeadBodies;
            return false;
        }

        internal virtual bool ImpostorCriteria()
        {
            if (Faction == Faction.Impostors && PlayerControl.LocalPlayer.Data.Role.IsImpostor &&
                CustomGameOptions.ImpostorSeeRoles) return true;
            return false;
        }

        internal virtual bool LoverCriteria()
        {
            if (PlayerControl.LocalPlayer.Is(ModifierEnum.Lover))
            {
                if (Local) return true;
                var lover = Modifier.GetModifier<Lover>(PlayerControl.LocalPlayer);
                return lover.OtherLover.Player == Player;
            }
            return false;
        }

        internal virtual bool SelfCriteria()
        {
            return GetRole(PlayerControl.LocalPlayer) == this;
        }

        internal virtual bool RoleCriteria()
        {
            return false;
        }

        public virtual List<PlayerControl> GetTeammates()
        {
            var team = new List<PlayerControl>();
            if (Faction == Faction.Impostors) {
                foreach(var player in PlayerControl.AllPlayerControls)
                {
                    if (player.Data.Role.IsImpostor) team.Add(player);
                }
            } else if (Faction == Faction.Neutral) {
                team.Add(PlayerControl.LocalPlayer);
            } else if (Faction == Faction.Crewmates) {
                foreach (var player in PlayerControl.AllPlayerControls)
                {
                    team.Add(player);
                }
            }

            return team;
        }

        public static void NobodyWinsFunc()
        {
            NobodyWins = true;
        }

        internal static bool NobodyEndCriteria(ShipStatus __instance)
        {
            bool CheckNoImpsNoCrews()
            {
                var alives = PlayerControl.AllPlayerControls.ToArray()
                    .Where(x => !x.Data.IsDead && !x.Data.Disconnected).ToList();
                if (alives.Count == 0) return false;
                var flag = alives.All(x =>
                {
                    var role = GetRole(x);
                    if (role == null) return false;
                    var flag2 = role.Faction == Faction.Neutral && !x.Is(RoleEnum.Glitch) && !x.Is(RoleEnum.Arsonist);
                    var flag3 = x.Is(RoleEnum.Arsonist) && ((Arsonist)role).IgniteUsed && alives.Count > 1;

                    return flag2 || flag3;
                });

                return flag;
            }

            if (CheckNoImpsNoCrews())
            {
                var messageWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.NobodyWins, SendOption.Reliable, -1);
                AmongUsClient.Instance.FinishRpcImmediately(messageWriter);

                NobodyWinsFunc();
                Utils.EndGame();
                return false;
            }

            return true;
        }

        internal virtual bool EABBNOODFGL(ShipStatus __instance)
        {
            return true;
        }

        protected virtual string NameText(bool revealTasks, bool revealRole, bool revealModifier, bool revealLover, PlayerVoteArea player = null)
        {
            if (CamouflageUnCamouflage.IsCamoed && player == null) return "";
            if (Player.name == "") return "";

            if (Player == null) return "";

            String PlayerName = Player.name;

            var modifier = Modifier.GetModifier(Player);
            if (modifier != null && modifier.GetColoredSymbol() != null)
            {
                if (modifier.ModifierType == ModifierEnum.Lover && (revealModifier || revealLover))
                    PlayerName += $" {modifier.GetColoredSymbol()}";
                else if (modifier.ModifierType != ModifierEnum.Lover && revealModifier)
                    PlayerName += $" {modifier.GetColoredSymbol()}";
            }

            if(revealTasks && Faction == Faction.Crewmates)
                PlayerName += $" ({TotalTasks - TasksLeft}/{TotalTasks})";

            if (player != null && (MeetingHud.Instance.state == MeetingHud.VoteStates.Proceeding ||
                                   MeetingHud.Instance.state == MeetingHud.VoteStates.Results)) return PlayerName;

            if (!revealRole) return PlayerName;

            Player.nameText.transform.localPosition = new Vector3(
                0f,
                Player.Data.DefaultOutfit.HatId == null ? 1.5f : 2.0f,
                -0.5f
            );

            return PlayerName + "\n" + Name;
        }

        public static bool operator ==(Role a, Role b)
        {
            if (a is null && b is null) return true;
            if (a is null || b is null) return false;
            return a.RoleType == b.RoleType && a.Player.PlayerId == b.Player.PlayerId;
        }

        public static bool operator !=(Role a, Role b)
        {
            return !(a == b);
        }

        public void RegenTask()
        {
            bool createTask;
            try
            {
                var firstText = Player.myTasks.ToArray()[0].Cast<ImportantTextTask>();
                createTask = !firstText.Text.Contains("Role:");
            }
            catch (InvalidCastException)
            {
                createTask = true;
            }

            if (createTask)
            {
                var task = new GameObject(Name + "Task").AddComponent<ImportantTextTask>();
                task.transform.SetParent(Player.transform, false);
                task.Text = $"{ColorString}Role: {Name}\n{TaskText()}</color>";
                Player.myTasks.Insert(0, task);
                return;
            }

            Player.myTasks.ToArray()[0].Cast<ImportantTextTask>().Text =
                $"{ColorString}Role: {Name}\n{TaskText()}</color>";
        }

        public static T Gen<T>(Type type, PlayerControl player, CustomRPC rpc)
        {
            var role = (T)Activator.CreateInstance(type, new object[] { player });

            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)rpc, SendOption.Reliable, -1);
            writer.Write(player.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            return role;
        }

        public static T Gen<T>(Type type, List<PlayerControl> players, CustomRPC rpc)
        {
            var player = players[Random.RandomRangeInt(0, players.Count)];
            
            var role = Gen<T>(type, player, rpc);
            players.Remove(player);
            return role;
        }
        
        public static Role GetRole(PlayerControl player)
        {
            if (player == null) return null;
            if (RoleDictionary.TryGetValue(player.PlayerId, out var role))
                return role;

            return null;
        }
        
        public static T GetRole<T>(PlayerControl player) where T : Role
        {
            return GetRole(player) as T;
        }

        public static Role GetRole(PlayerVoteArea area)
        {
            var player = PlayerControl.AllPlayerControls.ToArray()
                .FirstOrDefault(x => x.PlayerId == area.TargetPlayerId);
            return player == null ? null : GetRole(player);
        }

        public static IEnumerable<Role> GetRoles(RoleEnum roletype)
        {
            return AllRoles.Where(x => x.RoleType == roletype);
        }

        [HarmonyPatch(typeof(PlayerControl._CoSetTasks_d__102), nameof(PlayerControl._CoSetTasks_d__102.MoveNext))]
        public static class PlayerControl_SetTasks
        {
            public static void Postfix(PlayerControl._CoSetTasks_d__102 __instance)
            {
                if (__instance == null) return;
                var player = __instance.__4__this;
                var role = GetRole(player);
                var modifier = Modifier.GetModifier(player);

                if (modifier != null)
                {
                    var modTask = new GameObject(modifier.Name + "Task").AddComponent<ImportantTextTask>();
                    modTask.transform.SetParent(player.transform, false);
                    modTask.Text =
                        $"{modifier.ColorString}Modifier: {modifier.Name}\n{modifier.TaskText()}</color>";
                    player.myTasks.Insert(0, modTask);
                }

                if (role == null || role.Hidden) return;
                if (role.RoleType == RoleEnum.Shifter && role.Player != PlayerControl.LocalPlayer) return;
                var task = new GameObject(role.Name + "Task").AddComponent<ImportantTextTask>();
                task.transform.SetParent(player.transform, false);
                task.Text = $"{role.ColorString}Role: {role.Name}\n{role.TaskText()}</color>";
                player.myTasks.Insert(0, task);
            }
        }

        [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.CheckEndCriteria))]
        public static class ShipStatus_KMPKPPGPNIH
        {
            public static bool Prefix(ShipStatus __instance)
            {
                //System.Console.WriteLine("EABBNOODFGL");
                if (!AmongUsClient.Instance.AmHost) return false;
                if (__instance.Systems.ContainsKey(SystemTypes.LifeSupp))
                {
                    var lifeSuppSystemType = __instance.Systems[SystemTypes.LifeSupp].Cast<LifeSuppSystemType>();
                    if (lifeSuppSystemType.Countdown < 0f) return true;
                }

                if (__instance.Systems.ContainsKey(SystemTypes.Laboratory))
                {
                    var reactorSystemType = __instance.Systems[SystemTypes.Laboratory].Cast<ReactorSystemType>();
                    if (reactorSystemType.Countdown < 0f) return true;
                }

                if (__instance.Systems.ContainsKey(SystemTypes.Reactor))
                {
                    var reactorSystemType = __instance.Systems[SystemTypes.Reactor].Cast<ICriticalSabotage>();
                    if (reactorSystemType.Countdown < 0f) return true;
                }

                if (GameData.Instance.TotalTasks <= GameData.Instance.CompletedTasks) return true;

                var result = true;
                foreach (var role in AllRoles)
                {
                    var roleIsEnd = role.EABBNOODFGL(__instance);
                    var modifier = Modifier.GetModifier(role.Player);
                    bool modifierIsEnd = true;
                    if (modifier != null)
                        modifierIsEnd = modifier.EABBNOODFGL(__instance);
                    if (!roleIsEnd || !modifierIsEnd) result = false;
                }

                if (!NobodyEndCriteria(__instance)) result = false;

                return result;
            }
        }

        [HarmonyPatch(typeof(LobbyBehaviour), nameof(LobbyBehaviour.Start))]
        public static class LobbyBehaviour_Start
        {
            private static void Postfix(LobbyBehaviour __instance)
            {
                foreach (var role in AllRoles.Where(x => x.RoleType == RoleEnum.Snitch))
                {
                    ((Snitch)role).ImpArrows.DestroyAll();
                    ((Snitch)role).SnitchArrows.DestroyAll();
                }
                RoleDictionary.Clear();
                Modifier.ModifierDictionary.Clear();
                Lights.SetLights(Color.white);
                ModdedButton.UnregisterAllButtons();
            }
        }

        [HarmonyPatch(typeof(TranslationController), nameof(TranslationController.GetString), typeof(StringNames),
            typeof(Il2CppReferenceArray<Il2CppSystem.Object>))]
        public static class TranslationController_GetString
        {
            public static void Postfix(ref string __result, [HarmonyArgument(0)] StringNames name)
            {
                if (ExileController.Instance == null || ExileController.Instance.exiled == null) return;

                switch (name)
                {
                    case StringNames.ExileTextPN:
                    case StringNames.ExileTextSN:
                    case StringNames.ExileTextPP:
                    case StringNames.ExileTextSP:
                        {
                            var info = ExileController.Instance.exiled;
                            var role = GetRole(info.Object);
                            if (role == null) return;
                            var roleName = role.RoleType == RoleEnum.Glitch ? role.Name : $"The {role.Name}";
                            __result = $"{info.PlayerName} was {roleName}.";
                            return;
                        }
                }
            }
        }
        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
        public static class Player_FixedUpdate
        {
            private static void Postfix(PlayerControl __instance)
            {
                if (AddPrints.GameStarted)
                {
                    var role = Role.GetRole(__instance);
                    if (role == null) return;
                    /**
                    if (role.Trail != null)
                        role.Trail.AddPoint();
                    **/
                }

            }
        }

        [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
        public static class HudManager_Update
        {
            private static Vector3 oldScale = Vector3.zero;
            private static Vector3 oldPosition = Vector3.zero;

            private static void UpdateMeeting(MeetingHud __instance)
            {
                foreach (var player in __instance.playerStates)
                {
                    var role = GetRole(player);
                    if (role != null && role.Criteria())
                    {
                        bool selfFlag = role.SelfCriteria();
                        bool deadFlag = role.DeadCriteria();
                        bool impostorFlag = role.ImpostorCriteria();
                        bool loverFlag = role.LoverCriteria();
                        bool roleFlag = role.RoleCriteria();
                        player.NameText.text = role.NameText(
                            selfFlag || deadFlag || role.Local,
                            selfFlag || deadFlag || impostorFlag || roleFlag,
                            selfFlag || deadFlag,
                            loverFlag,
                            player
                        );
                        if(role.ColorCriteria())
                            player.NameText.color = role.Color;
                    }
                    else
                    {
                        try
                        {
                            player.NameText.text = role.Player.name;
                        }
                        catch
                        {
                        }
                    }
                }
            }

            [HarmonyPriority(Priority.First)]
            private static void Postfix(HudManager __instance)
            {
                if (MeetingHud.Instance != null) UpdateMeeting(MeetingHud.Instance);

                if (PlayerControl.AllPlayerControls.Count <= 1) return;
                if (PlayerControl.LocalPlayer == null) return;
                if (PlayerControl.LocalPlayer.Data == null) return;
                if (PlayerControl.LocalPlayer.Data.Role == null) return;
                foreach (var player in PlayerControl.AllPlayerControls)
                {
                    if (!(player.Data != null && player.Data.Role != null && player.Data.Role.IsImpostor && PlayerControl.LocalPlayer.Data.Role.IsImpostor))
                    {
                        player.nameText.text = player.name;
                        player.nameText.color = Color.white;
                    }
                    var role = GetRole(player);
                    if (role != null)
                        if (role.Criteria())
                        {

                            bool selfFlag = role.SelfCriteria();
                            bool deadFlag = role.DeadCriteria();
                            bool impostorFlag = role.ImpostorCriteria();
                            bool loverFlag = role.LoverCriteria();
                            bool roleFlag = role.RoleCriteria();
                            player.nameText.text = role.NameText(
                                selfFlag || deadFlag || role.Local,
                                selfFlag || deadFlag || impostorFlag || roleFlag,
                                selfFlag || deadFlag,
                                loverFlag
                             );

                            if (role.ColorCriteria())
                                player.nameText.color = role.Color;
                        }
                    //if (player.Data != null && PlayerControl.LocalPlayer.Data.Role.IsImpostor && player.Data.Role.IsImpostor) continue;
                }
            }
        }
    }
}