using HarmonyLib;
using Il2CppSystem.Collections.Generic;
using Reactor;
using System.Collections;
using TMPro;
using TownOfUs.Roles;
using TownOfUs.Roles.Modifiers;
using UnityEngine;

namespace TownOfUs.Patches
{
    public static class IntroCutScenePatch
    {
        public static TextMeshPro ModifierText;

        public static float Scale;

        public static List<PlayerControl> GetTeammates()
        {
            var roleTeammates = new List<PlayerControl>();
            var modifierTeammates = new List<PlayerControl>();
            var localPlayer = PlayerControl.LocalPlayer;

            var modifier = Modifier.GetModifier(PlayerControl.LocalPlayer);
            if (modifier != null)
            {
                foreach (var player in modifier.GetTeammates())
                {
                    if (player == localPlayer) continue;
                    modifierTeammates.Add(player);
                }
            }

            var role = Role.GetRole(PlayerControl.LocalPlayer);
            if (role != null)
            {
                foreach (var player in role.GetTeammates())
                {
                    if (player == localPlayer) continue;
                    roleTeammates.Add(player);
                }
            }

            var allTeammates = new List<PlayerControl>();
            allTeammates.Add(localPlayer);
            foreach (var player in modifierTeammates)
                allTeammates.Add(player);
            foreach (var player in roleTeammates)
                allTeammates.Add(player);

            return allTeammates;
        }

        [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.SetUpRoleText))]
        public static class SetModifierText
        {
            public static bool Prefix(IntroCutscene __instance)
            {
                var role = Role.GetRole(PlayerControl.LocalPlayer);
                __instance.RoleText.text = role.Name;
                __instance.RoleBlurbText.text = role.TaskText();
                __instance.RoleText.color = role.Color;
                __instance.YouAreText.color = role.Color;
                __instance.RoleBlurbText.color = role.Color;
                return false;
            }
        }


        [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.BeginCrewmate))]
        public static class IntroCutscene_BeginCrewmate
        {
            public static void Postfix(IntroCutscene __instance)
            {
                var modifier = Modifier.GetModifier(PlayerControl.LocalPlayer);
                if (modifier != null)
                    ModifierText = Object.Instantiate(__instance.TeamTitle, __instance.TeamTitle.transform.parent, false);
                else
                    ModifierText = null;
                Lights.SetLights();
            }
        }

        [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.BeginImpostor))]
        public static class IntroCutscene_BeginImpostor
        {
            public static void Postfix(IntroCutscene __instance)
            {
                var modifier = Modifier.GetModifier(PlayerControl.LocalPlayer);
                if (modifier != null)
                    ModifierText = Object.Instantiate(__instance.TeamTitle, __instance.TeamTitle.transform.parent, false);
                else
                    ModifierText = null;
                Lights.SetLights();
            }
        }

        [HarmonyPatch(typeof(IntroCutscene._CoBegin_d__18), nameof(IntroCutscene._CoBegin_d__18.MoveNext))]
        public static class IntroCutscene_CoBegin__d_MoveNext
        {
            public static float TestScale;

            public static void Prefix(IntroCutscene._CoBegin_d__18 __instance)
            {
                var teammates = GetTeammates();
                __instance.yourTeam = teammates;
            }

            public static void Postfix(IntroCutscene._CoBegin_d__18 __instance)
            {
                var role = Role.GetRole(PlayerControl.LocalPlayer);

                var alpha = __instance.__4__this.TeamTitle.color.a;
                if (role != null && !role.Hidden)
                {
                    __instance.__4__this.TeamTitle.text = role.FactionString();
                    __instance.__4__this.TeamTitle.color = role.FactionTitleColor();
                   // __instance.__4__this.ImpostorText.text = role.ImpostorText();
                   // __instance.__4__this.ImpostorText.gameObject.SetActive(true);
                   // __instance.__4__this.BackgroundBar.material.color = role.Color;
                }

                if (ModifierText != null)
                {
                    var modifier = Modifier.GetModifier(PlayerControl.LocalPlayer);
                    if(modifier.GetType() == typeof(Lover))
                    {
                        ModifierText.text = $"<size=3>{modifier.TaskText()}</size>";
                        ModifierText.color = modifier.Color;
                        ModifierText.transform.position =
                            __instance.__4__this.transform.position - new Vector3(0f, -1f, 0f);
                    } else {
                        ModifierText.text = "<size=4>Modifier: " + modifier.Name + "</size>";
                        ModifierText.color = modifier.Color;

                        ModifierText.transform.position =
                            __instance.__4__this.transform.position - new Vector3(0f, 2.0f, 0f);
                    }

                    ModifierText.gameObject.SetActive(true);
                }
            }
        }
    }
}
