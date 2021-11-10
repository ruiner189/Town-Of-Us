using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Il2CppSystem.Text;
using Reactor;
using Reactor.Extensions;
using TownOfUs.Extensions;
using TownOfUs.Patches.CustomOption;
using UnhollowerBaseLib;
using UnityEngine;
using Object = UnityEngine.Object; //using Il2CppSystem.Collections.Generic;

namespace TownOfUs.CustomOption
{
    public class CustomTabOption : CustomOption
    {
        private List<CustomOption> InternalOptions = new List<CustomOption>();
        private List<OptionBehaviour> OldButtons = new List<OptionBehaviour>();
        private CustomButtonOption BackButton;
        private CustomHeaderOption LoadingButton;
        private CustomHeaderOption Header;

        private float scrollPerc = 0f;
        private float scrollSize = 0f;

        public static List<CustomButtonOption> AllBackButtons = new List<CustomButtonOption>();
        protected internal CustomTabOption(int id, string name, String menuName = null) :
            base(id, name, CustomOptionType.Tab, 0, null, menuName)
        {
            BackButton = new CustomButtonOption(-1, "Back", delegate { Cancel(FlashWhite); }, "Custom");
            LoadingButton = new CustomHeaderOption(-1, "Loading...", "Custom");
            AllBackButtons.Add(BackButton);
            Type = CustomOptionType.Tab;
            Header = new CustomHeaderOption(-1, name, "Custom");
            InternalOptions.Add(Header);
        }

        public void AddOption(CustomOption option)
        {
            InternalOptions.Add(option);
        }

        public void AddOptions(params CustomOption[] options)
        {
            foreach (var option in options) AddOption(option);
        }

        protected internal void Action()
        {
            Coroutines.Start(ActionCoro());
        }

        protected internal OptionBehaviour CreateLoadingButton(ToggleOption togglePrefab)
        {
            if (LoadingButton.Setting == null)
            {
                var toggle = Object.Instantiate(togglePrefab, togglePrefab.transform.parent);
                toggle.transform.GetChild(1).gameObject.SetActive(true);
                toggle.transform.GetChild(2).gameObject.SetActive(false);
                toggle.transform.GetChild(0).localPosition += new Vector3(1f, 0f, 0f);
                BackButton.Setting = toggle;
            }
            LoadingButton.Setting.gameObject.SetActive(true);
            LoadingButton.OptionCreated();
            return LoadingButton.Setting;
        }

        protected internal OptionBehaviour CreateBackButton(ToggleOption togglePrefab)
        {
            if (BackButton.Setting == null)
            {
                var toggle = Object.Instantiate(togglePrefab, togglePrefab.transform.parent);
                toggle.transform.GetChild(1).gameObject.SetActive(true);
                toggle.transform.GetChild(2).gameObject.SetActive(false);
                toggle.transform.GetChild(0).localPosition += new Vector3(1f, 0f, 0f);
                BackButton.Setting = toggle;
            }
            BackButton.Setting.gameObject.SetActive(true);
            BackButton.OptionCreated();
            return BackButton.Setting;
        }

        protected internal IEnumerator ActionCoro()
        {
            var __instance = Object.FindObjectOfType<GameOptionsMenu>();
            var togglePrefab = Object.FindObjectOfType<ToggleOption>();
            var options = CreateOptions();
            var y = __instance.GetComponentsInChildren<OptionBehaviour>()
                .Max(option => option.transform.localPosition.y);
            var x = __instance.Children[1].transform.localPosition.x;
            var z = __instance.Children[1].transform.localPosition.z;
            var i = 0;

            // Stores so we can restore later
            OldButtons = __instance.Children.ToList();

            // Creates a loading button
            var loadingButton = CreateLoadingButton(togglePrefab);
            loadingButton.transform.localPosition = new Vector3(x,y,z);

            // Moves the scroller so we can see loading button. Saves previous scroll.
            var scroller = __instance.GetComponentInParent<Scroller>();
            scrollPerc = scroller.GetScrollPercY();
            scrollSize = scroller.YBounds.max;
            scroller.ScrollPercentY(0f);

            foreach (var option in OldButtons) option.gameObject.SetActive(false);
            __instance.Children = new[] {CreateLoadingButton(togglePrefab)};

            yield return new WaitForSeconds(0.25f);
            loadingButton.gameObject.SetActive(false);
            options.Add(CreateBackButton(togglePrefab));
            foreach (var option in options)
            {
                option.gameObject.SetActive(true);
                option.transform.localPosition = new Vector3(x, y - i++ * 0.5f, z);
            }

            yield return new WaitForEndOfFrame();

            __instance.Children = new Il2CppReferenceArray<OptionBehaviour>(options.ToArray());

        }

        private List<OptionBehaviour> CreateOptions()
        {
            var options = new List<OptionBehaviour>();

            var togglePrefab = Object.FindObjectOfType<ToggleOption>();
            var numberPrefab = Object.FindObjectOfType<NumberOption>();
            var stringPrefab = Object.FindObjectOfType<StringOption>();

            foreach (var option in InternalOptions)
            {
                if (option.Setting != null)
                {
                   // option.Setting.gameObject.SetActive(true);
                    options.Add(option.Setting);
                    continue;
                }

                switch (option.Type)
                {
                    case CustomOptionType.Header:
                        var toggle = Object.Instantiate(togglePrefab, togglePrefab.transform.parent);
                        toggle.transform.GetChild(1).gameObject.SetActive(false);
                        toggle.transform.GetChild(2).gameObject.SetActive(false);
                        option.Setting = toggle;
                        options.Add(toggle);
                        break;
                    case CustomOptionType.Toggle:
                        var toggle2 = Object.Instantiate(togglePrefab, togglePrefab.transform.parent);
                        option.Setting = toggle2;
                        options.Add(toggle2);
                        break;
                    case CustomOptionType.Tab:
                        var tab2 = Object.Instantiate(togglePrefab, togglePrefab.transform.parent);
                        tab2.transform.GetChild(2).gameObject.SetActive(false);
                        option.Setting = tab2;
                        options.Add(tab2);
                        break;
                    case CustomOptionType.Number:
                        var number = Object.Instantiate(numberPrefab, numberPrefab.transform.parent);
                        option.Setting = number;
                        options.Add(number);
                        break;
                    case CustomOptionType.String:
                        var str = Object.Instantiate(stringPrefab, stringPrefab.transform.parent);
                        option.Setting = str;
                        options.Add(str);
                        break;
                }

                option.OptionCreated();
            }
            return options;
        }

        protected internal void Cancel(Func<IEnumerator> flashCoro)
        {
            Coroutines.Start(CancelCoro(flashCoro));
        }

        private IEnumerator FlashWhite()
        {
            yield return null;
        }

        protected internal IEnumerator CancelCoro(Func<IEnumerator> flashCoro)
        {
            var __instance = UnityEngine.Object.FindObjectOfType<GameOptionsMenu>();
            var togglePrefab = Object.FindObjectOfType<ToggleOption>();
            var x = __instance.Children[1].transform.localPosition.x;
            var z = __instance.Children[1].transform.localPosition.z;

            // Creates a loading button
            var loadingButton = CreateLoadingButton(togglePrefab);
            loadingButton.transform.localPosition = new Vector3(
                x, __instance.GetComponentsInChildren<OptionBehaviour>()
                .Max(option => option.transform.localPosition.y), z
             );

            // Moves scroll to top so we can see the loading button
            var scroller = __instance.GetComponentInParent<Scroller>();
            scroller.YBounds.max = scrollSize;
            scroller.ScrollPercentY(0f);

            foreach (var option in __instance.Children) option.gameObject.SetActive(false);
            __instance.Children = new[] { loadingButton };

            yield return new WaitForSeconds(0.25f);
            loadingButton.gameObject.SetActive(false);

            foreach (var option in OldButtons) option.gameObject.SetActive(true);

            __instance.Children = OldButtons.ToArray();

            // Moves scroller to stored spot
            scroller.YBounds.max = scrollSize;
            scroller.ScrollPercentY(scrollPerc);

            yield return new WaitForEndOfFrame();
            yield return flashCoro();
        }

        public override void OptionCreated()
        {
            base.OptionCreated();
            Setting.Cast<ToggleOption>().TitleText.text = Name;
        }
    }
}
