using HarmonyLib;
using Reactor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TownOfUs.Roles;
using TownOfUs.Utility;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TownOfUs.Patches.Buttons
{
    public class ModdedButton
    {
        public static List<ModdedButton> AllButtons { get; } = new List<ModdedButton>();

        public ActionButton Button { get; set; }

        private Func<ModdedButton, bool> _action;
        private Action<ModdedButton> _actionEnd;
        private Func<ModdedButton, float> _cooldown;
        private Func<ModdedButton, float> _duration;
        private Func<ModdedButton, bool> _show;
        private Func<ModdedButton, bool> _enabled;
        public Func<ModdedButton, List<PlayerControl>> _filter;

        public Sprite Sprite { get; set; }
        public PlayerControl Player { get; set; }
        public Role Role { get; set; }
        public PlayerControl ClosestPlayer { get; set; }
        public DeadBody ClosestBody { get; set; }
        public Vent ClosestVent { get; set; }
         

        public bool UseDefault = true;
        public ButtonType ButtonType = ButtonType.KillButton;
        public ButtonTarget ButtonTarget = ButtonTarget.Player;

        public HudAlignment Alignment = HudAlignment.BottomRight;
        public int Offset = 0;

        private List<ModdedButton> LinkedUse;
        public bool ActionActive => ActionTimer > 0;

        public float CooldownTimer = 0;
        public float ActionTimer = 0;

        public bool ResetCooldown = true;
        private bool _actionIsActive = false;
        public bool SkipAction = false;
        public bool IsLocked = false;
        public bool ResetCooldownAtMeetingEnd = true;
        public Sprite LockedImage = Glitch.LockSprite;

    public ModdedButton(PlayerControl player)
        {
            Player = player;
            Role = Role.GetRole(player);
            SetEnabled(DefaultEnabled);
            SetShow(DefaultShow);
            SetCooldown(DefaultCooldown);
            SetAction(DefaultAction);
            LinkedUse = new List<ModdedButton>();
            // Intro takes about 10 seconds. Host is a bit faster but they should all synch up
            SetCooldownValue(20f); 
        }

        public ModdedButton(PlayerControl player, ActionButton button) : this(player)
        {
            Button = button;
        }

        public void RegisterButton()
        {
            if (Player == PlayerControl.LocalPlayer)
            {
                AllButtons.Add(this);
            }
        }

        public static void LinkUseTime(params ModdedButton[] buttons)
        {
            if (buttons.Length <= 1) return;
            foreach(var parent in buttons)
            {
                foreach(var child in buttons)
                {
                    if (parent == child) continue;
                    if (parent.LinkedUse.Contains(child)) continue;
                    parent.LinkedUse.Add(child);
                }
            }
        }

        public void SetPosition(HudAlignment alignment, int offset)
        {
            Alignment = alignment;
            Offset = offset;
        }

        public void SynchLastUsage()
        {
            if (LinkedUse.Count == 0) return;
            foreach(var button in LinkedUse)
            {
                button.SetCooldownValue(button.GetCooldown());
            }
        }

        public static List<ModdedButton> GetAllModdedButtonsFromPlayer(PlayerControl player)
        {
            return AllButtons.Where(button => button.Player == player).ToList();
        }

        public static List<ModdedButton> GetAllModdedButtonsFromRole(Role role)
        {
            return AllButtons.Where(button => button.Role == role).ToList();
        }

        public static void UnregisterAllButtonsFromPlayer(PlayerControl player)
        {
            var toRemove = new List<ModdedButton>();
            foreach (var button in AllButtons)
            {
                if (button.Player == player)
                    toRemove.Add(button);
            }
            foreach (var button in toRemove)
            {
                AllButtons.Remove(button);
                button.Button?.gameObject?.SetActive(false);
            }
        }

        public static void UnregisterAllButtons()
        {
            AllButtons.Clear();
        }

        public void SetAction(Func<ModdedButton, bool> action)
        {
            _action = action;
        }

        public void SetActionEnd(Action<ModdedButton> actionEnd)
        {
            _actionEnd = actionEnd;
        }


        public void SetCooldown(Func<ModdedButton, float> cooldown)
        {
            _cooldown = cooldown;
        }

        public void SetDuration(Func<ModdedButton, float> duration)
        {
            _duration = duration;
        }

        public bool IsCoolingDown()
        {
            return CooldownTimer > 0;
        }

        public void SetCooldownValue(float cd)
        {
            CooldownTimer = cd;
            if(ButtonType != ButtonType.VentButton)
                Button?.SetCoolDown(CooldownTimer, GetCooldown());
        }

        public void ResetCooldownValue()
        {
            if (_actionIsActive)
            {
                ActionEnd();
                _actionIsActive = false;
                ActionTimer = 0;
            }

            SetCooldownValue(GetCooldown());
            PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"Cooldown: {CooldownTimer} / {GetCooldown()}");
        }

        public void SetDurationValue(float timeRemaining)
        {
            ActionTimer = timeRemaining;
            if (ButtonType != ButtonType.VentButton)
                Button?.SetCoolDown(timeRemaining, GetDuration());
        }

        public void SwitchToActionTimer()
        {
            ActionTimer = GetDuration();
            _actionIsActive = true;
        }

        public void SetShow(Func<ModdedButton, bool> show)
        {
            _show = show;
        }

        public void SetEnabled(Func<ModdedButton, bool> enabled)
        {
            _enabled = enabled;
        }

        public void SetFilter(Func<ModdedButton,List<PlayerControl>> filter)
        {
            _filter = filter;
        }

        public static Vector3 GetPosition(HudManager hud, ModdedButton button)
        {
            float[] xValues =
            {
                hud.UseButton.transform.position.x,
                Camera.main.ScreenToWorldPoint(new Vector3(0, 0)).x + 0.75f
            };
            float[] yValues =
            {
                hud.UseButton.transform.position.y,
                hud.KillButton.transform.position.y
            };
            float zValue = hud.UseButton.transform.position.z;
            float distance =  0.8f;
            float xStart = 0f;
            int sign = 1;
            switch (button.Alignment)
            {
                case HudAlignment.BottomLeft:
                case HudAlignment.TopLeft:
                    xStart = xValues[1];
                    sign = 1;
                    break;
                case HudAlignment.BottomRight:
                case HudAlignment.TopRight:
                    xStart = xValues[0];
                    sign = -1;
                    break;
            }

            // 0 ->[ ][ ]   1-> [x][ ]  2-> [ ] [ ] 3-> [ ][x]
            //     [x][ ]       [ ][ ]      [ ] [x]     [ ][ ]

            int row = button.Offset % 2;
            int column = (button.Offset - row) / 2; 
            float xValue = xStart + (sign * distance * column);

            return new Vector3(xValue, yValues[row], zValue);
        }

        public void FindNearestBody()
        {
            var data = Player.Data;
            var isDead = data.IsDead;
            var truePosition = PlayerControl.LocalPlayer.GetTruePosition();
            var maxDistance = GameOptionsData.KillDistances[PlayerControl.GameOptions.KillDistance];
            var flag = (PlayerControl.GameOptions.GhostsDoTasks || !data.IsDead) &&
                       (!AmongUsClient.Instance || !AmongUsClient.Instance.IsGameOver) &&
                       PlayerControl.LocalPlayer.CanMove;
            var allocs = Physics2D.OverlapCircleAll(truePosition, maxDistance,
                LayerMask.GetMask(new[] { "Players", "Ghost" }));

            DeadBody closestBody = null;
            var closestDistance = float.MaxValue;

            foreach (var collider2D in allocs)
            {
                if (!flag || isDead || collider2D.tag != "DeadBody") continue;
                var component = collider2D.GetComponent<DeadBody>();

                if (!(Vector2.Distance(truePosition, component.TruePosition) <=
                      maxDistance)) continue;

                var distance = Vector2.Distance(truePosition, component.TruePosition);
                if (!(distance < closestDistance)) continue;
                closestBody = component;
                closestDistance = distance;
            }
            ClosestBody = closestBody;
        }

        public void FindNearestVent()
        {
            if (ShipStatus.Instance == null) return;
            if (ShipStatus.Instance.AllVents == null) return;
            if (ShipStatus.Instance.AllVents.Count == 0) return;

            var truePosition = PlayerControl.LocalPlayer.GetTruePosition();
            var closestDistance = float.MaxValue;
            Vent closestVent = null;

            foreach (Vent vent in ShipStatus.Instance.AllVents)
            {
                var distance = Vector2.Distance(truePosition, vent.transform.position);
                if (!(distance <= vent.UsableDistance)) continue;
                if (!(distance < closestDistance)) continue;
                closestVent = vent;
                closestDistance = distance;
            }
            ClosestVent = closestVent;
        }
        protected internal bool Action()
        {
            if (_action != null && Button != null)
            {
                if (Enabled())
                {
                    var flag = _action(this);
                    if (ResetCooldown)
                    {
                        if (_actionEnd != null && !SkipAction)
                        {
                            _actionIsActive = true;
                            SetDurationValue(GetDuration());
                        }
                        else
                        {
                            if(CooldownTimer <= 0)
                                SetCooldownValue(GetCooldown());
                            SkipAction = false;
                        }
                        SynchLastUsage();
                    } else
                    {
                        ResetCooldown = true;
                    }
                    return flag;
                }
                return false;
            }
            return true;
        }

        protected internal void ActionEnd()
        {
            if(_actionEnd != null && Button != null)
            {
                _actionEnd(this);
            }
        }

        protected internal float GetCooldown()
        {
            if (_cooldown != null)
                return _cooldown(this);
            return 0;
        }

        protected internal float GetDuration()
        {
            if (_duration != null)
                return _duration(this);
            return 0;
        }

        protected internal List<PlayerControl> GetFilter()
        {
            if (_filter != null && Button != null)
                return _filter(this);
            return PlayerControl.AllPlayerControls.ToArray().ToList();
        }

        protected internal bool Show()
        {
            if (_show != null && Button != null)
            {
                if (MeetingHud.Instance != null) return false;
                return _show(this);
            }
            return true;
        }

        protected internal bool Enabled()
        {
            if (_enabled != null && Button != null && !IsLocked)
                return _enabled(this);
            return false;
        }

        public static bool DefaultEnabled(ModdedButton button)
        {
            if (!button.Show()) return false;
            if (PlayerControl.AllPlayerControls.Count <= 1) return false;
            if (button.IsCoolingDown() || button.ActionActive) return false;
            if (!button.Player.CanMove) return false;

            switch (button.ButtonTarget)
            {
                case (ButtonTarget.Player):
                    return button.ClosestPlayer != null;
                case (ButtonTarget.DeadBody):
                    return button.ClosestBody != null;
                case (ButtonTarget.Vent):
                    return button.ClosestVent != null;
                case ButtonTarget.None:
                    return true;
            }
            return false;
        }

        public static bool DefaultShow(ModdedButton button)
        {
            if (PlayerControl.LocalPlayer != button.Player) return false;
            if (button.Player.Data.IsDead) return false;
            return true;
        }

        public static float DefaultCooldown(ModdedButton button)
        {
            return PlayerControl.GameOptions.KillCooldown;
        }

        public static bool DefaultAction(ModdedButton button)
        {
            var target = button.ClosestPlayer;
            if (target.IsShielded())
            {
                Medic.RpcInteractWithShield(target, true, true);
                if (!Medic.InteractWithShield(target, true, true))
                {
                    button.ResetCooldown = false;
                }
                return false;
            }
            else
            {
                Utils.RpcMurderPlayer(button.Player, target);
            }
            return false;
        }

        public static bool VentAction(ModdedButton button)
        {
            if (button.ClosestVent == null) return false;
            var player = button.Player;
            var vent = button.ClosestVent;
            if (player.inVent)
            {
                player.MyPhysics.RpcExitVent(vent.Id);
                vent.SetButtons(false);
                return false;
            }
            player.MyPhysics.RpcEnterVent(vent.Id);
            vent.SetButtons(true);
            return false;
        }

        public static ModdedButton GetButton(ActionButton button)
        {
            foreach(var moddedButton in AllButtons)
            {
                if (moddedButton.Button == button) return moddedButton;
            }
            return null;
        }

        [HarmonyPatch(typeof(KillButton), nameof(KillButton.DoClick))]
        public static class KillButtonDoClick
        {
            [HarmonyPriority(Priority.First)]
            public static bool Prefix(KillButton __instance) {
                var button = GetButton(__instance);
                if (button == null) return true;
                return button.Action();
            }
        }

        [HarmonyPatch(typeof(AbilityButton), nameof(AbilityButton.DoClick))]
        public static class ActionButtonDoClick
        {
            [HarmonyPriority(Priority.First)]
            public static bool Prefix(AbilityButton __instance)
            {
                var button = GetButton(__instance);
                if (button == null) return true;
                return button.Action();
            }
        }


        [HarmonyPatch(typeof(VentButton), nameof(VentButton.DoClick))]
        public static class VentButtonDoClick
        {
            [HarmonyPriority(Priority.First)]
            public static bool Prefix(VentButton __instance)
            {
                var button = GetButton(__instance);
                if (button == null) return true;
                return button.Action();
            }
        }

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
        public static class CooldownUpdate
        {
            public static void Postfix(PlayerControl __instance)
            {
                if (__instance != PlayerControl.LocalPlayer) return;
                if (!GameData.Instance) return;
                if (__instance.Data == null || __instance.Data.Role == null)
                    return;
                foreach (var moddedButton in ModdedButton.GetAllModdedButtonsFromPlayer(__instance))
                {
                    if (moddedButton.Button != null)
                    {
                        bool show = moddedButton.Show();
                        if (moddedButton._actionIsActive)
                        {
                            if(MeetingHud.Instance != null)
                            {
                                moddedButton._actionIsActive = false;
                                moddedButton.ActionEnd();
                                moddedButton.SetCooldownValue(moddedButton.GetCooldown() - Time.deltaTime);
                            }
                            else if (moddedButton.ActionTimer > 0)
                            {
                                switch (moddedButton.ButtonType)
                                {
                                    case ButtonType.KillButton:
                                        ((KillButton)moddedButton.Button).cooldownTimerText.color = new Color(0.1f, 0.6f, 1f);
                                        break;
                                    case ButtonType.AbilityButton:
                                        ((AbilityButton)moddedButton.Button).cooldownTimerText.color = new Color(0.1f, 0.6f, 1f);
                                        break;
                                }
                                moddedButton.SetDurationValue(moddedButton.ActionTimer - Time.deltaTime);
                            }
                            else
                            {
                                moddedButton._actionIsActive = false;
                                moddedButton.ActionEnd();
                                moddedButton.SetCooldownValue(moddedButton.GetCooldown() - Time.deltaTime);
                                switch (moddedButton.ButtonType)
                                {
                                    case ButtonType.KillButton:
                                        ((KillButton)moddedButton.Button).cooldownTimerText.color = new Color(1f, 1f, 1f);
                                        break;
                                    case ButtonType.AbilityButton:
                                        ((AbilityButton)moddedButton.Button).cooldownTimerText.color = new Color(1f, 1f, 1f);
                                        break;
                                }
                            }
                        }
                        else
                        {
                            moddedButton.SetCooldownValue(moddedButton.CooldownTimer - Time.deltaTime);
                            switch (moddedButton.ButtonType)
                            {
                                case ButtonType.KillButton:
                                    ((KillButton)moddedButton.Button).cooldownTimerText.color = new Color(1f, 1f, 1f);
                                    break;
                                case ButtonType.AbilityButton:
                                    ((AbilityButton)moddedButton.Button).cooldownTimerText.color = new Color(1f, 1f, 1f);
                                    break;
                            }
                        }

                        if (show)
                        {
                            switch (moddedButton.ButtonTarget)
                            {
                                case ButtonTarget.Player:
                                    PlayerControl target = null;
                                    Utils.SetClosestPlayer(ref target, float.NaN, moddedButton.GetFilter());
                                    moddedButton.ClosestPlayer = target;
                                    if (moddedButton.ButtonType == ButtonType.KillButton)
                                    {

                                        ((KillButton)moddedButton.Button).SetTarget(target);
                                    }
                                    break;
                                case ButtonTarget.DeadBody:
                                    moddedButton.FindNearestBody();
                                    break;
                                case ButtonTarget.Vent:
                                    moddedButton.FindNearestVent();
                                    if (moddedButton.ButtonType == ButtonType.VentButton)
                                    {
                                        ((VentButton)moddedButton.Button).SetTarget(moddedButton.ClosestVent);
                                    }
                                    break;
                            }

                            if (moddedButton.Enabled())
                                moddedButton.Button.SetEnabled();
                            else
                                moddedButton.Button.SetDisabled();
                        }
                    }
                }
            }
        }



        [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
        public static class ButtonUpdate
        {
            public static void Postfix(HudManager __instance)
            {
                var localPlayer = PlayerControl.LocalPlayer;
                if (localPlayer.Data == null || localPlayer.Data.Role == null)
                    return;
                foreach (ModdedButton moddedButton in AllButtons)
                {
                    if (moddedButton.Button == null)
                    {
                        if (moddedButton.UseDefault)
                        {
                            switch (moddedButton.ButtonType)
                            {
                                case ButtonType.KillButton:
                                    moddedButton.Button = __instance.KillButton;
                                    break;
                                case ButtonType.AbilityButton:
                                    moddedButton.Button = __instance.AbilityButton;
                                    ((AbilityButton)moddedButton.Button)?.OverrideText("");
                                    break;
                                case ButtonType.VentButton:
                                    moddedButton.Button = __instance.ImpostorVentButton;
                                    break;
                            }
                        }
                        else
                        {
                            switch (moddedButton.ButtonType)
                            {
                                case ButtonType.KillButton:
                                    moddedButton.Button = Object.Instantiate(__instance.KillButton, HudManager.Instance.transform);
                                    break;
                                case ButtonType.AbilityButton:
                                    moddedButton.Button = Object.Instantiate(__instance.AbilityButton, HudManager.Instance.transform);
                                    ((AbilityButton)moddedButton.Button).OverrideText("");
                                    break;
                                case ButtonType.VentButton:
                                    moddedButton.Button = Object.Instantiate(__instance.ImpostorVentButton, HudManager.Instance.transform);
                                    break;
                            }
                            moddedButton.Button.gameObject.transform.position = ModdedButton.GetPosition(__instance, moddedButton);
                            var pos = ModdedButton.GetPosition(__instance, moddedButton);
                            PluginSingleton<TownOfUs>.Instance.Log.LogMessage( $"Position: x: {pos.x}, y: {pos.y}, z: {pos.z}");
                        }
                    }

                    if (moddedButton.Button != null)
                    {
                        bool show = moddedButton.Show();
                        if (moddedButton.Sprite != null)
                            moddedButton.Button.graphic.sprite = moddedButton.Sprite;
                        moddedButton.Button.gameObject.SetActive(show);
                        moddedButton.Button.graphic.enabled = show;
                        if (!moddedButton.UseDefault && show)
                            moddedButton.Button.gameObject.transform.position = ModdedButton.GetPosition(__instance, moddedButton);
                        

                        if (moddedButton.Enabled())
                            moddedButton.Button.SetEnabled();
                        else
                            moddedButton.Button.SetDisabled();
                    }
                }
            }
        }

        [HarmonyPatch(typeof(Object), nameof(Object.Destroy), typeof(Object))]
        public static class MeetingCooldownReset
        {
            public static void Postfix(Object obj)
            {
                if (ExileController.Instance == null || obj != ExileController.Instance.gameObject) return;

                foreach (var button in AllButtons)
                {
                    if(button.ResetCooldownAtMeetingEnd)
                        button.ResetCooldownValue();
                }
            }
        }
        [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
        public static class FixVanillaButtons
        {
            public static void Postfix(HudManager __instance)
            {
                var localPlayer = PlayerControl.LocalPlayer;
                if (localPlayer.Data == null || localPlayer.Data.Role == null)
                    return;
                if (LobbyBehaviour.Instance != null) return;

                var useButton = __instance.UseButton;
                if(useButton != null)
                {
                   useButton.gameObject.SetActive(MeetingHud.Instance == null);
                }
                var reportButton = __instance.ReportButton;
                if(reportButton != null)
                {
                    reportButton.gameObject.SetActive(MeetingHud.Instance == null && !PlayerControl.LocalPlayer.Data.IsDead);
                }
            }
        }

    }
}
