using Hazel;
using InnerNet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Reactor;
using TownOfUs.CrewmateRoles.MedicMod;
using Reactor.Extensions;
using TownOfUs.Extensions;
using TownOfUs.Roles.Modifiers;
using UnityEngine;
using Object = UnityEngine.Object;
using TownOfUs.Patches.Buttons;
using TownOfUs.Utility;

namespace TownOfUs.Roles
{
    public class Glitch : Role
    {
        public static AssetBundle bundle = loadBundle();
        public static Sprite MimicSprite = bundle.LoadAsset<Sprite>("MimicSprite").DontUnload();
        public static Sprite HackSprite = bundle.LoadAsset<Sprite>("HackSprite").DontUnload();
        public static Sprite LockSprite = bundle.LoadAsset<Sprite>("Lock").DontUnload();

        public bool lastMouse;

        public ModdedButton HackButton;
        public ModdedButton MimicButton;

        public Glitch(PlayerControl player) : base(player)
        {
            Name = "The Glitch";
            Color = Patches.Colors.Glitch;
            RoleType = RoleEnum.Glitch;
            ImpostorText = () => "You are the glitch";
            TaskText = () => "Murder players as the Glitch:";
            Faction = Faction.Neutral;

            MimicButton = new ModdedButton(Player);
            MimicButton.UseDefault = false;
            MimicButton.ButtonType = ButtonType.AbilityButton;
            MimicButton.ButtonTarget = ButtonTarget.None;
            MimicButton.Sprite = MimicSprite;
            MimicButton.SetAction(MimicAction);
            MimicButton.SetActionEnd(MimicEnd);
            MimicButton.SetCooldown(button => { return CustomGameOptions.MimicCooldown; });
            MimicButton.SetDuration(button => { return CustomGameOptions.MimicDuration; });
            MimicButton.SetPosition(HudAlignment.BottomLeft, 0);
            MimicButton.RegisterButton();

            HackButton = new ModdedButton(player);
            HackButton.UseDefault = false;
            HackButton.ButtonType = ButtonType.AbilityButton;
            HackButton.ButtonTarget = ButtonTarget.Player;
            HackButton.Sprite = HackSprite;
            HackButton.SetAction(HackAction);
            HackButton.SetActionEnd(HackEnd);
            HackButton.SetCooldown(button => { return CustomGameOptions.HackCooldown; });
            HackButton.SetDuration(button => { return CustomGameOptions.HackDuration; });
            HackButton.SetPosition(HudAlignment.BottomLeft,1);
            HackButton.RegisterButton();

            GenerateKillButton();
            KillButton.SetCooldownValue(CustomGameOptions.InitialGlitchKillCooldown + 10);
        }


        public ChatController MimicList { get; set; }
        public bool IsUsingMimic { get; set; }
        public List<Tuple<GameObject, ActionButton>> HackIcons = new List<Tuple<GameObject, ActionButton>>();
        public PlayerControl HackedPlayer;
        public bool HackAction(ModdedButton button)
        {
            HackedPlayer = button.ClosestPlayer;
            RpcSetHacked(button.ClosestPlayer);
            return false;
        }

        public void HackEnd(ModdedButton button)
        {
            RpcRemoveHacked(HackedPlayer);
            HackedPlayer = null;
        }

        public void UpdateHack()
        {
            if(HackedPlayer != null && HackedPlayer == PlayerControl.LocalPlayer)
            {
                if (Minigame.Instance)
                {
                    Minigame.Instance.Close();
                    Minigame.Instance.Close();
                }
            }
            if(HackIcons != null && HackIcons.Count > 0)
                foreach(var tuple in HackIcons)
                {
                    var obj = tuple.Item1;
                    var button = tuple.Item2;
                    var pos = button.gameObject.transform.position;
                    var r = new System.Random();
                    var noiseX = 0.1 * r.NextDouble();
                    var noiseY = 0.1 * r.NextDouble();
                    obj.transform.position = new Vector3((float) (pos.x + noiseX), (float) (pos.y + noiseY), -50);
                    button.SetDisabled();
                }
        }

        public bool MimicAction(ModdedButton button)
        {
            button.ResetCooldown = false;
            OpenMimicMenu();
            return false;
        }

        private void OpenMimicMenu()
        {
            if (MimicList == null)
            {
                MimicList = Object.Instantiate(HudManager.Instance.Chat);

                MimicList.transform.SetParent(Camera.main.transform);
                MimicList.SetVisible(true);
                MimicList.Toggle();

                MimicList.TextBubble.enabled = false;
                MimicList.TextBubble.gameObject.SetActive(false);

                MimicList.TextArea.enabled = false;
                MimicList.TextArea.gameObject.SetActive(false);

                MimicList.BanButton.enabled = false;
                MimicList.BanButton.gameObject.SetActive(false);

                MimicList.CharCount.enabled = false;
                MimicList.CharCount.gameObject.SetActive(false);

                MimicList.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>()
                    .enabled = false;
                MimicList.gameObject.transform.GetChild(0).gameObject.SetActive(false);

                MimicList.BackgroundImage.enabled = false;

                foreach (var rend in MimicList.Content
                    .GetComponentsInChildren<SpriteRenderer>())
                    if (rend.name == "SendButton" || rend.name == "QuickChatButton")
                    {
                        rend.enabled = false;
                        rend.gameObject.SetActive(false);
                    }

                foreach (var bubble in MimicList.chatBubPool.activeChildren)
                {
                    bubble.enabled = false;
                    bubble.gameObject.SetActive(false);
                }

                MimicList.chatBubPool.activeChildren.Clear();

                foreach (var player in PlayerControl.AllPlayerControls.ToArray()
                    .Where(x => x != PlayerControl.LocalPlayer))
                {
                    var oldDead = player.Data.IsDead;
                    player.Data.IsDead = false;
                    MimicList.AddChat(player, "Click here");
                    player.Data.IsDead = oldDead;
                }
            }
            else
            {
                MimicList.Toggle();
                MimicList.SetVisible(false);
                MimicList.Destroy();
                MimicList = null;
            }
        }

        public void MimicMenuClicked()
        {
            if (MimicList != null)
            {
                if (Minigame.Instance)
                    Minigame.Instance.Close();

                if (!MimicList.IsOpen || MeetingHud.Instance)
                {
                    MimicList.Toggle();
                    MimicList.SetVisible(false);
                    MimicList.Destroy();
                    MimicList = null;
                }
                else
                {
                    foreach (var bubble in MimicList.chatBubPool.activeChildren)
                        if (!IsUsingMimic && MimicList != null)
                        {
                            Vector2 ScreenMin =
                                Camera.main.WorldToScreenPoint(bubble.Cast<ChatBubble>().Background.bounds.min);
                            Vector2 ScreenMax =
                                Camera.main.WorldToScreenPoint(bubble.Cast<ChatBubble>().Background.bounds.max);
                            if (Input.mousePosition.x > ScreenMin.x && Input.mousePosition.x < ScreenMax.x)
                                if (Input.mousePosition.y > ScreenMin.y && Input.mousePosition.y < ScreenMax.y)
                                {
                                    if (!Input.GetMouseButtonDown(0) && lastMouse)
                                    {
                                        lastMouse = false;
                                        MimicList.Toggle();
                                        MimicList.SetVisible(false);
                                        MimicList = null;
                                        RpcSetMimicked(PlayerControl.AllPlayerControls.ToArray().Where(x =>
                                                x.Data.PlayerName == bubble.Cast<ChatBubble>().NameText.text)
                                            .FirstOrDefault());
                                        IsUsingMimic = true;
                                        MimicButton.SwitchToActionTimer();
                                        break;
                                    }

                                    lastMouse = Input.GetMouseButtonDown(0);
                                }
                        }
                }
            }
        }

        public void MimicEnd(ModdedButton button)
        {
            RpcSetMimicked(Player);
            IsUsingMimic = false;
        }

        public void RpcSetMimicked(PlayerControl target)
        {
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
            (byte)CustomRPC.SetMimic,
            SendOption.Reliable, -1);
            writer.Write(Player.PlayerId);
            writer.Write(target.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            Utils.Morph(Player, target.Data.DefaultOutfit);
        }

        public void RpcSetHacked(PlayerControl target)
        {
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
            (byte)CustomRPC.SetHacked,
            SendOption.Reliable, -1);
            writer.Write(Player.PlayerId);
            writer.Write(target.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }

        public void RpcRemoveHacked(PlayerControl target)
        {
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
            (byte)CustomRPC.RemoveHacked,
            SendOption.Reliable, -1);
            writer.Write(Player.PlayerId);
            writer.Write(target.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }

        public void RemoveHack(PlayerControl target)
        {
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
            (byte)CustomRPC.RemoveHacked,
            SendOption.Reliable, -1);
            writer.Write(Player.PlayerId);
            writer.Write(target.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }

        public bool GlitchWins { get; set; }

        public static AssetBundle loadBundle()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream("TownOfUs.Resources.glitchbundle");
            var assets = stream.ReadFully();
            return AssetBundle.LoadFromMemory(assets);
        }

        internal override bool EABBNOODFGL(ShipStatus __instance)
        {
            if (Player.Data.IsDead || Player.Data.Disconnected) return true;

            if (PlayerControl.AllPlayerControls.ToArray().Count(x => !x.Data.IsDead && !x.Data.Disconnected) == 1)
            {
                var writer = AmongUsClient.Instance.StartRpcImmediately(
                    PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.GlitchWin,
                    SendOption.Reliable,
                    -1
                );
                writer.Write(Player.PlayerId);
                Wins();
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                Utils.EndGame();
                return false;
            }

            return false;
        }

        public void Wins()
        {
            GlitchWins = true;
        }

        public void Loses()
        {
           //Player.Data
        }

        public void Update(HudManager __instance)
        {

            Player.nameText.color = Color;

            if (MeetingHud.Instance != null)
                foreach (var player in MeetingHud.Instance.playerStates)
                    if (player.NameText != null && Player.PlayerId == player.TargetPlayerId)
                        player.NameText.color = Color;

            if (HudManager.Instance != null && HudManager.Instance.Chat != null)
                foreach (var bubble in HudManager.Instance.Chat.chatBubPool.activeChildren)
                    if (bubble.Cast<ChatBubble>().NameText != null &&
                        Player.Data.PlayerName == bubble.Cast<ChatBubble>().NameText.text)
                        bubble.Cast<ChatBubble>().NameText.color = Color;

            MimicMenuClicked();
        }
    }
}