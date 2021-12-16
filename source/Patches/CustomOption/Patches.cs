using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Reactor;
using Reactor.Extensions;
using TownOfUs.Extensions;
using TownOfUs.Patches.CustomOption;
using UnhollowerBaseLib;
using UnityEngine;

namespace TownOfUs.CustomOption
{
    public static class Patches
    {
        public static Export ExportButton;
        public static Import ImportButton;
        public static List<OptionBehaviour> DefaultOptions;
        public static float LobbyTextRowHeight { get; set; } = 0.081F;


        private static void CreateOptions(GameOptionsMenu __instance)
        {
            var togglePrefab = CustomOption.GetTogglePrefab();
            var numberPrefab = CustomOption.GetNumberPrefab();
            var stringPrefab = CustomOption.GetStringPrefab();

            string[] MenuNames =
            {
                MenuLoader.VanillaGameName,
                MenuLoader.VanillaRoleName,
                MenuLoader.ReduxMenuName,
                "Custom"
            };
            foreach (var name in MenuNames)
            {
                if (CustomMenu.MenuChildren.ContainsKey(name))
                    CustomMenu.MenuChildren.Remove(name);
                CustomMenu.MenuChildren.Add(name, new List<OptionBehaviour>());
            }

            if (ExportButton.Setting != null)
            {
                ExportButton.Setting.gameObject.SetActive(true);
                CustomMenu.MenuChildren.GetValueOrDefault(MenuLoader.VanillaGameName).Add(ExportButton.Setting);
            }
            else
            {
                var toggle = Object.Instantiate(togglePrefab, togglePrefab.transform.parent);
                toggle.transform.GetChild(2).gameObject.SetActive(false);
                toggle.transform.GetChild(0).localPosition += new Vector3(1f, 0f, 0f);

                ExportButton.Setting = toggle;
                ExportButton.OptionCreated();
                CustomMenu.MenuChildren.GetValueOrDefault(MenuLoader.VanillaGameName).Add(toggle);
            }

            if (ImportButton.Setting != null)
            {
                ImportButton.Setting.gameObject.SetActive(true);
                CustomMenu.MenuChildren.GetValueOrDefault(MenuLoader.VanillaGameName).Add(ImportButton.Setting);
            }
            else
            {
                var toggle = Object.Instantiate(togglePrefab, togglePrefab.transform.parent);
                toggle.transform.GetChild(2).gameObject.SetActive(false);
                toggle.transform.GetChild(0).localPosition += new Vector3(1f, 0f, 0f);

                ImportButton.Setting = toggle;
                ImportButton.OptionCreated();
                CustomMenu.MenuChildren.GetValueOrDefault(MenuLoader.VanillaGameName).Add(toggle);
            }

            DefaultOptions = __instance.Children.ToList();
            foreach (var defaultOption in __instance.Children)
                CustomMenu.MenuChildren.GetValueOrDefault(MenuLoader.VanillaGameName).Add(defaultOption);

            foreach (var option in CustomOption.AllOptions)
            {
                if (option.Setting != null)
                {
                    CustomMenu.MenuChildren.GetValueOrDefault(option.MenuName).Add(option.Setting);
                    continue;
                }

                switch (option.Type)
                {
                    case CustomOptionType.Header:
                        var toggle = Object.Instantiate(togglePrefab, togglePrefab.transform.parent);
                        toggle.transform.GetChild(1).gameObject.SetActive(false);
                        toggle.transform.GetChild(2).gameObject.SetActive(false);
                        option.Setting = toggle;
                        CustomMenu.MenuChildren.GetValueOrDefault(option.MenuName).Add(toggle);
                        break;
                    case CustomOptionType.Toggle:
                        var toggle2 = Object.Instantiate(togglePrefab, togglePrefab.transform.parent);
                        option.Setting = toggle2;
                        CustomMenu.MenuChildren.GetValueOrDefault(option.MenuName).Add(toggle2);
                        break;
                    case CustomOptionType.Tab:
                        var tab = Object.Instantiate(togglePrefab, togglePrefab.transform.parent);
                        tab.transform.GetChild(2).gameObject.SetActive(false);
                        tab.transform.GetChild(0).localPosition += new Vector3(1f, 0f, 0f);
                        option.Setting = tab;
                        CustomMenu.MenuChildren.GetValueOrDefault(option.MenuName).Add(tab);
                        break;
                    case CustomOptionType.Number:
                        var number = Object.Instantiate(numberPrefab, numberPrefab.transform.parent);
                        option.Setting = number;
                        CustomMenu.MenuChildren.GetValueOrDefault(option.MenuName).Add(number);
                        break;
                    case CustomOptionType.String:
                        var str = Object.Instantiate(stringPrefab, stringPrefab.transform.parent);
                        option.Setting = str;
                        CustomMenu.MenuChildren.GetValueOrDefault(option.MenuName).Add(str);
                        break;
                }
                if(option.MenuName == MenuLoader.VanillaGameName) option.Setting.gameObject.SetActive(true);
                else option.Setting.gameObject.SetActive(false);

                option.OptionCreated();
            }
        }
        private static bool OnEnable(OptionBehaviour opt)
        {
            if (opt == ExportButton.Setting)
            {
                ExportButton.OptionCreated();
                return false;
            }

            if (opt == ImportButton.Setting)
            {
                ImportButton.OptionCreated();
                return false;
            }


            var customOption =
                CustomOption.AllOptions.FirstOrDefault(option =>
                    option.Setting == opt); // Works but may need to change to gameObject.name check

            if (customOption == null)
            {
                customOption = ExportButton.SlotButtons.FirstOrDefault(option => option.Setting == opt);
                if (customOption == null)
                {
                    customOption = ImportButton.SlotButtons.FirstOrDefault(option => option.Setting == opt);
                    if (customOption == null) return true;
                }
            }

            customOption.OptionCreated();

            return false;
        }


        [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Start))]
        private class GameOptionsMenu_Start
        {
            private static GameOptionsMenu _lastInstance;
            public static void Postfix(GameOptionsMenu __instance)
            {
                PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"GameOptionsMenu_Start");
                if (__instance == _lastInstance) return;
                if (_lastInstance != null && _lastInstance.isActiveAndEnabled) return; 
                _lastInstance = __instance;
                CreateOptions(__instance);
                PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"GameOptionsMenu_Start C");
                var customOptions = CustomMenu.MenuChildren.GetValueOrDefault(MenuLoader.VanillaGameName);
                PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"GameOptionsMenu_Start");
                PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"Values: {customOptions} {customOptions.Count}");
                var y = __instance.GetComponentsInChildren<OptionBehaviour>()
                    .Max(option => option.transform.localPosition.y);
                var x = __instance.Children[1].transform.localPosition.x;
                var z = __instance.Children[1].transform.localPosition.z;
                var i = 0;

                foreach (var option in customOptions)
                    option.transform.localPosition = new Vector3(x, y - i++ * 0.5f, z);

                __instance.Children = new Il2CppReferenceArray<OptionBehaviour>(customOptions.ToArray());
            }
        }

        [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Update))]
        private class GameOptionsMenu_Update
        {
            public static void Postfix(GameOptionsMenu __instance)
            {
                if (__instance.Children.Length == 0) return;
                if (__instance.GetComponentsInChildren<OptionBehaviour>().Length == 0) return;
                var y = __instance.GetComponentsInChildren<OptionBehaviour>()
                    .Max(option => option.transform.localPosition.y);
                float x, z;
                if (__instance.Children.Length == 1)
                {
                    x = __instance.Children[0].transform.localPosition.x;
                    z = __instance.Children[0].transform.localPosition.z;
                }
                else
                {
                    x = __instance.Children[1].transform.localPosition.x;
                    z = __instance.Children[1].transform.localPosition.z;
                }

                var i = 0;
                foreach (var option in __instance.Children)
                    option.transform.localPosition = new Vector3(x, y - i++ * 0.5f, z);
            }
        }

        [HarmonyPatch(typeof(ToggleOption), nameof(ToggleOption.OnEnable))]
        private static class ToggleOption_OnEnable
        {
            private static bool Prefix(ToggleOption __instance)
            {
                return OnEnable(__instance);
            }
        }

        [HarmonyPatch(typeof(NumberOption), nameof(NumberOption.OnEnable))]
        private static class NumberOption_OnEnable
        {
            private static bool Prefix(NumberOption __instance)
            {
                return OnEnable(__instance);
            }
        }

        [HarmonyPatch(typeof(StringOption), nameof(StringOption.OnEnable))]
        private static class StringOption_OnEnable
        {
            private static bool Prefix(StringOption __instance)
            {
                return OnEnable(__instance);
            }
        }


        [HarmonyPatch(typeof(ToggleOption), nameof(ToggleOption.Toggle))]
        private class ToggleButtonPatch
        {
            public static bool Prefix(ToggleOption __instance)
            {
                var option =
                    CustomOption.AllOptions.FirstOrDefault(option =>
                        option.Setting == __instance); // Works but may need to change to gameObject.name check
                if (option is CustomToggleOption toggle)
                {
                    toggle.Toggle();
                    return false;
                }

                if(option is CustomTabOption tab)
                {
                    tab.Action();
                    return false;
                }

                if (__instance == ExportButton.Setting)
                {
                    if (!AmongUsClient.Instance.AmHost) return false;
                    ExportButton.Do();
                    return false;
                }

                if (__instance == ImportButton.Setting)
                {
                    if (!AmongUsClient.Instance.AmHost) return false;
                    ImportButton.Do();
                    return false;
                }


                if (option is CustomHeaderOption) return false;

                CustomOption option2 = ExportButton.SlotButtons.FirstOrDefault(option => option.Setting == __instance);
                if (option2 is CustomButtonOption button)
                {
                    if (!AmongUsClient.Instance.AmHost) return false;
                    button.Do();
                    return false;
                }

                CustomOption option3 = ImportButton.SlotButtons.FirstOrDefault(option => option.Setting == __instance);
                if (option3 is CustomButtonOption button2)
                {
                    if (!AmongUsClient.Instance.AmHost) return false;
                    button2.Do();
                    return false;
                }
                var backButton = CustomTabOption.AllBackButtons.FirstOrDefault(option => option.Setting == __instance);
                if(backButton != null)
                {
                    backButton.Do();
                    return false;
                }
                

                return true;
            }
        }

        [HarmonyPatch(typeof(NumberOption), nameof(NumberOption.Increase))]
        private class NumberOptionPatchIncrease
        {
            public static bool Prefix(NumberOption __instance)
            {
                var option =
                    CustomOption.AllOptions.FirstOrDefault(option =>
                        option.Setting == __instance); // Works but may need to change to gameObject.name check
                if (option is CustomNumberOption number)
                {
                    number.Increase();
                    return false;
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(NumberOption), nameof(NumberOption.Decrease))]
        private class NumberOptionPatchDecrease
        {
            public static bool Prefix(NumberOption __instance)
            {
                var option =
                    CustomOption.AllOptions.FirstOrDefault(option =>
                        option.Setting == __instance); // Works but may need to change to gameObject.name check
                if (option is CustomNumberOption number)
                {
                    number.Decrease();
                    return false;
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(StringOption), nameof(StringOption.Increase))]
        private class StringOptionPatchIncrease
        {
            public static bool Prefix(StringOption __instance)
            {
                var option =
                    CustomOption.AllOptions.FirstOrDefault(option =>
                        option.Setting == __instance); // Works but may need to change to gameObject.name check
                if (option is CustomStringOption str)
                {
                    str.Increase();
                    return false;
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(StringOption), nameof(StringOption.Decrease))]
        private class StringOptionPatchDecrease
        {
            public static bool Prefix(StringOption __instance)
            {
                var option =
                    CustomOption.AllOptions.FirstOrDefault(option =>
                        option.Setting == __instance); // Works but may need to change to gameObject.name check
                if (option is CustomStringOption str)
                {
                    str.Decrease();
                    return false;
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcSyncSettings))]
        private class PlayerControlPatch
        {
            public static void Postfix()
            {
                if (PlayerControl.AllPlayerControls.Count < 2 || !AmongUsClient.Instance ||
                    !PlayerControl.LocalPlayer || !AmongUsClient.Instance.AmHost) return;

                Rpc.SendRpc();
            }
        }

        [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
        private class HudManagerUpdate
        {
            private const float
                MinX = -5.233334F /*-5.3F*/,
                OriginalY = 2.9F,
                MinY = 3F; // Differs to cause excess options to appear cut off to encourage scrolling

            private static Scroller Scroller;
            private static Vector3 LastPosition = new Vector3(MinX, MinY);

            public static void Prefix(HudManager __instance)
            {
                if (__instance.GameSettings?.transform == null) return;


                // Scroller disabled
                if (!CustomOption.LobbyTextScroller)
                {
                    // Remove scroller if disabled late
                    if (Scroller != null)
                    {
                        __instance.GameSettings.transform.SetParent(Scroller.transform.parent);
                        __instance.GameSettings.transform.localPosition = new Vector3(MinX, OriginalY);

                        Object.Destroy(Scroller);
                    }

                    return;
                }

                CreateScroller(__instance);

                Scroller.gameObject.SetActive(__instance.GameSettings.gameObject.activeSelf);

                if (!Scroller.gameObject.active) return;

                var rows = __instance.GameSettings.text.Count(c => c == '\n');
                var maxY = Mathf.Max(MinY, rows * LobbyTextRowHeight + (rows - 38) * LobbyTextRowHeight);

                Scroller.ContentYBounds = new FloatRange(MinY, maxY);

                // Prevent scrolling when the player is interacting with a menu
                if (PlayerControl.LocalPlayer?.CanMove != true)
                {
                    __instance.GameSettings.transform.localPosition = LastPosition;

                    return;
                }

                if (__instance.GameSettings.transform.localPosition.x != MinX ||
                    __instance.GameSettings.transform.localPosition.y < MinY) return;

                LastPosition = __instance.GameSettings.transform.localPosition;
            }

            private static void CreateScroller(HudManager __instance)
            {
                if (Scroller != null) return;

                Scroller = new GameObject("SettingsScroller").AddComponent<Scroller>();
                Scroller.transform.SetParent(__instance.GameSettings.transform.parent);
                Scroller.gameObject.layer = 5;

                Scroller.transform.localScale = Vector3.one;
                Scroller.allowX = false;
                Scroller.allowY = true;
                Scroller.active = true;
                Scroller.velocity = new Vector2(0, 0);
                Scroller.ContentYBounds = new FloatRange(0, 0);
                Scroller.ContentXBounds = new FloatRange(MinX, MinX);
                Scroller.enabled = true;

                Scroller.Inner = __instance.GameSettings.transform;
                __instance.GameSettings.transform.SetParent(Scroller.transform);
            }
        }
    }
}