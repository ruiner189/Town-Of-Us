using HarmonyLib;
using Il2CppSystem.Collections.Generic;
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

            var modifier = Modifier.GetModifier(PlayerControl.LocalPlayer);
            if (modifier != null)
            {
                foreach (var player in modifier.GetTeammates())
                {
                    modifierTeammates.Add(player);
                }
            }

            var role = Role.GetRole(PlayerControl.LocalPlayer);
            if (role != null)
            {
                foreach (var player in role.GetTeammates())
                {
                    roleTeammates.Add(player);
                }
            }

            var allTeammates = modifierTeammates;
            if (modifier == null) allTeammates = roleTeammates;
            else if (!(modifier.GetType() == typeof(Lover) && role.Faction == Faction.Crewmates))
            {
                foreach (var player in roleTeammates)
                {
                    if (allTeammates.Contains(player)) continue;
                    allTeammates.Add(player);
                }
            }

            return allTeammates;
        }

        [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.BeginCrewmate))]
        public static class IntroCutscene_BeginCrewmate
        {
            public static void Postfix(IntroCutscene __instance)
            {
                var modifier = Modifier.GetModifier(PlayerControl.LocalPlayer);
                if (modifier != null)
                    ModifierText = Object.Instantiate(__instance.Title, __instance.Title.transform.parent, false);
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
                    ModifierText = Object.Instantiate(__instance.Title, __instance.Title.transform.parent, false);
                else
                    ModifierText = null;
                Lights.SetLights();
            }
        }

        [HarmonyPatch(typeof(IntroCutscene._CoBegin_d__14), nameof(IntroCutscene._CoBegin_d__14.MoveNext))]
        public static class IntroCutscene_CoBegin__d_MoveNext
        {
            public static float TestScale;

            public static void Prefix(IntroCutscene._CoBegin_d__14 __instance)
            {
                var teammates = GetTeammates();
                __instance.yourTeam = teammates;
            }

            public static void Postfix(IntroCutscene._CoBegin_d__14 __instance)
            {
                var role = Role.GetRole(PlayerControl.LocalPlayer);

                var alpha = __instance.__4__this.Title.color.a;
                if (role != null && !role.Hidden)
                {
                    __instance.__4__this.Title.text = role.Name;
                    __instance.__4__this.Title.color = role.Color;
                    __instance.__4__this.ImpostorText.text = role.ImpostorText();
                    __instance.__4__this.ImpostorText.gameObject.SetActive(true);
                    __instance.__4__this.BackgroundBar.material.color = role.Color;
                }

                if (ModifierText != null)
                {
                    var modifier = Modifier.GetModifier(PlayerControl.LocalPlayer);
                    if(modifier.GetType() == typeof(Lover))
                    {
                        ModifierText.text = $"<size=3>Modifier: {modifier.TaskText()}</size>";
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
