using HarmonyLib;
using Reactor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TownOfUs.Patches.CustomOption;
using TownOfUs.Utility;
using UnhollowerBaseLib;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TownOfUs.CustomOption
{
    public class CustomMenu
    {
        public static List<CustomMenu> AllMenus = new List<CustomMenu>();
        public GameObject Tab;
        public PassiveButton Button;
        public SpriteRenderer Highlight;
        public readonly String Name;
        public readonly Sprite Sprite;
        public readonly bool IsVanilla;
        public readonly bool IsRoleTab;
        public static bool isOnSettings = true;
        public int Offset;

        public static GameObject RoleTabPrefab;
        public static GameObject GameTabPrefab;

        public static CustomMenu VanillaRoleMenu;
        public static CustomMenu VanillaGameSettingMenu;
        public static CustomMenu _currentMenu;


        public static Dictionary<String, List<OptionBehaviour>> MenuChildren = new Dictionary<String, List<OptionBehaviour>>();
        public CustomMenu(String name, int offset, Sprite sprite = null, GameObject tab = null, bool isRoleTab = false, bool isVanilla = false)
        {
            Name = name;
            Sprite = sprite;
            Tab = tab;
            IsVanilla = isVanilla;
            IsRoleTab = isRoleTab;
            Offset = offset;
            if(!MenuChildren.ContainsKey(Name))
                MenuChildren.Add(Name, new List<OptionBehaviour>());
            SetVanillaMenus();
            if (isRoleTab) GenerateAsRolesTab();
            else GenerateAsSettingsTab();
            AddMenu(this);
        }
        public CustomMenu(GameObject tab, bool isRoleTab, bool isVanilla = true)
        {
            IsVanilla = isVanilla;
            IsRoleTab = isRoleTab;
            Tab = tab;
            Name = tab.name;
            if (!MenuChildren.ContainsKey(Name))
                MenuChildren.Add(Name, new List<OptionBehaviour>());
            if (isRoleTab) GenerateAsRolesTab();
            else GenerateAsSettingsTab();
            AddMenu(this);
        }
        public static void AddMenu(CustomMenu menu)
        {
            List<CustomMenu> ToRemove = new List<CustomMenu>();
            foreach (var m in AllMenus)
            {
                if (menu.Name == m.Name)
                {
                    ToRemove.Add(m);
                }
            }
            foreach (var m in ToRemove) AllMenus.Remove(m);
            AllMenus.Add(menu);
        }
        public CustomMenu GetMenuByName(String name)
        {
            foreach(var menu in AllMenus)
            {
                if (menu.Name == name) return menu;
            }
            return null;
        }

        public CustomMenu GetCurrentMenu()
        {
            return GetMenuByName(_currentMenu.Name);
        }

        public void SetActive(bool active)
        {
            Tab.gameObject.SetActive(active);
        }

        public void SetParent(GameSettingMenu gameSettingMenu)
        {
            Tab.transform.parent = gameSettingMenu.transform;
        }

        public static void SetVanillaMenus()
        {
            if (VanillaRoleMenu == null || VanillaRoleMenu.Tab != GameObject.Find("RoleTab"))
            {
                RoleTabPrefab = GameObject.Find("RoleTab");
                VanillaRoleMenu = new CustomMenu(RoleTabPrefab, true);
            }
            if (VanillaGameSettingMenu == null || VanillaGameSettingMenu.Tab != GameObject.Find("GameTab"))
            {
                GameTabPrefab = GameObject.Find("GameTab");
                VanillaGameSettingMenu = new CustomMenu(GameTabPrefab, false);
                _currentMenu = VanillaGameSettingMenu;
            }
        }

        public void GenerateAsRolesTab()
        {
            Generate(RoleTabPrefab, "Hat Button");
        }

        public void GenerateAsSettingsTab()
        {
            Generate(GameTabPrefab, "ColorButton");
        }

        public Vector3 GetDefaultOffset()
        {
            if (VanillaRoleMenu == null || VanillaRoleMenu.Tab != GameObject.Find("RoleTab"))
            {
                RoleTabPrefab = GameObject.Find("RoleTab");
            }
            if (VanillaGameSettingMenu == null || VanillaGameSettingMenu.Tab != GameObject.Find("GameTab"))
            {
                GameTabPrefab = GameObject.Find("GameTab");
            }
            return GameTabPrefab.transform.localPosition - RoleTabPrefab.transform.localPosition;
        }
        public void Generate(GameObject prefab, String ButtonName)
        {
            if(Tab == null)
            {
                Tab = Object.Instantiate(prefab, prefab.transform.parent);
                Tab.name = Name;
                Tab.gameObject.name = Name;
                var pos = Tab.transform.localPosition;
                int trueOffset = Offset;
                if(trueOffset >= 0)
                {
                    if (MenuLoader.IsVanillaRoleTabEnabled)
                        trueOffset++;
                } else
                {
                    trueOffset--;
                }

                Tab.transform.localPosition = new Vector3(-trueOffset * GetDefaultOffset().x, pos.y, pos.z);
                if(Sprite != null) SetSprite(Sprite);
            }
            if(Button == null && Tab != null)
            {
                Button = Tab.transform
                    .FindChild(ButtonName)?
                    .FindChild("Tab Background")?
                    .GetComponent<PassiveButton>();
            }
            if(Highlight == null && Tab != null)
            {
                Highlight = Tab.transform
                  .FindChild(ButtonName)?
                  .FindChild("Tab Background")?
                  .GetComponent<SpriteRenderer>();
                if (!IsVanilla)
                {
                    HighlightTab(false);
                }
            }
        }

        public void HighlightTab(bool highlight)
        {
            var color = Highlight.color;
            if (highlight) Highlight.color = new Color(color.r, color.g, color.b, 1);
            else Highlight.color = new Color(color.r, color.g, color.b, 0);
        }

        public void SetSprite(Sprite sprite)
        {
            var spriteR = Tab.transform.FindChild("ColorButton")?
            .FindChild("Icon")?
            .GetComponent<SpriteRenderer>();
            if (spriteR != null) spriteR.sprite = sprite;
        }

        public void ChangeGameMenu()
        {
            var __instance = Object.FindObjectOfType<GameOptionsMenu>();
            var options = MenuChildren.GetValueOrDefault(this.Name);
            var y = __instance.GetComponentsInChildren<OptionBehaviour>()
                .Max(option => option.transform.localPosition.y);
            var x = __instance.Children[1].transform.localPosition.x;
            var z = __instance.Children[1].transform.localPosition.z;
            var i = 0;

            var OldButtons = __instance.Children.ToList();
            foreach (var option in OldButtons) option.gameObject.SetActive(false);

            foreach (var option in options)
            {
                option.gameObject.SetActive(true);
                option.transform.localPosition = new Vector3(x, y - i++ * 0.5f, z);
            }

           __instance.Children = new Il2CppReferenceArray<OptionBehaviour>(options.ToArray());
            var scroller = __instance.GetComponentInParent<Scroller>();
            scroller?.ScrollPercentY(0f);
        }
    }

    [HarmonyPatch(typeof(PassiveButton), nameof(PassiveButton.ReceiveClickUp))]
    public static class PassiveButtonAction
    {
        public static bool Prefix(PassiveButton __instance)
        {
            bool shouldRun = true;
            foreach (var menu in CustomMenu.AllMenus)
            {
                if (menu.Button == __instance)
                {
                    shouldRun = false;
                    if (menu == CustomMenu._currentMenu) break;
                    CustomMenu._currentMenu = menu;
                    menu.HighlightTab(true);
                    var gameMenu = GameSettingMenu.Instance;
                    if (menu.IsRoleTab && CustomMenu.isOnSettings)
                    {
                        gameMenu.ToggleRoles();
                        CustomMenu.isOnSettings = false;
                    }
                    else if (!menu.IsRoleTab && !CustomMenu.isOnSettings)
                    {
                        gameMenu.ToggleRoles();
                        CustomMenu.isOnSettings = true;
                    }
                    if(!menu.IsRoleTab) menu.ChangeGameMenu();
                    // TODO Role isRoleTab
                    break;
                }
            }
            if(!shouldRun)
                foreach (var menu in CustomMenu.AllMenus) if(menu.Button != __instance) menu.HighlightTab(false);

            return shouldRun;
        }
    }

}
