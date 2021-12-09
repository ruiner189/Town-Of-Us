using System;
using TownOfUs.Patches.CustomOption;
using UnityEngine;

namespace TownOfUs.CustomOption
{
    public class CustomNumberOption : CustomOption
    {
        protected internal CustomNumberOption(int id, string name, float value, float min, float max, float increment,
            Func<object, string> format = null, String menuName = null) :
            base(id, name, CustomOptionType.Number, value, format, menuName)
        {
            Min = min;
            Max = max;
            Increment = increment;
        }

        protected internal CustomNumberOption(bool indent, int id, string name, float value, float min, float max,
            float increment, Func<object, string> format = null, String menuName = null) 
            : this(id, name, value, min, max, increment, format, menuName)
        {
            Indent = indent;
        }

        protected float Min { get; set; }
        protected float Max { get; set; }
        protected float Increment { get; set; }

        protected internal float Get()
        {
            return (float) Value;
        }

       
        protected internal void Increase()
        {
            var increment = Increment > 5 && Input.GetKeyInt(KeyCode.LeftShift)
                ? 5
                : Increment;
            Set(Mathf.Clamp(Get() + increment, Min, Max));
        }

        protected internal void Decrease()
        {
            var increment = Increment > 5 && Input.GetKeyInt(KeyCode.LeftShift)
                ? 5
                : Increment;
            Set(Mathf.Clamp(Get() - increment, Min, Max));
        }

        public override void OptionCreated()
        {
            base.OptionCreated();
            var number = Setting.Cast<NumberOption>();

            number.TitleText.text = Name;
            number.ValidRange = new FloatRange(Min, Max);
            number.Increment = Increment;
            number.Value = number.oldValue = Get();
            number.ValueText.text = ToString();
        }
    }
}