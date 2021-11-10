using HarmonyLib;
using Il2CppSystem.IO;
using Reactor;
using System;
using System.Collections.Generic;
using System.Text;
using TownOfUs.CustomOption;

namespace TownOfUs.Patches.CustomOption
{
    public class MenuLoader {
        public static CustomMenu ReduxMenu;
        public static readonly String VanillaGameName = "GameTab";
        public static readonly String VanillaRoleName = "RoleTab";
        public static readonly String ReduxMenuName = "Redux Menu";
        public static bool MenuOpen = false;
        public static bool IsVanillaRoleTabEnabled = false;
    }
    [HarmonyPatch(typeof(GameSettingMenu), nameof(GameSettingMenu.Start))]
    public static class GameSettingsStart
    {
        private static GameSettingMenu _lastInstance;
        public static void Prefix(GameSettingMenu __instance)
        {
            if (__instance == _lastInstance) return;
            if (_lastInstance != null && _lastInstance.isActiveAndEnabled) return;
            MenuLoader.MenuOpen = true;
            _lastInstance = __instance;
            PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"Loading Menus");
            MenuLoader.ReduxMenu = new CustomMenu(MenuLoader.ReduxMenuName, 0, TownOfUs.ReduxLogo);
            if (!MenuLoader.IsVanillaRoleTabEnabled)
                CustomMenu.VanillaRoleMenu.SetActive(false);
            PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"Loading Complete");
        }
    }

    [HarmonyPatch(typeof(GameSettingMenu), nameof(GameSettingMenu.Close))]

    public static class GameSettingsClose
    {
        public static void Prefix(GameSettingMenu __instance)
        {
            MenuLoader.MenuOpen = false;
        }
    }
    //
    [HarmonyPatch(typeof(OptionsConsole), nameof(OptionsConsole.Use))]
    public static class OptionsConsoleUse
    {
        public static bool Prefix(OptionsConsole __instance)
        {
            return !MenuLoader.MenuOpen;
        }
    }

    [HarmonyPatch(typeof(RoleOptionsData), nameof(RoleOptionsData.Deserialize), new Type[] { typeof(BinaryReader)})]
    public static class RemoveVanillaRoles
    {
        public static bool Prefix(BinaryReader reader, ref RoleOptionsData __result)
        {
            RoleOptionsData roleOptionsData = new RoleOptionsData();
            int num = reader.ReadInt32();
            for (int i = 0; i < num; i++)
            {
                RoleTypes key = (RoleTypes)reader.ReadInt16();
                RoleOptionsData.RoleRate value = new RoleOptionsData.RoleRate
                {


                    MaxCount = (int)reader.ReadByte() * 0,
                    Chance = (int)reader.ReadByte() * 0
                };
                roleOptionsData.roleRates[key] = value;
            }
            roleOptionsData.ShapeshifterLeaveSkin = reader.ReadBoolean();
            roleOptionsData.ShapeshifterCooldown = (float)reader.ReadByte();
            roleOptionsData.ShapeshifterDuration = (float)reader.ReadByte();
            roleOptionsData.ScientistCooldown = (float)reader.ReadByte();
            roleOptionsData.GuardianAngelCooldown = (float)reader.ReadByte();
            roleOptionsData.EngineerCooldown = (float)reader.ReadByte();
            roleOptionsData.EngineerInVentMaxTime = (float)reader.ReadByte();
            roleOptionsData.ScientistBatteryCharge = (float)reader.ReadByte();
            roleOptionsData.ProtectionDurationSeconds = (float)reader.ReadByte();
            roleOptionsData.ImpostorsCanSeeProtect = reader.ReadBoolean();
            __result = roleOptionsData;
            return false;
        }
    }
}
