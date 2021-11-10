using System;
using TMPro;

namespace TownOfUs.CustomOption
{
    public class CustomButtonOption : CustomOption
    {
        protected internal Action Do;

        protected internal CustomButtonOption(int id, string name, Action toDo = null,String menuName = null) 
            : base(id, name,CustomOptionType.Button, 0,null, menuName)
        {
            Do = toDo ?? BaseToDo;
        }

        public static void BaseToDo()
        {
        }


        public override void OptionCreated()
        {
            base.OptionCreated();
            Setting.Cast<ToggleOption>().TitleText.text = Name;
            Setting.transform.FindChild("Title_TMP")?.GetComponent<TextMeshPro>()?.SetText(Name, true);
        }
    }
}