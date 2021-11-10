using System;
using TownOfUs.Patches.CustomOption;

namespace TownOfUs.CustomOption
{
    public class CustomHeaderOption : CustomOption
    {
        protected internal CustomHeaderOption(int id, string name,
            String menuName = null) : base(id, name, CustomOptionType.Header, 0, null, menuName)
        {
        }

        public override void OptionCreated()
        {
            base.OptionCreated();
            Setting.Cast<ToggleOption>().TitleText.text = Name;
        }
    }
}