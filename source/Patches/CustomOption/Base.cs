using System;
using System.Collections.Generic;
using TownOfUs.Patches.CustomOption;
using Object = UnityEngine.Object;

namespace TownOfUs.CustomOption
{
    public class CustomOption
    {
        public static List<CustomOption> AllOptions = new List<CustomOption>();

        public readonly int ID;

        public Func<object, string> Format;
        public string Name;
        public string MenuName;
        protected internal CustomOption(int id, string name, CustomOptionType type, object defaultValue,
            Func<object, string> format = null,
            String menuName = null)
        {
            ID = id;
            Name = name;
            Type = type;
            DefaultValue = Value = defaultValue;
            Format = format ?? (obj => $"{obj}");

            if (Type == CustomOptionType.Button) return;
            AllOptions.Add(this);
            if (menuName == null) MenuName = MenuLoader.VanillaGameName;
            else MenuName = menuName;
            Set(Value);
        }

        protected internal object Value { get; set; }
        protected internal OptionBehaviour Setting { get; set; }
        protected internal CustomOptionType Type { get; set; }
        public object DefaultValue { get; set; }

        public static bool LobbyTextScroller { get; set; } = true;

        protected internal bool Indent { get; set; }

        private static ToggleOption _togglePrefab;
        private static NumberOption _numberPrefab;
        private static StringOption _stringPrefab;
        public static ToggleOption GetTogglePrefab()
        {
            if (_togglePrefab != null) return _togglePrefab;
            _togglePrefab = Object.FindObjectOfType<ToggleOption>();
            return _togglePrefab;
        }

        public static NumberOption GetNumberPrefab()
        {
            if (_numberPrefab != null) return _numberPrefab;
            _numberPrefab = Object.FindObjectOfType<NumberOption>();
            return _numberPrefab;
        }

        public static StringOption GetStringPrefab()
        {
            if (_stringPrefab != null) return _stringPrefab;
            _stringPrefab = Object.FindObjectOfType<StringOption>();
            return _stringPrefab;
        }

        public override string ToString()
        {
            return Format(Value);
        }

        public virtual void OptionCreated()
        {
            Setting.name = Setting.gameObject.name = Setting.transform.name = Name;
        }
        protected internal void Set(object value, bool SendRpc = true)
        {
            System.Console.WriteLine($"{Name} set to {value}");

            Value = value;

            if (Setting != null && AmongUsClient.Instance.AmHost && SendRpc) Rpc.SendRpc(this);

            try
            {
                if (Setting is ToggleOption toggle)
                {
                    var newValue = (bool) Value;
                    toggle.oldValue = newValue;
                    if (toggle.CheckMark != null) toggle.CheckMark.enabled = newValue;
                }
                else if (Setting is NumberOption number)
                {
                    var newValue = (float) Value;

                    number.Value = number.oldValue = newValue;
                    number.ValueText.text = ToString();
                }
                else if (Setting is StringOption str)
                {
                    var newValue = (int) Value;

                    str.Value = str.oldValue = newValue;
                    str.ValueText.text = ToString();
                }
            }
            catch
            {
            }
        }
    }
}