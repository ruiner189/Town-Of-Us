using HarmonyLib;
using Hazel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Reactor.Extensions;
using TownOfUs.CrewmateRoles.MedicMod;
using TownOfUs.Extensions;
using TownOfUs.ImpostorRoles.CamouflageMod;
using TownOfUs.Roles;
using TownOfUs.Roles.Modifiers;
using UnhollowerBaseLib;
using UnityEngine;
using Object = UnityEngine.Object;
using TownOfUs.Patches.Buttons;

namespace TownOfUs
{
    [HarmonyPatch]
    public static class Utils
    {
        internal static bool ShowDeadBodies = false;

        public static Dictionary<PlayerControl, Color> oldColors = new Dictionary<PlayerControl, Color>();

        public static List<WinningPlayerData> potentialWinners = new List<WinningPlayerData>();

        public static void SetSkin(PlayerControl Player, String skin)
        {
            Player.MyPhysics.SetSkin(skin);
        }

        public static void Morph(PlayerControl target, GameData.PlayerOutfit outfit)
        {
            target.RawSetName(outfit.PlayerName);
            target.RawSetColor(outfit.ColorId);
            target.RawSetHat(outfit.HatId, outfit.ColorId);
            target.RawSetSkin(outfit.SkinId);
            target.RawSetVisor(outfit.VisorId);
            target.RawSetPet(outfit.PetId, outfit.ColorId);
        }
        /**
        public static void Morph(PlayerControl Player, PlayerControl MorphedPlayer, bool resetAnim = false)
        {
            if (CamouflageUnCamouflage.IsCamoed) return;

            if (!PlayerControl.LocalPlayer.Is(RoleEnum.Seer))
            {
                Player.nameText.text = MorphedPlayer.Data.PlayerName;
            }

            var targetAppearance = MorphedPlayer.GetDefaultAppearance();

            PlayerControl.SetPlayerMaterialColors(targetAppearance.ColorId, Player.myRend);
            Player.HatRenderer.SetHat(targetAppearance.HatId, targetAppearance.ColorId);
            Player.nameText.transform.localPosition = new Vector3(
                0f,
                Player.Data.HatId == 0U ? 1.5f : 2.0f,
                -0.5f
            );

            if (Player.MyPhysics.Skin.skin.ProdId != DestroyableSingleton<HatManager>.Instance
                .AllSkins.ToArray()[targetAppearance.SkinId].ProdId)
                SetSkin(Player, targetAppearance.SkinId);

            if (Player.CurrentPet == null || Player.CurrentPet.ProdId !=
                DestroyableSingleton<HatManager>.Instance.AllPets.ToArray()[(int)targetAppearance.PetId].ProdId)
            {
                if (Player.CurrentPet != null) Object.Destroy(Player.CurrentPet.gameObject);

                Player.CurrentPet =
                    Object.Instantiate(
                        DestroyableSingleton<HatManager>.Instance.AllPets.ToArray()[(int)targetAppearance.PetId]).PetPrefab;
                Player.CurrentPet.transform.position = Player.transform.position;
                Player.CurrentPet.Source = Player;
                Player.CurrentPet.Visible = Player.Visible;
            }

            PlayerControl.SetPlayerMaterialColors(targetAppearance.ColorId, Player.CurrentPet.rend);

        }
        */

        /*
        public static void Unmorph(PlayerControl Player)
        {
            var appearance = Player.GetDefaultAppearance();

            Player.nameText.text = Player.Data.PlayerName;
            PlayerControl.SetPlayerMaterialColors(appearance.ColorId, Player.myRend);
            Player.HatRenderer.SetHat(appearance.HatId, appearance.ColorId);
            Player.nameText.transform.localPosition = new Vector3(
                0f,
                appearance.HatId == 0U ? 1.5f : 2.0f,
                -0.5f
            );

            if (Player.MyPhysics.Skin.skin.ProdId != DestroyableSingleton<HatManager>.Instance
                .AllSkins.ToArray()[(int)appearance.SkinId].ProdId)
                SetSkin(Player, appearance.SkinId);

            if (Player.CurrentPet != null) Object.Destroy(Player.CurrentPet.gameObject);

            Player.CurrentPet =
                Object.Instantiate(
                    DestroyableSingleton<HatManager>.Instance.AllPets.ToArray()[(int)appearance.PetId]);
            Player.CurrentPet.transform.position = Player.transform.position;
            Player.CurrentPet.Source = Player;
            Player.CurrentPet.Visible = Player.Visible;

            PlayerControl.SetPlayerMaterialColors(appearance.ColorId, Player.CurrentPet.rend);
        }
        */

        public static void Camouflage()
        {
            foreach (var player in PlayerControl.AllPlayerControls)
            {
                player.nameText.text = "";
                Morph(player, new GameData.PlayerOutfit());
                PlayerControl.SetPlayerMaterialColors(Color.grey, player.myRend);
            }
        }

        public static void UnCamouflage()
        {
            foreach (var player in PlayerControl.AllPlayerControls) {
                player.Shapeshift(player, false);
            }
        }

        public static GameData.PlayerOutfit CloneOutfit(GameData.PlayerOutfit outfit)
        {
            var clone = new GameData.PlayerOutfit();
            clone.ColorId = outfit.ColorId;
            clone.HatId = outfit.HatId;
            clone.PetId = outfit.PetId;
            clone.VisorId = outfit.VisorId;
            clone.SkinId = outfit.SkinId;
            clone.PlayerName = outfit.PlayerName;
            clone._playerName = outfit._playerName;
            return clone;
        }

        public static bool IsCrewmate(this PlayerControl player)
        {
            return GetRole(player) == RoleEnum.Crewmate;
        }

        public static void AddUnique<T>(this Il2CppSystem.Collections.Generic.List<T> self, T item)
            where T : IDisconnectHandler
        {
            if (!self.Contains(item)) self.Add(item);
        }

        public static bool IsLover(this PlayerControl player)
        {
            return player.Is(ModifierEnum.Lover) || player.Is(ModifierEnum.Lover);
        }

        public static bool Is(this PlayerControl player, RoleEnum roleType)
        {
            return Role.GetRole(player)?.RoleType == roleType;
        }

        public static bool Is(this PlayerControl player, ModifierEnum modifierType)
        {
            return Modifier.GetModifier(player)?.ModifierType == modifierType;
        }

        public static bool Is(this PlayerControl player, Faction faction)
        {
            return Role.GetRole(player)?.Faction == faction;
        }

        public static List<PlayerControl> GetCrewmates(List<PlayerControl> impostors)
        {
            return PlayerControl.AllPlayerControls.ToArray().Where(
                player => !impostors.Any(imp => imp.PlayerId == player.PlayerId)
            ).ToList();
        }

        public static List<PlayerControl> GetImpostors(
            List<GameData.PlayerInfo> infected)
        {
            var impostors = new List<PlayerControl>();
            foreach (var impData in infected)
                impostors.Add(impData.Object);

            return impostors;
        }

        public static RoleEnum GetRole(PlayerControl player)
        {
            if (player == null) return RoleEnum.None;
            if (player.Data == null) return RoleEnum.None;

            var role = Role.GetRole(player);
            if (role != null) return role.RoleType;

            return player.Data.Role.IsImpostor ? RoleEnum.Impostor : RoleEnum.Crewmate;
        }

        public static PlayerControl PlayerById(byte id)
        {
            foreach (var player in PlayerControl.AllPlayerControls)
                if (player.PlayerId == id)
                    return player;

            return null;
        }

        public static bool IsShielded(this PlayerControl player)
        {
            return Role.GetRoles(RoleEnum.Medic).Any(role =>
            {
                var shieldedPlayer = ((Medic)role).ShieldedPlayer;
                return shieldedPlayer != null && player.PlayerId == shieldedPlayer.PlayerId;
            });
        }

        public static Medic GetMedic(this PlayerControl player)
        {
            return Role.GetRoles(RoleEnum.Medic).FirstOrDefault(role =>
            {
                var shieldedPlayer = ((Medic)role).ShieldedPlayer;
                return shieldedPlayer != null && player.PlayerId == shieldedPlayer.PlayerId;
            }) as Medic;
        }

        public static PlayerControl GetClosestPlayer(PlayerControl refPlayer, List<PlayerControl> AllPlayers)
        {
            var num = double.MaxValue;
            var refPosition = refPlayer.GetTruePosition();
            PlayerControl result = null;
            foreach (var player in AllPlayers)
            {
                if (player.Data.IsDead || player.PlayerId == refPlayer.PlayerId || !player.Collider.enabled) continue;
                var playerPosition = player.GetTruePosition();
                var distBetweenPlayers = Vector2.Distance(refPosition, playerPosition);
                var isClosest = distBetweenPlayers < num;
                if (!isClosest) continue;
                var vector = playerPosition - refPosition;
                if (PhysicsHelpers.AnyNonTriggersBetween(
                    refPosition, vector.normalized, vector.magnitude, Constants.ShipAndObjectsMask
                )) continue;
                num = distBetweenPlayers;
                result = player;
            }

            return result;
        }

        public static PlayerControl GetClosestPlayer(PlayerControl refplayer)
        {
            return GetClosestPlayer(refplayer, PlayerControl.AllPlayerControls.ToArray().ToList());
        }
        public static void SetTarget(
            ref PlayerControl closestPlayer,
            KillButton button,
            float maxDistance = float.NaN,
            List<PlayerControl> targets = null
        )
        {
            if (!button.isActiveAndEnabled) return;

            button.SetTarget(
                SetClosestPlayer(ref closestPlayer, maxDistance, targets)
            );
        }

        public static PlayerControl SetClosestPlayer(
            ref PlayerControl closestPlayer,
            float maxDistance = float.NaN,
            List<PlayerControl> targets = null
        )
        {
            if (float.IsNaN(maxDistance))
                maxDistance = GameOptionsData.KillDistances[PlayerControl.GameOptions.KillDistance];
            var player = GetClosestPlayer(
                PlayerControl.LocalPlayer,
                targets ?? PlayerControl.AllPlayerControls.ToArray().ToList()
            );
            var closeEnough = player == null || (
                GetDistBetweenPlayers(PlayerControl.LocalPlayer, player) < maxDistance
            );
            return closestPlayer = closeEnough ? player : null;
        }

        public static double GetDistBetweenPlayers(PlayerControl player, PlayerControl refplayer)
        {
            var truePosition = refplayer.GetTruePosition();
            var truePosition2 = player.GetTruePosition();
            return Vector2.Distance(truePosition, truePosition2);
        }

        public static void RpcMurderPlayer(PlayerControl killer, PlayerControl target)
        {
            MurderPlayer(killer, target);
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)CustomRPC.BypassKill, SendOption.Reliable, -1);
            writer.Write(killer.PlayerId);
            writer.Write(target.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }

        public static void MurderPlayer(PlayerControl killer, PlayerControl target)
        {
            var data = target.Data;
            if (data != null && !data.IsDead)
            {
                if (killer == PlayerControl.LocalPlayer)
                    SoundManager.Instance.PlaySound(PlayerControl.LocalPlayer.KillSfx, false, 0.8f);

                target.gameObject.layer = LayerMask.NameToLayer("Ghost");
                if (target.AmOwner)
                {
                    try
                    {
                        if (Minigame.Instance)
                        {
                            Minigame.Instance.Close();
                            Minigame.Instance.Close();
                        }

                        if (MapBehaviour.Instance)
                        {
                            MapBehaviour.Instance.Close();
                            MapBehaviour.Instance.Close();
                        }
                    }
                    catch
                    {
                    }

                    DestroyableSingleton<HudManager>.Instance.KillOverlay.ShowKillAnimation(killer.Data, data);
                    DestroyableSingleton<HudManager>.Instance.ShadowQuad.gameObject.SetActive(false);
                    target.nameText.GetComponent<MeshRenderer>().material.SetInt("_Mask", 0);
                    target.RpcSetScanner(false);
                    var importantTextTask = new GameObject("_Player").AddComponent<ImportantTextTask>();
                    importantTextTask.transform.SetParent(AmongUsClient.Instance.transform, false);
                    if (!PlayerControl.GameOptions.GhostsDoTasks)
                    {
                        for (var i = 0; i < target.myTasks.Count; i++)
                        {
                            var playerTask = target.myTasks.ToArray()[i];
                            playerTask.OnRemove();
                            Object.Destroy(playerTask.gameObject);
                        }

                        target.myTasks.Clear();
                        importantTextTask.Text = DestroyableSingleton<TranslationController>.Instance.GetString(
                            StringNames.GhostIgnoreTasks,
                            new Il2CppReferenceArray<Il2CppSystem.Object>(0));
                    }
                    else
                    {
                        importantTextTask.Text = DestroyableSingleton<TranslationController>.Instance.GetString(
                            StringNames.GhostDoTasks,
                            new Il2CppReferenceArray<Il2CppSystem.Object>(0));
                    }

                    target.myTasks.Insert(0, importantTextTask);
                }

                killer.MyPhysics.StartCoroutine(killer.KillAnimations.Random().CoPerformKill(killer, target));
                var deadBody = new DeadPlayer
                {
                    PlayerId = target.PlayerId,
                    KillerId = killer.PlayerId,
                    KillTime = DateTime.UtcNow
                };

                Murder.KilledPlayers.Add(deadBody);
                
                if (!killer.AmOwner) return;

                if (target.Is(ModifierEnum.Diseased) && killer.Is(RoleEnum.Glitch))
                {
                    var glitch = Role.GetRole<Glitch>(killer);
                    glitch.Player.SetKillTimer(CustomGameOptions.GlitchKillCooldown * 3);
                    return;
                }

                if (target.Is(ModifierEnum.Diseased) && killer.Data.Role.IsImpostor)
                {
                    killer.SetKillTimer(PlayerControl.GameOptions.KillCooldown * 3);
                    return;
                }

                if (killer.Is(RoleEnum.Underdog))
                {
                    //killer.SetKillTimer(PlayerControl.GameOptions.KillCooldown * (PerformKill.LastImp() ? 0.5f : 1.5f));
                    return;
                }

                if (killer.Data.Role.IsImpostor)
                {
                    killer.SetKillTimer(PlayerControl.GameOptions.KillCooldown);
                }
            }
        }

        public static void Hack(PlayerControl glitch, PlayerControl target)
        {
            if (PlayerControl.LocalPlayer != target) return;
            Glitch glitchRole = Role.GetRole<Glitch>(glitch);
            glitchRole.HackedPlayer = target;
            glitchRole.HackIcons.Clear();
            var LockedButtons = new List<ActionButton>();
            foreach (ModdedButton moddedButton in ModdedButton.GetAllModdedButtonsFromPlayer(target))
            {
                var lockImg = new GameObject();
                lockImg.layer = 5;
                var lockImgR = lockImg.AddComponent<SpriteRenderer>();
                lockImgR.sprite = Glitch.LockSprite;

                var pos = moddedButton.Button.gameObject.transform.position;
                lockImg.transform.position = new Vector3(pos.x, pos.y, -50);

                moddedButton.IsLocked = true;
                LockedButtons.Add(moddedButton.Button);
                glitchRole.HackIcons.Add(new Tuple<GameObject,ActionButton>(lockImg, moddedButton.Button));
            }

            var vanillaButtons = new List<ActionButton>();
            ActionButton b = HudManager.Instance.UseButton;
            if (b != null && !LockedButtons.Contains(b))
                vanillaButtons.Add(b);
            b = HudManager.Instance.ReportButton;
            if (b != null && !LockedButtons.Contains(b))
                vanillaButtons.Add(b);
            b = HudManager.Instance.KillButton;
            if (b != null && !LockedButtons.Contains(b))
                vanillaButtons.Add(b);
            b = HudManager.Instance.SabotageButton;
            if (b != null && !LockedButtons.Contains(b))
                vanillaButtons.Add(b);
            b = HudManager.Instance.ImpostorVentButton;
            if (b != null && !LockedButtons.Contains(b))
                vanillaButtons.Add(b);

            foreach (var vanillaButton in vanillaButtons)
            {
                if (vanillaButton.gameObject.active)
                {
                    var lockImg = new GameObject();
                    lockImg.layer = 5;
                    var lockImgR = lockImg.AddComponent<SpriteRenderer>();
                    lockImgR.sprite = Glitch.LockSprite;

                    var pos = vanillaButton.gameObject.transform.position;
                    lockImg.transform.position = new Vector3(pos.x, pos.y, -50);
                    glitchRole.HackIcons.Add(new Tuple<GameObject, ActionButton>(lockImg, vanillaButton));

                    vanillaButton.SetDisabled();
                }
            }
        }

        public static void RemoveHack(PlayerControl glitch, PlayerControl target)
        {
            if (PlayerControl.LocalPlayer != target) return;
            Glitch glitchRole = Role.GetRole<Glitch>(glitch);
            foreach (ModdedButton moddedButton in ModdedButton.GetAllModdedButtonsFromPlayer(target))
            {
                moddedButton.IsLocked = false;
            }

            var vanillaButtons = new List<ActionButton>();
            ActionButton b = HudManager.Instance.UseButton;
            if (b != null)
                vanillaButtons.Add(b);
            b = HudManager.Instance.ReportButton;
            if (b != null)
                vanillaButtons.Add(b);
            b = HudManager.Instance.KillButton;
            if (b != null)
                vanillaButtons.Add(b);
            b = HudManager.Instance.SabotageButton;
            if (b != null)
                vanillaButtons.Add(b);
            b = HudManager.Instance.ImpostorVentButton;
            if (b != null)
                vanillaButtons.Add(b);

            foreach (var vanillaButton in vanillaButtons)
            {
                if (vanillaButton.gameObject.active)
                {
                    if (!vanillaButton.isCoolingDown)
                        vanillaButton.SetEnabled();
                }
            }

            foreach (var tuple in glitchRole.HackIcons)
            {
                tuple.Item1.Destroy();
            }
            glitchRole.HackedPlayer = null;
            glitchRole.HackIcons.Clear();
        }

        public static IEnumerator FlashCoroutine(Color color, float waitfor = 1f, float alpha = 0.3f)
        {
            color.a = alpha;
            if (HudManager.InstanceExists && HudManager.Instance.FullScreen)
            {
                var fullscreen = DestroyableSingleton<HudManager>.Instance.FullScreen;
                var oldcolour = fullscreen.color;
                fullscreen.enabled = true;
                fullscreen.color = color;
            }

            yield return new WaitForSeconds(waitfor);

            if (HudManager.InstanceExists && HudManager.Instance.FullScreen)
            {
                var fullscreen = DestroyableSingleton<HudManager>.Instance.FullScreen;
                fullscreen.enabled = false;
            }
        }

        public static IEnumerable<(T1, T2)> Zip<T1, T2>(List<T1> first, List<T2> second)
        {
            return first.Zip(second, (x, y) => (x, y));
        }

        public static void DestroyAll(this IEnumerable<Component> listie)
        {
            foreach (var item in listie)
            {
                if (item == null) continue;
                Object.Destroy(item);
                if (item.gameObject == null) return;
                Object.Destroy(item.gameObject);
            }
        }

        public static void EndGame(GameOverReason reason = GameOverReason.ImpostorByVote, bool showAds = false)
        {
            ShipStatus.RpcEndGame(reason, showAds);
        }
    }
}
